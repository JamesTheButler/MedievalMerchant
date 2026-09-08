using System.Collections.Generic;
using Common.Infrastructure;
using Common.Utility;
using Features.Combat.Logic;
using JetBrains.Annotations;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Combat.UI
{
    public sealed class TeamBattleUI : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text partyNameText;

        [SerializeField, Required]
        private Image tierIcon;

        [SerializeField, Required]
        private UnitCountRow unitCountRow;

        [SerializeField, Required]
        private RectTransform topRow, bottomRow;

        [SerializeField, Required]
        private UnitToken unitTokenPrefab;

        [SerializeField, Required]
        private Sprite unitCountIcon;

        [SerializeField]
        private int maxUnitsPerRow = 10;

        private Combatant _combatant;

        private readonly Dictionary<CombatUnit, UnitToken> _tokens = new();

        public void Bind(Combatant combatant, string partyName)
        {
            Unbind();

            if (combatant == null)
                return;

            _combatant = combatant;

            partyNameText.text = partyName;
            tierIcon.sprite = ResourceManager.Instance.TierResources.GetTierIconByLevel(combatant.Level);

            unitCountRow.SetCount(unitCountIcon, combatant.AliveCount, combatant.UnitCount);

            SpawnTokens();
        }

        public void Unbind()
        {
            if (_combatant == null)
                return;

            _tokens.Clear();
            topRow.DestroyChildren();
            bottomRow.DestroyChildren();

            _combatant = null;
        }

        [CanBeNull]
        public UnitToken GetToken(CombatUnit unit)
        {
            return unit != null && _tokens.TryGetValue(unit, out var token) ? token : null;
        }

        public void SyncAllTokens()
        {
            foreach (var token in _tokens.Values)
            {
                token.SyncToModel();
            }
        }

        private void SpawnTokens()
        {
            var units = _combatant.Units;
            var perRow = units.Count <= maxUnitsPerRow
                ? units.Count
                : Mathf.CeilToInt(units.Count / 2f);

            for (var index = 0; index < units.Count; index++)
            {
                var unit = units[index];
                var parent = index < perRow ? topRow : bottomRow;

                var token = Instantiate(unitTokenPrefab, parent);
                token.SetUnit(_combatant.UnitIcon, unit);

                _tokens[unit] = token;
            }

            bottomRow.gameObject.SetActive(units.Count > perRow);
        }

        private void OnDestroy()
        {
            Unbind();
        }
    }
}