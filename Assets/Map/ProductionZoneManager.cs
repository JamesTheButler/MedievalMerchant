using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Map
{
    public sealed class ProductionZoneManager : MonoBehaviour
    {
        public List<ProductionZone> Zones { get; private set; }

        [field: SerializeField]
        public UnityEvent<ProductionZone> OnZoneSelected { get; private set; }
    }
}