using System;
using Common.Infrastructure;
using Common.UI.Elements;
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

            ParseCheat(cheat);
            Close();
        }

        private void ParseCheat(string cheat)
        {
            try
            {
                var split = cheat
                    .ToLowerInvariant()
                    .TrimEnd(' ')
                    .Split(" ");
                switch (split.Length)
                {
                    case 1:
                        _cheatService.HandleSimpleCheat(split[0]);
                        break;
                    case 2:
                        _cheatService.HandleParamCheat(split[0], split[1]); break;
                    default:
                        _cheatService.HandleInvalidInput(cheat);
                        break;
                }
            }
            catch (Exception exception)
            {
                _cheatService.HandleInvalidInput(exception.Message);
            }
        }
    }
}