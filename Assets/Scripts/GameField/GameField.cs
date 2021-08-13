using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Quiz 
{
    public class GameField : MonoBehaviour
    {
        public event Action OnRightChoice;

        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Transform _background;
        [SerializeField] private CellsPalette _palette;
        [SerializeField] private float _margin;
        [SerializeField] private float _animationDuration;
        private Cell[,] _cells;
        private float _innerOffset = 0.5f;
        private float _defaultOutlineOffset = 1;
        private float _outlineOffset;
        private int _width;
        private int _height;
        private bool _isTargetClicked;

        public void Generate(int width, int height, Symbol[] symbols, Symbol target, bool firstGeneration)
        {
            if (symbols.Length < width * height)
                throw new Exception("Not enough symbols to generate field");

            Init(width, height);

            if (!firstGeneration)
            {
                ClearCells();
                ScaleBackground();
            }
            else
            {
                _background.gameObject.SetActive(false);
                StartCoroutine(WaitAnimationEnd(() => ScaleBackground()));
            }

            FillField(symbols, target, firstGeneration);
        }

        public void Clear()
        {
            _background.gameObject.SetActive(false);
            ClearCells();
        }

        public void SetActive(bool activate)
        {
            foreach (Cell cell in _cells)
                cell.SetActive(activate);
        }

        private void Init(int width, int height)
        {
            _width = width;
            _height = height;
            _outlineOffset = _defaultOutlineOffset + _margin;
            _isTargetClicked = false;
        }

        private void FillField(Symbol[] symbols, Symbol target, bool appearance)
        {
            Vector2 offset = new Vector2(GetOffset(_width), GetOffset(_height));
            float outlineOffset = _defaultOutlineOffset + _margin;

            _cells = new Cell[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector3 position = (new Vector2(x, y) - offset) * outlineOffset;
                    Symbol symbol = symbols[x * _height + y];

                    Cell cell = Instantiate(_cellPrefab, position, Quaternion.identity, transform);
                    cell.Init(symbol.Sprite, _palette.Colors.GetRandom(), _animationDuration, appearance, symbol.Equals(target));
                    cell.OnClick += OnClickCell;

                    _cells[x, y] = cell;
                }
            }

            float GetOffset(int value) => value / 2f - _innerOffset;
        }

        private void ClearCells()
        {
            if (_cells != null)
            {
                foreach (Cell cell in _cells)
                    Destroy(cell.gameObject);

                _cells = null;
            }
        }

        private void OnClickCell(Cell cell)
        {
            if (!_isTargetClicked && cell.IsTarget)
            {
                _isTargetClicked = true;
                OnRightChoice?.Invoke();
            }
        }

        private void ScaleBackground()
        {
            _background.gameObject.SetActive(true);
            _background.localScale = new Vector2(_width * _outlineOffset + _margin, _height * _outlineOffset + _margin);
        }

        private IEnumerator WaitAnimationEnd(Action action)
        {
            yield return new WaitForSeconds(_animationDuration);
            action?.Invoke();
        }
    }
}
