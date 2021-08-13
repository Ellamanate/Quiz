using UnityEngine;

namespace Quiz
{
    [CreateAssetMenu(fileName = "CellsPalette", menuName = "Data/CellsPalette")]
    public class CellsPalette : ScriptableObject
    {
        [SerializeField] private Color[] _colors;

        public Color[] Colors => _colors;
    }
}