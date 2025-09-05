using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelInfo), menuName = AssetMenu.ConfigDataFolder + nameof(LevelInfo))]
public sealed class LevelInfo : ScriptableObject
{
    [SerializeField, Required]
    private GameObject mapPrefab;

    [field: SerializeField]
    public int StartPlayerFunds { get; private set; }
    
    public UnityEngine.Tilemaps.Tilemap Map => mapPrefab.GetComponent<UnityEngine.Tilemaps.Tilemap>();
}