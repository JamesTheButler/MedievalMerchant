using Common.Infrastructure.Observation;
using Common.UI.Elements.Panels;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteCaravanUI : DynamicPanel
    {
        private readonly Bindings _bindings = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
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