using System.Collections.Generic;
using System.Linq;
using Common.Types;
using Common.Utility;
using Features.Map;
using Features.Towns.Flags.Logic;
using UnityEngine;

namespace Features.Towns
{
    public sealed class TownFactory
    {
        // 1.5 accounts for diagonally adjacent zones (where distance would be sqrt(2) == 1.41)
        private const float ZoneDistance = 1.5f;

        private readonly FlagFactory _flagFactory = new();

        public List<Town> GenerateTowns(List<Vector2Int> townPositions, ProductionZone[] zones, Grid tileGrid)
        {
            var towns = new List<Town>();
            var zonesPerTown = GetZonesPerTown(townPositions, zones);

            foreach (var townPosition in townPositions)
            {
                var town = GenerateTown(townPosition, zonesPerTown[townPosition], tileGrid);
                towns.Add(town);
            }

            return towns;
        }

        private Town GenerateTown(Vector2Int townPosition, List<ProductionZone> adjacentZones, Grid tileGrid)
        {
            var worldPosition = tileGrid.CellToWorld(townPosition.FromXY());
            var townRegions = adjacentZones.Select(zone => zone.Region.AsRegions()).AggregateFlags();
            var availableGoods = GetAllZoneGoods(adjacentZones);

            var town = new Town(
                townPosition,
                worldPosition,
                townRegions,
                availableGoods,
                _flagFactory);
            return town;
        }

        private static Dictionary<Vector2Int, List<ProductionZone>> GetZonesPerTown(
            List<Vector2Int> townPositions,
            ProductionZone[] zones)
        {
            var zonesPerTown = new Dictionary<Vector2Int, List<ProductionZone>>();

            foreach (var townPosition in townPositions)
            {
                zonesPerTown.Add(townPosition, new List<ProductionZone>());
                foreach (var zone in zones)
                {
                    if (zone.IsAdjacentTo(townPosition, ZoneDistance))
                        zonesPerTown[townPosition].Add(zone);
                }
            }

            return zonesPerTown;
        }

        private static HashSet<Good> GetAllZoneGoods(List<ProductionZone> zones)
        {
            var allGoods = new HashSet<Good>();

            foreach (var good in zones.SelectMany(zone => zone.AvailableGoods))
            {
                allGoods.Add(good);
            }

            return allGoods;
        }
    }
}