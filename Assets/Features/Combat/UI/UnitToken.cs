using System.Collections;
using Features.Combat.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    // The token deliberately does NOT observe CombatUnit.Health. The logic resolves a
    // round instantly, so by the time the first sword is in the air the model is already
    // at its final value. The token walks its own displayed health down as each attack
    // lands, and derives death from that rather than from IsAlive - otherwise a unit
    // would grey out before the blow that killed it arrives.
    public sealed class UnitToken : MonoBehaviour
    {
        [SerializeField, Required, Tooltip("Image Type must be Filled / Radial 360.")]
        private Image healthFill;

        [SerializeField, Required]
        private Image character;

        [SerializeField, Required]
        private GameObject blinker;

        [SerializeField, Required]
        private CanvasGroup canvasGroup;

        [SerializeField, Required]
        private UnitTokenTooltipHandler tooltipHandler;

        [SerializeField]
        private Gradient healthGradient;

        [SerializeField]
        private float blinkSeconds = 0.2f;

        [SerializeField]
        private float deadAlpha = 0.3f;

        private CombatUnit _unit;
        private Coroutine _blink;
        private float _displayHealth;

        public CombatUnit Unit => _unit;

        public void SetUnit(Sprite characterIcon, CombatUnit unit)
        {
            _unit = unit;
            tooltipHandler.SetData(_unit);

            character.sprite = characterIcon;

            if (_unit == null)
                return;

            SyncToModel();
        }

        public void ApplyHit(float damage)
        {
            if (_unit == null)
                return;

            _displayHealth = Mathf.Max(0f, _displayHealth - damage);

            Render();
            Blink();
        }

        public void SyncToModel()
        {
            if (_unit == null)
                return;

            _displayHealth = _unit.Health.Value;
            Render();
        }

        public void Blink()
        {
            if (!isActiveAndEnabled)
                return;

            if (_blink != null)
            {
                StopCoroutine(_blink);
            }

            _blink = StartCoroutine(BlinkRoutine());
        }

        private IEnumerator BlinkRoutine()
        {
            blinker.SetActive(true);
            yield return new WaitForSeconds(blinkSeconds);
            blinker.SetActive(false);
            _blink = null;
        }

        private void Render()
        {
            var fraction = _unit.MaxHealth <= 0f ? 0f : Mathf.Clamp01(_displayHealth / _unit.MaxHealth);

            healthFill.fillAmount = fraction;
            healthFill.color = healthGradient.Evaluate(fraction);
            canvasGroup.alpha = _displayHealth > 0f ? 1f : deadAlpha;
        }
    }
}