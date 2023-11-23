using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoppableObject : MonoBehaviour
{
    private GameObject player;
    private BoxCollider2D playerBoxCollider;
    private BoxCollider2D thisCollider;

    private float upperSurface = 0f;
    private float playerBottom = 0f;
    private float playerCeiling = 0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER_Character");
        thisCollider = this.GetComponent<BoxCollider2D>();
        playerBoxCollider = player.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        playerCeiling =  playerBoxCollider.bounds.center.y + playerBoxCollider.bounds.extents.y;
        playerBottom = player.transform.position.y;
        upperSurface = thisCollider.bounds.max.y;
    }

    private void Update()
    {
        ActivateColliders();
    }

    protected void ActivateColliders()
    {
        if (PlayerIsAboveTheCollider())
        {
            thisCollider.isTrigger = false;
        }
        else if (PlayerIsBelowTheCollider())
        {
            thisCollider.isTrigger = true;
        }
        else
        {
            return;
        }
    }

    private bool PlayerIsBelowTheCollider()
    {
        return playerCeiling < upperSurface;
    }

    private bool PlayerIsAboveTheCollider()
    {
        return playerBottom > upperSurface;
    }

    private void OnDisable()
    {
        thisCollider.isTrigger = false;
    }
}
