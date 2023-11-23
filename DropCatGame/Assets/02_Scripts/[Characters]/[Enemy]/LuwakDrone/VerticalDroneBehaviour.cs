using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDroneBehaviour : DroneBehaviour
{
    [SerializeField] private float minimumY, maximumY;

    public override void Awake()
    {
        base.Awake();
        Direction = Vector2.up;
    }

    private void Update()
    {
        PatrolRoutine();
    }

    public override void PatrolRoutine()
    {
        SetPatrolDirection();
        CharacterMovement.VerticalMove(Direction, luwakDroneAttribute.maxSpeed * SpeedMultiply, false);
    }
    public virtual void SetPatrolDirection()
    {
        if (HasReachedBoundUp())
        {
            if (Direction == Vector2.up)
            {
                Direction = Vector2.down;
            }
        }
        else if (HasReachedBoundDown())
        {
            if (Direction == Vector2.down)
            {
                Direction = Vector2.up;
            }
        }
    }

    private bool HasReachedBoundUp()
    {
        if (maximumY < BoxCollider2D.bounds.max.y)
        {
            return true;
        }
        return false;
    }

    private bool HasReachedBoundDown()
    {
        if (BoxCollider2D.bounds.min.y < minimumY)
        {
            return true;
        }
        return false;
    }
}
