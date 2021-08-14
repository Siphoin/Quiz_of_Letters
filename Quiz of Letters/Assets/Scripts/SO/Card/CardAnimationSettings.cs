using Others.TweenSettings;
using UnityEngine;

namespace SO.Cards
{
    /// <summary>
    /// Настройки поведения анимаций карточки
    /// </summary>
    [CreateAssetMenu(menuName = "SO/Cards/Card Transition Settings", order = 1)]
    public class CardAnimationSettings : ScriptableObject
    {
        [Header("Параметры твина Fade"), SerializeField]
        private TweenParams _fadeParams;

        [Header("Параметры твина Ease Bounce"), SerializeField]
        private TweenParams _easeBounceParams;

        public TweenParams FadeParams => _fadeParams;
        public TweenParams EaseBounceParams => _easeBounceParams;
    }
}