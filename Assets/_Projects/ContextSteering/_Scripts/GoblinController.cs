using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.ContextSteering
{
    public class GoblinController : MonoBehaviour
    {
        [Header("Context Steering")]
        [SerializeField] LayerMask _obstacleLayer = 1 << 6;
        [SerializeField] float _detectionRadius = 3f;
        [SerializeField] Vector2 _targetPosition;


        [Header("Movement")]
        [SerializeField] float _moveSpeed = 2f;
        [SerializeField] float _stoppingDistance = 0.3f;
        public Vector2 _moveDirection;

        Camera _camera;
        Rigidbody2D _rigidbody;
        Animator _animator;
        RaycastHit2D _hitInfo;

        private void Awake()
        {
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                _targetPosition = mousePos;
            }

            if (Input.GetMouseButton(1))
            {
                _targetPosition = transform.position;
            }

            if (Vector3.Distance(transform.position, _targetPosition) <= _stoppingDistance) { return; }
            _moveDirection = (_targetPosition - (Vector2)transform.position).normalized;
            _hitInfo = Physics2D.CircleCast(transform.position, 0.3f, _moveDirection, _detectionRadius, _obstacleLayer);
            Debug.DrawRay(transform.position, _moveDirection * _detectionRadius, _hitInfo ? Color.red : Color.green);
            if (_hitInfo)
            {
                // calculate new direction
                Debug.DrawLine(_hitInfo.point, _hitInfo.normal, _hitInfo ? Color.magenta : Color.clear);
                Debug.DrawLine(transform.position, _hitInfo.normal, _hitInfo ? Color.yellow : Color.clear);
                _moveDirection = (_hitInfo.normal - (Vector2)transform.position).normalized;
            }
        }
        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, _targetPosition) <= _stoppingDistance) { return; }
            _rigidbody.MovePosition(_rigidbody.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
            // Gizmos.color = Color.white;
            // foreach (var direction in Directions.Octal)
            // {
            //     Gizmos.DrawLine(transform.position, direction);
            // }
            if (_hitInfo)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_hitInfo.point, 0.3f);
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