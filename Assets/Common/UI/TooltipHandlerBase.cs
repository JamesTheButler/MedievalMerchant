using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI
{
    public abstract class TooltipHandlerBase<TData> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Required]
        private GameObject toolTipPrefab;

        [SerializeField]
        private float offset = 8f;

        [SerializeField]
        private bool enabledOnStart;

        private TooltipBase<TData> _activeToolTip;
        private bool _isEnabled;
        private TData _data;
        private Canvas _canvas;

        protected virtual void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            SetEnabled(enabledOnStart);
        }

        public void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;

            if (!_isEnabled)
            {
                HideTooltip();
            }
        }

        public void SetTooltip(TData data)
        {
            _data = data;
            if (_activeToolTip != null)
            {
                _activeToolTip.SetData(data);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isEnabled) return;

            if (_activeToolTip == null)
            {
                _activeToolTip = Instantiate(toolTipPrefab, _canvas.transform).GetComponent<TooltipBase<TData>>();
                _activeToolTip.SetData(_data);
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