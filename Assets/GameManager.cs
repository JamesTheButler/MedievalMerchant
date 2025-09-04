using System.Collections.Generic;
using Data;
using Data.Setup;
using Data.Towns;
using NaughtyAttributes;
using Tilemap;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    [SerializeField, Required]
    private TilemapManager tilemapManager;

    [SerializeField, Required]
    private GameTicker ticker;

    [Header("Map Setup"), SerializeField]
    private List<Vector2Int> townLocations;

    [SerializeField]
    private List<TownSetupInfo> townInfos;

    [SerializeField]
    private int mapSize;

    private Model _model;

    private void Awake()
    {
        _model = Model.Instance;
        var towns = GenerateMap();
        _model.SetTowns(towns);
        ticker.OnTick += Tick;
    }

    private List<Town> GenerateMap()
    {
        var towns = new List<Town>();

        for (var i = 0; i < townLocations.Count; i++)
        {
            towns.Add(new Town(townInfos[i], townLocations[i]));
        }

        tilemapManager.InitTilemap(mapSize, townLocations);

        return towns;
    }

    private void Tick()
    {
        foreach (var town in _model.Towns.Values)
        {
            town.Tick();
        }
    }
}