using Common.Utility;
using UnityEngine;

namespace Features.Levels.GameModifiers.Data
{
    [CreateAssetMenu(
        fileName = nameof(LevelGameModifierData),
        menuName = AssetMenu.GameplayModifiersFolder + nameof(LevelGameModifierData))]
    public sealed class LevelGameModifierData : GameModifierData { }
}