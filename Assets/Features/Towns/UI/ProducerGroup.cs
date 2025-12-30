using System;
using System.Collections.Generic;
using Common.Types;
using Common.UI.Tooltips;
using Common.UI.Utility;
using Features.Towns.Production.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Towns.UI
{
    public sealed class ProducerGroup : MonoBehaviour
    {
        public event Action<ProductionCell, Tier> UpgradeButtonClicked;
        public event Action<ProductionCell> ProductionCellClicked;

        private readonly Dictionary<Tier, ProductionCell> _productionCells = new();

        [SerializeField, Required]
        private GameObject unavailableGroup;

        [SerializeField, Required]
        private ProductionCell t1Cell, t2Cell, t3Cell;

        [SerializeField, Required]
        private TMP_Text titleText;

        private int _producerIndex;
        private bool _isAvailable;

        private void Awake()
        {
            _productionCells.Add(Tier.Tier1, t1Cell);
            _productionCells.Add(Tier.Tier2, t2Cell);
            _productionCells.Add(Tier.Tier3, t3Cell);
        }

        public void Initialize(int producerIndex, bool isAvailable)
        {
            _isAvailable = isAvailable;
            unavailableGroup.SetActive(!isAvailable);
            _producerIndex = producerIndex;
            var style = isAvailable ? Style.Default : Style.Subtitle;
            titleText.text = $"Producer {_producerIndex}".WithStyle(style);
        }

        public void Bind(ProductionManager productionManager)
        {
            if (!_isAvailable) return;

            productionManager.ProductionAddedIndexed += OnProducerAdded;
        }

        private void OnProducerAdded(Producer producer, int producerIndex)
        {
            if (!_isAvailable) return;

            if (producerIndex != _producerIndex)
                return;
        }

        public void Reset()
        {
            if (!_isAvailable) return;

            foreach (var productionCell in _productionCells.Values)
            {
                // reset and hide all
            }
        }
    }
}