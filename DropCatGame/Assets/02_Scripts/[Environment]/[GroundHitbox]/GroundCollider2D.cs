using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundCollider2D : MonoBehaviour
{
    public static GroundCollider2D instance = null;
    public Vector2 GroundColliderHalf { get; private set; }
    private float boundsLeft = 0f; private float boundsRight = 0f;
    public float BoundsLeft { get { return boundsLeft; } }
    public float BoundsRight { get { return boundsRight; } }
    private float offset = 5f;

    private void Awake()
    {
        instance = this.GetComponent<GroundCollider2D>();
        Collider2D groundCollider = this.GetComponent<Collider2D>();
        GroundColliderHalf = groundCollider.bounds.center + groundCollider.bounds.extents;
        boundsLeft = groundCollider.bounds.min.x + 43f;
        boundsRight= groundCollider.bounds.max.x - 43f;
    }
}
