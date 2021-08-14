using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Extensions;
using SO.ColorTransitions;
using Others.ColorTransitions;

namespace UI
{
    public class RestartWindow : MonoBehaviour
    {
        [Header("Скорость затухания заднего фона"), SerializeField]
        private float _speedAttenuation = 1;

        [Header("Кнопка перезапуска сцены"), SerializeField]
        private Button _buttonRestart;

        [Header("Задний фон"), SerializeField]
        private Image _background;

        [Header("Настройки переходов цвета звездочек"), SerializeField]
        private ColorTransitionSettings _colorTransitionSettings;

        private ColorTransition _colorTransitionBackground;

        public event UnityAction OnRestartButtonTapped;

       private void Start()
        {
            if (!_buttonRestart)
            {
                throw new NullReferenceException("button restart not seted");
            }

            if (!_background)
            {
                throw new NullReferenceException("background not seted");
            }

            if (!_colorTransitionSettings)
            {
                throw new NullReferenceException("color transitions not seted");
            }

            _colorTransitionBackground = _colorTransitionSettings.Transitions;

            _buttonRestart.onClick.AddListener(CallRestartLevel);

            PlayFadeAnimationBackground(_colorTransitionBackground.StartColor, _colorTransitionBackground.EndColor);
        }

        private void CallRestartLevel ()
        {
            RestartLevel();

            Destroy(_buttonRestart.gameObject);
        }

        private void RestartLevel ()
        {
            PlayFadeAnimationBackground(_colorTransitionBackground.EndColor, _colorTransitionBackground.StartColor);

            OnRestartButtonTapped?.Invoke();

            DestroyWindow();
        }

        private void PlayFadeAnimationBackground (Color startColor, Color endColor)
        {
            _background.material.DOFadeColor(startColor, endColor, _speedAttenuation);
        }
        private void OnValidate ()
        {
            if (_speedAttenuation <= 0)
            {
                _speedAttenuation = 1;
            }
        }

        private void DestroyWindow() => Destroy(gameObject, _speedAttenuation);
    }
}