using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Common.UI.Elements
{
    public sealed class GoodCellClickHandler : InitializableBehavior
    {
        [SerializeField]
        private UnityEvent<GoodCell> cellClicked;

        [SerializeField]
        private bool autoFindCells;

        [SerializeField, HideIf(nameof(autoFindCells))]
        private List<GoodCell> goodCells;

        public override void Initialize()
        {
            if (autoFindCells)
            {
                goodCells = gameObject
                    .GetComponentsInChildren<GoodCell>()
                    .ToList();
            }

            foreach (var cell in goodCells)
            {
                Add(cell);
            }
        }

        public void Add(GoodCell cell)
        {
            cell.Clicked += () => { cellClicked?.Invoke(cell); };
        }
    }
}