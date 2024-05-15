using System.Collections;
using System.Collections.Generic;
using CharacterBehaviour.Attack;
using UnityEngine;

namespace CharacterBehaviour.Move
{
    public class HumanoidMovement : CharacterMovement
    {
        private bool jumpIsTriggered = false;

        public virtual void Jump(Rigidbody2D movementRB2D, float jumpInput, float jumpHeight, ref int availableJump)
        {
            if (jumpInput > 0)
            {
                if(availableJump > 0)
                {
                    float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * movementRB2D.gravityScale));
                    movementRB2D.velocity = Vector2.zero;
                    if (jumpForce < 700f)
                        movementRB2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    jumpIsTriggered = true;
                }
                availableJump--;
            }
            else
                jumpIsTriggered = false;
        }

        public void ResetAvailableJump(ref int availableJump, int maxJumpCount)
        {
            if (!jumpIsTriggered) { availableJump = maxJumpCount; }
        }
    }
}

