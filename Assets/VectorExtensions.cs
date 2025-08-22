using UnityEngine;

public static class VectorExtensions
{
    public static Vector2Int XY(this Vector3Int vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }
}