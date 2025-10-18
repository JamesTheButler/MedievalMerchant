using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        public void SetData(TData data)
        {
            _data = data;
            if (_activeToolTip != null)
            {
                _activeToolTip.SetData(data);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isEnabled)
                return;

            if (_activeToolTip != null)
                return;

            _activeToolTip = Instantiate(toolTipPrefab, _canvas.transform).GetComponent<TooltipBase<TData>>();
            _activeToolTip.SetData(_data);

            var canvasGroup = _activeToolTip.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogError("Tooltips should have a CanvasGroup to enforce proper spawning.");
            }
            else
            {
                canvasGroup.blocksRaycasts = false;
            }

            var topCenter = ((RectTransform)gameObject.transform).GetTopCenter();
            _activeToolTip.transform.SetCanvasClampedPosition(
                topCenter + new Vector3(0, offset, 0),
                _canvas);

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_activeToolTip.transform);

            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = true;
            }
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