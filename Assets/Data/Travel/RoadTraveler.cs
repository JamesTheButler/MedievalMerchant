using System.Collections;
using System.Collections.Generic;
using Data.Towns;
using UnityEngine;

namespace Data.Travel
{
    public class RoadTraveler : MonoBehaviour
    {
        private const int RoadZIndex = 1;
        public UnityEngine.Tilemaps.Tilemap roads;
        public float speedUnitsPerSec = 4f; // tune to your tile/world scale
        public float cornerCut = 0.2f; // 0..0.45 tiles, small value to smooth turns

        private RoadGraph _graph;

        private void Awake()
        {
            _graph = RoadGraphBuilder.Build(roads);
        }

        public void TravelBetweenTowns(Town from, Town to)
        {
            var startCell = from.GridLocation.FromXY(RoadZIndex);
            var endCell = to.GridLocation.FromXY(RoadZIndex);

            // Snap to nearest road if the town is slightly off-road
            startCell = NearestRoadCell(startCell);
            endCell = NearestRoadCell(endCell);

            if (AStar.FindPath(_graph, startCell, endCell, out var path))
            {
                StopAllCoroutines(); // cancel any current travel
            }

            StartCoroutine(MoveAlongPath(path));
        }

        Vector3Int NearestRoadCell(Vector3Int origin)
        {
            if (_graph.IsNode(origin)) return origin;
            // simple spiral search up to radius 8
            const int R = 8;
            for (var r = 1; r <= R; r++)
            {
                for (var dx = -r; dx <= r; dx++)
                {
                    int dy1 = r, dy2 = -r;
                    if (_graph.IsNode(new Vector3Int(origin.x + dx, origin.y + dy1)))
                        return new(origin.x + dx, origin.y + dy1);
                    if (_graph.IsNode(new Vector3Int(origin.x + dx, origin.y + dy2)))
                        return new(origin.x + dx, origin.y + dy2);
                }

                for (var dy = -r + 1; dy <= r - 1; dy++)
                {
                    int dx1 = r, dx2 = -r;
                    if (_graph.IsNode(new Vector3Int(origin.x + dx1, origin.y + dy)))
                        return new(origin.x + dx1, origin.y + dy);
                    if (_graph.IsNode(new Vector3Int(origin.x + dx2, origin.y + dy)))
                        return new(origin.x + dx2, origin.y + dy);
                }
            }

            return origin; // fallback
        }

        IEnumerator MoveAlongPath(List<Vector3Int> path)
        {
            if (path == null || path.Count == 0) yield break;

            // Convert to world points (center of each cell)
            var pts = new List<Vector3>(path.Count);
            foreach (var c in path) pts.Add(roads.GetCellCenterWorld(c));

            // Optional: corner smoothing by shaving a bit off entry/exit of corners
            var smoothed = SmoothCorners(pts, cornerCut);

            // Move
            var t = transform;
            t.position = smoothed[0];
            for (var i = 1; i < smoothed.Count; i++)
            {
                var a = smoothed[i - 1];
                var b = smoothed[i];
                var dist = Vector3.Distance(a, b);
                var dur = dist / Mathf.Max(0.01f, speedUnitsPerSec);
                var elapsed = 0f;
                while (elapsed < dur)
                {
                    elapsed += Time.deltaTime;
                    var u = Mathf.Clamp01(elapsed / dur);
                    t.position = Vector3.Lerp(a, b, u);
                    yield return null;
                }
            }
        }

        // Creates short "chamfers" at turns so motion doesn't hard-stop then turn
        List<Vector3> SmoothCorners(List<Vector3> pts, float cut)
        {
            if (pts.Count <= 2 || cut <= 0f) return pts;

            var outPoints = new List<Vector3>(pts.Count * 2) { pts[0] };

            for (var i = 1; i < pts.Count - 1; i++)
            {
                var prev = pts[i - 1];
                var curr = pts[i];
                var next = pts[i + 1];

                var v1 = (curr - prev);
                var v2 = (next - curr);

                if (v1.sqrMagnitude < 1e-6f || v2.sqrMagnitude < 1e-6f ||
                    Vector3.Dot(v1.normalized, v2.normalized) < -0.999f)
                {
                    // straight or 180* turn—don’t cut
                    outPoints.Add(curr);
                    continue;
                }

                var a = curr - v1.normalized * cut;
                var b = curr + v2.normalized * cut;
                // Ensure order and no overcut beyond segment length
                if ((a - prev).sqrMagnitude > (curr - prev).sqrMagnitude) a = curr;
                if ((b - next).sqrMagnitude > (curr - next).sqrMagnitude) b = curr;

                outPoints.Add(a);
                outPoints.Add(b);
            }

            outPoints.Add(pts[^1]);
            return outPoints;
        }
    }
}