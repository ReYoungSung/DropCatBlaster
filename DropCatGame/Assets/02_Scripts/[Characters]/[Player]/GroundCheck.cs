using UnityEngine;
using CharacterManager;
using CharacterBehaviour.Attack;

namespace CharacterBehaviour.Move 
{
    public class GroundCheck : MonoBehaviour
    {
        private float distanceToGround;
        [SerializeField] private float maxDistance = 100.0f;
        [SerializeField] private LayerMask groundLayer;
        private PlayerBasicAttack playerBasicAttack;
        float colliderExtent = 0f;

        void Awake()
        {
            colliderExtent = this.GetComponent<BoxCollider2D>().bounds.extents.x;

        }

        public float GetDistanceToGround()
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(this.transform.position - (Vector3.left * colliderExtent), Vector2.down, maxDistance, groundLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(this.transform.position - (Vector3.right * colliderExtent), Vector2.down, maxDistance, groundLayer);
            if (hitLeft)
            {
                distanceToGround = hitLeft.distance - this.transform.localScale.y / 2f;
                return distanceToGround;
            }
            else if(hitRight)
            {
                distanceToGround = hitRight.distance - this.transform.localScale.y / 2f;
                return distanceToGround;
            }
            else
            {
                return maxDistance;
            }
        }

        public bool IsGrounded()
        {
            if(GetDistanceToGround() > 0)
                return false;
            return true;
        }
    }
}
