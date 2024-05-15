using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterBehaviour.Move;

public class DroneBehaviour : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;
    [SerializeField] protected LuwakDroneAttribute luwakDroneAttribute;
    private CharacterMovement characterMovement = null;
    private BoxCollider2D boxCollider2D = null;
    [SerializeField] private float speedMultiply = 1;
    private bool isWithinRange = true;

    protected Vector2 Direction { get { return direction; } set { direction = value; } }
    protected CharacterMovement CharacterMovement { get { return characterMovement; } }
    protected BoxCollider2D BoxCollider2D { get { return boxCollider2D; } }
    protected float SpeedMultiply { get { return speedMultiply; } set { speedMultiply = value; } }
    protected bool IsWithInRange { get { return isWithinRange; } set { isWithinRange = value; } }

    public virtual void Awake()
    {
        characterMovement = this.GetComponent<CharacterMovement>();
        boxCollider2D = this.GetComponent<BoxCollider2D>();
    }

    public virtual void PatrolRoutine()
    {
        return;
    }
}
