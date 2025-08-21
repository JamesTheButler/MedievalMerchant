using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public Model Model { get; private set; }

    [FormerlySerializedAs("mapManager"),SerializeField]
    private TilemapManager tilemapManager;
    
    [SerializeField]
    private GameTicker ticker;
    
    [Header("Map Setup"), SerializeField]
    private List<Vector2Int> townLocations;

    [SerializeField]
    private List<TownSetupInfo> townInfos;

    [SerializeField]
    private int mapSize;

    private void Start()
    {
        var towns = GenerateMap();
        Model = new Model(towns);
        
        ticker.OnTick += Tick;
    }

    private List<Town> GenerateMap()
    {
        var towns = new List<Town>();

        for (var i = 0; i < townLocations.Count; i++)
        {
            towns.Add(new Town(townInfos[i], townLocations[i]));
        }
        
        tilemapManager.PlaceTiles(mapSize, townLocations);
        
        return towns;
    }

    private void Tick()
    {
        foreach (var town in Model.Towns)
        {
            town.Tick();
        }
    }
}