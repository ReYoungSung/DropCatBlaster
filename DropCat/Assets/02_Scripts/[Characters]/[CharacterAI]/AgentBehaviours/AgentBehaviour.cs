using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterAI;

namespace CharacterBehaviour.Move
{
    public class AgentBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        public GameObject Target { get { return target; } set { target = value; } }

        public virtual void Awake()
        {
            target = GameObject.FindGameObjectWithTag("PLAYER_BaseHouse");
        }

        public virtual Steering GetSteering()
        {
            return new Steering();
        }
    }
}
