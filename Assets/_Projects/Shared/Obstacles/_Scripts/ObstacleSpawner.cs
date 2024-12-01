using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] Obstacle _obstaclePrefab;
    [SerializeField] float _spawnRadius = 3f;
    [SerializeField] int _spawnCount = 10;
    private void Start()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            var obstacle = Instantiate(_obstaclePrefab, transform);
            obstacle.transform.position = transform.position + (Vector3)Random.insideUnitCircle * _spawnRadius;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
}
