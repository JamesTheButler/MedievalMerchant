using Common.UI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public sealed class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Required]
        private GameObject toolTipPrefab;

        [SerializeField]
        private float offset = 8f;

        [SerializeField]
        private bool enabledOnStart;

        [SerializeField]
        private string defaultText;

        private Tooltip _activeToolTip;
        private bool _isEnabled;
        private string _text = string.Empty;

        private Canvas _canvas;

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            SetEnabled(enabledOnStart);
            if (!string.IsNullOrEmpty(defaultText))
            {
                SetTooltip(defaultText);
            }
        }

        public void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;

            if (!_isEnabled)
            {
                HideTooltip();
            }
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
            if (!_isEnabled) return;

            if (_activeToolTip == null)
            {
                _activeToolTip = Instantiate(toolTipPrefab, _canvas.transform).GetComponent<Tooltip>();
                _activeToolTip.SetText(_text);
            }

            var topCenter = ((RectTransform)gameObject.transform).GetTopCenter();
            _activeToolTip.transform.SetCanvasClampedPosition(
                topCenter + new Vector3(0, offset, 0),
                _canvas);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip();
        }

        private void HideTooltip()
        {
            Destroy(_activeToolTip?.gameObject);
            _activeToolTip = null;
        }
    }
}