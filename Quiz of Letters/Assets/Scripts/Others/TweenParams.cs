using System;
using UnityEngine;

namespace Others.TweenSettings
{
    /// <summary>
    /// Параметры твина
    /// </summary>
    [Serializable]
   public struct TweenParams
    {
        [Header("Длительность"), SerializeField]
        private float _duration;

        [Header("Значение"), SerializeField]
        private float _value;

        public float Duration => _duration;
        public float Value => _value;
    }
}
