using System;
using UnityEngine;

namespace Cards
{
    /// <summary>
    /// Данные карточки
    /// </summary>
    [Serializable]
   public class CardData
    {
        [Header("Идентификатор"), SerializeField]
        private string _indentifier;

        [Header("Изображение"), SerializeField]
        private Sprite _pictogram;

        public string Indentifier => _indentifier;
        public Sprite Pictogram => _pictogram;
    }
}
