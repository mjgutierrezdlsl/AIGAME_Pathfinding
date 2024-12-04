using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding.AStar
{
    public class Pathfinding : MonoBehaviour
    {
        public Tile _startTile, _endTile;
        [SerializeField] Color _startColor = Color.cyan, _endColor = Color.blue;
        [SerializeField] Color OpenColor = Color.cyan, ClosedColor = Color.blue, PathColor = Color.magenta;
        Camera _camera;
        List<NodeBase> path;
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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                path = FindPath(_startTile.Node, _endTile.Node);
            }
        }
        public List<NodeBase> FindPath(NodeBase startNode, NodeBase targetNode)
        {
            var toSearch = new List<NodeBase>() { startNode };
            var processed = new List<NodeBase>();

            while (toSearch.Any())
            {
                var current = toSearch[0];
                foreach (var t in toSearch)
                    if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;

                processed.Add(current);
                toSearch.Remove(current);

                current.Tile.SetColor(ClosedColor);

                if (current == targetNode)
                {
                    var currentPathTile = targetNode;
                    var path = new List<NodeBase>();
                    var count = 100;
                    while (currentPathTile != startNode)
                    {
                        path.Add(currentPathTile);
                        currentPathTile = currentPathTile.Connection;
                        count--;
                        if (count < 0) throw new Exception();
                        Debug.Log("sdfsdf");
                    }

                    foreach (var node in path) node.Tile.SetColor(PathColor);
                    startNode.Tile.SetColor(PathColor);
                    Debug.Log(path.Count);
                    return path;
                }

                foreach (var neighbor in current.Neighbors.Where(t => t.Walkable && !processed.Contains(t)))
                {
                    var inSearch = toSearch.Contains(neighbor);

                    var costToNeighbor = current.G + current.GetDistance(neighbor);

                    if (!inSearch || costToNeighbor < neighbor.G)
                    {
                        neighbor.SetG(costToNeighbor);
                        neighbor.SetConnection(current);

                        if (!inSearch)
                        {
                            neighbor.SetH(neighbor.GetDistance(targetNode));
                            toSearch.Add(neighbor);
                            neighbor.Tile.SetColor(OpenColor);
                        }
                    }
                }
            }
            return null;
        }
    }
}
