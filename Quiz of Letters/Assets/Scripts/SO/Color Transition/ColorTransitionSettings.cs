using Others.ColorTransitions;
using UnityEngine;

namespace SO.ColorTransitions
{
    /// <summary>
    /// SO, который содержит в себе данные о переходах цветов звездочек
    /// </summary>
    [CreateAssetMenu(menuName = "SO/Color/Color Transitions", order = 0)]
    public class ColorTransitionSettings : ScriptableObject
    {
        [Header("Параметры"), SerializeField]
        private ColorTransition _transitions;

        public ColorTransition Transitions => _transitions;
    }
}