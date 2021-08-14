using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using DG.Tweening;
using SO.Cards;
using Extensions;

namespace Cards
{
    // ��� ������������� ����� ����� ������������ � ������, �.� ��� �������� ������� Button, �� ��������� ����������.
    // + Unity �� ���� ������� ��������� Button
    [RequireComponent(typeof(Button))]
    public class Card : MonoBehaviour
    {
        /// <summary>
        /// ��������� �������� x ������� ������ ��������,
        /// ����� ��� �������������� ������� �������� ease bounce (�������, �� ��������)
        /// </summary>
        private float _startXPictogram;

        private Others.TweenSettings.TweenParams _tweenParamsFade;

        private Others.TweenSettings.TweenParams _tweenParamsEaseBounce;

        /// <summary>
        /// �������, ������� ���������, ���� �� ������ �� ��������. 
        /// � ���� ������ �� ��������� ��������� � ���, ��� �� ������� ��������, ��������� ����������� ID ��������
        /// </summary>
        public event UnityAction<string> OnTap;

        /// <summary>
        /// ���� ������ ��������
        /// </summary>
        /// 
        private Button _button;

        [Header("�����������"), SerializeField]
        private Image _pictogram;

        [Header("������ �� ���������"), SerializeField]
        private CardAnimationSettings _animationSettings;

        private CardData _cardData;

        private Transform _transformPictogram;

        /// <summary>
        /// ����� �� ����������� �������� fade in ��� ��������
        /// </summary>
        public bool UseFakeInOnStart { get; set; } = true;
        /// <summary>
        /// ����� �������� ���������� ��� ������ ������
        /// </summary>
        public bool IsValidCard { get; set; } = false;
        /// <summary>
        /// �������������� �������� �� ������������� ������
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

            // ������ GetComponent �������� TryGetComponent,
            // ����� ��������� ���� �� ������ ��������� �� �������, �� ������� ����� ���� ������, ���� ���, ����������� ����������

            if (!TryGetComponent(out _button))
            {
                throw new NullReferenceException("GameObject have script Card, but not have component Button");
            }

            _transformPictogram = _pictogram.transform;

            // ����������� ������ �� ����� Tap, ������ ������ �� ���� ������ ��������� ���������� � �������� ���� ��������, �.� ��� �� ��� ����� �� �����,
            // + �� ����� � ��������� ��������� � OnClick

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

            // ���� ����� �� ������� - �������
            if (!IsValidCard && _startXPictogram == _transformPictogram.localPosition.x)
            {
                EaseInBounceIn();
            }
            // ����� - fade in ������ ��������
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


        // P.S � �� ����� ������� ������� �� �����, ������� ������ ���-�� ����, ���� ��� �� ����, �� ��������

        #region Animations
        /// <summary>
        /// ������������� Fade ��������
        /// </summary>
        /// <param name="transform">�����</param>
       private void Fade (Transform transform)
        {
            float duration = _tweenParamsFade.Duration;

            transform.DOFade(_tweenParamsFade.Value, duration);
        }

        /// <summary>
        /// ����������� �������� ��������
        /// </summary>
        /// <param name="transform">�����</param>
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