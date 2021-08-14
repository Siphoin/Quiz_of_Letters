using UnityEngine;

namespace SO.Level
{
    /// <summary>
    /// Общие настройки поведения уровня
    /// </summary>
    [CreateAssetMenu(menuName = "SO/Level/Level Logic Settings", order = 1)]
    public class LevelLogicSettings : ScriptableObject
    {
        // перед переходом на новый уровень (для анимации карточки и звезд)
        [Header("Задержка перед новым уровнем"), SerializeField]
        private float _timeOutNewLevel = 1.5f;

        public float TimeOutNewLevel => _timeOutNewLevel;

        private void OnValidate()
        {
            if (_timeOutNewLevel <= 0)
            {
                _timeOutNewLevel = 1.5f;
            }
        }
    }
}