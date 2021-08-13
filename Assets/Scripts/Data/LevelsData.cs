using System;
using UnityEngine;

namespace Quiz
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
    public class LevelsData : ScriptableObject
    {
        [SerializeField] private Level[] _levels;

        public Level[] Levels => _levels;
    }

    [Serializable]
    public struct Level
    {
        [SerializeField] private QuizDataSet _data;
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public QuizDataSet Data => _data;
        public int Width => _width;
        public int Height => _height;
    }
}