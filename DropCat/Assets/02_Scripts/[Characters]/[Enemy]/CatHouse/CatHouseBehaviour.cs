using System.Collections;
using UnityEngine;
using Spine.Unity;

namespace CharacterBehaviour.Enemy.CatHouse
{
    public class CatHouseBehaviour : EnemyBehaviour
    {
        [SerializeField] private CatHouseAttribute catHouseData;
        private EnemyEffects enemyEffectsControl = null;
        private _EnemyDetection enemyDetection = null;
        private float dropDistance = 0f;
        private float dropTiming = 0f;
        private float dropBreak = 0f;

        private bool isFalling = true;
        public bool IsFalling { get { return isFalling; } set { isFalling = value; } }
        private bool hasCollided = false;
        public bool HasCollided { get { return hasCollided; } set { hasCollided = value; } }
        private Coroutine coroutine = null;
        private VFXGenerator vFXGenerator = null;
        private AudioManager audioManager = null;

        private CatHouseFall fallBehaviour;
        private SkeletonAnimation skeletonAnimation = null;
        private Spine.TrackEntry trackEntry;
        [SerializeField] private GameObject catResidentObj;
        private bool spawned = false;
        private bool restrictDamageFlag = false;
        
        protected override void Awake()
        {
            base.Awake();
            InitializeCachingReferences();
            InitializeCatHouseAttributes();
            vFXGenerator = this.transform.GetChild(1).GetComponent<VFXGenerator>();
            audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
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
            fallBehaviour = this.GetComponent<CatHouseFall>();
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
            if(trackEntry == null)
                trackEntry =  skeletonAnimation.state.SetAnimation(1, "HOVERING", true);
            while (!hasCollided)
            {
                yield return new WaitForSeconds(dropBreak);
                fallBehaviour.CatHouseDrop(dropDistance, dropTiming);
            }
        }

        private void Update()
        {
            if(spawned == false)
            {
                DestructionProcess(enemyEffectsControl);
            }
            else
            {
                DestructionProcess(catResidentObj, enemyEffectsControl);
            }
            if (hasCollided)
            {
                StopCoroutine(coroutine);
            }
            PeekingOutAnimation();
        }

        public override void DestructionProcess(EnemyEffects enemyEffectsControl)
        {
            if (health <= 0)
            {
                --objectSpawnManager.CatHouseCount;
                enemyEffectsControl.DestructionProcess(this.transform.position);
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("PLAYER_BaseHouse"))
            {
                health = 0;
                if (!restrictDamageFlag)
                {
                    BaseHouseDurability.instance.DamageObject(catHouseData.damage);
                    restrictDamageFlag = true;
                }
                audioManager.PlaySFX("HousePunchImpact");
            }
            else if (collision.gameObject.CompareTag("GROUNDOBJ"))
            {
                health = 0;
                if(!spawned)
                {
                    spawned = true;
                }
                audioManager.PlaySFX("HousePunchImpact");
            }
            else if (collision.gameObject.CompareTag("Platform"))
            {
                health = 0;
                if(!spawned)
                {
                    spawned = true;
                }
                audioManager.PlaySFX("HousePunchImpact");
            }
            else if(collision.gameObject.CompareTag("DamageCollisionObject"))
            {
                health = 0;
                audioManager.PlaySFX("HousePunchImpact");
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
        private void OnEnable() 
        {
            vFXGenerator.SetVFXAnimationState(VFXAnimationState.Teleporting);
        }
    }
}
