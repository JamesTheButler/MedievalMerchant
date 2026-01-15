using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Utility;
using Features.Levels.GameModifiers.Events;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Levels.GameModifiers.UI
{
    public sealed class GameEventListUI : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject eventContainer;

        [SerializeField, Required]
        private TMP_Text label;

        [SerializeField, Required]
        private GameObject eventItemPrefab;

        private EventModel _gameEventModel;

        private readonly Dictionary<GameEvent, TimedGameModifierUIElement> _uiElements = new();
        private readonly Dictionary<GameEvent, Action<int>> _handlers = new();

        public void Bind()
        {
            Unbind();
            _gameEventModel = GameplayContext.Instance.Model.Events;
            _gameEventModel.EventAdded += OnEventAdded;
            _gameEventModel.EventRemoved += OnEventRemoved;

            UpdateHeader();

            foreach (var gameEvent in _gameEventModel.OngoingEvents)
            {
                OnEventAdded(gameEvent);
            }
        }

        public void Unbind()
        {
            eventContainer.DestroyChildren();

            foreach (var (gameEvent, handler) in _handlers)
            {
                gameEvent.DaysLeft.StopObserving(handler);
            }

            _uiElements.Clear();
            _handlers.Clear();
    
            if (_gameEventModel != null)
            {
                _gameEventModel.EventAdded -= OnEventAdded;
                _gameEventModel.EventRemoved -= OnEventRemoved;
            }
        }

        private void OnEventAdded(GameEvent gameEvent)
        {
            UpdateHeader();
            var uiElement = Instantiate(eventItemPrefab, eventContainer.transform);
            var gameModifierScript = uiElement.GetComponentInChildren<TimedGameModifierUIElement>();
            gameModifierScript.Setup(gameEvent.Data);

            _uiElements.Add(gameEvent, gameModifierScript);

            gameEvent.DaysLeft.Observe(HandleDaysLeft);
            _handlers.Add(gameEvent, HandleDaysLeft);
            return;

            // local func to allow collection and un-subscription of handlers
            void HandleDaysLeft(int days)
            {
                gameModifierScript.SetTimeLeft(days);
            }
        }

        private void OnEventRemoved(GameEvent gameEvent)
        {
            UpdateHeader();
            if (!_uiElements.TryGetValue(gameEvent, out var uiElement))
                return;

            if (_handlers.Remove(gameEvent, out var handler))
            {
                gameEvent.DaysLeft.StopObserving(handler);
            }

            Destroy(uiElement.gameObject);
        }

        private void UpdateHeader()
        {
            label.text = $"Ongoing Events ({_gameEventModel.OngoingEvents.Count})";
        }
    }
}