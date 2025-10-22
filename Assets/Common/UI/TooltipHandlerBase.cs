using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI
{
    public abstract class TooltipHandlerBase<TData> :
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField, Required]
        private GameObject toolTipPrefab;

        [SerializeField]
        private bool enabledOnStart, hideOnClick;

        [SerializeField]
        private bool useSelfAsOrigin = true;

        [SerializeField, HideIf(nameof(useSelfAsOrigin))]
        private RectTransform originTransform;

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
            if (!_isEnabled || _data == null)
                return;

            if (_activeToolTip != null)
                return;

            _activeToolTip = Instantiate(toolTipPrefab, _canvas.transform).GetComponent<TooltipBase<TData>>();
            var origin = useSelfAsOrigin | !originTransform ? (RectTransform)gameObject.transform : originTransform; 
            _activeToolTip.SetOriginObject(origin);
            _activeToolTip.SetData(_data);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!hideOnClick)
                return;

            HideTooltip();
        }

        private void HideTooltip()
        {
            Destroy(_activeToolTip?.gameObject);
            _activeToolTip = null;
        }
    }
}