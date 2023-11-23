using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterAI;

namespace CharacterBehaviour.Move
{
    public class HumanoidSeek : AgentBehaviour
    {
        private HumanoidMovement humanoidMovement = null;
        private AgentManager agentManager;
        private GroundCheck groundCheck = null;

        public override void Awake()
        {
            base.Awake();
            humanoidMovement = this.GetComponent<HumanoidMovement>();
            agentManager = this.GetComponent<AgentManager>();
            groundCheck = this.GetComponent<GroundCheck>();
        }

        private void Update()
        {
            if (this.GetSteering() != null)
            {
                humanoidMovement.FlipCharacter(-GetSteering().GetLinear.x);
                agentManager.SetSteering(GetSteering());
            }
        }

        public override Steering GetSteering()
        {
            if (Target != null)
            {
                Steering steering = new Steering();
                if(groundCheck.IsGrounded())
                {
                    steering.SetLinear = Target.transform.position - this.transform.position;
                    steering.GetLinear.Normalize();
                    steering.SetLinear = new Vector2(steering.GetLinear.x * agentManager.MaxAccel, 0f);
                }
                else
                {
                    steering.SetLinear = Vector2.zero;
                }
                return steering;
            }
            return null;
        }
    }
}

