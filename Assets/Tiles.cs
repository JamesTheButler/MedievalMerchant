using Data.Configuration;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tiles", menuName = AssetMenu.RootFolder + "Tiles")]
public sealed class Tiles : ScriptableObject
{
    [field: SerializeField]
    public TileBase GrassTile { get; private set; }

    [field: SerializeField]
    public TileBase TownTileT1 { get; private set; }

    [field: SerializeField]
    public TileBase TownTileT2 { get; private set; }

    [field: SerializeField]
    public TileBase TownTileT3 { get; private set; }

    [field: SerializeField]
    public Tile SelectionTile { get; private set; }

    [field: SerializeField]
    public RuleTile FrameTile { get; private set; }

    [field: SerializeField]
    public TileBase FrameTile2 { get; private set; }
}