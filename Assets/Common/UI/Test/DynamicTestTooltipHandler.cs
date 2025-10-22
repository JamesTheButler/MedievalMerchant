using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UI.Test
{
    public sealed class DynamicTestTooltipHandler : TooltipHandlerBase<DynamicTestTooltip.Data>
    {
        [SerializeField]
        private float regenerateInterval = 2f;

        [SerializeField]
        private int maxContentLength = 5, maxElementCount = 5;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(RegenerateRoutine());
        }

        private IEnumerator RegenerateRoutine()
        {
            var wait = new WaitForSeconds(regenerateInterval);

            while (true)
            {
                RegenerateData();
                yield return wait;
            }
        }

        private void RegenerateData()
        {
            SetData(GenerateRandomData());
        }

        private DynamicTestTooltip.Data GenerateRandomData()
        {
            var elementCount = Random.Range(1, maxElementCount + 1);

            var title = $"{elementCount} elements";
            var contents = new List<string>();

            for (var i = 0; i < elementCount; i++)
            {
                var y = Random.Range(1, maxContentLength + 1);
                var contentString = string.Empty;
                for (var j = 0; j <= y; j++)
                {
                    contentString += "content ";
                }

                contents.Add(contentString);
            }

            return new DynamicTestTooltip.Data(title, contents);
        }
    }
}