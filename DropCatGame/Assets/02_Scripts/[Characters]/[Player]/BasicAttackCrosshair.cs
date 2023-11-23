using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackCrosshair : MonoBehaviour
{
    [SerializeField] private Animator crosshairAnimator;

    private void Start()
    {
        crosshairAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(crosshairAnimator != null)
        {
            crosshairAnimator.SetBool("crosshairOn", true);
            Debug.Log("playing anim...");
        }
    }
}
