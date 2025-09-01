using Art.Tiles;
using Data.Configuration;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tiles", menuName = AssetMenu.RootFolder + "Tiles")]
public sealed class Tiles : ScriptableObject
{
    [field: SerializeField]
    public Tile GrassTile { get; private set; }

    [field: SerializeField]
    public RandomTileSet TownTileSetT1 { get; private set; }

    [field: SerializeField]
    public RandomTileSet TownTileSetT2 { get; private set; }

    [field: SerializeField]
    public RandomTileSet TownTileSetT3 { get; private set; }

    [field: SerializeField]
    public Tile SelectionTile { get; private set; }

    [field: SerializeField]
    public RuleTile FrameTile { get; private set; }

    [field: SerializeField]
    public TileBase FrameTile2 { get; private set; }
}