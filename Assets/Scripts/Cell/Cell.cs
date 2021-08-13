using System;
using UnityEngine;
using DG.Tweening;

namespace Quiz
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Cell : MonoBehaviour
    {
        public event Action<Cell> OnClick;

        [SerializeField] private SpriteRenderer _symbolRenderer;
        private SpriteRenderer _renderer;
        private Tweener _appearance;
        private Sequence _scaleSequence;
        private Sequence _moveSequence;
        private Vector3 _symbolposition;
        private bool _isTarget;
        private bool _isActive;
        private float _maxOffset = 0.075f;
        private float _maxScaleReduction = 0.8f;
        private float _duration;

        public bool IsTarget => _isTarget;

        public void Init(Sprite symbolSprite, Color color, float animationDuration, bool animateAppearance, bool isTarget)
        {
            _symbolRenderer.sprite = symbolSprite;
            _renderer.color = color;
            _duration = animationDuration;
            _isTarget = isTarget;
            _isActive = true;

            SetMoveSequence(_symbolRenderer.transform);
            SetScaleSequence(_symbolRenderer.transform);
            SetAppearanceAnimation();

            if (animateAppearance)
                Appearance();
        }

        public void ClickAnimation(bool isTarget)
        {
            if (isTarget)
                _scaleSequence?.Restart();
            else
                _moveSequence?.Restart();
        }

        public void SetActive(bool activate)
        {
            _isActive = activate;
        }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _symbolposition = _symbolRenderer.transform.position;
        }

        private void OnMouseDown()
        {
            if (_isActive)
                ClickAnimation(_isTarget);
        }

        private void OnDestroy()
        {
            _scaleSequence.Kill();
            _moveSequence.Kill();
            _appearance.Kill();
        }

        private void Appearance()
        {
            transform.localScale = Vector3.zero;

            _appearance?.Restart();
        }

        private void Click()
        {
            OnClick?.Invoke(this);
        }

        private void SetAppearanceAnimation()
        {
            _appearance = transform.DOScale(Vector3.one, _duration)
                .SetEase(Ease.OutBounce)
                .SetAutoKill(false)
                .Pause();
        }

        private void SetScaleSequence(Transform symbolTransform)
        {
            symbolTransform.localScale = Vector3.one;

            float halfDuration = _duration / 2;

            _scaleSequence = DOTween.Sequence();
            _scaleSequence
                .Append(symbolTransform.DOScale(Vector3.one * _maxScaleReduction, halfDuration).SetEase(Ease.OutBounce))
                .Append(symbolTransform.DOScale(Vector3.one, halfDuration).SetEase(Ease.OutBounce))
                .AppendCallback(Click)
                .SetAutoKill(false)
                .Pause();
        }

        private void SetMoveSequence(Transform symbolTransform)
        {
            float quaterDuration = _duration / 4;
            float halfDuration = _duration / 2;

            _moveSequence = DOTween.Sequence();
            _moveSequence
                .Append(symbolTransform.DOMove(_symbolposition + Vector3.right * _maxOffset, quaterDuration).SetEase(Ease.InBounce))
                .Append(symbolTransform.DOMove(_symbolposition + Vector3.left * _maxOffset, halfDuration).SetEase(Ease.InBounce))
                .Append(symbolTransform.DOMove(_symbolposition, quaterDuration).SetEase(Ease.Linear))
                .SetAutoKill(false)
                .Pause();
        }
    }
}