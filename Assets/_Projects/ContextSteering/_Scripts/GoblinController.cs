using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.ContextSteering
{
    public class GoblinController : MonoBehaviour
    {
        [Header("Context Steering")] [SerializeField]
        LayerMask _obstacleLayer = 1 << 6;

        [SerializeField] float _detectionRadius = 3f;
        [SerializeField] Vector2 _targetPosition;
        Camera _camera;

        [Header("Movement")] [SerializeField] float _moveSpeed = 2f;
        [SerializeField] float _stoppingDistance = 0.3f;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                _targetPosition = mousePos;
            }

            if (Input.GetMouseButton(1))
            {
                _targetPosition = transform.position;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
            Gizmos.color = Color.white;
            foreach (var direction in Directions.Octal)
            {
                Gizmos.DrawLine(transform.position, direction);
            }

            if (Vector3.Distance(transform.position,_targetPosition)<=Mathf.Epsilon) return;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_targetPosition, 0.3f);
        }
    }

    public static class Directions
    {
        public static Vector2[] Octal =
        {
            new Vector2(0, 1).normalized,
            new Vector2(1, 1).normalized,
            new Vector2(1, 0).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(0, -1).normalized,
            new Vector2(-1, -1).normalized,
            new Vector2(-1, 0).normalized,
            new Vector2(-1, 1).normalized
        };
    }
}