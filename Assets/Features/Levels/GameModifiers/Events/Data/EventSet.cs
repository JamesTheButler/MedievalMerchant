using System.Collections.Generic;
using Common.Utility;
using UnityEngine;

namespace Features.Levels.GameModifiers.Events.Data
{
    [CreateAssetMenu(
        fileName = nameof(EventSet),
        menuName = AssetMenu.EventsFolder + nameof(EventSet))]
    public sealed class EventSet : ScriptableObject
    {
        [SerializeField]
        private List<EventGameModifierData> availableEvents;

        public IReadOnlyList<EventGameModifierData> AvailableEvents => availableEvents;
    }
}