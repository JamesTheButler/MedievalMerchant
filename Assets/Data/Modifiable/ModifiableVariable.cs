using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Data.Modifiable
{
    public sealed class ModifiableVariable : Observable<float>
    {
        public float BaseValue { get; private set; }
        public IReadOnlyList<IModifier> Modifiers => _modifiers;

        private float _percentageChanges;

        private readonly List<IModifier> _modifiers = new();

        public ModifiableVariable(BaseValueModifier baseValue)
        {
            AddModifier(baseValue);
        }

        public ModifiableVariable(BaseValueModifier baseValue, IEnumerable<IModifier> modifiers)
        {
            AddModifier(baseValue);

            foreach (var modifier in modifiers)
            {
                ApplyModifier(modifier);
            }
        }

        public void AddModifier(IModifier modifier)
        {
            ApplyModifier(modifier);
        }

        public void RemoveModifier(IModifier modifier)
        {
            UnapplyModifier(modifier);
        }

        private void ApplyModifier(IModifier modifier)
        {
            _modifiers.Add(modifier);
            switch (modifier)
            {
                case BaseValueModifier baseModifier:
                    BaseValue = baseModifier.Value;
                    break;
                case BasePercentageModifier basePercentageModifier:
                    _percentageChanges += basePercentageModifier.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier));
            }

            RefreshValue();
        }

        private void UnapplyModifier(IModifier modifier)
        {
            _modifiers.Add(modifier);
            switch (modifier)
            {
                case BaseValueModifier:
                    Debug.LogError($"{nameof(BaseValueModifier)} should never be removed");
                    break;
                case BasePercentageModifier basePercentageModifier:
                    _percentageChanges -= basePercentageModifier.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier));
            }

            RefreshValue();
        }

        private void RefreshValue()
        {
            Value = BaseValue * (1 + _percentageChanges);
        }
    }
}