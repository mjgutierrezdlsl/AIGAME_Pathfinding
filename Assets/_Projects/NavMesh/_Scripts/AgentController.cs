using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Pathfinding.NavMesh
{
    public class AgentController : MonoBehaviour
    {
        NavMeshAgent _agent;
        Camera _camera;
        [SerializeField] LayerMask _floorLayer = 1 << 7;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _camera = Camera.main;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseRay, out var hitInfo, Mathf.Infinity, _floorLayer))
                {
                    print(hitInfo.point);
                    _agent.SetDestination(hitInfo.point);
                }
            }
        }
    }
}
