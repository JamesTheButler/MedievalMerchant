namespace UI.Popups
{
    public static class PopupManagerExtension
    {
        public static void HideActive(this PopupManager self)
        {
            if (!self.ActivePopup) return;
            self.ActivePopup.Hide();
        }
    }
}