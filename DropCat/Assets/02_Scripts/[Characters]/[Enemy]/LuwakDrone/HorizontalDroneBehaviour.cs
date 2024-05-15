using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalDroneBehaviour : DroneBehaviour
{
    [SerializeField] private float minimumX, maximumX;
    private float boundsLeft = 0;
    private float boundsRight = 0;
    protected float BoundsLeft { get { return boundsLeft; } }
    protected float BoundsRight { get { return boundsRight; } }

    public override void Awake()
    {
        base.Awake();
        GroundCollider2D groundCollider2D = GameObject.Find("[GroundCollider2D]").GetComponent<GroundCollider2D>();
        boundsLeft = groundCollider2D.BoundsLeft;
        boundsRight = groundCollider2D.BoundsRight;
        Direction = Vector2.right;
    }

    private void Update()
    {
        PatrolRoutine();
    }

    public override void PatrolRoutine()
    {
        SetPatrolDirection();
        CharacterMovement.HorizontalMove(Direction, luwakDroneAttribute.maxSpeed * SpeedMultiply, false);
    }

    public virtual void SetPatrolDirection()
    {
        if (HasReachedBoundRight())
        {
            if (Direction == Vector2.right)
            {
                Direction = Vector2.left;
            }
        }
        else if(HasReachedBoundLeft())
        {
            if(Direction == Vector2.left)
            {
                Direction = Vector2.right;
            }
        }
    }

    private bool HasReachedBoundLeft()
    {
        if (BoxCollider2D.bounds.min.x < minimumX)
        {
            return true;
        }
        return false;
    }

    private bool HasReachedBoundRight()
    {
        if (maximumX < BoxCollider2D.bounds.max.x)
        {
            return true;
        }
        return false;
    }
}
