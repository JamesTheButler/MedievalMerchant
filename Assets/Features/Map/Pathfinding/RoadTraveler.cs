using System.Collections;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Common.Utility;
using Features.Player.Logic;
using Features.Ticking.Logic;
using Features.Towns;
using UnityEngine;

namespace Features.Map.Pathfinding
{
    public sealed class RoadTraveler : InitializableBehavior
    {
        [SerializeField, Range(0, 0.45f)]
        public float smoothing = 0.2f;

        [SerializeField]
        private Grid tileGrid;

        private readonly Bindings _bindings = new();

        private RoadGraph _graph;
        private PlayerModel _player;
        private PlayerLocation _playerLocation;
        private GameSpeedModel _gameSpeedModel;
        private NavigationService _navigationService;

        private float _mapSpeed;
        private float _fastSpeedMultiplier;

        private Town _town;

        public override void Initialize()
        {
            var model = GameplayContext.Instance.Model;

            _graph = RoadGraphBuilder.Build(model.TileFlagMap);
            _player = model.Player;
            _playerLocation = _player.Location;
            _gameSpeedModel = model.GameSpeed;
            _navigationService = GameplayContext.Instance.Services.NavigationService;

            var tickConfig = ConfigurationManager.Configurations.TickConfig;
            _fastSpeedMultiplier = tickConfig.SecondsPerDayDefault / tickConfig.SecondsPerDayFast;

            _bindings.Track(
                _player.SpeedInTilesPerDay.Observe(OnMapSpeedChanged),
                _navigationService.NavigationStarted.Observe(TravelTo)
            );
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _bindings.UnbindAll();
        }

        private void TravelTo(Town town)
        {
            if (town == _playerLocation.CurrentTown.Value || town == null)
                return;

            if (town == _town)
                return;

            _town = town;
            var startCell = tileGrid.WorldToCell(_playerLocation.WorldLocation.Value).XY();
            var endCell = town.GridLocation;

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

            smoothed[0] = _playerLocation.WorldLocation.Value;
            _playerLocation.CurrentTown.Value = null;

            for (var i = 1; i < smoothed.Count; i++)
            {
                var a = smoothed[i - 1];
                var b = smoothed[i];
                var dist = Vector3.Distance(a, b);

                var traveled = 0f;
                while (traveled < dist)
                {
                    yield return null;
                    if (_gameSpeedModel.IsPaused.Value) continue;

                    traveled += Mathf.Max(0.01f, GetMapSpeed()) * Time.deltaTime;
                    var u = Mathf.Clamp01(traveled / dist);
                    _playerLocation.WorldLocation.Value = Vector3.Lerp(a, b, u);
                }
            }

            // we arrived
            _playerLocation.CurrentTown.Value = _town;
            _town = null;
        }

        private float GetMapSpeed()
        {
            return _gameSpeedModel.GameSpeed.Value == GameSpeed.Normal
                ? _mapSpeed
                : _mapSpeed * _fastSpeedMultiplier;
        }

        private static List<Vector3> SmoothCorners(List<Vector3> points, float cut)
        {
            if (points.Count <= 2 || cut <= 0f)
                return points;

            var outPoints = new List<Vector3>(points.Count * 2) { points[0] };

            for (var i = 1; i < points.Count - 1; i++)
            {
                var previous = points[i - 1];
                var current = points[i];
                var next = points[i + 1];

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

            outPoints.Add(points[^1]);
            return outPoints;
        }
    }
}