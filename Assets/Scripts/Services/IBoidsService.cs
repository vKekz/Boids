using Boids;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// Service that manages the Boids.
    /// </summary>
    public interface IBoidsService
    {
        /// <summary>
        /// Generates a given amount of Boids.
        /// </summary>
        public void GenerateBoids(Boid boidsPrefab, Bounds bounds);

        /// <summary>
        /// Updates all Boids in the scene.
        /// </summary>
        public void UpdateBoids(Bounds bounds);
    }
}