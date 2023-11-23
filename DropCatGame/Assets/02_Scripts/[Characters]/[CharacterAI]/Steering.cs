using System.Collections;
using UnityEngine;

namespace CharacterAI
{
    public class Steering
    {
        private float angular;
        public float GetAngular { get { return angular; } }
        public float SetAngular { set { angular = value; } }
        private Vector2 linear;
        public Vector2 GetLinear { get { return linear; } }
        public Vector2 SetLinear { set { linear = value; } }
    }
}