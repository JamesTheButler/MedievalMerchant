using System.Collections.Generic;
using Common;
using Data;
using Data.Towns;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Map.Tiling
{
    public sealed class TilemapManager : MonoBehaviour
    {
        [SerializeField, Required]
        private Grid grid;

        [SerializeField, Required]
        private Tiles tiles;

        [SerializeField]
        private UnityEvent<Town> onTownClicked;

        [SerializeField]
        private UnityEvent onGroundClicked;

        private Model _model;
        private Tilemap _tilemap;

        private const int TownZIndex = 3;

        public void Initialize()
        {
            _model = Model.Instance;
            foreach (var town in _model.Towns.Values)
            {
                town.Tier.Observe(_ => UpdateTown(town), false);
            }

            _tilemap = grid.gameObject.GetComponentInChildren<Tilemap>();
        }

        // TODO: should use input system
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                LeftClick();
            }

            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                RightClick();
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

            var pos = _model.TileFlagMap.Origin + town.GridLocation;
            _tilemap.SetTile(new Vector3Int(pos.x, pos.y, TownZIndex), tile);
        }

        private void RightClick()
        {
            var clickedCell = GetCellOnMousePosition();

            _model.Player.Location.CurrentTown = _model.Towns.GetValueOrDefault(clickedCell);
        }

        private void LeftClick()
        {
            var cellPos = GetCellOnMousePosition();

            if (_model.Towns.TryGetValue(cellPos, out var town))
            {
                onTownClicked?.Invoke(town);
            }
            else
            {
                onGroundClicked?.Invoke();
            }
        }

        private Vector2Int GetCellOnMousePosition()
        {
            var mouseWorldPos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            var gridPos = grid.WorldToCell(mouseWorldPos).XY() - _model.TileFlagMap.Origin;

            return gridPos;
        }
    }
}