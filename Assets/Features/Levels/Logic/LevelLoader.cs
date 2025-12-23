using Features.Levels.Conditions.Data;
using Features.Map;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Levels.Logic
{
    public sealed class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent levelLoaded;

        [SerializeField, Required]
        private Grid tileGrid;

        [SerializeField, Required]
        private LevelInfo debugLevelInfo;

        [SerializeField, Required]
        private ProductionZoneInteractions productionZoneInteractions;

        private LevelInfo _levelInfo;

        private void Start()
        {
            LoadLevel();
        }

        private void LoadLevel()
        {
            var level = Instantiate(_levelInfo.MapPrefab, tileGrid.gameObject.transform);
            var zones = level.GetComponentsInChildren<ProductionZone>();
            productionZoneInteractions = FindAnyObjectByType<ProductionZoneInteractions>();
            productionZoneInteractions.Initialize(zones);
        }

    }
}