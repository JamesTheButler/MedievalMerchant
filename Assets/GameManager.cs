using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Model Model { get; private set; }

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
    
    [SerializeField]
    private TransactionUI transactionUI;

    private void Awake()
    {
        var towns = GenerateMap();
        Model = new Model(towns);
    }

    private void Start()
    {
        ticker.OnTick += Tick;
        
        transactionUI.Initialize(Good.Leather, TransactionType.Buy, 250);
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
        foreach (var town in Model.Towns.Values)
        {
            town.Tick();
        }
    }
}