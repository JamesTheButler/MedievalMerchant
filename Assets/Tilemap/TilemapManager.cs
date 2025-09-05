using System.Collections.Generic;
using Data;
using Data.Towns;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Tilemap
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
        private UnityEngine.Tilemaps.Tilemap _tilemap;

        private const int TownZIndex = 3;

        public void Initialize()
        {
            _model = Model.Instance;
            foreach (var town in _model.Towns.Values)
            {
                town.TierChanged += _ => UpdateTown(town);
            }

            _tilemap = grid.gameObject.GetComponentInChildren<UnityEngine.Tilemaps.Tilemap>();
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
            var tile = town.Tier switch
            {
                Tier.Tier1 => tiles.TownTileT1,
                Tier.Tier2 => tiles.TownTileT2,
                Tier.Tier3 => tiles.TownTileT3,
                _ => tiles.TownTileT3
            };

            var pos = _model.TileFlagMap.Origin + town.Location;
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

            // TODO: re-implement selection  without using tiles
            //if (_selectedCoordinate != null)
            //{
            //    var pos = new Vector3Int(_selectedCoordinate.Value.x, _selectedCoordinate.Value.y, SelectionZIndex);
            //    tilemap.SetTile(pos, null);
            //    _selectedCoordinate = null;
            //}

            if (_model.Towns.TryGetValue(cellPos, out var town))
            {
                // TODO: re-implement selection  without using tiles
                //var pos = cellPos + _origin;
                //_selectedCoordinate = pos;
                //tilemap.SetTile(new Vector3Int(pos.x, pos.y, SelectionZIndex), tiles.SelectionTile);
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
            return grid.WorldToCell(mouseWorldPos).XY() - _model.TileFlagMap.Origin;
        }
    }
}