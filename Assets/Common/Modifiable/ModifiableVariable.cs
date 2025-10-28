using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Common.Modifiable
{
    public sealed class ModifiableVariable : Observable<float>
    {
        public event Action ModifiersChanged;

        public IReadOnlyList<IModifier> Modifiers => _modifiers;

        public string Description { get; private set; }
        public bool IsBiggerBetter { get; }
        public BaseValueModifier BaseValueModifier { get; }

        private readonly ObservableSum _flatModifiers = new();
        private readonly ObservableSum _percentModifiers = new();
        private readonly List<IModifier> _modifiers = new();

        public ModifiableVariable(string description, bool isBiggerBetter, BaseValueModifier baseValue = null)
        {
            Description = description;
            IsBiggerBetter = isBiggerBetter;

            BaseValueModifier = baseValue;

            BaseValueModifier?.Value.Observe(OnAnyChanged);
            _flatModifiers.Observe(OnAnyChanged, false);
            _percentModifiers.Observe(OnAnyChanged, false);
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
                case Modifiable.BaseValueModifier:
                    Debug.LogError($"Cannot add a {nameof(Modifiable.BaseValueModifier)}.");
                    break;
                case FlatModifier:
                    _flatModifiers.AddValue(modifier.Value);
                    break;
                case BasePercentageModifier:
                    _percentModifiers.AddValue(modifier.Value);
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
                case Modifiable.BaseValueModifier:
                    Debug.LogError($"Cannot remove a {nameof(Modifiable.BaseValueModifier)}.");
                    break;
                case FlatModifier:
                    _flatModifiers.RemoveValue(modifier.Value);
                    break;
                case BasePercentageModifier:
                    _percentModifiers.RemoveValue(modifier.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(modifier.GetType().FullName);
            }

            RefreshValue();
        }

        private void OnAnyChanged(float _)
        {
            RefreshValue();
        }

        private void RefreshValue()
        {
            var baseValue = BaseValueModifier?.Value ?? 0f;
            Value = (baseValue + _flatModifiers) * (1 + _percentModifiers);
        }

        public override string ToString()
        {
            var allOtherModifiers = Modifiers.Where(modifier => modifier is not Modifiable.BaseValueModifier);

            var builder = new StringBuilder()
                .AppendLine($"{BaseValueModifier.FormattedValue} .. {BaseValueModifier.Description}");

            if (_modifiers.Count > 1)
            {
                builder.AppendLine("====================");
            }

            if (BaseValueModifier is not null)
            {
                builder
                    .AppendLine(BaseValueModifier.Description)
                    .AppendLine("--------------------");
            }

            builder
                .AppendJoin("\n", allOtherModifiers.Select(modifier => modifier.Description));
            return builder.ToString();
        }
    }
}