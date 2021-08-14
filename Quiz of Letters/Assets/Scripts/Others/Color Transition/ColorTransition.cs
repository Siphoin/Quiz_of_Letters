using UnityEngine;

namespace Others.ColorTransitions
{
    /// <summary>
    /// Структура перехода цвета к начальному к конечному
    /// </summary>
    [System.Serializable]
    public struct ColorTransition
    {
        [Header("Длительность"), SerializeField]
        private float _duration;

        [Header("Стартовый цвет при запуске перехода"), SerializeField]
        private Color _startColor;

        [Header("Конечный цвет при конце перехода"), SerializeField]
        private Color _endColor;

        public float Duration => _duration;

        public Color StartColor => _startColor;

        public Color EndColor => _endColor;

    }
}
