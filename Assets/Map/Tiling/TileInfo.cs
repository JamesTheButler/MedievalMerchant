using UnityEngine;

namespace Map.Tiling
{
    public class TileInfo : MonoBehaviour
    {
        [field: SerializeField]
        public TileType TileType { get; private set; }
    }
}