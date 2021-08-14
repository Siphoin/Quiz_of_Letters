using UnityEngine;
using Others.TweenSettings;
namespace SO.TextQuest
{
    /// <summary>
    /// Настройки поведения UI текста
    /// </summary>
    [CreateAssetMenu(menuName = "SO/UI/Text Quest Settings", order = 0)]
    public class TextQuestSettings : ScriptableObject
    {
        [Header("Настройки текста UI"), SerializeField]
        
        private TweenParams _params;

        public TweenParams Params => _params;
    }
}