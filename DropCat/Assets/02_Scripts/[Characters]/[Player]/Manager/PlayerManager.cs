using System;
using UnityEngine;
using UnityEngine.InputSystem;
using CharacterBehaviour.Move;
using CharacterBehaviour.Attack;

namespace CharacterManager
{
    public class PlayerManager : MonoBehaviour
    {
        private GameObject playerCharacter = null;
        private InputManager inputManager = null;
        private _PlayerInputActions playerInputActions = null;
        private InputAction playerMoveAction = null;
        private PlayerMovement playerMovement = null;
        private Rigidbody2D playerRigidBody2D = null;
        private GroundCheck groundCheck = null;
        private PlayerBasicAttack playerBasicAttack = null;
        private AudioManager audioManager = null;
        private PlayerAnimation playerAnimation = null;
        private PauseManager pauseManager = null;
        private int availableJump = 0;
        private VFXSpawningManager vFXSpawningManager = null;

        private Vector2 moveDirection = Vector2.zero;

        [SerializeField] private PlayerCharacterData characterData;
        public GameObject PlayerCharacter { get { return playerCharacter; } }
        public static GameObject PlayerAttackRayOriginObj { get; private set; }
        public int AvailableJump { get { return availableJump; } }

        private void Awake()
        {
            InitializeCharacter();
            InitializeCharacterManager();
        }

        private void InitializeCharacter()
        {
            if (playerCharacter == null)
            {
                playerCharacter = GameObject.Find("[Player]");
            }
            availableJump = characterData.maxJumpCount;
        }

        private void InitializeCharacterManager()
        {
            InitializeCachingReferences();
            LoadJumpEvent();
            LoadSwipeEvent();
            LoadPauseGameEvent();
        }

        private void InitializeCachingReferences()
        {
            if (playerCharacter != null)
            {
                playerMovement = this.GetComponent<PlayerMovement>();
                playerRigidBody2D = this.GetComponent<Rigidbody2D>();
                inputManager = GameObject.Find("[InputManager]").GetComponent<InputManager>();
                playerInputActions = inputManager.PlayerInputActions;
                playerMoveAction = playerInputActions.FindAction("PlayerMove");
                PlayerAttackRayOriginObj = this.transform.GetChild(0).gameObject;
                playerBasicAttack = PlayerAttackRayOriginObj.GetComponent<PlayerBasicAttack>();
                groundCheck = this.GetComponent<GroundCheck>();
                playerAnimation = this.GetComponent<PlayerAnimation>();
                audioManager = this.transform.GetChild(1).GetComponent<AudioManager>();
                pauseManager = GameObject.Find("[SceneEventManager]").GetComponent<PauseManager>();
                vFXSpawningManager = this.GetComponent<VFXSpawningManager>();
            }
        }

        private void LoadJumpEvent()
        {
            if (playerInputActions != null)
            {
                playerInputActions.Player.PlayerJump.performed += ctx
                => playerMovement.Jump(playerRigidBody2D, ctx.ReadValue<float>(), characterData.jumpHeight, ref availableJump);
                playerInputActions.Player.PlayerJump.canceled += ctx
                    => playerMovement.Jump(playerRigidBody2D, ctx.ReadValue<float>(), characterData.jumpHeight, ref availableJump);

                playerInputActions.Player.PlayerJump.performed += ctx
                    => playerAnimation.SetAnimationState(PlayerAnimationState.Jumping);
                
                playerInputActions.Player.PlayerJump.performed += ctx => audioManager.PlaySFX("Jumping_Sound");
                playerInputActions.Player.PlayerJump.canceled += ctx => audioManager.PlaySFX("Jumping_Sound");
            }
        }

        private void LoadPauseGameEvent()
        {
            playerInputActions.Player.PauseGame.performed += ctx
                => pauseManager.DoPauseGame(ctx);
        }

        private void LoadSwipeEvent()
        {
            inputManager.MoveJoystick.OnSwipeEvent += SwipeEvent;
        }

        private void SwipeEvent()
        {
            if(playerBasicAttack.DetectEnemy(inputManager.MoveJoystick.Direction) && !playerMovement.IsStunned)
            {
                playerBasicAttack.SetAnimationByAngle(inputManager.MoveJoystick.Direction);
                playerBasicAttack.PerformBasicAttack();
            }
        }

        /*
        private void Teleport()
        {
            playerMovement.TeleportDown
            (characterData.teleportDownDistance, GroundCollider2D.instance.GroundColliderHalf.y, audioManager, vFXSpawningManager);
        }
        

        private void SwipeEvent()
        {
            if(inputManager.SwipeDirection == "UP")
            {
                playerBasicAttack.PerformVerticalAttack(1f);
            }
            else if (inputManager.SwipeDirection == "DOWN")
            {
                playerBasicAttack.PerformVerticalAttack(-1f);
            }
            else if (inputManager.SwipeDirection == "LEFT")
            {
                playerBasicAttack.PerformHorizontalAttack(-1f);
            }
            else if (inputManager.SwipeDirection == "RIGHT")
            {
                playerBasicAttack.PerformHorizontalAttack(1f);
            }
            else
            {
                return;
            }
        }
        */

        private void OnTriggerEnter2D(Collider2D collision)
        {
            playerMovement.OnStunnedEnter(playerBasicAttack.IsAttacking, collision, audioManager);
        }

        private void Start()
        {
            playerMovement.ResetAvailableJump(ref availableJump, characterData.maxJumpCount);
        }

        private void Update()
        {
            UpdateRoutine();
            playerBasicAttack.VisualFeedbackSystem(inputManager.IsMovingJoystick(), inputManager.MoveJoystick.Direction);
        }

        public void ChargeJumpCount()
        {
            if (availableJump < 3) { availableJump++; }
        }

        private void UpdateRoutine()
        {
            if(!inputManager.InputDisabled)
            {
                MoveCharacter(moveDirection);
                if(MathF.Abs(moveDirection.x) > 0f)
                {
                    PlayRunningSound();
                }
                else
                {
                    audioManager.StopLoopingSFX();
                    moveDirection = Vector2.zero;
                }
            }
            else
            {
                audioManager.StopLoopingSFX();
                moveDirection = Vector2.zero;
            }
            playerMovement.ConstrainPosition
                (GroundCollider2D.instance.BoundsLeft, GroundCollider2D.instance.BoundsRight);

            if (groundCheck.IsGrounded())
            {
                playerMovement.ResetAvailableJump(ref availableJump, characterData.maxJumpCount);
            }

            UpdateMoveDirection();
        }

        private void MoveCharacter(Vector2 moveInput)
        {
            if (!playerBasicAttack.IsAttacking)
            {
                playerMovement.HorizontalMove(moveInput, characterData.moveXSpeed, true);
                UpdatePlayerMovementAnimation(moveInput);
            }
        }

        private void PlayRunningSound()
        {
            if (groundCheck.IsGrounded())
                audioManager.PlaySFXLoop("Running_Sound");
            else
            {
                audioManager.StopLoopingSFX();
            }
        }

        private void UpdatePlayerMovementAnimation(Vector2 moveInput)
        {
            // ���� ���� ���� �ڵ�� LoadJumpEvent() ���� Delegate�ν� �ε� ��
            if (!playerMovement.IsStunned && !playerBasicAttack.IsAttacking)
            {
                if (moveInput.x != 0)
                {
                    if (groundCheck.IsGrounded())
                        playerAnimation.SetAnimationState(PlayerAnimationState.Running);
                    else
                    {
                        if(0 <= availableJump)
                        {
                            playerAnimation.SetAnimationState(PlayerAnimationState.Jumping);
                            if (playerInputActions.Player.PlayerJump.triggered)
                            {
                                playerAnimation.RestartCurrentAnimation();
                            }
                        }
                    }
                }
                else
                {
                    if (groundCheck.IsGrounded())
                        playerAnimation.SetAnimationState(PlayerAnimationState.Idling);
                    else
                    {
                        if (0 <= availableJump)
                        {
                            playerAnimation.SetAnimationState(PlayerAnimationState.Jumping);
                            if (playerInputActions.Player.PlayerJump.triggered)
                            {
                                playerAnimation.RestartCurrentAnimation();
                            }
                        }
                    }
                }
            }
        }

        public void EnablePlayerInputAction()
        {
            playerInputActions.Enable();
        }

        public void DisablePlayerInputAction()
        {
            playerInputActions.Disable();
        }

        private void UnLoadJumpEvent()
        {
            if (playerInputActions != null)
            {
                playerInputActions.Player.PlayerJump.performed -= ctx
                => playerMovement.Jump(playerRigidBody2D, ctx.ReadValue<float>(), characterData.jumpHeight, ref availableJump);
                playerInputActions.Player.PlayerJump.canceled -= ctx
                    => playerMovement.Jump(playerRigidBody2D, ctx.ReadValue<float>(), characterData.jumpHeight, ref availableJump);

                playerInputActions.Player.PlayerJump.performed -= ctx
                    => playerAnimation.SetAnimationState(PlayerAnimationState.Jumping);
                playerInputActions.Player.PlayerJump.performed -= ctx => audioManager.PlaySFX("Jumping_Sound");
                playerInputActions.Player.PlayerJump.canceled -= ctx => audioManager.PlaySFX("Jumping_Sound");
            }
        }

        private void UnLoadPauseGameEvent()
        {
            playerInputActions.Player.PauseGame.performed -= ctx
                => pauseManager.DoPauseGame(ctx);
        }

        private void OnDestroy()
        {
            UnLoadJumpEvent();
            UnLoadPauseGameEvent();
        }

        private void UpdateMoveDirection()
        {
            moveDirection = inputManager.MoveJoystick.Direction;
        }
    }
}