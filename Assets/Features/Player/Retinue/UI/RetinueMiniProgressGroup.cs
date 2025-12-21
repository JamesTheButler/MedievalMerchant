using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinueMiniProgressGroup : MonoBehaviour
    {
        [field: SerializeField]
        public CompanionType CompanionType { get; private set; }

        [SerializeField]
        private CompanionTooltipHandler tooltip;

        private RetinueMiniProgressElement[] _elements;

        private void Start()
        {
            _elements = GetComponentsInChildren<RetinueMiniProgressElement>();

            tooltip.SetData(new CompanionTooltip.Data(CompanionType, 0));
        }

        public void SetProgress(int level)
        {
            for (var i = 0; i < _elements.Length; i++)
            {
                _elements[i].SetCompleted(i < level);
            }

            tooltip.SetData(new CompanionTooltip.Data(CompanionType, level));
        }
    }
}