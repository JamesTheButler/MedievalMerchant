using Common.UI.Elements;

namespace Common.UI.Credits
{
    public sealed class CreditsUI : DynamicPanel
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