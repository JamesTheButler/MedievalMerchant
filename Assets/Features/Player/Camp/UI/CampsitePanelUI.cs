using Common.UI.Elements.Panels;

namespace Features.Player.Camp.UI
{
    public sealed class CampsitePanelUI : DynamicPanel
    {
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