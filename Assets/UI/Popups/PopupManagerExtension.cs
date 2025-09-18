namespace UI.Popups
{
    public static class PopupManagerExtension
    {
        public static void HideActive(this PopupManager self)
        {
            self.Hide(self.ActivePopup);
        }
    }
}