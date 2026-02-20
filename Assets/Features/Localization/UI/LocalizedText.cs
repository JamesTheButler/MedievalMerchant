using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.UI
{
    public sealed class LocalizedText : MonoBehaviour
    {
        [SerializeField]
        private LocalizedString staticString;

        [SerializeField]
        private bool autoFindTextfield = true;

        [SerializeField, HideIf(nameof(autoFindTextfield))]
        private TMP_Text textfield;

        private void OnEnable()
        {
            if (autoFindTextfield)
            {
                textfield = GetComponent<TMP_Text>();
            }

            Refresh();
        }

        private void OnValidate()
        {
            if (autoFindTextfield)
            {
                textfield = GetComponent<TMP_Text>();
            }

            Refresh();
        }

        private void Refresh()
        {
            textfield.text = staticString.IsEmpty ? "<none>" : staticString.GetLocalizedString();
        }
    }
}