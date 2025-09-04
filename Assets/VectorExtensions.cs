using UnityEngine;

public static class VectorExtensions
{
    public static Vector2Int XY(this Vector3Int vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }

    public static Vector3 Clamp(Vector3 self, Bounds bounds)
    {
        return new Vector3(
            Mathf.Clamp(self.x, bounds.min.x, bounds.max.x),
            Mathf.Clamp(self.y, bounds.min.y, bounds.max.y),
            Mathf.Clamp(self.z, bounds.min.z, bounds.max.z)
        );
    }

    public static Vector3Int FromXY(this Vector2Int vector, int z = 0)
    {
        return new Vector3Int(vector.x, vector.y, z);
    }
}