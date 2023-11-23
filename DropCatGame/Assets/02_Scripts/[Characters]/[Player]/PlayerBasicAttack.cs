using System.Collections;
using UnityEngine;
using CharacterManager;
using CharacterBehaviour.Move;
using CharacterBehaviour.Enemy.CatHouse;
using CharacterBehaviour.Enemy.CatResident;
using System.Runtime.CompilerServices;

namespace CharacterBehaviour.Attack
{
    public enum EnemyType
    {
        None, CatHouse, BombCat, CatResident, SpaceshipHatch, Drone, TutorialCatHouse
    }

    public class PlayerBasicAttack : MonoBehaviour
    {
        private EnemyType currentPunchingEnemy = EnemyType.None;
        private RaycastHit2D currentlyDetectedEnemy = new RaycastHit2D();
        public RaycastHit2D CurrentlyDetectedEnemy { get { return currentlyDetectedEnemy; } set { currentlyDetectedEnemy = value; } }
        private GameObject playerObj = null;
        private BoxCollider2D playerCollider = null;
        private Rigidbody2D playerRigidBody;
        private bool isAttackDrop = false;
        private float attackDropVelocity = 0f; //공격 후 추락 속도
        [SerializeField] private LayerMask enemyLayer;
        private float attackDistance = 80f;
        private PlayerAnimation playerAnimation;
        private PlayerMovement playerMovement = null;
        [SerializeField] private ParticleSystem explosion;
        [SerializeField] private PauseManager pauseManager;
        private bool isAttacking = false;
        private PlayerManager playerManager = null;
        [SerializeField] private float defaultGravityScale = 70;
        private float horizontalHitGravityScale = 0f;
        private float verticalHitGravityScale = 0f;

        private Coroutine ImmunityCoroutine = null;
        private Coroutine attackCoroutine = null;
        private Coroutine fixedDropCoroutine = null;
        private WaitForSeconds attackAnimationDelay = new WaitForSeconds(0.01f);
        private WaitForSeconds attackFinishDelayHorizontal = new WaitForSeconds(0.1f);
        private WaitForSeconds attackFinishDelayVertical = new WaitForSeconds(0.18f);
        private string attackDirection = null;
        private PlayerAttackVisualFeedback visualFeedback = null;
        private PlayerBasicAttackLogger basicAttackLogger = null;

        public EnemyType CurrentPunchingEnemy { get { return currentPunchingEnemy; } }
        public bool IsAttacking { get { return isAttacking; } }
        public float AttackDistance { get { return attackDistance; } }
        private CircleCollider2D enemyApproachEventTrigger = null;


        private void Awake()
        {
            InitializeCachingReference();
        }

        private void InitializeCachingReference()
        {
            playerObj = this.transform.parent.gameObject;
            playerManager = playerObj.GetComponent<PlayerManager>();
            playerMovement = playerObj.GetComponent<PlayerMovement>();
            playerAnimation = playerObj.GetComponent<PlayerAnimation>();
            playerCollider = playerObj.GetComponent<BoxCollider2D>();
            playerRigidBody = playerObj.GetComponent<Rigidbody2D>();
            basicAttackLogger = this.GetComponent<PlayerBasicAttackLogger>();
            visualFeedback = this.GetComponent<PlayerAttackVisualFeedback>();
            playerRigidBody.gravityScale = defaultGravityScale;
            enemyApproachEventTrigger = this.GetComponentInParent<CircleCollider2D>();
        }

        private void Update()
        {
            FlushCurrentPunchingEnemy();
            ApplyDropVelocity();
        }

        public void VisualFeedbackSystem(bool toggleVal, Vector2 aimingDirection)
        {
            visualFeedback.DisplayVisualFeedback(toggleVal);
            if(toggleVal)
                visualFeedback.UpdateVisualFeedback(aimingDirection, attackDistance);
        }

        private void ApplyDropVelocity()
        {
            if (isAttackDrop == true)
            {
                playerRigidBody.velocity = new Vector2(0, attackDropVelocity);
            }
            else
            {
                playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
            }
        }
        public bool DetectEnemy(Vector2 aimingDirection)
        {
            RaycastHit2D hitSensor = Physics2D.Raycast(this.transform.position, aimingDirection, attackDistance, enemyLayer);
            EnemySensor(hitSensor);

            DisplaySensorGizmo(this.transform.position, aimingDirection, attackDistance);
            return hitSensor;
        }

        public void PerformBasicAttack()
        {
            if (ImmunityCoroutine != null)
            {
                StopCoroutine(ImmunityCoroutine);
            }
            if (EnemyHitNotNull(currentlyDetectedEnemy))
            { 
                attackCoroutine = StartCoroutine(BasicAttackRoutine());
                ImmunityCoroutine = StartCoroutine(ImmunityStun());
            }
        }

        private IEnumerator BasicAttackRoutine()
        {
            if (!isAttacking)
            {
                isAttacking = true;
                CancelFixedDropRoutine();
                DetectEnemyType();
                AdjustHorizontalHitGravity();
                yield return AttackSequence();
                FinishAttackSequence();
            }
        }

        private void AdjustHorizontalHitGravity()
        {
            playerRigidBody.gravityScale = horizontalHitGravityScale;
            float velocityX = playerRigidBody.velocity.x;
            playerRigidBody.velocity = new Vector2(velocityX, 0);
        }

        private void AdjustVerticalGravity()
        {
            playerRigidBody.gravityScale = verticalHitGravityScale;
        }

        private IEnumerator AttackSequence()
        {
            DamageEnemy();
            PunchDashToEnemy();
            InitializeEnemySensors();
            yield return attackFinishDelayVertical;
        }

        private void FinishAttackSequence()
        {
            playerRigidBody.gravityScale = defaultGravityScale;
            basicAttackLogger.IncreasePlayerScore(10);
            basicAttackLogger.IncreaseComboCount();
            basicAttackLogger.StartComboTimer();
            fixedDropCoroutine = StartCoroutine(FixDropVelocity());
            ResetCharacterAngle();
            isAttacking = false;
        }

        public void SetAnimationByAngle(Vector2 aimingDirection)
        {
            float angle = Vector2.Angle(aimingDirection, this.transform.right);
            if (IsAimingHorizontalDirection(angle))
            {
                playerAnimation.SetAnimationState(PlayerAnimationState.Attacking_Horizontal);
                RotateAttackingCharacterHorizontal(aimingDirection.x);
            }
            else if(IsAimingVerticalDirection(angle))
            {
                if(aimingDirection.y > 0)
                {
                    playerAnimation.SetAnimationState(PlayerAnimationState.Attacking_Vertical_UP);
                }
                else if(aimingDirection.y < 0)
                {
                    playerAnimation.SetAnimationState(PlayerAnimationState.Attacking_Vertical_DOWN);
                }
                RotateAttackingCharacterVertical();
            }
            else
            {
                playerAnimation.SetAnimationState(PlayerAnimationState.Idling);
            }
        }

        private void RotateAttackingCharacterHorizontal(float xDirection)
        {
            Vector2 targetDirection = this.transform.position - currentlyDetectedEnemy.transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.identity;
            if (xDirection > 0)
            {
                angleAxis = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
            }
            else if(xDirection < 0)
            {
                angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                angleAxis = Quaternion.identity;
            }
            transform.parent.rotation = angleAxis;
        }

        private void RotateAttackingCharacterVertical()
        {
            Vector2 targetDirection = Vector2.zero;
            if(currentlyDetectedEnemy.transform.position.y > 0)
            {
                targetDirection = this.transform.position - currentlyDetectedEnemy.transform.position;
            }
            else if(currentlyDetectedEnemy.transform.position.y < 0)
            {
                targetDirection = currentlyDetectedEnemy.transform.position - this.transform.position;
            }
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            if(angle < 0)
            {
                angle += 90f;
            }
            else
            {
                angle -= 90f;
            }
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.parent.rotation = angleAxis;
        }

        private void ResetCharacterAngle()
        {
            transform.parent.rotation = Quaternion.identity;
        }

        private bool IsAimingHorizontalDirection(float angle)
        {
            return Mathf.Abs(angle) < 45f || 135f < angle && angle < 225f;
        }

        private bool IsAimingVerticalDirection(float angle)
        {
            return 45f <= angle && angle <= 135f;
        }

        private bool EnemyHitNotNull(RaycastHit2D hit)
        {
            return hit.collider != null;
        }

        private void CancelFixedDropRoutine()
        {
            if (fixedDropCoroutine != null)
            {
                StopCoroutine(fixedDropCoroutine);
                fixedDropCoroutine = null;
            }
        }

        private IEnumerator FixDropVelocity()
        {
            isAttackDrop = true;
            yield return new WaitForSecondsRealtime(0.1f);
            isAttackDrop = false;  
        }

        private EnemyType DetectEnemyType()
        {
            GameObject enemyObject = currentlyDetectedEnemy.transform.gameObject;
            if (enemyObject != null)
            {
                if (enemyObject.CompareTag("ENEMY_CatHouse"))
                {
                    currentPunchingEnemy = EnemyType.CatHouse;
                }
                else if (enemyObject.CompareTag("ENEMY_CatResident"))
                {
                    currentPunchingEnemy = EnemyType.CatResident;
                }
                else if (enemyObject.CompareTag("ENEMY_TutorialCatHouse"))
                {
                    currentPunchingEnemy = EnemyType.TutorialCatHouse;
                }
                else if(enemyObject.CompareTag("ENEMY_BombCatDrone"))
                {
                    currentPunchingEnemy = EnemyType.BombCat;
                }
            }
            else
            {
                currentPunchingEnemy = EnemyType.None;
            }
            return currentPunchingEnemy;
        }

        private void FlushCurrentPunchingEnemy()
        {
            if (!isAttacking)
            {
                currentPunchingEnemy = EnemyType.None;
            }
        }

        private void PunchDashToEnemy()
        {
            if (currentlyDetectedEnemy)
            {
                LeanTween.move(this.transform.parent.gameObject, 
                    currentlyDetectedEnemy.transform.position, 0.3f)
                    .setEaseOutExpo();
                //this.transform.parent.position = currentlyDetectedEnemy.transform.position;
            }
        }

        private void InitializeEnemySensors() //양자택일 경우 커서 유지 해결
        {
            if (currentlyDetectedEnemy)
            {
                GameObject enemyObj = currentlyDetectedEnemy.collider.transform.parent.gameObject;
                if (enemyObj.GetComponent<_EnemyDetection>() != null)
                {
                    enemyObj.GetComponent<_EnemyDetection>().IsDetected = false;
                }
            }
            currentlyDetectedEnemy = new RaycastHit2D();
        }

        private void DamageEnemy()
        {
            if (currentlyDetectedEnemy)
            {
                Transform enemyObject = currentlyDetectedEnemy.collider.transform.parent;
                if (currentPunchingEnemy == EnemyType.CatHouse)
                {
                    enemyObject.gameObject.GetComponent<CatHouseBehaviour>().DamageObject();
                    playerManager.ChargeJumpCount();
                }
                else if (currentPunchingEnemy == EnemyType.CatResident)
                {
                    enemyObject.GetComponent<CatResidentBehaviour>().DamageObject();
                    playerManager.ChargeJumpCount();
                    
                }
                else if (currentPunchingEnemy == EnemyType.TutorialCatHouse)
                {
                    enemyObject.GetComponent<TutorialCatHouseBehaviour>().DamageObject();
                    playerManager.ChargeJumpCount();
                }
                else if(currentPunchingEnemy == EnemyType.BombCat)
                {
                    enemyObject.GetComponent<BombCatDroneBehaviour>().DamageObject();
                    playerManager.ChargeJumpCount();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void EnemySensor(RaycastHit2D currentlyDetected)
        {
            if (currentlyDetected)
            {
                //Update Sensored Enemy
                if (DetectedEnemyHasChanged(currentlyDetected))
                {
                    EmptyDetectionCache();
                    UpdateDetectionCache(currentlyDetected);
                }
            }
            else
            {
                EmptyDetectionCache();
            }
        }

        private bool DetectedEnemyHasChanged(RaycastHit2D currDetected)
        {
            return currentlyDetectedEnemy.collider != currDetected.collider;
        }

        private void EmptyDetectionCache()
        {
            currentlyDetectedEnemy = new RaycastHit2D();
        }

        private void UpdateDetectionCache(RaycastHit2D currentlyDetected)
        {
            currentlyDetectedEnemy = currentlyDetected;
        }

        private void OnTriggerEnter2D(Collider2D enemyApproaching)
        {
            if (enemyApproaching.GetComponent<_EnemyDetection>() != null)
                enemyApproaching.GetComponent<_EnemyDetection>().IsDetected = true;
        }

        private void OnTriggerExit2D(Collider2D enemyApproaching)
        {
            if (enemyApproaching.GetComponent<_EnemyDetection>() != null)
                enemyApproaching.GetComponent<_EnemyDetection>().IsDetected = false;
        }

        private void DisplaySensorGizmo(Vector2 origin, Vector2 direction, float attackDistance)
        {
            Debug.DrawLine(origin, origin + direction * attackDistance);
        }

        private IEnumerator ImmunityStun()
        {
            playerMovement.ImmunityStunTime = true;
            playerMovement.StunAwaiting = false;
            yield return new WaitForSecondsRealtime(1f);
            playerMovement.ImmunityStunTime = false;
        }

        #region Conventional 4 Directional Attack
        /*
        public void PerformHorizontalAttack(float aimingDirection)
        {
            if (ImmunityCoroutine != null)
            {
                StopCoroutine(ImmunityCoroutine);
            }
            string attackDirection = GetHorizontalAttackDirection(aimingDirection);
            if (EnemyHitNotNull(detectedEnemies[attackDirection]))
            {
                if (attackDirection == "LEFT" || attackDirection == "RIGHT")
                {
                    attackCoroutine = StartCoroutine(BasicAttackHorizontal(attackDirection));
                }
                ImmunityCoroutine = StartCoroutine(ImmunityStun());
            }
        }

        public void PerformVerticalAttack(float aimingDirection)
        {
            if (ImmunityCoroutine != null)
            {
                StopCoroutine(ImmunityCoroutine);
            }
            string attackDirection = GetVerticalAttackDirection(aimingDirection);
            if (EnemyHitNotNull(detectedEnemies[attackDirection]))
            {
                if (attackDirection == "UP" || attackDirection == "DOWN")
                {
                    attackCoroutine = StartCoroutine(BasicAttackVertical(attackDirection));
                }
                ImmunityCoroutine = StartCoroutine(ImmunityStun());
            }
        }

        private string GetHorizontalAttackDirection(float aimingDirection)
        {
            attackDirection = null;
            if (aimingDirection > 0.001f)
            {
                attackDirection = "RIGHT";
            }
            else if (aimingDirection < -0.001f)
            {
                attackDirection = "LEFT";
            }
            playerMovement.FlipCharacter(aimingDirection);
            return attackDirection;
        }

        private string GetVerticalAttackDirection(float aimingDirection)
        {
            attackDirection = null;
            if (aimingDirection > 0.001f)
            {
                attackDirection = "UP";
            }
            else if (aimingDirection < -0.001f)
            {
                attackDirection = "DOWN";
            }
            return attackDirection;
        }

        private Vector2 ComputePunchPosition(Vector2 enemyObjPos, string attackDir)
        {
            if (attackDir == "UP" || attackDir == "DOWN")
            {
                return new Vector2(this.transform.parent.position.x, enemyObjPos.y - 25f);
            }
            else if (attackDir == "LEFT" || attackDir == "RIGHT")
            {
                return new Vector2(enemyObjPos.x, this.transform.parent.position.y);
            }
            else
            {
                return Vector2.zero;
            }
        }

        private Vector2 ComputePunchPosition(GameObject enemyObj, string attackDir)
        {
            Vector2 enemyPos = enemyObj.transform.position;
            if (attackDir == "UP" || attackDir == "DOWN")
            {
                return new Vector2(this.transform.parent.position.x, enemyPos.y);
            }
            else if (attackDir == "LEFT" || attackDir == "RIGHT")
            {
                return new Vector2(enemyPos.x, this.transform.parent.position.y);
            }
            else
            {
                return Vector2.zero;
            }
        }

        private Vector2 ComputePunchPosition(RaycastHit2D hit2D, string attackDir)
        {
            if (attackDir == "UP" || attackDir == "DOWN")
            {
                return new Vector2(this.transform.position.x, hit2D.point.y);
            }
            else if (attackDir == "LEFT" || attackDir == "RIGHT")
            {
                return new Vector2(hit2D.point.x, this.transform.position.y);
            }
            else
            {
                return Vector2.zero;
            }
        }

        private IEnumerator BasicAttackHorizontal(string attackDirection)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                RaycastHit2D hit = detectedEnemies[attackDirection];
                CancelFixedDropRoutine();
                DetectEnemyType(hit.transform.gameObject);
                AdjustHorizontalHitGravity();
                yield return AnimationSequenceHorizontal();
                yield return AttackSequence(hit, attackFinishDelayHorizontal);
                FinishAttackSequence();
            }
        }

        private IEnumerator BasicAttackVertical(string attackDirection)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                RaycastHit2D hit = detectedEnemies[attackDirection];
                CancelFixedDropRoutine();
                DetectEnemyType(hit.transform.gameObject);
                AdjustVerticalGravity();
                yield return AnimationSequenceVertical();
                yield return AttackSequence(hit, attackFinishDelayVertical);
                FinishAttackSequence();
            }
        }

        private IEnumerator AnimationSequenceHorizontal()
        {
            playerAnimation.SetAnimationState(PlayerAnimationState.Attacking_Horizontal);
            yield return attackAnimationDelay;
        }

        private IEnumerator AnimationSequenceVertical()
        {
            if (attackDirection == "UP")
                playerAnimation.SetAnimationState(PlayerAnimationState.Attacking_Vertical_UP);
            else if (attackDirection == "DOWN")
                playerAnimation.SetAnimationState(PlayerAnimationState.Attacking_Vertical_DOWN);
            yield return attackAnimationDelay;
        }

        private void EnemySensor(string sensorDirection, RaycastHit2D currentlyDetected)
        {
            if (EnemyDetectedAtDirection(currentlyDetected))
            {
                //Update Sensored Enemy
                if (DetectedEnemyHasChanged(sensorDirection, currentlyDetected))
                {
                    EmptyDetectionCache(sensorDirection);
                }
                UpdateDetectionCache(sensorDirection, currentlyDetected);
                MarkGameObjectDetection(currentlyDetected, true);
            }
            else
            {
                EmptyDetectionCache(sensorDirection);
            }
        }
        */
        #endregion
    }
}