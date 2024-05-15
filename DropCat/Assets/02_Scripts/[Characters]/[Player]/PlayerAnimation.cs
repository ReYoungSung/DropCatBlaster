using System;
using System.Collections;
using UnityEngine;

public enum PlayerAnimationState
{
    Idling, Running, Jumping, Attacking_Horizontal, Attacking_Vertical_UP, Attacking_Vertical_DOWN, Stunned, Falling
}

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerAnimationState currentAnimationState = PlayerAnimationState.Idling;
    private string currentAnimation = null;

    public Animator Animator { get { return animator; } }
    public PlayerAnimationState CurrentAnimationState { get { return currentAnimationState; } }

    private void Awake()
    {
        InitializeCachingReference();
    }

    private void InitializeCachingReference()
    {
        animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateRoutine();
    }

    private void UpdateRoutine()
    {
        if (animator != null)
        {
            PlayAnimation();
        }
    }

    private void PlayAnimation()
    {
        switch(currentAnimationState)
        {
            case PlayerAnimationState.Idling:
                currentAnimation = "IDLING";
                break;
            case PlayerAnimationState.Running:
                currentAnimation = "RUNNING";
                break;
            case PlayerAnimationState.Jumping:
                currentAnimation = "JUMPING";
                break;
            case PlayerAnimationState.Attacking_Horizontal:
                currentAnimation = "ATTACKING_HORIZONTAL";
                break;
            case PlayerAnimationState.Attacking_Vertical_UP:
                currentAnimation = "ATTACKING_VERTICAL_UP";
                break;
            case PlayerAnimationState.Attacking_Vertical_DOWN:
                currentAnimation = "ATTACKING_VERTICAL_DOWN";
                break;
            case PlayerAnimationState.Stunned:
                currentAnimation = "STUNNED";
                break;
            case PlayerAnimationState.Falling:
                currentAnimation = "FALLING";
                break;
            default:
                currentAnimation = "AnimationState-Null";
                break;
        }
        animator.Play(currentAnimation);
    }

    public void RestartCurrentAnimation()
    {
        animator.Play(currentAnimation, -1, 0);
    }

    public void SetAnimationState(PlayerAnimationState state)
    {
        currentAnimationState = state;
    }

    public IEnumerator CheckAnimationCompleted(string CurrentAnim, Action Oncomplete, float duration)
    {
        while (!Animator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnim))
            yield return null;
        if (Oncomplete != null)
        {
            Oncomplete();
        }
        yield return new WaitForSeconds(duration);
    }

    public bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
