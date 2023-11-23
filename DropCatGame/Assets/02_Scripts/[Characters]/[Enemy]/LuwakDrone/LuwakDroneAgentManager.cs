using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace CharacterAI
{
    public class LuwakDroneAgentManager : AgentManager
    {
        [SerializeField] private LuwakDroneAttribute luwakDroneData;

        private void Awake()
        {
            maxSpeed = luwakDroneData.maxSpeed;
            maxAccel = luwakDroneData.maxAccel;
        }

        private void Start()
        {
            velocity = Vector2.zero;
        }

        public override void SetSteering(Steering steering)
        {
            this.steering = steering;
        }
    }
}
