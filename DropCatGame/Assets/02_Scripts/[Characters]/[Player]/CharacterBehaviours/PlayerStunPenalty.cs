using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterBehaviour.Move;
using CharacterBehaviour.Attack;
using CharacterBehaviour.Enemy.CatHouse;
using UnityEngine.Events;

public class PlayerStunPenalty : MonoBehaviour
{
    private PlayerBasicAttack playerBasicAttack;
    private PlayerMovement playerMovement;
    private BoxCollider2D playerCollider = null;
    private Rigidbody2D playerRigidBody = null;
    private PlayerAnimation playerAnimation = null;
    private InputManager inputManager = null;
    private bool stunDropStop = false;
    private float stunDropVelocity = 10f; 
    private Coroutine fixDropVelocityCoroutine = null;
    
    private void Awake()
    {
        InitializeCachingReference();
    } 

    private void InitializeCachingReference()
    {
        playerBasicAttack = GameObject.Find("PlayerAttackRayOrigin").GetComponent<PlayerBasicAttack>();
        playerRigidBody = this.GetComponent<Rigidbody2D>();
        playerMovement = this.GetComponent<PlayerMovement>();
        playerCollider = this.GetComponent<BoxCollider2D>();
        playerAnimation = this.GetComponent<PlayerAnimation>();
        inputManager = GameObject.Find("[InputManager]").GetComponent<InputManager>();
    }

    private void Update()
    { 
        StunPlayer(); 
    } 

    private void StunPlayer()
    { 
        if (playerMovement.IsStunned && !playerMovement.StunAwaiting)
        { 
            playerMovement.MoveInputConnection = 0f; 
            inputManager.PlayerInputActions.Player.PlayerJump.Disable();
            playerBasicAttack.enabled = false;
            if(fixDropVelocityCoroutine == null)
            {
                fixDropVelocityCoroutine = StartCoroutine(FixDropVelocity()); 
            }
            StunAnimation(); 
        } 
        else 
        { 
            playerMovement.MoveInputConnection = 1f;
            inputManager.PlayerInputActions.Player.PlayerJump.Enable();
            playerBasicAttack.enabled = true;
        } 

        if (stunDropStop == true) 
        { 
            playerRigidBody.velocity = new Vector2(0,stunDropVelocity); 
        } 
        else  
        { 
            playerRigidBody.velocity = new Vector2(0,playerRigidBody.velocity.y); 
        } 
    } 

    private IEnumerator FixDropVelocity()
    { 
        stunDropStop = true; 
        
        yield return new WaitForSecondsRealtime(0.5f);
      
        stunDropStop = false; 
        for(int i = -50; i >= -500; i -= 50) 
        {
            playerRigidBody.velocity = new Vector2(0,i); 
            yield return new WaitForSecondsRealtime(0.05f);
        }
        yield return new WaitForSecondsRealtime(0.51f);
        playerRigidBody.velocity = new Vector2(0,playerRigidBody.velocity.y);
        fixDropVelocityCoroutine = null;
    } 

    private void StunAnimation()
    {
        if (!playerAnimation.Animator.GetCurrentAnimatorStateInfo(0).IsName("STUNNED"))
        {
            playerAnimation.SetAnimationState(PlayerAnimationState.Stunned);
            if (playerAnimation.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                if (playerAnimation.CurrentAnimationState != PlayerAnimationState.Falling)
                {
                    playerAnimation.SetAnimationState(PlayerAnimationState.Falling);
                }
            }
        }
    }

    public bool CatHouseIsFalling(GameObject catHouseObj)
    {
        CatHouseBehaviour catHouseBehav = catHouseObj.GetComponent<CatHouseBehaviour>();
        if (catHouseBehav != null)
        {
            return catHouseBehav.GetComponent<CatHouseBehaviour>().IsFalling;
        }
        return false;
    }

    public bool PlayerCollidedBottom(Collider2D enemyCollider)
    {
        float playerTopY = transform.position.y + playerCollider.bounds.extents.y;
        float enemyBottomY = enemyCollider.transform.position.y - enemyCollider.bounds.extents.y;

        bool playerInBound = 
            transform.position.x > enemyCollider.bounds.min.x &&
            transform.position.x < enemyCollider.bounds.max.x;
        
        return playerTopY >= enemyBottomY && playerInBound;
    }
}
