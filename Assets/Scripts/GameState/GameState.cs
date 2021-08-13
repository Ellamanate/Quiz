using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Quiz
{
    public class GameState : MonoBehaviour
    {
        [SerializeField] private GameField _field;
        [SerializeField] private UserIntrface _userIntrface;
        [SerializeField] private LoadingScreen _loading;
        [SerializeField] private LevelState _levelState;
        private List<Symbol> _targets = new List<Symbol>();

        private void Awake()
        {
            GenerateField(true);
        }

        private void OnEnable()
        {
            _userIntrface.OnRestartClick += OnRestartClick;
            _loading.OnLoadingStart += OnLoadingStart;
            _loading.OnLoadingEnd += OnLoadingEnd;
            _levelState.OnLevelComplete += OnLevelComplete;
            _levelState.OnLastLevelComplete += OnLastLevelComplete;
        }
        private void OnDisable()
        {
            _userIntrface.OnRestartClick -= OnRestartClick;
            _loading.OnLoadingStart -= OnLoadingStart;
            _loading.OnLoadingEnd -= OnLoadingEnd;
            _levelState.OnLevelComplete -= OnLevelComplete;
            _levelState.OnLastLevelComplete -= OnLastLevelComplete;
        }

        private void OnLevelComplete()
        {
            GenerateField(false);
        }

        private void OnLastLevelComplete()
        {
            _field.SetActive(false);
            _userIntrface.ShowEndGamePanel();
        }

        private void OnRestartClick()
        {
            _loading.ShowLoadScreen();
        }

        private void OnLoadingStart()
        {
            _levelState.Restart();
            _targets.Clear();
            _field.Clear();
        }

        private void OnLoadingEnd()
        {
            GenerateField(true);
        }

        private void GenerateField(bool firstGeneration)
        {
            Level currentLevel = _levelState.CurrentLevel;
            int width = currentLevel.Width;
            int height = currentLevel.Height;

            Symbol[] symbols = currentLevel.Data.GetUniqueSymbols(width * height);
            Symbol target = symbols.Where(x => !_targets.Contains(x)).GetRandom();
            _targets.Add(target);

            _userIntrface.ShowMessage(target.Name, firstGeneration);

            _field.Generate(width, height, symbols, target, firstGeneration);
        }
    }
}