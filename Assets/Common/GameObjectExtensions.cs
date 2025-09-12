using UnityEngine;

namespace Common
{
    public static class GameObjectExtensions
    {
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static void DestroyChildren(this GameObject gameObject)
        {
            foreach (Transform child in gameObject.transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}