using System;
using Common.Config;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI
{
    public sealed class Linkable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private string link;

        private Cursors _cursors;

        private void Awake()
        {
            _cursors = ConfigurationManager.Instance.Cursors;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(link);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Use the system default hand cursor
            Cursor.SetCursor(_cursors.Pointer, Vector2.zero, CursorMode.Auto);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Revert to your normal cursor (also system default)
            Cursor.SetCursor(_cursors.Default, Vector2.zero, CursorMode.Auto);
        }
    }
}