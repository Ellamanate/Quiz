using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utils;

namespace Quiz
{
    [RequireComponent(typeof(Image))]
    public class LoadingScreen : MonoBehaviour
    {
        public event Action OnLoadingStart;
        public event Action OnLoadingEnd;
        public event Action OnLoadingAnimationEnd;

        [SerializeField] private Text _loadText;
        [SerializeField] private float _animationDuration;
        [SerializeField] private float _loadingTime;
        private Sequence _loadSequence;
        private Image _image;

        public void ShowLoadScreen()
        {
            gameObject.SetActive(true);
            _image.color = _image.color.ChangeAlpha(0);

            _loadSequence?.Restart();
        }

        private void Awake()
        {
            _image = GetComponent<Image>();

            _loadSequence = DOTween.Sequence();
            _loadSequence
                .Append(_image.DOFade(1, _animationDuration).SetEase(Ease.Linear))
                .AppendCallback(LoadingAnimationStart)

                .AppendInterval(_loadingTime)
                .AppendCallback(LoadingAnimationEnd)

                .Append(_image.DOFade(0, _animationDuration).SetEase(Ease.Linear))
                .AppendCallback(LoadingEnd)

                .SetAutoKill(false)
                .Pause();
        }

        private void LoadingAnimationStart()
        {
            _loadText.gameObject.SetActive(true);
            OnLoadingStart?.Invoke();
        }

        private void LoadingAnimationEnd()
        {
            _loadText.gameObject.SetActive(false);
            OnLoadingAnimationEnd?.Invoke();
        }

        private void LoadingEnd()
        {
            gameObject.SetActive(false);
            OnLoadingEnd?.Invoke();
        }
    }
}