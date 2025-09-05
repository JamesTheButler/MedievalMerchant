using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = nameof(LevelInfo), menuName = AssetMenu.ConfigDataFolder + nameof(LevelInfo))]
public sealed class LevelInfo : ScriptableObject
{
    [SerializeField, Required]
    private GameObject mapPrefab;

    [field: SerializeField]
    public int StartPlayerFunds { get; private set; }
    
    public Tilemap Map => mapPrefab.GetComponent<Tilemap>();
}