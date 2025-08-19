using System.Collections.Generic;
using Data;
using Model;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private MapManager mapManager;
    
    [SerializeField]
    private GameTicker ticker;
    
    [SerializeField]
    private List<Vector2Int> townLocations;

    [SerializeField]
    private List<TownSetupInfo> townInfos;

    [SerializeField]
    private int mapSize;

    private List<Town> _towns;

    private void Start()
    {
        GenerateMap();
        
        ticker.OnTick += TickTowns;
    }

    private void GenerateMap()
    {
        for (var i = 0; i < townLocations.Count; i++)
        {
            _towns.Add(new Town(townInfos[i]));
        }
        
        mapManager.PlaceTiles(mapSize, townLocations);
    }

    private void TickTowns()
    {
        foreach (var town in _towns)
        {
            town.Tick();
        }
    }
}