using System.Collections.Generic;
using System.Linq;
using Common;
using Data;
using Data.Configuration;
using Data.Towns;
using Map.Tiling;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public sealed class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private UnityEvent levelLoaded;

    [SerializeField, Required]
    private Grid tileGrid;

    [SerializeField, Required]
    private LevelInfo levelInfo;

    [SerializeField]
    private List<TownSetupInfo> townInfos;

    private Model _model;

    private void Start()
    {
        _model = Model.Instance;

        var tilemap = Instantiate(levelInfo.Map, tileGrid.gameObject.transform);
        var flagMap = TilemapScanner.Scan(tilemap);
        var townPositions = flagMap.GetAllCells(TileType.Town);
        var towns = townPositions
            .Select(pos => new Town(townInfos.GetRandom(), pos, tileGrid.CellToWorld(pos.FromXY())))
            .ToList();
        var player = new Player(levelInfo.StartPlayerFunds);

        _model.Initialize(player, towns, flagMap);

        var startTown = towns.GetRandom();
        player.Location.CurrentTown = startTown;
        player.Location.WorldLocation.Value = startTown.WorldLocation;

        levelLoaded.Invoke();
    }
}