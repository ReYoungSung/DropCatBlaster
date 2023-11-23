using System.Collections;
using UnityEngine;

namespace CharacterBehaviour.Move
{
    public class PlayerMovement : HumanoidMovement
    {
        private InputManager inputManager = null;
        private PlayerStunPenalty playerStunPenalty = null;
        private Coroutine playerCoroutine = null;
        private GroundCheck groundCheck = null;
        private GameObject contactPlatform;
        private float currentConstraintPositionX;
        private float currentConstraintPositionY;
        private Vector3 distanceFromPlatform;
        private bool hasCollidedRight = false;
        private bool hasCollidedLeft = false;
        private bool hasCollidedDown = false;
        private bool stunAwaiting = true;
        private bool immunityStunTime = false;
        private bool isStunned = false;
        private float stunTimer = 1.5f;
        private float stunEscapeTimer = 0.5f;
        private float stunCountdown = 0;

        public bool StunAwaiting { get { return stunAwaiting; } set { stunAwaiting = value; } }
        public bool ImmunityStunTime { get { return immunityStunTime; } set { immunityStunTime = value; } } //공격 후 스턴 면역
        public bool IsStunned { get { return isStunned; } }
        
        void Update() 
        {     
            //움직이는 발판에서 같이 움직이기
            SyncPositionOnPlatform();
            SetBoundaryToCollision();
            StunActivationCountDown();
        }

        private void SyncPositionOnPlatform()
        {
            if (contactPlatform != null)
            {
                //바닥을 밟고 있고, 좌우로 움직이고 있지 않은 경우
                if (groundCheck.IsGrounded() && inputManager.MoveJoystick.Horizontal == 0)
                {
                    transform.position = contactPlatform.transform.position - distanceFromPlatform;
                    //캐릭터의 위치는 밟고 있는 플랫폼과 distance 만큼 떨어진 위치
                }
                else if (groundCheck.IsGrounded())
                {
                    distanceFromPlatform = contactPlatform.transform.position - transform.position;
                }
            }
        }

        private void SetBoundaryToCollision()
        {
            if (hasCollidedLeft)
            {
                if (inputManager.MoveJoystick.Horizontal < 0)
                    this.transform.position = new Vector2(currentConstraintPositionX, this.transform.position.y);
                else
                    hasCollidedLeft = false;
            }
            else if (hasCollidedRight)
            {
                if (inputManager.MoveJoystick.Horizontal > 0)
                    this.transform.position = new Vector2(currentConstraintPositionX, this.transform.position.y);
                else
                    hasCollidedRight = false;
            }
            if (hasCollidedDown)
            {
                if (groundCheck.IsGrounded())
                    this.transform.position = new Vector2(this.transform.position.x, currentConstraintPositionY);
                else
                    hasCollidedDown = false;
            }
        }

        private void StunActivationCountDown()
        {
            if (stunAwaiting == false)
            {
                stunCountdown += Time.deltaTime;
                if (stunCountdown >= 2f)
                {
                    stunCountdown = 0f;
                    stunAwaiting = true;
                }
            }
            else
            {
                stunCountdown = 0f;
            }
        }

        private void OnCollisionEnter2D(Collision2D other) 
        {
            //접촉한 오브젝트의 태그가 platform 일 때,
            if (other.gameObject.CompareTag("Platform")) 
            {
                //접촉한 순간의 오브젝트 위치를 저장
                contactPlatform = other.gameObject;
                //접촉한 순간의 오브젝트 위치와 캐릭터 위치의 차이를 distance에 저장
                distanceFromPlatform = contactPlatform.transform.position - transform.position;
            }
        }

        private void OnCollisionStay2D(Collision2D collision) 
        {   
            if (collision.gameObject.CompareTag("Platform")) 
            {
                foreach(ContactPoint2D contactPoint in collision.contacts)
                {
                    if(hasCollidedLeft == false)
                    {
                        if (contactPoint.normal == Vector2.right)
                        {
                            hasCollidedLeft = true;
                            currentConstraintPositionX = this.transform.position.x;
                        }
                    }
                    if(hasCollidedRight == false)
                    {
                        if (contactPoint.normal == Vector2.left)
                        {
                            hasCollidedRight = true;
                            currentConstraintPositionX = this.transform.position.x;
                        }
                    }
                    if(hasCollidedDown)
                    {
                        if (contactPoint.normal == Vector2.up)
                        {
                            hasCollidedDown = true;
                            currentConstraintPositionY = contactPoint.collider.bounds.max.y;
                        }
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision) 
        {   
            //접촉한 오브젝트의 태그가 platform 일 때,
            if (collision.gameObject.CompareTag("Platform")) 
            {
                contactPlatform = null;
                hasCollidedRight = false;
                hasCollidedLeft = false;
                hasCollidedDown = false;
            }
        }
        
        private void Awake()
        {
            InitializeCachingReferences();
        }

        private void InitializeCachingReferences()
        {
            playerStunPenalty = this.GetComponent<PlayerStunPenalty>();
            groundCheck = this.GetComponent<GroundCheck>();
            inputManager = GameObject.Find("[InputManager]").GetComponent<InputManager>();
        }

        public void OnStunnedEnter(bool isAttacking, Collider2D collision, AudioManager audioManager)
        {
            if (!isAttacking)
            {
                if (BasicEnemyStunVerified(collision))
                {
                    AllocateStunRoutine(audioManager);
                }
                if (SpecialEnemyStunVerified(collision))
                {
                    AllocateSpecialStunRoutine(audioManager);
                }
            }
        }

        private bool BasicEnemyStunVerified(Collider2D collided)
        {
            GameObject collidedEnemy = collided.gameObject;
            if (collidedEnemy.CompareTag("ENEMY_CatHouse"))
            {
                if (CatHouseStunActive(collided))
                {
                    return true;
                }
            }
            return false;
        }

         private bool SpecialEnemyStunVerified(Collider2D collided)
        {
            GameObject collidedEnemy = collided.gameObject;
            if(collidedEnemy.CompareTag("ENEMY_Drone"))
            {
                return true;
            }
            else if(collidedEnemy.CompareTag("ENEMY_Spaceship"))
            {
                return true;
            }
            else if (collidedEnemy.CompareTag("DamageCollisionObject"))
            {
                return true;
            }
            return false;
        }

        private bool CatHouseStunActive(Collider2D collided)
        {
            return playerStunPenalty.CatHouseIsFalling(collided.gameObject) &&
                playerStunPenalty.PlayerCollidedBottom(collided);
        }

        private void AllocateStunRoutine(AudioManager audioManager)
        {
            //코루틴이 끝나며 stunAwaiting이 true가 되거나 공격 무적 시간 후 immunityStuntime이 false로 변하면 stun 실행
            if (stunAwaiting || !immunityStunTime)
            {
                if(playerCoroutine == null)
                {
                    playerCoroutine = StartCoroutine(PlayerStunRoutine());
                    audioManager.PlaySFX("Stunned_Sound");
                }
            }
            else
            {
                return;
            }
        }

        private void AllocateSpecialStunRoutine(AudioManager audioManager)
        {
            //코루틴이 끝나며 stunAwaiting이 true가 되면 stun 실행
            if (stunAwaiting || immunityStunTime)
            {
                if(playerCoroutine == null)
                {
                    playerCoroutine = StartCoroutine(PlayerStunRoutine());
                    audioManager.PlaySFX("Stunned_Sound");
                }
            }
            else
            {
                return;
            }
        }

        private IEnumerator PlayerStunRoutine()
        {
            stunAwaiting = false;
            isStunned = true;
            yield return new WaitForSecondsRealtime(stunTimer);
            isStunned = false;
            yield return new WaitForSecondsRealtime(stunEscapeTimer);
            stunAwaiting = true;
            playerCoroutine = null;
        }

        #region SkillArea_Postponed Usage - Teleport
        /*
        public void TeleportDown(float teleportDownDistance, float groundColliderHalfY, AudioManager audioManager, VFXSpawningManager vFXSpawningManager)
        {
            Vector2 currentPosition = this.transform.position;

            if (teleportDownDistance < currentPosition.y - groundColliderHalfY)
            {
                this.transform.position = new Vector2(currentPosition.x, currentPosition.y - teleportDownDistance);
                vFXSpawningManager.ActivateHitEffect(this.transform.position, 0);
                audioManager.PlaySFX("Teleporting_Sound");
            }
            else
            {
                return;
            }
        }
        */
        #endregion
    }
}

