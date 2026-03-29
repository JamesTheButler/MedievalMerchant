using Common.UI.Elements.Panels;

namespace Features.Player.Camp.UI
{
    public sealed class CampsitePanelUI : DynamicPanel
    {
        private bool _isInteractable = true;

        public bool IsInteractable => _isInteractable;

        public void SetInteractable(bool isInteractable)
        {
            _isInteractable = isInteractable;
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