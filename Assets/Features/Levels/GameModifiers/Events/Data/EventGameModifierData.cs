using Common.Utility;
using Features.Levels.GameModifiers.Data;
using UnityEngine;

namespace Features.Levels.GameModifiers.Events.Data
{
    [CreateAssetMenu(
        fileName = nameof(EventGameModifierData),
        menuName = AssetMenu.GameplayModifiersFolder + nameof(EventGameModifierData))]
    public sealed class EventGameModifierData : GameModifierData { }
}