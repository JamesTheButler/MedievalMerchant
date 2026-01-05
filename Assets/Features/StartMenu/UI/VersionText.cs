using TMPro;
using UnityEngine;

namespace Features.StartMenu.UI
{
    public sealed class VersionText : MonoBehaviour
    {
        private void Awake()
        {
            var versionText = GetComponent<TMP_Text>();
            versionText.text = $"v. {Application.version}";
        }
    }
}