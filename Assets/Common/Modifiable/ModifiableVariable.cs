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

        private readonly ObservableSum _flatModifiers = new();
        private readonly ObservableSum _percentModifiers = new();

        public bool IsBiggerBetter { get; }

        private readonly BaseValueModifier _baseValueModifier;
        private readonly List<IModifier> _modifiers = new();

        public ModifiableVariable(string description, BaseValueModifier baseValue = null, bool isBiggerBetter = false)
        {
            Description = description;
            IsBiggerBetter = isBiggerBetter;

            _baseValueModifier = baseValue;

            _baseValueModifier?.Value.Observe(OnAnyChanged, false);
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
                case BaseValueModifier:
                    Debug.LogError($"Cannot add a {nameof(BaseValueModifier)}.");
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
                case BaseValueModifier:
                    Debug.LogError($"Cannot remove a {nameof(BaseValueModifier)}.");
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
            var baseValue = _baseValueModifier?.Value ?? 0f;
            Value = (baseValue + _flatModifiers) * (1 + _percentModifiers);
        }

        public override string ToString()
        {
            var baseModifier = Modifiers.FirstOfType<BaseValueModifier, IModifier>();
            var allOtherModifiers = Modifiers.Where(modifier => modifier is not BaseValueModifier);

            var builder = new StringBuilder()
                .AppendLine(Value.ToString("0.##"));

            if (_modifiers.Count > 1)
            {
                builder.AppendLine("====================");
            }

            if (baseModifier is not null)
            {
                builder
                    .AppendLine(baseModifier.Description)
                    .AppendLine("--------------------");
            }

            builder
                .AppendJoin("\n", allOtherModifiers.Select(modifier => modifier.Description));
            return builder.ToString();
        }
    }
}