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
            var cursor = _cursors.Pointer;
            Cursor.SetCursor(cursor.Texture, cursor.HotSpot, CursorMode.Auto);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var cursor = _cursors.Default;
            Cursor.SetCursor(cursor.Texture, cursor.HotSpot, CursorMode.Auto);
        }
    }
}