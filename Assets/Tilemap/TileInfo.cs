using UnityEngine;

namespace Tilemap
{
    public class TileInfo : MonoBehaviour
    {
        [field: SerializeField]
        public TileType TileType { get; private set; }
    }
}