using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Common.UI
{
    public sealed class SimpleTooltip : TooltipBase<string>
    {
        [SerializeField, Required]
        private TMP_Text textfield;

        public override void SetData(string data)
        {
            textfield.text = data;
        }

        public override void Reset()
        {
            textfield.text = string.Empty;
        }
    }
}