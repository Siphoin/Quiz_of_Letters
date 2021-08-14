using Cards;
using UnityEngine;

namespace SO.Cards
{
    /// <summary>
    /// Контейнер с определенным набором карточек
    /// </summary>
    /// 
    [CreateAssetMenu(menuName = "SO/Cards/Card Bundle", order = 0)]
    public class CardBundleData : ScriptableObject
    {
        [Header("Список карточек для выбора при генерации"), SerializeField]
        private CardData[] _cards;

        public CardData[] Cards => _cards;
    }
}