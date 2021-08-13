using System;
using UnityEngine;

namespace Quiz
{
    public class LevelState : MonoBehaviour
    {
        public event Action OnLevelComplete;
        public event Action OnLastLevelComplete;

        [SerializeField] private GameField _field;
        [SerializeField] private LevelsData _data;
        [SerializeField] private ParticleSystem _stars;
        private int _levelIndex;

        public Level CurrentLevel => _data.Levels[_levelIndex];

        public void Restart()
        {
            _levelIndex = 0;
        }

        private void OnEnable()
        {
            _field.OnRightChoice += OnRightChoice;
        }
        private void OnDisable()
        {
            _field.OnRightChoice -= OnRightChoice;
        }

        private void OnRightChoice()
        {
            _levelIndex++;
            _stars.Play();

            if (_levelIndex < _data.Levels.Length)
            {
                OnLevelComplete?.Invoke();
            }
            else
            {
                OnLastLevelComplete?.Invoke();
            }
        }
    }
}