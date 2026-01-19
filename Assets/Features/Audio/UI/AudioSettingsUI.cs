using Common.UI.Elements;

namespace Features.Audio.UI
{
    public sealed class AudioSettingsUI : DynamicPanel
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