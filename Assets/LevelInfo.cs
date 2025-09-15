using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(LevelInfo), menuName = AssetMenu.ConfigDataFolder + nameof(LevelInfo))]
public sealed class LevelInfo : ScriptableObject
{
    [field: SerializeField, Required]
    public GameObject MapPrefab { get; private set; }

    [field: SerializeField]
    public string LevelName { get; private set; }

    [field: SerializeField]
    public int StartPlayerFunds { get; private set; }
}