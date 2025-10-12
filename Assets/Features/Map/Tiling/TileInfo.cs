using UnityEngine;

namespace Features.Map.Tiling
{
    public class TileInfo : MonoBehaviour
    {
        [field: SerializeField]
        public TileType TileType { get; private set; }
    }
}