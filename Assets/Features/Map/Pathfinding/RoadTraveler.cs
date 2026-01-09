using System;
using System.Collections;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.UI;
using Common.Utility;
using Features.Player.Logic;
using Features.Ticking.Logic;
using Features.Towns;
using UnityEngine;

namespace Features.Map.Pathfinding
{
    // TODO - CORE: this is not generic, but it should be. anything that can walk on roads should be able to use this
    public sealed class RoadTraveler : MonoBehaviour
    {
        [SerializeField, Range(0, 0.45f)]
        public float smoothing = 0.2f;

        [SerializeField]
        private Grid tileGrid;

        private readonly Lazy<GameplayModel> _model = new(() => GameplayContext.Instance.Model);

        private RoadGraph _graph;
        private PlayerModel _player;
        private PlayerLocation _playerLocation;
        private GameSpeedModel _gameSpeedModel;
        private UIBridgeService _uiBridgeService;

        private float _mapSpeed;

        private Town _town;

        private void Start()
        {
            _graph = RoadGraphBuilder.Build(_model.Value.TileFlagMap);
            _player = _model.Value.Player;
            _playerLocation = _player.Location;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;

            _player.SpeedInTilesPerDay.Observe(OnMapSpeedChanged);
        }

        public void TravelTo(Town town)
        {
            if (town == _playerLocation.CurrentTown.Value || town == null)
                return;

            _uiBridgeService.NavigateToTown(town);

            _town = town;
            var startCell = tileGrid.WorldToCell(_playerLocation.WorldLocation.Value).XY();
            var endCell = town.GridLocation;

            // BUG: when changing between target towns mid-travel, we get buggy-ness from this.
            //   we'd want to continue wherever we are right now, if we are already on a road tile
            startCell = NearestRoadCell(startCell);
            endCell = NearestRoadCell(endCell);

            if (AStar.FindPath(_graph, startCell, endCell, out var path))
            {
                StopAllCoroutines(); // cancel any current travel
            }

            StartCoroutine(MoveAlongPath(path));
        }

        private void OnMapSpeedChanged(float mapSpeed)
        {
            _mapSpeed = mapSpeed;
        }

        private Vector2Int NearestRoadCell(Vector2Int cell)
        {
            var graph = _graph;
            if (graph.IsNode(cell))
                return cell;

            // simple spiral search up to radius 8
            const int maxRadius = 8;
            for (var radius = 0; radius <= maxRadius; radius++)
            {
                for (var dx = -radius; dx <= radius; dx++)
                {
                    var dy2 = -radius;

                    if (graph.IsNode(new Vector2Int(cell.x + dx, cell.y + radius)))
                        return new Vector2Int(cell.x + dx, cell.y + radius);
                    if (graph.IsNode(new Vector2Int(cell.x + dx, cell.y + dy2)))
                        return new Vector2Int(cell.x + dx, cell.y + dy2);
                }

                for (var dy = -radius + 1; dy <= radius - 1; dy++)
                {
                    var dx2 = -radius;
                    if (graph.IsNode(new Vector2Int(cell.x + radius, cell.y + dy)))
                        return new Vector2Int(cell.x + radius, cell.y + dy);
                    if (graph.IsNode(new Vector2Int(cell.x + dx2, cell.y + dy)))
                        return new Vector2Int(cell.x + dx2, cell.y + dy);
                }
            }

            return cell; // fallback
        }

        private IEnumerator MoveAlongPath(List<Vector2Int> path)
        {
            if (path == null || path.Count == 0)
                yield break;

            // use center of each tile as navigation points
            var points = new List<Vector3>(path.Count);
            foreach (var cell in path)
            {
                points.Add(tileGrid.CellToWorld(cell.FromXY()));
            }

            var smoothed = SmoothCorners(points, smoothing);

            _playerLocation.WorldLocation.Value = smoothed[0];
            _playerLocation.CurrentTown.Value = null;

            for (var i = 1; i < smoothed.Count; i++)
            {
                var a = smoothed[i - 1];
                var b = smoothed[i];
                var dist = Vector3.Distance(a, b);

                var dur = dist / Mathf.Max(0.01f, GetMapSpeed());
                var elapsed = 0f;
                while (elapsed < dur)
                {
                    // TODO - Optimization: would be cheaper to observe _tickingService and have local _isPaused
                    // pause movement when game is paused
                    yield return new WaitUntil(() => !_gameSpeedModel.IsPaused.Value);

                    elapsed += Time.deltaTime;
                    var u = Mathf.Clamp01(elapsed / dur);
                    _playerLocation.WorldLocation.Value = Vector3.Lerp(a, b, u);

                    yield return null;
                }
            }

            // we arrived
            _playerLocation.CurrentTown.Value = _town;
            _town = null;
        }

        private float GetMapSpeed()
        {
            return _mapSpeed;
        }

        private static List<Vector3> SmoothCorners(List<Vector3> pts, float cut)
        {
            if (pts.Count <= 2 || cut <= 0f) return pts;

            var outPoints = new List<Vector3>(pts.Count * 2) { pts[0] };

            for (var i = 1; i < pts.Count - 1; i++)
            {
                var previous = pts[i - 1];
                var current = pts[i];
                var next = pts[i + 1];

                var v1 = current - previous;
                var v2 = next - current;

                if (v1.sqrMagnitude < 0.001f ||
                    v2.sqrMagnitude < 0.001f ||
                    Vector3.Dot(v1.normalized, v2.normalized) < -0.999f)
                {
                    // straight or 180* turn—don’t cut
                    outPoints.Add(current);
                    continue;
                }

                var a = current - v1.normalized * cut;
                var b = current + v2.normalized * cut;
                // Ensure order and no over-cut beyond segment length
                if ((a - previous).sqrMagnitude > (current - previous).sqrMagnitude)
                {
                    a = current;
                }

                if ((b - next).sqrMagnitude > (current - next).sqrMagnitude)
                {
                    b = current;
                }

                outPoints.Add(a);
                outPoints.Add(b);
            }

            outPoints.Add(pts[^1]);
            return outPoints;
        }
    }
}