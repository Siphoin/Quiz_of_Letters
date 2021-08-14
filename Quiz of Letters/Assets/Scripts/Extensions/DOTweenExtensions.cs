using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Расширения для более простого взаимодействия с DOTween, я первый раз использую эту библимотеку, поэтому здесь немного дубляж есть
    /// </summary>
    public static  class DOTweenExtensions
    {
        private const int INTERVAL_NEXT_TWEEN = 2;

        public static void DOFade (this Transform transform, float value, float duration)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOScale(value, duration));
            sequence.AppendInterval(duration / INTERVAL_NEXT_TWEEN);
            sequence.Append(transform.DOScale(1, duration));
        }

        public static void DOEaseInBounce (this Transform transform, float offset, float duration)
        {
            float startX = transform.position.x;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMoveX(startX - offset, duration));

            sequence.AppendInterval(duration / INTERVAL_NEXT_TWEEN);
            sequence.Append(transform.DOMoveX(startX + offset, duration));
            sequence.AppendInterval(duration / INTERVAL_NEXT_TWEEN);

            sequence.Append(transform.DOMoveX(startX, duration));
        }

        public static void DOFadeColor(this Material mat, Color startColor, Color colorEnd, float duration)
        {
            mat.color = startColor;

            Sequence sequence = DOTween.Sequence();           
            sequence.Append(mat.DOColor(colorEnd, duration));
        }


    }
}
