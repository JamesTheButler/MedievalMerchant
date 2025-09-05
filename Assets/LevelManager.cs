using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Configuration;
using Data.Setup;
using Data.Towns;
using NaughtyAttributes;
using Tilemap;
using UnityEngine;
using UnityEngine.Events;

public sealed class LevelManager : MonoBehaviour
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
        var towns = townPositions.Select(pos => new Town(townInfos.GetRandom(), pos));
        var player = new Player(levelInfo.StartPlayerFunds);

        _model.Initialize(player, towns, flagMap);

        levelLoaded.Invoke();
    }

    public void Tick()
    {
        foreach (var town in _model.Towns.Values)
        {
            town.Tick();
        }
    }
}