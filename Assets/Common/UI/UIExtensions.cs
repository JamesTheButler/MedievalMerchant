using TMPro;
using UnityEngine.UI;

namespace Common.UI
{
    public static class UIExtensions
    {
        public static TMP_Text GetText(this Button button)
        {
            return button.GetComponentInChildren<TMP_Text>();
        }

        public static void Clear(this TMP_InputField inputField)
        {
            inputField.text = string.Empty;
        }
    }
}