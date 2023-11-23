using UnityEngine;

public enum VFXAnimationState
{
    None, Teleporting
}

public class VFXGenerator : MonoBehaviour
{
    private Animator vfxAnimator;
    private VFXAnimationState currentAnimationState = VFXAnimationState.None;
    private string currentAnimation = null;
    public string GetCurrentVFXAnimation { get { return currentAnimation; } }

    private void Awake()
    {
        vfxAnimator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (vfxAnimator != null)
        {
            PlayAnimation();
        }
    }

    private void PlayAnimation()
    {
        switch (currentAnimationState)
        {
            case VFXAnimationState.None:
                break;
            case VFXAnimationState.Teleporting:
                currentAnimation = "TELEPORTING";
                break;
            default:
                break;
        }
        vfxAnimator.Play(currentAnimation);
    }

    public void RestartCurrentVFXAnimation()
    {
        this.transform.position = this.transform.parent.position;
        vfxAnimator.Play(currentAnimation, -1, 0);
    }

    public void SetVFXAnimationState(VFXAnimationState state)
    {
        this.transform.position = this.transform.parent.position;
        currentAnimationState = state;
    }
}
