using Boids;
using Services;
using Settings;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Controllers
{
    public class BoidsController : MonoBehaviour
    {
        public Boid boidsPrefab;
        public BoxCollider2D border;
        public BoidSettings boidSettings;

        private IBoidsService _boidsService;
        private Boid _selectedBoid;

        private void Awake()
        {
            _boidsService ??= new BoidsService(boidSettings);
        }

        private void Start()
        {
            _boidsService.GenerateBoids(boidsPrefab, border.bounds);
        }

        private void Update()
        {
            _boidsService.UpdateBoids(border.bounds);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_selectedBoid)
                {
                    _selectedBoid.GetComponent<SpriteRenderer>().material.color = Color.white;
                }

                _selectedBoid = _boidsService.Boids[new Random().Next(_boidsService.Boids.Count)];
                _selectedBoid.GetComponent<SpriteRenderer>().material.color = Color.red;
            }
        }

        private void OnDrawGizmos()
        {
            if (!_selectedBoid)
            {
                return;
            }

            if (boidSettings.renderSeparation)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_selectedBoid.transform.position, _selectedBoid.transform.position + _selectedBoid.previousSeparation);
            }

            Handles.color = Color.cyan;
            Handles.DrawWireDisc(_selectedBoid.transform.position, Vector3.forward, boidSettings.perceptionRadius);

            if (boidSettings.renderAlignment)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_selectedBoid.transform.position, _selectedBoid.transform.position + _selectedBoid.previousAlignment);
            }
        }
    }
}