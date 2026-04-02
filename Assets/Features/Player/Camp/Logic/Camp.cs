using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Features.Inventory;
using Features.Map.Pathfinding;
using Features.Map.Tiling;
using UnityEngine;

namespace Features.Player.Camp.Logic
{
    public sealed class Camp : IMapLocation
    {
        public Inventory.Inventory Inventory { get; }

        public Vector2Int GridLocation { get; }
        public Vector2 WorldLocation { get; }

        public Observable<CampMapTile> MapTile { get; }

        public Camp(Vector2Int gridLocation, Vector2 worldLocation, CampMapTile tile)
        {
            GridLocation = gridLocation;
            WorldLocation = worldLocation;
            MapTile = new Observable<CampMapTile>(tile);

            var campConfig = ConfigurationManager.Configurations.CampConfig;

            var inventoryPolicy = new SlotCountInventoryPolicy(campConfig.InventorySlotCount);
            Inventory = new Inventory.Inventory(inventoryPolicy);
        }
    }
}