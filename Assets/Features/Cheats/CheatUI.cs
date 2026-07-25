using System;
using Common.Infrastructure.Gameplay;
using Common.UI.Elements.Panels;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Features.Cheats
{
    public sealed class CheatUI : DynamicPanel
    {
        [SerializeField, Required]
        private TMP_InputField cheatInput;

        private CheatService _cheatService;

        private string _lastCheat;

        protected override void OnInitialize()
        {
            _cheatService = GameplayContext.Instance.Services.Cheats;
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(gameObject);
            cheatInput.ActivateInputField();
            cheatInput.Select();
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
            cheatInput.text = string.Empty;
        }

        public void ConfirmInput(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            CheatInputConfirmed(cheatInput.text);
        }

        private void CheatInputConfirmed(string cheat)
        {
            if (!enabled)
                return;

            if (cheat == string.Empty)
                return;

            ParseCheat(cheat);
            Close();
        }

        private void ParseCheat(string cheat)
        {
            try
            {
                if (cheat == "l")
                {
                    if (string.IsNullOrEmpty(_lastCheat))
                        return;
                    cheat = _lastCheat;
                }

                var split = cheat
                    .ToLowerInvariant()
                    .TrimEnd(' ')
                    .Split(" ");

                var wasSuccess = ParseCheat(cheat, split);
                if (wasSuccess)
                {
                    _lastCheat = cheat;
                }
            }
            catch (Exception exception)
            {
                _cheatService.HandleInvalidInput(exception.Message);
            }
        }

        private bool ParseCheat(string cheat, string[] split)
        {
            bool wasSuccess;
            switch (split.Length)
            {
                case 1:
                    wasSuccess = _cheatService.TryHandleSimpleCheat(split[0]);
                    break;
                case 2:
                    wasSuccess = _cheatService.TryHandleParamCheat(split[0], split[1]);
                    break;
                default:
                    _cheatService.HandleInvalidInput(cheat);
                    wasSuccess = false;
                    break;
            }

            return wasSuccess;
        }
    }
}