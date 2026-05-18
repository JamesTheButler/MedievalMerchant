using Common.UI.Elements.Panels;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteCompanionPanelUI : DynamicPanel
    {
        private CampsiteCompanionPanelUiItem[] _companionGroups;

        public override void Initialize()
        {
            _companionGroups = GetComponentsInChildren<CampsiteCompanionPanelUiItem>();
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
            foreach (var group in _companionGroups)
            {
                group.Bind();
            }
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
            foreach (var group in _companionGroups)
            {
                group.Unbind();
            }
        }
    }
}