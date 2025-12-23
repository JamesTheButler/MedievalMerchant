using Common.Utility;
using UnityEngine;

namespace Features.Levels.GameModifiers.Data
{
    [CreateAssetMenu(
        fileName = nameof(EventGameModifierData),
        menuName = AssetMenu.GameplayModifiersFolder + nameof(EventGameModifierData))]
    public sealed class EventGameModifierData : GameModifierData { }
}