using Common.Infrastructure;
using Features.Ticking;
using Features.Ticking.Logic;
using UnityEngine;

namespace Common.UI
{
    public class EscapeMenuHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject escapeMenuRoot;

        private GameSpeedModel _gameSpeedModel;

        private bool IsActive => escapeMenuRoot.activeSelf;

        public void ToggleMenu()
        {
            ToggleEscMenu(!IsActive);
        }

        public void OpenMenu()
        {
            ToggleEscMenu(true);
        }

        private void Start()
        {
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            ToggleEscMenu(false);
        }

        private void ToggleEscMenu(bool isActive)
        {
            escapeMenuRoot.SetActive(isActive);
            if (isActive)
            {
                _gameSpeedModel.Pause();
            }
            else
            {
                _gameSpeedModel.Resume();
            }
        }
    }
}