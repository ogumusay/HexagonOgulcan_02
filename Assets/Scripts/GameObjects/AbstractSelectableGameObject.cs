using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.State;
using Hexagon.Board;

namespace Hexagon.GameObjects
{
    public abstract class AbstractSelectableGameObject : MonoBehaviour, ISelectable
    {
        [SerializeField] protected BoardSettings _boardSettings;
        [SerializeField] protected SelectableGameObjectData _selectableGameObjectData;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public SelectableGameObjectColor Color;
        public Vector2 PositionOnGrid;

        private event Action onMouseUp;

        private void Awake()
        {
            onMouseUp += SelectObject;
        }

        private void Update()
        {
            // Checking 'world position' if it's on right position according to 'grid position'
            if (!IsOnPosition())
            {
                // If it is not, move it to right 'world position'
                MoveToPosition();
            }
        }

        //Move hexagon to true world position 
        public void MoveToPosition()
        {
            Vector3 targetPosition = GridToWorldPosition();

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _selectableGameObjectData.Speed);

            //If distance is smaller than 0.1f, snap it to targetPosition
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
            }

        }

        //Check if it's true world position
        public bool IsOnPosition()
        {
            Vector3 targetPosition = GridToWorldPosition();

            if (transform.position == targetPosition)
            {
                return true;
            }

            return false;
        }

        public void SetRandomColor(SelectableGameObjectColor[] colors)
        {
            int randomNumber = UnityEngine.Random.Range(0, colors.Length);
            Color = colors[randomNumber];

            SetColor();
        }

        public void SetColor()
        {
            _spriteRenderer.color = _selectableGameObjectData.GetColor(Color);
        }

        public virtual void SelectObject()
        {
            //Finding angle between 'center of hexagon' and 'mouse position'
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0f);
            Vector3 vectorBetweenMouseAndHex = mousePos - transform.position;

            float angle = Vector3.SignedAngle(transform.up, vectorBetweenMouseAndHex, Vector3.forward);

            //
            //RIGHT CORNER
            //
            if (angle > -120f && angle < -60f &&
                    PositionOnGrid.y > 1 && PositionOnGrid.x < _boardSettings.Column && PositionOnGrid.y < _boardSettings.Row * 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(PositionOnGrid.x + 1, PositionOnGrid.y + 1),
                                            new Vector2(PositionOnGrid.x + 1, PositionOnGrid.y - 1),
                                                PositionOnGrid};

                SelectHexagons(adjacentHexagons);
            }
            //
            //BOTTOM RIGHT CORNER
            //
            else if (angle > -180f && angle < -120f &&
                        PositionOnGrid.y > 2 && PositionOnGrid.x < _boardSettings.Column)
            {
                Vector2[] adjacentHexagons = {new Vector2(PositionOnGrid.x + 1, PositionOnGrid.y - 1),
                                            new Vector2(PositionOnGrid.x, PositionOnGrid.y - 2),
                                                PositionOnGrid};

                SelectHexagons(adjacentHexagons);
            }
            //
            //BOTTOM LEFT CORNER
            //
            else if (angle < 180f && angle > 120f &&
                        PositionOnGrid.x > 1 && PositionOnGrid.y > 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(PositionOnGrid.x, PositionOnGrid.y - 2),
                                            new Vector2(PositionOnGrid.x - 1, PositionOnGrid.y - 1),
                                                PositionOnGrid};

                SelectHexagons(adjacentHexagons);
            }
            //
            //LEFT CORNER
            //     
            else if (angle < 120f && angle > 60f &&
                        PositionOnGrid.x > 1 && PositionOnGrid.y > 1 && PositionOnGrid.y < _boardSettings.Row * 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(PositionOnGrid.x - 1, PositionOnGrid.y - 1),
                                            new Vector2(PositionOnGrid.x - 1, PositionOnGrid.y + 1),
                                                PositionOnGrid};

                SelectHexagons(adjacentHexagons);
            }
            //
            //TOP LEFT CORNER
            //         
            else if (angle < 60f && angle > 0f &&
                        PositionOnGrid.x > 1 && PositionOnGrid.y + 1 < _boardSettings.Row * 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(PositionOnGrid.x - 1, PositionOnGrid.y + 1),
                                            new Vector2(PositionOnGrid.x, PositionOnGrid.y + 2),
                                                PositionOnGrid};

                SelectHexagons(adjacentHexagons);
            }
            //
            //TOP RIGHT CORNER
            // 
            else if (angle < 0f && angle > -60f &&
                        PositionOnGrid.x < _boardSettings.Column && PositionOnGrid.y + 1 < _boardSettings.Row * 2)
            {
                Vector2[] adjacentHexagons = {new Vector2(PositionOnGrid.x, PositionOnGrid.y + 2),
                                            new Vector2(PositionOnGrid.x + 1, PositionOnGrid.y + 1),
                                                PositionOnGrid};

                SelectHexagons(adjacentHexagons);
            }
        }

        public void SelectHexagons(Vector2[] positions)
        {
            //If there is already selected hexagons, remove them from list
            if (_boardSettings.SelectedGameObjects.Count > 0)
            {
                foreach (var gameObject in _boardSettings.SelectedGameObjects)
                {
                    //Reset color of hexagon
                    if (gameObject != null)
                    {
                        gameObject.SetColor();
                    }
                }

                _boardSettings.SelectedGameObjects.Clear();
            }

            //Add hexagons that found by gridPosition to 'selectedHexagons' list
            foreach (var position in positions)
            {
                AbstractSelectableGameObject hexagon = _boardSettings.GameObjectList.Where(hex => hex.PositionOnGrid == position).FirstOrDefault();

                _boardSettings.SelectedGameObjects.Add(hexagon);
            }

            //Make selected hexagons more visible
            foreach (var hex in _boardSettings.SelectedGameObjects)
            {
                hex.MakeHighlighted();
            }

        }

        public void MakeHighlighted()
        {
            _spriteRenderer.color += new Color(0.2f, 0.2f, 0.2f, 1f);
        }

        private void OnMouseUp()
        {
            if (true/*StateManager.CurrentState == StateManager.State.EMPTY*/)
            {
                onMouseUp?.Invoke();
            }
        }

        public Vector3 GridToWorldPosition()
        {
            float yPos = PositionOnGrid.y - _selectableGameObjectData.ROW_STEP;
            float xPos = (PositionOnGrid.x - 1) * _selectableGameObjectData.COLUMN_STEP;

            return new Vector3(xPos, yPos, 0f);
        }

        //Send rays to 6 directions
        public void SendRays()
        {
            Vector3[,] corners = _selectableGameObjectData.Corners;

            List<AbstractSelectableGameObject> sameColorGameObjects = new List<AbstractSelectableGameObject>();

            //Send 2 rays to 2 hexagons that is neighbors of each corner
            for (int i = 0; i < 6; i++)
            {
                RaycastHit2D hit_1 = Physics2D.Raycast(transform.position + corners[i, 0] * 2f, corners[i, 0], 0.1f);
                RaycastHit2D hit_2 = Physics2D.Raycast(transform.position + corners[i, 1] * 2f, corners[i, 1], 0.1f);

                //If corner has 2 neighbor hexagons
                if (hit_1.collider != null && hit_2.collider != null)
                {
                    AbstractSelectableGameObject gameObject_1 = hit_1.transform.GetComponent<AbstractSelectableGameObject>();
                    AbstractSelectableGameObject gameObject_2 = hit_2.transform.GetComponent<AbstractSelectableGameObject>();

                    //If these 2 neighbor hexagons have same color with main hexagon, add them to 'sameColorHexagons' list (including main)
                    if (gameObject_1.Color == gameObject_2.Color && Color == gameObject_2.Color)
                    {
                        if (!sameColorGameObjects.Contains(gameObject_1))
                        {
                            sameColorGameObjects.Add(gameObject_1);
                        }
                        if (!sameColorGameObjects.Contains(gameObject_2))
                        {
                            sameColorGameObjects.Add(gameObject_2);
                        }
                        if (!sameColorGameObjects.Contains(this))
                        {
                            sameColorGameObjects.Add(this);
                        }
                    }
                }
            }

            //If there are 3 or more same color hexagons, add them to 'hexagonsToDestroy' list
            if (sameColorGameObjects.Count > 2)
            {
                foreach (var gameObject in sameColorGameObjects)
                {
                    if (!_boardSettings.GameObjectsToDestroy.Contains(gameObject))
                    {
                        _boardSettings.GameObjectsToDestroy.Add(gameObject);
                    }
                }
            }

        }

        private void OnDestroy()
        {
            onMouseUp -= SelectObject;
        }
    }
}
