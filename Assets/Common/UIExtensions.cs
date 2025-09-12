using TMPro;
using UnityEngine.UI;

namespace Common
{
    public static class UIExtensions
    {
        public static TMP_Text GetText(this Button button)
        {
            return button.GetComponentInChildren<TMP_Text>();
        }
    }
}