using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Tiles;
using Tarodev_Pathfinding._Scripts.Grid.Scriptables;
using Tarodev_Pathfinding._Scripts.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tarodev_Pathfinding._Scripts.Grid {
    public class GridManager : MonoBehaviour {
        public static GridManager Instance;

        [SerializeField] private Sprite _playerSprite, _goalSprite;
        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private ScriptableGrid _scriptableGrid;
        [SerializeField] private bool _drawConnections;
        [SerializeField] private bool _followPath;

        public Dictionary<Vector2, NodeBase> Tiles { get; private set; }

        private Camera _camera;
        private NodeBase _playerNodeBase, _goalNodeBase;
        private Unit _spawnedPlayer, _spawnedGoal;

        private void Awake()
        {
            Instance = this;

            _camera = Camera.main;
        }

        private void Start() {
            Tiles = _scriptableGrid.GenerateGrid();
         
            foreach (var tile in Tiles.Values) tile.CacheNeighbors();

            SpawnUnits();
            NodeBase.OnHoverTile += OnTileHover;

            _camera.transform.position = GetGridCenter();
        }

        private Vector3 GetGridCenter() => new(_scriptableGrid.Width * 0.5f - 0.5f, _scriptableGrid.Height * 0.5f - 0.5f, _camera.transform.position.z);

        private void OnDestroy() => NodeBase.OnHoverTile -= OnTileHover;

        private void OnTileHover(NodeBase nodeBase) {
            _goalNodeBase = nodeBase;
            _spawnedGoal.transform.position = _goalNodeBase.Coords.Pos;

            foreach (var t in Tiles.Values) t.RevertTile();

            var path = Pathfinding.FindPath(_playerNodeBase, _goalNodeBase);
            if (_followPath)
            {
                StartCoroutine(FollowPath(path));
            }
        }

        private IEnumerator FollowPath(List<NodeBase> path)
        {
            for (int i = path.Count - 1; i >= 0; i--)
            {
                yield return new WaitForSeconds(1f);
                var nodePos = path[i].Coords.Pos;
                _spawnedPlayer.transform.position = nodePos;
            }
            _spawnedGoal.gameObject.SetActive(false);
        }

        void SpawnUnits() {
            _playerNodeBase = Tiles.Where(t => t.Value.Walkable).OrderBy(t => Random.value).First().Value;
            _spawnedPlayer = Instantiate(_unitPrefab, _playerNodeBase.Coords.Pos, Quaternion.identity);
            _spawnedPlayer.Init(_playerSprite);

            _spawnedGoal = Instantiate(_unitPrefab, new Vector3(50, 50, 50), Quaternion.identity);
            _spawnedGoal.Init(_goalSprite);
        }

        public NodeBase GetTileAtPosition(Vector2 pos) => Tiles.TryGetValue(pos, out var tile) ? tile : null;

        private void OnDrawGizmos() {
            if (!Application.isPlaying || !_drawConnections) return;
            Gizmos.color = Color.red;
            foreach (var tile in Tiles) {
                if (tile.Value.Connection == null) continue;
                Gizmos.DrawLine((Vector3)tile.Key + new Vector3(0, 0, -1), (Vector3)tile.Value.Connection.Coords.Pos + new Vector3(0, 0, -1));
            }
        }
    }
}