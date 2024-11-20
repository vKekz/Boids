using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Boids
{
    public class Boid : MonoBehaviour
    {
        public GameObject boidPrefab;
        
        [HideInInspector]
        public Vector3 velocity;

        private BoidSettings _boidSettings;
        
        public void Initialize(BoidSettings settings)
        {
            _boidSettings = settings;

            var initialSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
            velocity = transform.up * initialSpeed;
        }

        public void UpdateBoid(List<Boid> nearbyBoids, Bounds bounds)
        {
            var acceleration = CalculateAcceleration(nearbyBoids);
            
            // Apply acceleration
            velocity += acceleration * Time.deltaTime;

            var speed = Mathf.Clamp(velocity.magnitude, _boidSettings.minSpeed, _boidSettings.maxSpeed);
            var direction = velocity / speed;
            velocity = direction * speed;

            // We're only moving in 2-dimensions
            velocity.z = 0f;

            transform.position += velocity * Time.deltaTime;
            transform.up = direction;

            HandleBorderBounds(bounds);
        }

        private Vector3 CalculateAcceleration(List<Boid> nearbyBoids)
        {
            var acceleration = Vector3.zero;
            
            // Target following
            if (_boidSettings.target)
            {
                var offsetToTarget = _boidSettings.target.transform.position - transform.position;
                acceleration = SteerTowards(offsetToTarget) * _boidSettings.targetWeight;
            }

            // acceleration += CalculateObstacleAvoidance();
            
            var amountOfNearbyBoids = nearbyBoids.Count;
            if (amountOfNearbyBoids <= 0)
            {
                return acceleration;
            }

            var averageAlignment = Vector3.zero;
            var averageCohesion = Vector3.zero;
            var averageSeparation = Vector3.zero;
                
            foreach (var nearbyBoid in nearbyBoids)
            {
                var distanceToNearbyBoid = Vector3.Distance(transform.position, nearbyBoid.transform.position);
                var positionDifference = transform.position - nearbyBoid.transform.position;
                positionDifference /= distanceToNearbyBoid;
                averageSeparation += positionDifference;

                averageAlignment += nearbyBoid.velocity;
                averageCohesion += nearbyBoid.transform.position;
            }

            averageAlignment /= amountOfNearbyBoids;
            averageCohesion /= amountOfNearbyBoids;
            averageSeparation /= amountOfNearbyBoids;

            // Rule of separation
            var separationForce = SteerTowards(averageSeparation) * _boidSettings.separationWeight;
                
            // Rule of alignment
            var alignmentForce = SteerTowards(averageAlignment) * _boidSettings.alignmentWeight;
                
            // Rule of cohesion
            var cohesionForce = SteerTowards(averageCohesion - transform.position) * _boidSettings.cohesionWeight;

            acceleration += separationForce;
            acceleration += alignmentForce;
            acceleration += cohesionForce;

            return acceleration;
        }

        private Vector3 CalculateObstacleAvoidance()
        {
            var acceleration = Vector3.zero;
            var colliders = Physics.OverlapSphere(transform.position, 3f);

            foreach (var hitCollider in colliders)
            {
                var positionDifference = hitCollider.transform.position - transform.position;
                acceleration -= positionDifference.normalized / positionDifference.sqrMagnitude;
            }

            return acceleration.normalized;
        }

        private void HandleBorderBounds(Bounds bounds)
        {
            var posX = transform.position.x;
            if (posX < bounds.min.x)
            {
                posX = bounds.max.x;
            }

            if (posX > bounds.max.x)
            {
                posX = bounds.min.x;
            }
            
            var posY = transform.position.y;
            if (posY < bounds.min.y)
            {
                posY = bounds.max.y;
            }

            if (posY > bounds.max.y)
            {
                posY = bounds.min.y;
            }

            transform.position = new Vector3(posX, posY, transform.position.z);
        }
        
        private Vector3 SteerTowards(Vector3 target)
        {
            var vector = target.normalized * _boidSettings.maxSpeed - velocity;
            return Vector3.ClampMagnitude(vector, _boidSettings.maxSteeringForce);
        }
    }
}
