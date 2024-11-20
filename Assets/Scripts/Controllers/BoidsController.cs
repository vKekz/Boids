using Boids;
using Services;
using Settings;
using UnityEngine;

namespace Controllers
{
    public class BoidsController : MonoBehaviour
    {
        public Transform target;
        public Boid boidsPrefab;
        public BoxCollider2D border;
        public BoidSettings boidSettings;

        private IBoidsService _boidsService;

        private void Awake()
        {
            boidSettings.target = target;
            _boidsService ??= new BoidsService(boidSettings);
        }

        private void Start()
        {
            _boidsService.GenerateBoids(boidsPrefab, border.bounds);
        }

        private void Update()
        {
            _boidsService.UpdateBoids(border.bounds);
        }
    }
}