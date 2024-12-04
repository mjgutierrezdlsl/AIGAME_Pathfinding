using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.AStar
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Color _baseColor = Color.green, _offsetColor = Color.yellow;
        [SerializeField] GameObject _highlight;
        SpriteRenderer _renderer;
        private Color _defaultColor;
        public Vector2Int Position => Vector2Int.RoundToInt(transform.position);
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
        public void Init(bool isOffset)
        {
            _renderer.color = isOffset ? _offsetColor : _baseColor;
            _defaultColor = _renderer.color;
        }
        private void OnMouseEnter()
        {
            _highlight.SetActive(true);
        }
        private void OnMouseExit()
        {
            _highlight.SetActive(false);
        }
        public void ResetColor()
        {
            _renderer.color = _defaultColor;
        }
        public void SetColor(Color color)
        {
            _renderer.color = color;
        }
    }
}
