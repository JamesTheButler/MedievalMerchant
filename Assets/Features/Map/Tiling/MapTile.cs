using UnityEngine;

namespace Features.Map.Tiling
{
    public class MapTile : MonoBehaviour
    {
        [field: SerializeField]
        public TileType TileType { get; private set; }
    }
}