using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Map
{
    public sealed class ProductionZoneManager : MonoBehaviour
    {
        public List<ProductionZone> Zones { get; private set; }

        [field: SerializeField]
        public UnityEvent<ProductionZone> OnZoneSelected { get; private set; }
    }
}