using System.Collections.Generic;
using System.Linq;

namespace Data
{
    // TODO: make this a ScriptableObject, no?
    public sealed class PlayerUpgradeProgression
    {
        public IReadOnlyList<List<PlayerUpgrade>> UpgradeProgressions => _upgradeProgressions;

        // ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
        private readonly List<List<PlayerUpgrade>> _upgradeProgressions = new()
        {
            new()
            {
                PlayerUpgrade.Tier1Slots3,
                PlayerUpgrade.Tier1Slots6,
                PlayerUpgrade.Tier1Slots9,
                PlayerUpgrade.Tier1Slots12
            },
            new()
            {
                PlayerUpgrade.Tier2Slots3,
                PlayerUpgrade.Tier2Slots6
            },
            new()
            {
                PlayerUpgrade.Tier3Slots3,
                PlayerUpgrade.Tier3Slots6
            },
        };

        public PlayerUpgrade GetNext(PlayerUpgrade upgrade)
        {
            foreach (var list in _upgradeProgressions)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    if (list[i] == upgrade)
                    {
                        return list.ElementAtOrDefault(i + 1);
                    }
                }
            }

            return PlayerUpgrade.None;
        }
    }
}