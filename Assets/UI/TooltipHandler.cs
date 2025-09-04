using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public sealed class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Required]
        private GameObject toolTipPrefab;

        private Tooltip _activeToolTip;
        private bool _isEnabled;
        private string _text = string.Empty;

        private Canvas _canvas;

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        public void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public void SetTooltip(string text)
        {
            _text = text;
            if (_activeToolTip != null)
            {
                _activeToolTip.SetText(text);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Mouse entered: " + gameObject.name);

            if (!_isEnabled) return;

            if (_activeToolTip == null)
            {
                _activeToolTip = Instantiate(toolTipPrefab, _canvas.transform).GetComponent<Tooltip>();
                _activeToolTip.SetText(_text);
            }

            _activeToolTip.transform.position = eventData.position;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Mouse exited: " + gameObject.name);
            Destroy(_activeToolTip?.gameObject);
            _activeToolTip = null;
        }
    }
}