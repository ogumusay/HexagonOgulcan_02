using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hexagon.Board;
using Hexagon.GameObjects;

namespace Hexagon.Game
{
    [CreateAssetMenu(menuName = "Hexagon/Game/SelectionControllerSettings")]
    public class SelectionControllerSettings : ScriptableObject
    {
        [SerializeField] private BoardData _boardData;

        private Vector2[] FindGridPositionsOfAdjacentObjects(AbstractSelectableGameObject selectableGameObject)
        {
            //Finding angle between 'center of hexagon' and 'mouse position'
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0f);
            Vector3 vectorBetweenMouseAndHex = mousePos - selectableGameObject.transform.position;

            float angle = Vector3.SignedAngle(selectableGameObject.transform.up, vectorBetweenMouseAndHex, Vector3.forward);

            //
            //RIGHT CORNER
            //
            if (angle > -120f && angle < -60f &&
                    selectableGameObject.PositionOnGrid.y > 1 &&
                    selectableGameObject.PositionOnGrid.x < _boardData.Column &&
                    selectableGameObject.PositionOnGrid.y < _boardData.Row * 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(selectableGameObject.PositionOnGrid.x + 1, selectableGameObject.PositionOnGrid.y + 1),
                                            new Vector2(selectableGameObject.PositionOnGrid.x + 1, selectableGameObject.PositionOnGrid.y - 1),
                                                selectableGameObject.PositionOnGrid};

                return adjacentHexagons;
            }
            //
            //BOTTOM RIGHT CORNER
            //
            else if (angle > -180f && angle < -120f &&
                        selectableGameObject.PositionOnGrid.y > 2 &&
                        selectableGameObject.PositionOnGrid.x < _boardData.Column)
            {
                Vector2[] adjacentHexagons = {new Vector2(selectableGameObject.PositionOnGrid.x + 1, selectableGameObject.PositionOnGrid.y - 1),
                                            new Vector2(selectableGameObject.PositionOnGrid.x, selectableGameObject.PositionOnGrid.y - 2),
                                                selectableGameObject.PositionOnGrid};

                return adjacentHexagons;
            }
            //
            //BOTTOM LEFT CORNER
            //
            else if (angle < 180f && angle > 120f &&
                        selectableGameObject.PositionOnGrid.x > 1 && selectableGameObject.PositionOnGrid.y > 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(selectableGameObject.PositionOnGrid.x, selectableGameObject.PositionOnGrid.y - 2),
                                            new Vector2(selectableGameObject.PositionOnGrid.x - 1, selectableGameObject.PositionOnGrid.y - 1),
                                                selectableGameObject.PositionOnGrid};

                return adjacentHexagons;
            }
            //
            //LEFT CORNER
            //     
            else if (angle < 120f && angle > 60f &&
                        selectableGameObject.PositionOnGrid.x > 1 &&
                        selectableGameObject.PositionOnGrid.y > 1 &&
                        selectableGameObject.PositionOnGrid.y < _boardData.Row * 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(selectableGameObject.PositionOnGrid.x - 1, selectableGameObject.PositionOnGrid.y - 1),
                                            new Vector2(selectableGameObject.PositionOnGrid.x - 1, selectableGameObject.PositionOnGrid.y + 1),
                                                selectableGameObject.PositionOnGrid};

                return adjacentHexagons;
            }
            //
            //TOP LEFT CORNER
            //         
            else if (angle < 60f && angle > 0f &&
                        selectableGameObject.PositionOnGrid.x > 1 &&
                        selectableGameObject.PositionOnGrid.y + 1 < _boardData.Row * 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(selectableGameObject.PositionOnGrid.x - 1, selectableGameObject.PositionOnGrid.y + 1),
                                            new Vector2(selectableGameObject.PositionOnGrid.x, selectableGameObject.PositionOnGrid.y + 2),
                                                selectableGameObject.PositionOnGrid};

                return adjacentHexagons;
            }
            //
            //TOP RIGHT CORNER
            // 
            else if (angle < 0f && angle > -60f &&
                        selectableGameObject.PositionOnGrid.x < _boardData.Column &&
                        selectableGameObject.PositionOnGrid.y + 1 < _boardData.Row * 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(selectableGameObject.PositionOnGrid.x, selectableGameObject.PositionOnGrid.y + 2),
                                            new Vector2(selectableGameObject.PositionOnGrid.x + 1, selectableGameObject.PositionOnGrid.y + 1),
                                                selectableGameObject.PositionOnGrid};

                return adjacentHexagons;
            }

            return null;
        }

        private void SelectAdjacentObjects(Vector2[] positions)
        {
            if (positions != null)
            {
                //If there is already selected hexagons, remove them from list
                if (_boardData.SelectedGameObjects.Count > 0)
                {
                    foreach (var gameObject in _boardData.SelectedGameObjects)
                    {
                        //Reset color of hexagon
                        if (gameObject != null)
                        {
                            gameObject.SetColor();
                        }
                    }

                    _boardData.SelectedGameObjects.Clear();
                }

                //Add hexagons that found by gridPosition to 'selectedHexagons' list
                foreach (var position in positions)
                {
                    AbstractSelectableGameObject hexagon = _boardData.GameObjectList.Where(hex => hex.PositionOnGrid == position).FirstOrDefault();

                    _boardData.SelectedGameObjects.Add(hexagon);
                }

                //Make selected hexagons more visible
                foreach (var hex in _boardData.SelectedGameObjects)
                {
                    hex.MakeHighlighted();
                }
            }

        }

        public void SelectObjects(AbstractSelectableGameObject selectableGameObject)
        {
            SelectAdjacentObjects(FindGridPositionsOfAdjacentObjects(selectableGameObject));
        }

    }
}