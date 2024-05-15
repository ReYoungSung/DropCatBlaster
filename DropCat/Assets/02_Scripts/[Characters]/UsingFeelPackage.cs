using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using CharacterBehaviour.Move;
using UnityEngine.Events;

public class UsingFeelPackage : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public MMF_Player stunFeedbacks;

    private Rigidbody2D _rigidbody;
    private float _velocityLastFrame;
    
    private void Awake()
    {
        _rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        playerMovement = this.gameObject.GetComponent<PlayerMovement>();
        stunFeedbacks = this.transform.GetChild(2).GetComponent<MMF_Player>();
    }

    private void Update()
    {
        if (playerMovement.IsStunned)
        {
            StunFeedB();
        }
    }

    private void StunFeedB()
    {
        if (stunFeedbacks != null)
        {
            stunFeedbacks.PlayFeedbacks();
        }
    }
}

