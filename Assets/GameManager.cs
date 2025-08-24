using System.Collections.Generic;
using Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TilemapManager tilemapManager;

    [SerializeField]
    private GameTicker ticker;

    [Header("Map Setup"), SerializeField]
    private List<Vector2Int> townLocations;

    [SerializeField]
    private List<TownSetupInfo> townInfos;

    [SerializeField]
    private int mapSize;

    private void Awake()
    {
        var towns = GenerateMap();
        Model.Instance.SetTowns(towns);
        ticker.OnTick += Tick;
    }

    private List<Town> GenerateMap()
    {
        var goodInfoManager = Setup.Instance.GoodInfoManager;
        var towns = new List<Town>();

        for (var i = 0; i < townLocations.Count; i++)
        {
            towns.Add(new Town(townInfos[i], townLocations[i], goodInfoManager));
        }

        tilemapManager.InitTilemap(mapSize, townLocations);

        return towns;
    }

    private void Tick()
    {
        foreach (var town in Model.Instance.Towns.Values)
        {
            town.Tick();
        }
    }
}