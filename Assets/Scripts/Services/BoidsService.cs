using System.Collections.Generic;
using System.Linq;
using Boids;
using Settings;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Services
{
    /// <inheritdoc />
    public class BoidsService : IBoidsService
    {
        public const string ParentName = "BoidsParent";

        public BoidSettings BoidSettings;
        public List<Boid> Boids;
        
        private Random _random;

        public BoidsService(BoidSettings boidSettings)
        {
            BoidSettings = boidSettings;
            Boids = new List<Boid>();
            
            _random = new Random(1337);
        }
        
        /// <inheritdoc />
        public void GenerateBoids(Boid boidsPrefab, Bounds bounds)
        {
            var parent = new GameObject(ParentName).transform;
            
            for (var i = 0; i < BoidSettings.amountOfBoids; i++)
            {
                var posX = _random.NextFloat(bounds.min.x, bounds.max.x);
                var posY = _random.NextFloat(bounds.min.y, bounds.max.y);
                
                var position = new Vector2(posX, posY);
                var rotation = Quaternion.Euler(
                    boidsPrefab.transform.rotation.x,
                    boidsPrefab.transform.rotation.y,
                    _random.NextInt(0, 360));

                var boidObject = Object.Instantiate(boidsPrefab, position, rotation);
                boidObject.Initialize(BoidSettings);
                boidObject.transform.SetParent(parent);

                Boids.Add(boidObject);
            }
        }

        /// <inheritdoc />
        public void UpdateBoids(Bounds bounds)
        {
            foreach (var boid in Boids)
            {
                var nearbyBoids = Boids
                    .Where(b =>
                            b != boid &&
                            Vector3.Distance(b.transform.position, boid.transform.position) < BoidSettings.perceptionRadius)
                    .Select(b => b)
                    .ToList();

                boid.UpdateBoid(nearbyBoids, bounds);
            }
        }
    }
}