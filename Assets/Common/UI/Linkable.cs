using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI
{
    public sealed class Linkable : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text _tmpText;
        private Camera _camera;

        private void Awake()
        {
            _tmpText = GetComponent<TMP_Text>();
            _camera = Camera.main;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var linkIndex = TMP_TextUtilities.FindIntersectingLink(_tmpText, eventData.position, _camera);
            if (linkIndex == -1)
                return;

            var linkInfo = _tmpText.textInfo.linkInfo[linkIndex];
            var linkId = linkInfo.GetLinkID();

            Application.OpenURL(linkId);
        }
    }
}