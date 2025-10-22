using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Common.UI.Test
{
    public sealed class DynamicTestTooltip : TooltipBase<DynamicTestTooltip.Data>
    {
        public sealed record Data(string Title, List<string> Contents);

        [SerializeField, Required]
        private TMP_Text titleText;

        [SerializeField, Required]
        private GameObject dynamicContainer;

        [SerializeField, Required]
        private GameObject testElementPrefab;

        protected override void UpdateUI(Data data)
        {
            titleText.text = data.Title;
            dynamicContainer.DestroyChildren();
            foreach (var entry in data.Contents)
            {
                var element = Instantiate(testElementPrefab, dynamicContainer.transform);
                element.GetComponentInChildren<TMP_Text>().text = entry;
            }
        }

        public override void Reset() { }
    }
}