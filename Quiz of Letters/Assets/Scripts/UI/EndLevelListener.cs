using Others;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class EndLevelListener : ListenerLevelLogicComponent
    {
        [Header("Префаб окна рестарта"), SerializeField]
        private RestartWindow _restartWindowPrefab;

        public event UnityAction<RestartWindow> OnCreateRestartWindow;

        private void Start () => Init();

        public override void Init ()
        {
            base.Init();

            if (!_restartWindowPrefab)
            {
                throw new NullReferenceException("restart window not seted");
            }

            LevelLogicInstance.OnEndLevels += CreateRestartWindow;
        }

        private void CreateRestartWindow() => OnCreateRestartWindow?.Invoke(Instantiate(_restartWindowPrefab));

        private void OnDestroy() => LevelLogicInstance.OnEndLevels -= CreateRestartWindow;
    }
}