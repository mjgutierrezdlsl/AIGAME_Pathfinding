using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding.AStar
{
    [Serializable]
    public class NodeBase
    {
        public Tile Tile { get; private set; }
        private static readonly List<Vector2> Dirs = new()
        {
            new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1)
        };
        public NodeBase Connection { get; private set; }
        [field: SerializeField] public float G { get; private set; }
        [field: SerializeField] public float H { get; private set; }
        public float F => G + H;
        public bool Walkable { get; private set; }
        public List<NodeBase> Neighbors { get; protected set; }
        [field: SerializeField] public Vector2 Pos { get; set; }

        public void SetConnection(NodeBase nodeBase) => Connection = nodeBase;
        public void SetG(float g) => G = g;
        public void SetH(float h) => H = h;
        public void SetWalkable(bool walkable) => Walkable = walkable;
        public float GetDistance(NodeBase node)
        {
            var dist = new Vector2Int(Mathf.Abs((int)Pos.x - (int)node.Pos.x), Mathf.Abs((int)Pos.y - (int)node.Pos.y));

            var lowest = Mathf.Min(dist.x, dist.y);
            var highest = Mathf.Max(dist.x, dist.y);

            var horizontalMovesRequired = highest - lowest;

            return lowest * 14 + horizontalMovesRequired * 10;
        }
        public void CacheNeighbors()
        {
            Neighbors = new();
            foreach (var tile in Dirs.Select(dir => GridManager.Instance.GetTile(Pos + dir)).Where(tile => tile != null))
            {
                Neighbors.Add(tile.Node);
            }
        }
        public NodeBase(Tile tile)
        {
            Tile = tile;
            Pos = tile.transform.position;
            Walkable = true;
        }
    }
}
