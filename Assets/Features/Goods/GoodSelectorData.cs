using System;
using Common.Types;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods
{
    [Serializable]
    public sealed class GoodSelectorData
    {
        [SerializeField]
        private bool specificGood;

        [SerializeField]
        private bool applyToAll;

        [SerializeField, HideIf(nameof(specificGood))]
        private bool limitRegion;

        [SerializeField, HideIf(nameof(specificGood))]
        private bool limitTier;

        [SerializeField, ShowIf(nameof(specificGood))]
        private Good good;

        [SerializeField, HideIf(nameof(specificGood)), ShowIf(nameof(limitRegion))]
        private Regions region;

        [SerializeField, HideIf(nameof(specificGood)), ShowIf(nameof(limitTier))]
        private Tier tier;

        [CanBeNull]
        private IGoodSelector _selector;

        public IGoodSelector Selector => _selector ??= GetSelector();

        private IGoodSelector GetSelector()
        {
            if (applyToAll) return new AllGoodsSelector();
            if (specificGood) return new SpecificGoodSelector(good);

            return new ComplexGoodSelector(limitTier ? tier : null, limitRegion ? region : Regions.All);
        }
    }
}