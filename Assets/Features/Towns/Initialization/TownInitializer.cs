using Common.Utility;
using UnityEngine;

namespace Features.Towns.Initialization
{
    public sealed class TownInitializer : MonoBehaviour
    {
        [SerializeField]
        private Color tileOutline = Color.green;

        [field: SerializeField]
        public TownInitializationData InitializationData { get; private set; }

        public Vector2Int GridPosition => new(
            Mathf.FloorToInt(gameObject.transform.position.x),
            Mathf.FloorToInt(gameObject.transform.position.y));

        private void OnDrawGizmos()
        {
            MyGizmos.DrawRect(new Rect(GridPosition, Vector2.one), tileOutline);
        }
    }
}