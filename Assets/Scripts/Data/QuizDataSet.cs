using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Quiz
{
    [Serializable]
    public struct Symbol
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;

        public string Name => _name;
        public Sprite Sprite => _sprite;
    }

    [CreateAssetMenu(fileName = "QuizDataSet", menuName = "Data/QuizDataSet")]
    public class QuizDataSet : ScriptableObject
    {
        [SerializeField] private Symbol[] _symbols;

        public Symbol[] GetUniqueSymbols(int number)
        {
            if (number > _symbols.Length)
                throw new Exception("Not enough symbols");

            return GetSymbols(number, Enumerable.Range(0, _symbols.Length).ToList());
        }

        private Symbol[] GetSymbols(int number, List<int> indexes)
        {
            Symbol[] symbols = new Symbol[number];

            for (int i = 0; i < number; i++)
            {
                int randomIndex = indexes.GetRandom();
                indexes.Remove(randomIndex);
                symbols[i] = _symbols[randomIndex];
            }

            return symbols;
        }
    }
}