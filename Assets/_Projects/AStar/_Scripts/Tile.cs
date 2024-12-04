using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.AStar
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] Color _baseColor = Color.green, _offsetColor = Color.yellow, _deselectedColor = Color.white, _selectedColor = Color.red;
        [SerializeField] GameObject _highlight;
        SpriteRenderer _renderer;
        bool _isSelected;
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }
        public void Init(bool isOffset)
        {
            _renderer.color = isOffset ? _offsetColor : _baseColor;
            _deselectedColor = _renderer.color;
        }
        private void OnMouseEnter()
        {
            _highlight.SetActive(true);
        }
        private void OnMouseExit()
        {
            _highlight.SetActive(false);
        }
        private void OnMouseDown()
        {
        }
    }
}
