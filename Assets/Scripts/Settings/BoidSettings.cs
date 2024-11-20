using UnityEngine;

namespace Settings
{
    [CreateAssetMenu]
    public class BoidSettings : ScriptableObject
    {
        public int amountOfBoids = 10;

        public float minSpeed = 2f;
        public float maxSpeed = 6f;
        public float maxSteeringForce = 3f;

        public float perceptionRadius = 2.5f;
        
        public float separationWeight = 1;
        public float alignmentWeight = 1;
        public float cohesionWeight = 1;

        public bool renderSeparation;
        public bool renderAlignment;

        public LayerMask obstacleMask;
    }
}