using UnityEngine;
using System;
using Others;
using Extensions;
using SO.ColorTransitions;
using Others.ColorTransitions;

namespace VFX
{
    public class StarsEffectSpawner : ListenerLevelLogicComponent
    {
        [Header("Префаб партиклов звезд"), SerializeField]
        private ParticleSystem _starParticlesPrefab;

        [Header("Настройки переходов цвета звездочек"), SerializeField]
        private ColorTransitionSettings _colorTransitionSettings;

        private ColorTransition _colorTransitionStars;

        private ParticleSystem _activeEffectStars;


       private void Start () => Init();

        public override void Init ()
        {
            base.Init();

            if (!_starParticlesPrefab)
            {
                throw new NullReferenceException("prefab particles stars not seted");
            }

            if (!_colorTransitionSettings)
            {
                throw new NullReferenceException("color transitions not seted");
            }

            _colorTransitionStars = _colorTransitionSettings.Transitions;

            LevelLogicInstance.OnValidAnswer += CreateParticles;
            LevelLogicInstance.OnRestartLevel += DestroyLastStarsEffect;
        }

        private void CreateParticles ()
        {
            DestroyLastStarsEffect();

            _activeEffectStars = Instantiate(_starParticlesPrefab);
            _activeEffectStars.transform.position = transform.position;
            _activeEffectStars.transform.SetParent(transform);

           ParticleSystemRenderer renderParticles = _activeEffectStars.GetComponent<ParticleSystemRenderer>();
            renderParticles.material.DOFadeColor(_colorTransitionStars.StartColor, _colorTransitionStars.EndColor, _colorTransitionStars.Duration);

        }

        private void DestroyLastStarsEffect ()
        {
            if (_activeEffectStars)
            {
                Destroy(_activeEffectStars.gameObject);
            }
        }

        private void OnDestroy ()
        {
            LevelLogicInstance.OnValidAnswer -= CreateParticles;
            LevelLogicInstance.OnRestartLevel -= DestroyLastStarsEffect;
        }
    }
}