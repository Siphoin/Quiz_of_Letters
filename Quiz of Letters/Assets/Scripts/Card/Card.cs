using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using DG.Tweening;
using SO.Cards;
using Extensions;

namespace Cards
{
    // При перекидывании лучше всего перекидывать и кнопку, т.к эта сущность требует Button, во избежании исключений.
    // + Unity не даст удалить компонент Button
    [RequireComponent(typeof(Button))]
    public class Card : MonoBehaviour
    {
        /// <summary>
        /// Начальное значение x объекта внутри карточки,
        /// нужно для предотвращения повтора анимации ease bounce (костыль, но пришлось)
        /// </summary>
        private float _startXPictogram;

        private Others.TweenSettings.TweenParams _tweenParamsFade;

        private Others.TweenSettings.TweenParams _tweenParamsEaseBounce;

        /// <summary>
        /// Событие, которое сработает, если мы нажмем на карточку. 
        /// В этот момент мы оповестим слушателя о том, что мы выбрали карточку, слушателю перекинется ID карточки
        /// </summary>
        public event UnityAction<string> OnTap;

        /// <summary>
        /// Сама кнопка карточки
        /// </summary>
        /// 
        private Button _button;

        [Header("Пиктограмма"), SerializeField]
        private Image _pictogram;

        [Header("Данные по переходам"), SerializeField]
        private CardAnimationSettings _animationSettings;

        private CardData _cardData;

        private Transform _transformPictogram;

        /// <summary>
        /// Можно ли проигрывать анимация fade in при создании
        /// </summary>
        public bool UseFakeInOnStart { get; set; } = true;
        /// <summary>
        /// Карта является правильной при выборе ответа
        /// </summary>
        public bool IsValidCard { get; set; } = false;
        /// <summary>
        /// Индентификатор карточки из установленных данных
        /// </summary>
        public string Indentifier => _cardData != null ? _cardData.Indentifier : null;

        

        private  void Start ()
        {
                
            

            if (!_animationSettings)
            {
                throw new NullReferenceException("animation settings not seted on card");
            }

            if (!_pictogram)
            {
                throw new NullReferenceException("pictogram not seted on card");
            }

            // вместо GetComponent вызываем TryGetComponent,
            // чтобы отследить есть ли вообще компонент на объекте, на котором висит этот скрипт, если нет, выбрасываем исключение

            if (!TryGetComponent(out _button))
            {
                throw new NullReferenceException("GameObject have script Card, but not have component Button");
            }

            _transformPictogram = _pictogram.transform;

            // подписываем кнопку на метод Tap, данный способ не даст другим сущностям обращаться к механике тапа карточки, т.к это им это знать не нужно,
            // + не нужно в редакторе указывать в OnClick

            _button.onClick.AddListener(Tap);

            _tweenParamsFade = _animationSettings.FadeParams;

            _tweenParamsEaseBounce = _animationSettings.EaseBounceParams;

            _startXPictogram = _transformPictogram.localPosition.x;

            if (UseFakeInOnStart)
            {
                FadeIn();
            }
        }


        private void Tap ()
        {
            if (_cardData == null)
            {
                throw new NullReferenceException($"card {name} not have Card Data");
            }

            OnTap?.Invoke(_cardData.Indentifier);

            // Если карта не валидна - дергаем
            if (!IsValidCard && _startXPictogram == _transformPictogram.localPosition.x)
            {
                EaseInBounceIn();
            }
            // иначе - fade in внутри карточки
            else if (IsValidCard)
            {
                FadePictogram();
            }

#if UNITY_EDITOR
            Debug.Log($"on event card: ID {_cardData.Indentifier}");
#endif
        }

        public void SetData (CardData cardData)
        {
            if (cardData == null)
            {
                throw new ArgumentNullException("card data is null");
            }

            _cardData = cardData;

            _pictogram.sprite = cardData.Pictogram;

#if UNITY_EDITOR
            name = $"{name}_{_cardData.Indentifier}";
#endif
        }


        // P.S Я не нашел готовое решение по фейду, поэтому сделал что-то свое, если все же есть, то извините

        #region Animations
        /// <summary>
        /// Проигрывывает Fade анимацию
        /// </summary>
        /// <param name="transform">Цеель</param>
       private void Fade (Transform transform)
        {
            float duration = _tweenParamsFade.Duration;

            transform.DOFade(_tweenParamsFade.Value, duration);
        }

        /// <summary>
        /// Проигрывает анимацию дергания
        /// </summary>
        /// <param name="transform">Цеель</param>
        private void EaseInBounce (Transform transform)
        {
            float duration = _tweenParamsEaseBounce.Duration;

            transform.DOEaseInBounce(_tweenParamsEaseBounce.Value, duration);

        }

        private void EaseInBounceIn() => EaseInBounce(_pictogram.transform);

        private void FadeIn() => Fade(transform);

        private void FadePictogram() => Fade(_pictogram.transform);

        public void SetStateButton(bool state) => _button.interactable = state;

        #endregion
    }
}