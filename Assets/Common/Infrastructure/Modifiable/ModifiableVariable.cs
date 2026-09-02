using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Infrastructure.Observation;
using Common.Utility;
using UnityEngine;

namespace Common.Infrastructure.Modifiable
{
    public sealed class ModifiableVariable : Observable<float>
    {
        public event Action ModifiersChanged;
        public event Action<IModifier> ModifiersAdded, ModifiersRemoved;

        public IReadOnlyList<IModifier> Modifiers => _modifiers;

        public string Description { get; }
        public bool IsBiggerBetter { get; }
        public BaseValueModifier BaseValueModifier { get; }

        public bool IsModified => _modifiers.Count > 0;

        public float BaseValue => BaseValueModifier?.Value.Value ?? 0f;

        // Both sums are already maintained as modifiers come and go, so these are reads
        // rather than recomputes. TotalPercentage is a fraction: -0.3f reads as -30%.
        public float TotalPercentage => _percentModifiers.Value;
        public float TotalFlat => _flatModifiers.Value;

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
            if (modifier == null)
                return;

            _modifiers.Add(modifier);
            ApplyModifier(modifier);

            ModifiersAdded?.Invoke(modifier);
            ModifiersChanged?.Invoke();
        }

        public void RemoveModifier(IModifier modifier)
        {
            if (modifier == null)
                return;

            if (!_modifiers.Remove(modifier))
                return;

            UnapplyModifier(modifier);
            ModifiersRemoved?.Invoke(modifier);
            ModifiersChanged?.Invoke();
        }

        /// <summary>
        /// Creates a deep copy that is updated alongside the original, i.e. this ModifiableVariable.
        /// </summary>
        public ModifiableVariable Copy()
        {
            var copy = new ModifiableVariable(Description, IsBiggerBetter, BaseValueModifier);
            foreach (var modifier in _modifiers)
            {
                copy.AddModifier(modifier);
            }

            ModifiersAdded += copy.AddModifier;
            ModifiersRemoved += copy.RemoveModifier;

            return copy;
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

            var nonNullModifiers = allOtherModifiers.Where(modifier => !modifier.Value.Value.IsApproximately(0));
            builder.AppendJoin("\n", nonNullModifiers.Select(modifier => modifier.Description));
            return builder.ToString();
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
    }
}