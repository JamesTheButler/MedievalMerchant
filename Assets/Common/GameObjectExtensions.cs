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
            DestroyChildren(gameObject.transform);
        }

        /// <summary>
        /// Destroys all children that have the specified component type.
        /// </summary>
        public static void DestroyChildren<TComponent>(this GameObject gameObject) where TComponent : Component
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.GetComponent<TComponent>())
                {
                    Object.Destroy(child.gameObject);
                }
            }
        }
    }
}