using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tiles", menuName = AssetMenu.RootFolder + "Tiles")]
public sealed class Tiles : ScriptableObject
{
    [field: SerializeField, Required]
    public TileBase GrassTile { get; private set; }

    [field: SerializeField, Required]
    public TileBase TownTileT1 { get; private set; }

    [field: SerializeField, Required]
    public TileBase TownTileT2 { get; private set; }

    [field: SerializeField, Required]
    public TileBase TownTileT3 { get; private set; }

    [field: SerializeField, Required]
    public Tile SelectionTile { get; private set; }

    [field: SerializeField, Required]
    public RuleTile FrameTile { get; private set; }

    [field: SerializeField, Required]
    public TileBase FrameTile2 { get; private set; }
}