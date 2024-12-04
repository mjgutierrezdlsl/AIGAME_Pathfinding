using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.AStar
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] int _width, _height;
        [SerializeField] Tile _tilePrefab;
        public static GridManager Instance;
        Camera _camera;
        private Dictionary<Vector2, Tile> _tiles;
        private void Awake()
        {
            _camera = Camera.main;
            Instance = FindObjectOfType<GridManager>();

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        private void Start()
        {
            GenerateGrid();
        }
        void GenerateGrid()
        {
            _tiles = new();
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var tile = Instantiate(_tilePrefab, new(x, y), Quaternion.identity, transform);
                    tile.name = $"Tile {x} {y}";
                    var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                    tile.Init(isOffset);
                    _tiles[new(x, y)] = tile;
                }
            }
            _camera.transform.position = new((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        }
        public Tile GetTile(Vector2 pos)
        {
            return _tiles.TryGetValue(pos, out var tile) ? tile : null;

        }
    }
}
