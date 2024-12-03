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
        bool _isFacingLeft;

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

            _animator.SetBool("isMoving", _moveDirection != Vector2.zero);

            if (Vector3.Distance(transform.position, _targetPosition) <= _stoppingDistance)
            {
                _moveDirection = Vector2.zero;
                return;
            }
            _moveDirection = (_targetPosition - (Vector2)transform.position).normalized;
            _isFacingLeft = _moveDirection.x switch
            {
                < 0 => true,
                > 0 => false,
                _ => _isFacingLeft,
            };
            transform.rotation = Quaternion.Euler(new(0, _isFacingLeft ? 180f : 0, 0));
            _hitInfo = Physics2D.CircleCast(transform.position, 0.3f, _moveDirection, _detectionRadius, _obstacleLayer);
            Debug.DrawRay(transform.position, _moveDirection * _detectionRadius, _hitInfo ? Color.red : Color.green);
            var dot = Vector2.Dot(transform.right, _moveDirection);
            Debug.DrawLine(transform.position, transform.position + transform.right, Color.cyan);
            print($"Dot: {dot}");
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
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawWireSphere(transform.position, _detectionRadius);
            // Gizmos.color = Color.white;
            // foreach (var direction in Directions.Octal)
            // {
            //     Gizmos.DrawLine(transform.position, direction);
            // }
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + transform.right);
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