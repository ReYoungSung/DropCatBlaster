using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace CharacterBehaviour.Enemy.CatHouse
{
    public class TutorialCatHouseBehaviour_Static : EnemyBehaviour
    {
        [SerializeField] private CatHouseAttribute catHouseData;
        private EnemyEffects enemyEffectsControl = null;
        private _EnemyDetection enemyDetection = null;
        private float dropDistance = 0f;
        private float dropTiming = 0f;
        private float dropBreak = 0f;

        private bool isFalling = true;
        public bool IsFalling { set { isFalling = value; } }
        private bool hasCollided = false;
        public bool HasCollided { get { return hasCollided; } set { hasCollided = value; } }
        private Coroutine coroutine = null;

        private TutorialCatHouseFall fallBehaviour;
        private SkeletonAnimation skeletonAnimation = null;
        private Spine.TrackEntry trackEntry;
        [SerializeField] private GameObject catResidentObj;

        protected override void Awake()
        {
            base.Awake();
            InitializeCachingReferences();
            InitializeCatHouseAttributes();
        }

        private void InitializeCatHouseAttributes()
        {
            health = catHouseData.health;
            dropDistance = catHouseData.dropDistance;
            dropTiming = catHouseData.dropTiming;
            dropBreak = catHouseData.dropBreak;
        }

        private void InitializeCachingReferences()
        {
            fallBehaviour = this.GetComponent<TutorialCatHouseFall>();
            enemyDetection = this.GetComponent<_EnemyDetection>();
            skeletonAnimation = this.GetComponent<SkeletonAnimation>();
            enemyEffectsControl = this.GetComponent<EnemyEffects>();
        }

        private void Start()
        {
            StartCatHouseFall();
        }

        private void StartCatHouseFall()
        {
            if (isFalling)
            {
                if (coroutine == null)
                    coroutine = StartCoroutine(TriggerCatHouseFall());
            }
            else
            {
                return;
            }
        }

        IEnumerator TriggerCatHouseFall()
        {
            if (trackEntry == null)
                trackEntry = skeletonAnimation.state.SetAnimation(1, "HOVERING", true);
            while (!hasCollided)
            {
                yield return new WaitForSeconds(dropBreak);
                fallBehaviour.CatHouseDrop(dropDistance, dropTiming);
            }
        }

        private void Update()
        {
            DestructionProcess(enemyEffectsControl);
            if (hasCollided)
            {
                StopCoroutine(coroutine);
            }
            PeekingOutAnimation();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("PLAYER_BaseHouse"))
            {
                health = 0;
                BaseHouseDurability.instance.DamageObject(catHouseData.damage);
                DestructionProcess(enemyEffectsControl);
            }
            else if (collision.gameObject.CompareTag("DamageCollisionObject"))
            {
                health = 0;
                DestructionProcess(null, enemyEffectsControl);
                Debug.Log("Destroyed");
            }
        }

        private void PeekingOutAnimation()
        {
            if (trackEntry != null)
            {
                if (enemyDetection.IsDetected)
                {

                    if (skeletonAnimation.state.GetCurrent(0).Animation
                        == skeletonAnimation.skeleton.Data.FindAnimation("CatPeekingIn"))
                    {
                        if (trackEntry.IsComplete)
                            trackEntry = skeletonAnimation.state.SetAnimation(0, "CatPeekingOut", false);
                    }
                }
                else
                {
                    if (skeletonAnimation.state.GetCurrent(0).Animation
                        == skeletonAnimation.skeleton.Data.FindAnimation("CatPeekingOut"))
                    {
                        if (trackEntry.IsComplete)
                            trackEntry = skeletonAnimation.state.SetAnimation(0, "CatPeekingIn", false);
                    }
                }
            }
            else
            {
                trackEntry = skeletonAnimation.state.SetAnimation(0, "CatPeekingIn", false);
            }

        }
    }
}
