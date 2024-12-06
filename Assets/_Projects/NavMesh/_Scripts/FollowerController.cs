using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Pathfinding.NavMesh
{
    public class FollowerController : MonoBehaviour
    {
        [SerializeField] Transform _targetTransform;
        NavMeshAgent _agent;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        private void Update()
        {
            _agent.SetDestination(_targetTransform.position);

        }
    }
}
