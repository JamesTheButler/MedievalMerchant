using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Data.Modifiable
{
    public sealed class ModifiableVariable : Observable<float>
    {
        public event Action ModifiersChanged;

        public float BaseValue { get; private set; }
        public IReadOnlyList<IModifier> Modifiers => _modifiers;

        private float _percentageChanges;
        private float _floatChanges;

        private readonly List<IModifier> _modifiers = new();

        public ModifiableVariable()
        {
        }

        public ModifiableVariable(BaseValueModifier baseValue)
        {
            AddModifier(baseValue);
        }

        public void AddModifier(IModifier modifier)
        {
            if (modifier == null) return;

            _modifiers.Add(modifier);
            ApplyModifier(modifier);
            ModifiersChanged?.Invoke();
        }

        public void RemoveModifier(IModifier modifier)
        {
            if (modifier == null) return;

            _modifiers.Remove(modifier);
            UnapplyModifier(modifier);
            ModifiersChanged?.Invoke();
        }

        private void ApplyModifier(IModifier modifier)
        {
            if (modifier == null) return;

            switch (modifier)
            {
                case BaseValueModifier baseModifier:
                    BaseValue = baseModifier.Value;
                    break;
                case FlatModifier flatModifier:
                    _floatChanges += flatModifier.Value;
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
            if (modifier == null) return;

            switch (modifier)
            {
                case BaseValueModifier:
                    Debug.LogError($"{nameof(BaseValueModifier)} should never be removed");
                    break;
                case BasePercentageModifier basePercentageModifier:
                    _percentageChanges -= basePercentageModifier.Value;
                    break;
                case FlatModifier flatModifier:
                    _floatChanges -= flatModifier.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(modifier.GetType().FullName);
            }

            RefreshValue();
        }

        private void RefreshValue()
        {
            Value = BaseValue * (1 + _percentageChanges) + _floatChanges;
        }
    }
}