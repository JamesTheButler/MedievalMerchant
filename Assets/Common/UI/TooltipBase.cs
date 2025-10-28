using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public abstract class TooltipBase<TData> : MonoBehaviour
    {
        [SerializeField]
        private bool drawDebugLines = true;

        private const int Padding = 16;

        private Canvas _canvas;
        private RectTransform _origin;
        private RectTransform _rectTransform;
        private Rect _previousRect;

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
            Justify();
        }

        public void SetData(TData data)
        {
            UpdateUI(data);
            Justify();
        }

        private void OnDestroy()
        {
            Reset();
        }

        private readonly Dictionary<Rect, Color> _debugRects = new();

        private void OnDrawGizmos()
        {
            if (!drawDebugLines)
                return;

            foreach (var (rect, color) in _debugRects)
            {
                MyGizmos.DrawRect(rect, color);
            }
        }

        protected void Justify()
        {
            if (!_origin | !_canvas | !_rectTransform)
                return;

            var canvasRectTransform = _canvas.transform as RectTransform;
            var canvasRect = canvasRectTransform!.GetWorldRect();
            var originRect = _origin.GetWorldRect();
            var worldRect = _rectTransform.GetWorldRect();

            var spaceOnTop = canvasRect.yMax - originRect.yMax - 2 * Padding;
            var spaceOnRight = canvasRect.xMax - originRect.xMax - Padding - Padding;

            var fitsOnTop = spaceOnTop >= worldRect.height;
            var fitsOnRight = spaceOnRight >= worldRect.width;
            var halfSize = new Vector2(worldRect.width / 2f, worldRect.height / 2f);

            // if it fits above origin object, use origins top edge to align
            // otherwise use the top of the canvas to align
            var y = fitsOnTop
                ? originRect.yMax + Padding + halfSize.y
                : canvasRect.yMax - Padding - halfSize.y;

            float x;

            // if it fits on top, use the origins center to align
            if (fitsOnTop)
            {
                x = Mathf.Clamp(
                    originRect.center.x,
                    Padding + halfSize.x,
                    canvasRect.width - Padding - halfSize.x);
            }
            // otherwise place either to the right (preferred) or to the left of the origin object
            else
            {
                x = fitsOnRight
                    ? originRect.xMax + Padding + halfSize.x
                    : originRect.xMin - Padding - halfSize.x;
            }

            var targetCenterPosition = new Vector2(x, y);

            RegisterDebugShapes(
                originRect,
                spaceOnTop,
                fitsOnTop,
                spaceOnRight,
                fitsOnRight,
                targetCenterPosition,
                halfSize, worldRect);

            // move the actual tooltip
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                RectTransformUtility.WorldToScreenPoint(null, targetCenterPosition),
                null,
                out var localPoint
            );
            _rectTransform.anchoredPosition = localPoint;

            // this seems to help!
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }

        private void RegisterDebugShapes(
            Rect originRect,
            float spaceOnTop,
            bool fitsOnTop,
            float spaceOnRight,
            bool fitsOnRight,
            Vector2 targetCenterPosition,
            Vector2 halfSize,
            Rect worldRect)
        {
            _debugRects.Clear();

            if (!drawDebugLines)
                return;

            var paddingSize = new Vector2(Padding, Padding);

            var canvasPaddingRect = new Rect(
                Vector2.zero + paddingSize,
                _canvas.pixelRect.size - paddingSize * 2f);
            _debugRects.Add(canvasPaddingRect, Color.yellow);

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

            var targetBottomPosition = targetCenterPosition - halfSize;
            var clampedRect = new Rect(targetBottomPosition, worldRect.size);
            _debugRects.Add(clampedRect, Color.white);
        }
    }
}