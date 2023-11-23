using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterAI;

namespace CharacterBehaviour.Move
{
    public class FloatingSeek : AgentBehaviour
    {
        private CharacterMovement characterMovement = null;
        private LuwakDroneAgentManager luwakDroneAgentManager = null;

        public override void Awake()
        {
            Target = GameObject.FindGameObjectWithTag("PLAYER_Character");
            characterMovement = this.GetComponent<CharacterMovement>();
            luwakDroneAgentManager = this.GetComponent<LuwakDroneAgentManager>();
        }

        private void Update()
        {
            if (GetSteering() != null)
            {
                luwakDroneAgentManager.SetSteering(GetSteering());
                characterMovement.FlipCharacter(-GetSteering().GetLinear.x);
            }
        }

        public override Steering GetSteering()
        {
            if (Target != null)
            {
                Steering steering = new Steering();
                steering.SetLinear = Target.transform.position - this.transform.position;
                steering.GetLinear.Normalize();
                steering.SetLinear = steering.GetLinear * luwakDroneAgentManager.MaxAccel;
                return steering;
            }
            return null;
        }
    }
}