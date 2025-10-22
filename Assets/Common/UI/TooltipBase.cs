using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public abstract class TooltipBase<TData> : MonoBehaviour
    {
        private const int CanvasPadding = 16;
        private const int OriginObjectPadding = 16;

        private Canvas _canvas;
        private RectTransform _origin;
        private RectTransform _rectTransform;

        private Rect _previousRect;

        public abstract void Reset();

        protected abstract void UpdateUI(TData data);

        private void Awake()
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

        private void OnRectTransformDimensionsChange()
        {
            Justify();
        }

        private readonly Dictionary<Rect, Color> _debugRects = new();

        private void OnDrawGizmos()
        {
            DrawRectTransform(_rectTransform, Color.green, 4f);

            foreach (var (rect, color) in _debugRects)
            {
                DrawRect(rect, color, 0);
            }
        }

        private void DrawRectTransform(RectTransform rectTransform, Color color, float padding)
        {
            if (!rectTransform)
                return;

            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            var bottomLeft = corners[0];
            var topRight = corners[2];
            var center = bottomLeft + (topRight - bottomLeft) / 2f;
            var size = rectTransform.sizeDelta;

            Gizmos.color = color;
            Gizmos.DrawWireCube(center + Vector3.one * padding, size - Vector2.one * 2 * padding);
        }

        private void DrawRect(Rect rect, Color color, float padding)
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(
                rect.center + Vector2.one * padding,
                rect.size - Vector2.one * 2 * padding);
        }

        private void Justify()
        {
            if (!_origin)
                return;

            _debugRects.Clear();

            var canvasRectTransform = _canvas.transform as RectTransform;
            var canvasRect = canvasRectTransform!.GetWorldRect();
            var originRect = _origin.GetWorldRect();
            var worldRect = _rectTransform.GetWorldRect();

            // render canvas padding
            var canvasPaddingSize = new Vector2(CanvasPadding, CanvasPadding);
            var originPaddingSize = new Vector2(OriginObjectPadding, OriginObjectPadding);

            var canvasPaddingRect = new Rect(
                Vector2.zero + canvasPaddingSize,
                _canvas.pixelRect.size - canvasPaddingSize * 2f);
            _debugRects.Add(canvasPaddingRect, Color.yellow);

            // render padding around origin
            var objectPaddingRect = new Rect(
                originRect.position - originPaddingSize,
                originRect.size + originPaddingSize * 2f);
            _debugRects.Add(objectPaddingRect, Color.yellow);

            // render space between origin.top and canvas.top
            var spaceOnTop = canvasRect.yMax - originRect.yMax - CanvasPadding - OriginObjectPadding;
            var fitsOnTop = spaceOnTop >= worldRect.height;
            var topRect = new Rect(
                new Vector2(originRect.xMin, originRect.yMax + OriginObjectPadding),
                new Vector2(originRect.width, spaceOnTop));
            _debugRects.Add(topRect, fitsOnTop ? Color.green : Color.red);

            // render space between origin.right and canvas.right
            var spaceOnRight = canvasRect.xMax - originRect.xMax - CanvasPadding - OriginObjectPadding;
            var fitsOnRight = spaceOnRight >= worldRect.width;
            var rightRect = new Rect(
                new Vector2(originRect.xMax + OriginObjectPadding, originRect.yMin),
                new Vector2(spaceOnRight, originRect.height));
            _debugRects.Add(rightRect, fitsOnRight ? Color.green : Color.red);


            var bottomCenterAnchor = new Vector2(worldRect.width / 2f, 0);
            var centerAnchor = new Vector2(worldRect.width / 2f, worldRect.height / 2f);
            
            Debug.LogError($"anchor min:{_rectTransform.anchorMin} - max:{_rectTransform.anchorMax}");

            var y = fitsOnTop
                ? originRect.yMax + OriginObjectPadding + centerAnchor.y
                : canvasRect.yMax - CanvasPadding - centerAnchor.y;

            float x;

            if (fitsOnTop)
            {
                x = Mathf.Clamp(
                    originRect.center.x,
                    CanvasPadding + worldRect.width / 2f,
                    canvasRect.width - CanvasPadding - worldRect.width / 2f);
            }
            else
            {
                x = 0f;
            }

            var clampedPosition = new Vector2(0, y);
/*
            switch (fitsOnTop, fitsOnRight)
            {
                case (fitsOnTop: true, fitsOnRight: true):
                    var spaceOnLeft = canvasRect.xMin - originRect.xMax - CanvasPadding - OriginObjectPadding;
                    var fitsOnLeft = spaceOnLeft >= worldRect.width;
                    Debug.LogWarning("Default Placement");
                    clampedPosition = new Vector2(
                        originRect.center.x,
                        originRect.yMax + OriginObjectPadding);
                    break;
                case (fitsOnTop: true, fitsOnRight: false):
                    Debug.LogWarning("Clamped by the right");
                    clampedPosition = new Vector2(
                        canvasRect.xMax - CanvasPadding - worldRect.width / 2f,
                        originRect.yMax + OriginObjectPadding);
                    break;
                case (fitsOnTop: false, fitsOnRight: true):
                    Debug.LogWarning("Clamped by the top");
                    clampedPosition = new Vector2(
                        originRect.xMax + OriginObjectPadding + worldRect.width / 2f,
                        canvasRect.yMax - CanvasPadding - worldRect.height);
                    break;
                case (fitsOnTop: false, fitsOnRight: false):
                    Debug.LogWarning("Forced to place left");
                    clampedPosition = new Vector2(
                        originRect.xMin - OriginObjectPadding - worldRect.width / 2f,
                        canvasRect.yMax - CanvasPadding - worldRect.height);
                    break;
            }*/


            var anchoredPos = clampedPosition - centerAnchor;
            var clampedRect = new Rect(anchoredPos, worldRect.size);
            _debugRects.Add(clampedRect, Color.blue);

            // this seems to help!
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);

            // TODO: this is causing flickering, i think. maybe another ForceRebuild?
            //_rectTransform.anchoredPosition = new Vector3(clampedPosition.x / 2f, clampedPosition.y, 0);
        }
    }
}