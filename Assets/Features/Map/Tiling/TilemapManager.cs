using System.Collections.Generic;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using Features.Player.Camp.Logic;
using Features.Towns;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Features.Map.Tiling
{
    public sealed class TilemapManager : InitializableBehavior
    {
        [SerializeField, Required]
        private Grid grid;

        [SerializeField, Required]
        private Tiles tiles;

        [SerializeField]
        private UnityEvent<Town> onTownClicked, onTownRightClicked, townHovered, townUnhovered;

        [SerializeField]
        private UnityEvent onCampClicked, onCampRightClicked;

        [SerializeField]
        private UnityEvent onGroundClicked;

        public Tilemap Tilemap { get; private set; }

        private GameplayModel _model;
        private NavigationService _navigationService;

        public override void Initialize()
        {
            _model = GameplayContext.Instance.Model;
            _navigationService = GameplayContext.Instance.Services.NavigationService;
            Tilemap = grid.gameObject.GetComponentInChildren<Tilemap>();

            foreach (var town in _model.Towns.Values)
            {
                town.Tier.Observe(_ => UpdateTown(town));
                town.MapTile.Observe(mapTile => BindMapTile(mapTile, town));
            }

            if (_model.Camp != null)
            {
                _model.Camp.MapTile.Observe(BindCampTile);
            }
        }

        private void BindMapTile(TownMapTile townTile, Town town)
        {
            townTile.LeftClicked += () => onTownClicked?.Invoke(town);
            townTile.RightClicked += () =>
            {
                onTownRightClicked?.Invoke(town);
                _navigationService.NavigationStarted?.Invoke(town);
            };
            townTile.Hovered += () => townHovered?.Invoke(town);
            townTile.Unhovered += () => townUnhovered?.Invoke(town);
        }

        private void BindCampTile(CampMapTile campTile)
        {
            if (campTile == null)
                return;

            campTile.LeftClicked += () => onCampClicked?.Invoke();
            campTile.RightClicked += () =>
            {
                onCampRightClicked?.Invoke();
                _navigationService.NavigationStarted?.Invoke(_model.Camp);
            };
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                LeftClick();
            }
        }

        private void UpdateTown(Town town)
        {
            var tile = town.Tier.Value switch
            {
                Tier.Tier1 => tiles.TownTileT1,
                Tier.Tier2 => tiles.TownTileT2,
                Tier.Tier3 => tiles.TownTileT3,
                _ => tiles.TownTileT3
            };

            var pos2D = town.GridLocation;
            var z = _model.TileFlagMap.TownZLevels.GetValueOrDefault(pos2D, 5);
            var pos3D = new Vector3Int(pos2D.x, pos2D.y, z);
            Tilemap.SetTile(pos3D, tile);
            var tileGo = Tilemap.GetInstantiatedObject(pos3D);
            var mapTile = tileGo.GetComponent<TownMapTile>();
            if (mapTile == null)
                return;

            town.MapTile.Value = mapTile;

            if (town.Tier.Value != Tier.Tier1)
            {
                mapTile.PlayUpgradeEffects();
            }
        }

        private void LeftClick()
        {
            onGroundClicked?.Invoke();
        }
    }
}