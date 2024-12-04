using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.AStar
{
    public class Pathfinding : MonoBehaviour
    {
        Tile _startTile, _endTile;
        [SerializeField] Color _startColor = Color.cyan, _endColor = Color.blue;
        Camera _camera;
        private void Awake()
        {
            _camera = Camera.main;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startTile?.ResetColor();
                var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                var mousePosInt = Vector2Int.RoundToInt(mousePos);
                var tile = GridManager.Instance.GetTile(mousePosInt);
                _startTile = tile;
                tile.SetColor(_startColor);
            }

            if (Input.GetMouseButtonDown(1))
            {
                _endTile?.ResetColor();
                var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                var mousePosInt = Vector2Int.RoundToInt(mousePos);
                var tile = GridManager.Instance.GetTile(mousePosInt);
                _endTile = tile;
                tile.SetColor(_endColor);
            }
        }
    }
}
