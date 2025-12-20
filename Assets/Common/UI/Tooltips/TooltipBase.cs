using System.Collections.Generic;
using Common.UI.Utility;
using Common.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Tooltips
{
    public abstract class TooltipBase<TData> : MonoBehaviour
    {
        [SerializeField]
        private bool drawDebugLines = true;

        private const int Padding = 16;

        private Canvas _canvas;
        private RectTransform _canvasRectTransform;
        private RectTransform _origin;
        private RectTransform _rectTransform;
        private Rect _previousRect;

        private readonly Dictionary<Rect, Color> _debugRects = new();

        public abstract void Reset();
        protected abstract void UpdateUI(TData data);

        protected virtual void Awake()
        {
            _rectTransform = (RectTransform)transform;
        }

        public void SetOriginObject(RectTransform origin)
        {
            _origin = origin;
            _canvas = _origin.GetComponentInParent<Canvas>();
            _canvasRectTransform = (RectTransform)_canvas.transform;
            RequestJustify();
        }

        public void SetData(TData data)
        {
            UpdateUI(data);
            RequestJustify();
        }

        private void OnDestroy()
        {
            Reset();
        }

        private const int JustifyMaxIterations = 4;
        private int _justifyTriesLeft;
        private Vector2 _lastSize;

        private void RequestJustify()
        {
            _justifyTriesLeft = JustifyMaxIterations;
            _lastSize = Vector2.zero;
        }

        private void LateUpdate()
        {
            if (_justifyTriesLeft <= 0)
                return;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);

            var sizeNow = _rectTransform.rect.size;

            Justify();

            _justifyTriesLeft--;

            // Stop early if size has stabilized
            if (Vector2.Distance(sizeNow, _lastSize) < 0.1f)
                _justifyTriesLeft = 0;

            _lastSize = sizeNow;
        }

        private void OnDrawGizmos()
        {
            if (!drawDebugLines || _canvasRectTransform == null)
                return;

            foreach (var (rect, color) in _debugRects)
            {
                MyGizmos.DrawRectOnCanvas(_canvas, rect, color);
            }
        }

        protected void Justify()
        {
            if (_origin == null || _canvas == null || _rectTransform == null || _canvasRectTransform == null)
                return;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);

            var tooltipSize = _rectTransform.rect.size;
            var tooltipHalfSize = tooltipSize * 0.5f;

            var originBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(_canvasRectTransform, _origin);
            var originRect = new Rect(originBounds.min, originBounds.size);

            var canvasRect = _canvasRectTransform.rect;

            var spaceOnTop = canvasRect.yMax - originRect.yMax - 2f * Padding;
            var spaceOnRight = canvasRect.xMax - originRect.xMax - 2f * Padding;

            var fitsOnTop = spaceOnTop >= tooltipSize.y;
            var fitsOnRight = spaceOnRight >= tooltipSize.x;

            var targetY = fitsOnTop
                ? originRect.yMax + Padding + tooltipHalfSize.y
                : canvasRect.yMax - Padding - tooltipHalfSize.y;

            float targetX;
            if (fitsOnTop)
            {
                targetX = Mathf.Clamp(
                    originRect.center.x,
                    canvasRect.xMin + Padding + tooltipHalfSize.x,
                    canvasRect.xMax - Padding - tooltipHalfSize.x);
            }
            else
            {
                targetX = fitsOnRight
                    ? originRect.xMax + Padding + tooltipHalfSize.x
                    : originRect.xMin - Padding - tooltipHalfSize.x;
            }

            var tooltipCenterPosition = new Vector2(targetX, targetY);
            RegisterDebugShapes(
                originRect,
                spaceOnTop,
                fitsOnTop,
                spaceOnRight,
                fitsOnRight,
                tooltipCenterPosition,
                tooltipHalfSize,
                _rectTransform.rect,
                _canvasRectTransform.rect);

            _rectTransform.anchoredPosition = new Vector2(targetX, targetY);
        }

        private void RegisterDebugShapes(
            Rect originRect,
            float spaceOnTop,
            bool fitsOnTop,
            float spaceOnRight,
            bool fitsOnRight,
            Vector2 tooltipCenterPosition,
            Vector2 tooltipHalfSize,
            Rect worldRect,
            Rect canvasRect)
        {
            _debugRects.Clear();

            if (!drawDebugLines)
                return;

            var paddingSize = new Vector2(Padding, Padding);

            var canvasPaddingRect = new Rect(
                canvasRect.min + paddingSize,
                canvasRect.size - paddingSize * 2f);
            _debugRects.Add(canvasPaddingRect, Color.blue);

            // render padding around origin
            var objectPaddingRect = new Rect(
                originRect.position - paddingSize,
                originRect.size + paddingSize * 2f);
            _debugRects.Add(objectPaddingRect, Color.yellow);

            // render space between origin.top and canvas.top
            var topRect = new Rect(
                new Vector2(originRect.xMin, originRect.yMax + Padding),
                new Vector2(originRect.width, spaceOnTop));
            _debugRects.Add(topRect, fitsOnTop ? Color.green : Color.red);

            // render space between origin.right and canvas.right
            var rightRect = new Rect(
                new Vector2(originRect.xMax + Padding, originRect.yMin),
                new Vector2(spaceOnRight, originRect.height));
            _debugRects.Add(rightRect, fitsOnRight ? Color.green : Color.red);

            var targetBottomPosition = tooltipCenterPosition - tooltipHalfSize;
            var clampedRect = new Rect(targetBottomPosition, worldRect.size);
            _debugRects.Add(clampedRect, Color.white);
        }
    }
}