using UnityEngine;

namespace CharacterAI
{
    public class BombCatDroneAgentManager : LuwakDroneAgentManager
    {
        [SerializeField] private LuwakDroneAttribute bombCatData;
        private Collider2D characterCollider;
        private float extentX; private float extentY;

        private void Awake()
        {
            characterCollider = this.GetComponent<Collider2D>();
            extentX = characterCollider.bounds.extents.x;
            extentY = characterCollider.bounds.extents.y;
            maxSpeed = bombCatData.maxSpeed;
            maxAccel = bombCatData.maxAccel;
        }

        private void Start()
        {
            velocity = Vector2.zero;
            steering = new Steering();
        }

        public override void SetSteering(Steering steering)
        {
            this.steering = steering;
        }
    }
}