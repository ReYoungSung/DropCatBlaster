using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace CharacterAI
{
    public class CatResidentAgentManager : AgentManager
    {
        [SerializeField] private CatResidentAttribute catResidentData;
        private Collider2D characterCollider;
        private float extentX; private float extentY;
        private float attackOffset = 0f;
        private SkeletonAnimation skeletonAnimation = null;
        private GameObject baseHouse = null;
        private BaseHouseDurability baseHouseDurability = null;
        private Spine.TrackEntry trackEntry = null;

        private void Awake()
        {
            characterCollider = this.GetComponent<Collider2D>();
            extentX = characterCollider.bounds.extents.x;
            extentY = characterCollider.bounds.extents.y;
            skeletonAnimation = this.GetComponent<SkeletonAnimation>();
            baseHouse = GameObject.FindGameObjectWithTag("PLAYER_BaseHouse");
            maxSpeed = catResidentData.maxSpeed;
            maxAccel = catResidentData.maxAccel;
            attackOffset = catResidentData.attackOffset;
            trackEntry = skeletonAnimation.state.SetAnimation(0, "RUNNING", true);
        }

        private void Start()
        {
            velocity = Vector2.zero;
            steering = new Steering();
            if(baseHouse != null)
            {
                Physics2D.IgnoreCollision(baseHouse.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
                baseHouseDurability = baseHouse.GetComponent<BaseHouseDurability>();
                skeletonAnimation.AnimationState.Event += delegate { baseHouseDurability.DamageObject(1); };
            }
            
        }

        public override void SetSteering(Steering steering)
        {
            this.steering = steering;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision != null)
            {
                if(collision.gameObject.CompareTag("PLAYER_BaseHouse"))
                {
                    maxSpeed = 0f;
                    maxAccel = 0f;
                    if (trackEntry.Animation.Name != "ATTACKING")
                    {
                        trackEntry = skeletonAnimation.state.SetAnimation(0, "ATTACKING", true);
                        trackEntry.TimeScale = 0.5f;
                    }
                }
                else if(collision.gameObject.CompareTag("ENEMY_CatResident"))
                {
                    return;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision != null)
            {
                if (collision.gameObject.CompareTag("PLAYER_BaseHouse"))
                {
                    maxSpeed = 0f;
                    maxAccel = 0f;
                    if (trackEntry.Animation.Name != "ATTACKING")
                    {
                        trackEntry = skeletonAnimation.state.SetAnimation(0, "ATTACKING", true);
                        trackEntry.TimeScale = 0.5f;
                    }
                }
                else if (collision.gameObject.CompareTag("ENEMY_CatResident"))
                {
                    return;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            maxSpeed = catResidentData.maxSpeed;
            maxAccel = catResidentData.maxAccel;
            if (collision == null)
            {
                if (trackEntry.Animation.Name != "RUNNING")
                {
                    trackEntry = skeletonAnimation.state.SetAnimation(0, "RUNNING", true);
                    trackEntry.TimeScale = 1f;
                }
            }
            else
            {
                if (collision.gameObject.CompareTag("PLAYER_BaseHouse"))
                {
                    if (trackEntry.Animation.Name != "RUNNING")
                    {
                        trackEntry = skeletonAnimation.state.SetAnimation(0, "RUNNING", true);
                        trackEntry.TimeScale = 1f;
                    }
                }
            }
        }

        private bool HasApproachedTarget(Collider2D targetCollider)
        {
            if(targetCollider != null)
            {
                if(ApproachedTargetX(targetCollider))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        private bool ApproachedTargetX(Collider2D targetCollider)
        {
            return this.transform.position.x + extentX > targetCollider.bounds.min.x - attackOffset
                || this.transform.position.x - extentX < targetCollider.bounds.max.x + attackOffset;
        }
    }
}