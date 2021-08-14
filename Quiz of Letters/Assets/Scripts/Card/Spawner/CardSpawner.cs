using UnityEngine;
using System;
using SO.Cards;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace Cards.Spawner
{
    /// <summary>
    /// Отвечает за спавн ячеек, подбирает случайный набор визуализации, может блокировать ячейки
    /// </summary>
    public class CardSpawner : MonoBehaviour
    {
        private const string PATH_CARD_BUNDLES = "Data/Card Bundles";

        public event UnityAction<string> OnCardSelect;

        private CardBundleData[] _bundles;

        [Header("Префаб карточки"), SerializeField]
        private Card _cardPrefab;

        [Header("Grid карточек"), SerializeField]
        private GridLayoutGroup _grid;

        private List<Card> _activeCards;

        public List<Card> ActiveCards => _activeCards;

       private void Awake ()
        {
            if (!_cardPrefab)
            {
                throw new NullReferenceException("card prefab not seted");
            }

            if (!_grid)
            {
                throw new NullReferenceException("grid cards not seted");
            }

            LoadBundles();

            _activeCards = new List<Card>();
        }
        /// <summary>
        /// Загружает все пакеты визуализации
        /// </summary>
        private void LoadBundles ()
        {
            _bundles = Resources.LoadAll<CardBundleData>(PATH_CARD_BUNDLES);

            if (_bundles.Length == 0)
            {
                throw new NullReferenceException("bundlws cards not found");
            }
        }

        public void SpawnCards (int count, bool useFakeInAnimation)
        {
            // выбираем случайный пакет визуализации
            CardBundleData randomBundle = _bundles[Random.Range(0, _bundles.Length)];

            // иницилизируем экземпляр контейнера,
            // он понадобится для избежании повторности карточек при генерации
            List<CardData> usedCards = new List<CardData>();

            for (int i = 0; i < count; i++)
            {
                Card newCard = Instantiate(_cardPrefab, _grid.transform);
                newCard.UseFakeInOnStart = useFakeInAnimation;

                // Выбираем карточки из пакета, которых еще не было в текущей сессии

                CardData[] variantsCards = randomBundle.Cards.Where(x => !usedCards.Contains(x)).ToArray();
               
                CardData data = variantsCards[Random.Range(0, variantsCards.Length)];

                newCard.SetData(data);
                newCard.OnTap += CallEventSelectCard;

                _activeCards.Add(newCard);

                usedCards.Add(data);
            }
        }

        public void ClearGrid ()
        {
            for (int i = 0; i < _activeCards.Count; i++)
            {
                Card card = _activeCards[i];
                card.OnTap -= CallEventSelectCard;
                Destroy(card.gameObject);
            }

            _activeCards.Clear();
        }

        public void BlockCards ()
        {
            for (int i = 0; i < _activeCards.Count; i++)
            {
               _activeCards[i].SetStateButton(false);

            }
        }

        private void CallEventSelectCard(string id) => OnCardSelect?.Invoke(id);
    }
}