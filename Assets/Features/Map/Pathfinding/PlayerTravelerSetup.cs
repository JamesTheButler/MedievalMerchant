using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Pathfinding
{
    public sealed class PlayerTravelerSetup : InitializableBehavior
    {
        [SerializeField, Required]
        private RoadTraveler roadTraveler;

        private readonly Bindings _bindings = new();

        public override void Initialize()
        {
            var model = GameplayContext.Instance.Model;
            var player = model.Player;
            var graph = RoadGraphBuilder.Build(model.TileFlagMap);
            var navigationService = GameplayContext.Instance.Services.NavigationService;

            roadTraveler.Setup(
                player.Location,
                player.SpeedInTilesPerDay,
                graph
            );

            _bindings.Track(
                navigationService.NavigationStarted.Observe(roadTraveler.TravelTo)
            );

            roadTraveler.Arrived += OnArrived;
            roadTraveler.Departed += OnDeparted;
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _bindings.Unbind();

            if (roadTraveler != null)
            {
                roadTraveler.Arrived -= OnArrived;
                roadTraveler.Departed -= OnDeparted;
                roadTraveler.CleanUp();
            }
        }

        private void OnArrived(IMapLocation location)
        {
            var player = GameplayContext.Instance.Model.Player;
            player.Location.MapLocation.Value = location;
        }

        private void OnDeparted()
        {
            var player = GameplayContext.Instance.Model.Player;
            player.Location.MapLocation.Value = null;
        }
    }
}