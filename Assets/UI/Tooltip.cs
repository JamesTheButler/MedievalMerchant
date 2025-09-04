using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace UI
{
    public sealed class Tooltip : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text textfield;

        public void SetText(string text)
        {
            textfield.text = text;
        }
    }
}