using Level;
using System;
using UnityEngine;

namespace Others
{
    /// <summary>
    /// Содержит поле, которое неолбходимо TextQuest и EndLevelListener, во избежании копипаста поля
    /// </summary>
    public abstract class ListenerLevelLogicComponent : MonoBehaviour
    {
        [Header("Экземпляр объекта, который отвечает за логику уровня")]
        [SerializeField]
        private LevelLogic _levelLogic;

        public LevelLogic LevelLogicInstance => _levelLogic;

        public virtual void Init ()
        {
            if (!LevelLogicInstance)
            {
                throw new NullReferenceException("on UI Element Listener Level not seted level logic object for subcribe events");
            }
        }
    }
}