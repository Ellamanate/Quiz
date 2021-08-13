using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utils;

namespace Quiz
{
    public class UserIntrface : MonoBehaviour
    {
        public event Action OnRestartClick;

        [SerializeField] private LoadingScreen _loading;
        [SerializeField] private Text _message;
        [SerializeField] private Image _endGamePanel;
        [SerializeField] private Button _restart;
        [SerializeField] private float _animationDuration;
        [SerializeField] private float _fadeValue;
        private Tweener _messageTweener;
        private Tweener _endGameTweener;

        public void ShowMessage(string symbolName, bool animate)
        {
            _message.text = "Find " + symbolName;

            if (animate)
            {
                _message.color = _message.color.ChangeAlpha(0);

                _messageTweener?.Kill();
                _messageTweener = _message.DOFade(1, _animationDuration).SetEase(Ease.Linear);
            }
            else
            {
                _message.color = _message.color.ChangeAlpha(1);
            }
        }

        public void ShowEndGamePanel()
        {
            _endGamePanel.gameObject.SetActive(true);
            _endGamePanel.color = _message.color.ChangeAlpha(0);

            _endGameTweener?.Kill();
            _endGameTweener = _endGamePanel.DOFade(_fadeValue, _animationDuration).SetEase(Ease.Linear);
            _endGameTweener.onComplete += ActivateRestartButton;
        }

        private void OnEnable()
        {
            _restart.onClick.AddListener(Restart);
            _loading.OnLoadingStart += OnLoadingStart;
        }

        private void OnDisable()
        {
            _restart.onClick.RemoveListener(Restart);
            _loading.OnLoadingStart -= OnLoadingStart;
        }

        private void Restart()
        {
            OnRestartClick?.Invoke();
        }

        private void ActivateRestartButton()
        {
            _restart.gameObject.SetActive(true);
        }

        private void OnLoadingStart()
        {
            _message.text = string.Empty;
            _restart.gameObject.SetActive(false);
            _endGamePanel.gameObject.SetActive(false);
        }
    }
}