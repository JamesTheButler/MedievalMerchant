using System.Linq;
using Common.Modifiable;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Common.UI
{
    public sealed class ModifiableTooltip : TooltipBase<ModifiableVariable>
    {
        [SerializeField, Required]
        private GameObject modifierItemPrefab;

        [SerializeField, Required]
        private TMP_Text finalValueText, finalDescriptionText, modifierSumText;

        [SerializeField, Required]
        private GameObject flatModifierContainer, percentModifierContainer, percentModifierGroup;

        private ModifiableVariable _modifiableVariable;

        public override void SetData(ModifiableVariable data)
        {
            _modifiableVariable = data;
            _modifiableVariable.Observe(OnValueChanged);
            _modifiableVariable.ModifiersChanged += RegenerateTooltip;
        }

        private void OnDestroy()
        {
            _modifiableVariable.StopObserving(OnValueChanged);
            _modifiableVariable.ModifiersChanged -= RegenerateTooltip;
        }

        private void OnValueChanged(float _)
        {
            RegenerateTooltip();
        }

        private void RegenerateTooltip()
        {
            flatModifierContainer.DestroyChildren();
            percentModifierContainer.DestroyChildren();

            finalValueText.text = $"{_modifiableVariable.Value:0.##}";
            finalDescriptionText.text = _modifiableVariable.Description;

            var baseModifier = _modifiableVariable.Modifiers.FirstOfType<BaseValueModifier, IModifier>();
            if (baseModifier != null)
            {
                AddModifierElement(flatModifierContainer, baseModifier, false);
            }

            var flatModifiers = _modifiableVariable.Modifiers.OfType<FlatModifier>().ToArray<IModifier>();
            AddModifierElements(flatModifiers, flatModifierContainer);

            var percentageModifiers =
                _modifiableVariable.Modifiers.OfType<BasePercentageModifier>().ToArray<IModifier>();
            percentModifierGroup.SetActive(percentageModifiers.Length > 0);
            AddModifierElements(percentageModifiers, percentModifierContainer);
            var sum = percentageModifiers.Sum(modifier => modifier.Value);
            modifierSumText.text = sum.ToPercentString();
        }

        private void AddModifierElements(IModifier[] modifiers, GameObject container)
        {
            foreach (var modifier in modifiers)
            {
                AddModifierElement(container, modifier, true);
            }
        }

        private void AddModifierElement(GameObject container, IModifier modifier, bool useDynamicColor)
        {
            var uiElementObject = Instantiate(modifierItemPrefab, container.transform);
            var uiElement = uiElementObject.GetComponent<ModifierUIElement>();
            uiElement.SetUp(modifier, useDynamicColor, _modifiableVariable.IsBiggerBetter);
        }
    }
}