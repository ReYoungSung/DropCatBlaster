using System.Collections;
using UnityEngine;

namespace CharacterBehaviour.Move
{
    public class CharacterMovement : MonoBehaviour
    {
        private Collider2D characterCollider2D = null;
        private float characterHalfX = 0f;
        private float moveInputConnection = 1.0f;
        public float MoveInputConnection { get { return moveInputConnection; } set { moveInputConnection = value; } }

        #region SkillArea_Postponed Usage - Teleport
        //private float groundY = -160f;
        //private float teleportDownDistance = 60f;
        #endregion

        private void Awake()
        {
            InitializeGroundObjBounds();
        }

        private void InitializeGroundObjBounds()
        {
            characterCollider2D = this.GetComponent<Collider2D>();
            characterHalfX = characterCollider2D.bounds.extents.x;
        }

        #region GameLooping Phase
        private void Update()
        {
            ConstrainPosition(GroundCollider2D.instance.BoundsLeft, GroundCollider2D.instance.BoundsRight);
        }

        public void ConstrainPosition(float boundsLeft, float boundsRight)
        {
            float positionX = this.transform.position.x;
            if (positionX >= boundsRight)
            {
                this.transform.position = new Vector2(boundsRight - characterHalfX, transform.position.y);
            }
            if (positionX <= boundsLeft)
            {
                this.transform.position = new Vector2(boundsLeft + characterHalfX, transform.position.y);
            }
        }

        public virtual void HorizontalMove(Vector2 moveInput, float speed, bool flip)
        {
            float velX = moveInput.x * speed * moveInputConnection; 
            transform.Translate(Vector2.right * velX * Time.deltaTime );
            if(flip)
                FlipCharacter(moveInput);
        }

        public virtual void HorizontalMove(Vector2 moveInput, float speed)
        {
            float velX = moveInput.x * speed;
            transform.Translate(Vector2.right * velX * Time.deltaTime);
        }

        public virtual void VerticalMove(Vector2 moveInput, float speed, bool flip)
        {
            transform.Translate(moveInput * speed * Time.deltaTime);
            if (flip)
                FlipCharacter(moveInput);
        }

        public void FlipCharacter(Vector2 velocity)
        {
            if (velocity.x > 0.01)
            {
                if (this.transform.localScale.x == -1)
                {
                    this.transform.localScale = new Vector3(
                        1f,
                        this.transform.localScale.y,
                        this.transform.localScale.z
                        );
                }
            }
            else if (velocity.x < -0.01)
            {
                if (this.transform.localScale.x == 1)
                {
                    this.transform.localScale = new Vector3(
                        -1,
                        this.transform.localScale.y,
                        this.transform.localScale.z
                        );
                }
            }
        }

        public void FlipCharacter(float aimDirection)
        {
            if (aimDirection > 0.001)
            {
                if (this.transform.localScale.x == -1)
                {
                    this.transform.localScale = new Vector3(
                        1f,
                        this.transform.localScale.y,
                        this.transform.localScale.z
                        );
                }
            }
            else if (aimDirection < -0.001)
            {
                if (this.transform.localScale.x == 1)
                {
                    this.transform.localScale = new Vector3(
                        -1,
                        this.transform.localScale.y,
                        this.transform.localScale.z
                        );
                }
            }
        }
        #endregion
    }
}

