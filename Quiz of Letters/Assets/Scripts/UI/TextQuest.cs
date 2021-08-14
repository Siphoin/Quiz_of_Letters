using TMPro;
using UnityEngine;
using System;
using Extensions;
using Others;
using SO.TextQuest;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextQuest : ListenerLevelLogicComponent
    {
        private TextMeshProUGUI _sameText;

        [Header("Настройки текста задачи"), SerializeField]
        private TextQuestSettings _settings;

       private void Awake () => Init();

        public override void Init ()
        {
            base.Init();

            if (!_settings)
            {
                throw new NullReferenceException("text quest not have settings");
            }

            if (!TryGetComponent(out _sameText))
            {
                throw new NullReferenceException("text quest not have component TMPro GUI text");
            }

            LevelLogicInstance.OnNewQuest += RefreshTextQuest;
            LevelLogicInstance.OnRestartLevel += Fade;

            Fade();
        }


        private void OnDestroy ()
        {
            LevelLogicInstance.OnNewQuest -= RefreshTextQuest;
            LevelLogicInstance.OnRestartLevel -= Fade;
        }

        private void Fade() => transform.DOFade(_settings.Params.Value, _settings.Params.Duration);

        private void RefreshTextQuest(string id) => _sameText.text = $"Find {id}";

    }
}