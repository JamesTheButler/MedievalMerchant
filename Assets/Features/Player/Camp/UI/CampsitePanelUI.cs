using Common.Infrastructure.Gameplay;
using Common.UI.Elements.Panels;
using Features.Levels.FeatureFlags;
using UnityEngine;

namespace Features.Player.Camp.UI
{
    public sealed class CampsitePanelUI : DynamicPanel
    {
        [SerializeField]
        private GameObject companionTentButton;

        private bool _isInteractable = true;

        public bool IsInteractable => _isInteractable;

        public void SetInteractable(bool isInteractable)
        {
            _isInteractable = isInteractable;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            var levelInfo = GameplayContext.Instance.LevelInfo;
            companionTentButton.SetActive(levelInfo.HasFeature(LevelFeatureFlags.Retinue));
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}