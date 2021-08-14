using UnityEngine;

namespace SO.Level
{
   /// <summary>
   /// Данные уровня, с их помощью можно не органичиваться в количествах уровнях.
   /// Сделать например не 3, а 100
   /// </summary>
    [CreateAssetMenu(menuName = "SO/Level/Level Bundle", order = 0)]
    public class LevelBundle : ScriptableObject
    {
        [Header("Количество ячеек на уровне"), SerializeField]
        private int _countCells;

        public int CountCells => _countCells;

        private void OnValidate()
        {
            if (_countCells <= 0)
            {
                _countCells = 1;
            }
        }
    }
}