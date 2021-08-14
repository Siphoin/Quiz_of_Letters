using Cards.Spawner;
using UnityEngine;
using System;
using UnityEngine.Events;
using SO.Level;
using System.Linq;
using System.Collections.Generic;
using UI;

namespace Level
{
    /// <summary>
    /// Осуществляет логику перенаправления на уровни, обращаясь к разным сущностям, которые осуществляют свою логику
    /// </summary>
    public class LevelLogic : MonoBehaviour
    {
        /// <summary>
        /// Путь ко всем уровням. Поскольку в задании было написано, что нельзя ограничитьсчя тремя, 
        /// то я загружаю все из определенной папки
        /// </summary>
        private const string PATH_LEVELS_DATA = "Data/Levels";
        /// <summary>
        /// Текущий валидный ID карточки
        /// </summary>
        private string _currentValidIDCard;

        private int _currentIndexLevel = 0;

        #region Events
        /// <summary>
        /// Вызывается, если уровни закончились
        /// </summary>
        public event UnityAction OnEndLevels;

        /// <summary>
        /// Вызывается, если все уровни перезапускаются
        /// </summary>
        public event UnityAction OnRestartLevel;

        /// <summary>
        /// Вызывается, если был выбран правильный ответ
        /// </summary>
        public event UnityAction OnValidAnswer;

        /// <summary>
        /// Вызывается, если было сгенерировано следующее задание
        /// </summary>
        public event UnityAction<string> OnNewQuest;
        #endregion
        
        /// <summary>
        /// Текущее окно рестарта, с которым LevelLogic прослушивает события этого окна для рестарта всех уровней
        /// </summary>
        private RestartWindow _activeRestartWindow;

        [Header("Спавнер карточек")]
        [SerializeField]
        private CardSpawner _cardSpawner;

        [Header("Слушатель конца уровня")]
        [SerializeField]
        private EndLevelListener _endLevelListener;

        [Header("Настройки логики уровней")]
        [SerializeField]
        private  LevelLogicSettings _levelLogicSettings;

        private LevelBundle[] _levels;

        private List<string> _usedVariantsCards;

        private void Start ()
        {
            if (!_cardSpawner)
            {
                throw new NullReferenceException("card spawner not seted");
            }

            if (!_endLevelListener)
            {
                throw new NullReferenceException("end level listener not seted");
            }


            if (!_levelLogicSettings)
            {
                throw new NullReferenceException("level logic settings not seted");
            }

            LoadLevelsData();

            _usedVariantsCards = new List<string>();

            _cardSpawner.OnCardSelect += CheckCardID;
            _endLevelListener.OnCreateRestartWindow += SubcribeEventRestartWindow;

            NextLevel();
        }

        private void LoadLevelsData ()
        {
            _levels = Resources.LoadAll<LevelBundle>(PATH_LEVELS_DATA);

            if (_levels.Length == 0)
            {
                throw new Exception("levels list is emtry or not found");
            }
        }

        private void SubcribeEventRestartWindow (RestartWindow window)
        {
            _activeRestartWindow = window;
            _activeRestartWindow.OnRestartButtonTapped += RestartGame;
        }

        private void CheckCardID (string id)
        {
            if (id == _currentValidIDCard)
            {
                _cardSpawner.BlockCards();

                string methodName = _currentIndexLevel < _levels.Length ? nameof(NextLevel) : nameof(CallEventEndLevel);

                Invoke(methodName, _levelLogicSettings.TimeOutNewLevel);

                OnValidAnswer?.Invoke();
            }
        }

        private void RestartGame ()
        {
            _currentIndexLevel = 0;
             NextLevel();

            if (_activeRestartWindow)
            {
                UncribeRestartWindowEvents();
            }

            OnRestartLevel?.Invoke();

        }

        private void UncribeRestartWindowEvents ()
        {
            _activeRestartWindow.OnRestartButtonTapped -= RestartGame;
        }


        private void NextLevel ()
        {
            LevelBundle level = _levels[_currentIndexLevel];

            _cardSpawner.ClearGrid();

            _cardSpawner.SpawnCards(level.CountCells, _currentIndexLevel == 0);

            Cards.Card[] variantsCards = null;

            try
            {
                variantsCards = _cardSpawner.ActiveCards.Where(x => !_usedVariantsCards.Contains(x.Indentifier)).ToArray();

                Cards.Card validCard = variantsCards[UnityEngine.Random.Range(0, variantsCards.Length)];

               _currentValidIDCard = validCard.Indentifier;
                validCard.IsValidCard = true;

                OnNewQuest?.Invoke(_currentValidIDCard);

                _usedVariantsCards.Add(_currentValidIDCard);

                _currentIndexLevel++;

            }
            catch
            {
                NextLevel();
            }

        }


        private void OnDestroy ()
        {
            _cardSpawner.OnCardSelect -= CheckCardID;
            _endLevelListener.OnCreateRestartWindow -= SubcribeEventRestartWindow;

            if (_activeRestartWindow != null)
            {
                UncribeRestartWindowEvents();
            }
        }

        private void CallEventEndLevel() => OnEndLevels?.Invoke();
    }
}