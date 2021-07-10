using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.GameObjects;
using System.Linq;

namespace Hexagon.Board
{
    [CreateAssetMenu(menuName = "Hexagon/Board/BoardSettings")]
    public class BoardSettings : ScriptableObject
    {
        [Header("Size")]
        [SerializeField]
        public int Row;
        [SerializeField]
        public int Column;

        [Header("Timing")]
        [SerializeField]
        public float StartDelay;

        [Header("Bomb Settings")]
        [SerializeField]
        public int BombShowUpScore;
        [SerializeField]
        public int BombShowUpEveryScore;

        [Header("Prefabs")]
        [SerializeField]
        public AbstractSelectableGameObject HexagonObjectPrefab;
        [SerializeField]
        public AbstractSelectableGameObject HexagonBombObjectPrefab;

        [SerializeField]
        public SelectableGameObjectColor[] Colors;

        public List<AbstractSelectableGameObject> GameObjectList = new List<AbstractSelectableGameObject>();
        public List<AbstractSelectableGameObject> GameObjectsToDestroy = new List<AbstractSelectableGameObject>();
        public List<AbstractSelectableGameObject> SelectedGameObjects = new List<AbstractSelectableGameObject>();

        public void ClearLists()
        {
            GameObjectList.Clear();
            GameObjectsToDestroy.Clear();
            SelectedGameObjects.Clear();
        }

        //Find 6 adjacent hexagons by gridPosition
        //Set conditions because hexagons can be on edge of the board
        public Vector2[] FindAdjacentHexagons(Vector2 gridPos)
        {
            Vector2 topHex = gridPos + Vector2.up * 2;
            Vector2 topRightHex = gridPos + Vector2.one;
            Vector2 bottomRightHex = gridPos + Vector2.down + Vector2.right;
            Vector2 bottomHex = gridPos + Vector2.down * 2;
            Vector2 bottomLeftHex = gridPos + Vector2.down + Vector2.left;
            Vector2 topLeftHex = gridPos + Vector2.left + Vector2.up;

            if (gridPos.x == 1)
            {
                if (gridPos.y == 1)
                {
                    Vector2[] grids = {
                    topHex,
                    topRightHex,
                };

                    return grids;
                }
                else if (gridPos.y == (Row * 2) - 1)
                {
                    Vector2[] grids = {
                    topRightHex,
                    bottomRightHex,
                    bottomHex,
                };

                    return grids;
                }
                else
                {
                    Vector2[] grids = {
                    topHex,
                    topRightHex,
                    bottomRightHex,
                    bottomHex,
                };

                    return grids;
                }

            }
            else if (gridPos.x == Column)
            {
                if (Column % 2 == 0 && gridPos.y == 2)
                {
                    Vector2[] grids = {
                    topHex,
                    bottomLeftHex,
                    topLeftHex
                };

                    return grids;
                }
                else if (Column % 2 != 0 && gridPos.y == 1)
                {
                    Vector2[] grids = {
                    topHex,
                    topLeftHex
                };

                    return grids;
                }
                else if (Column % 2 != 0 && gridPos.y == (Row * 2) - 1)
                {
                    Vector2[] grids = {
                    topLeftHex,
                    bottomLeftHex,
                    bottomHex,
                };

                    return grids;
                }
                else if (Column % 2 == 0 && gridPos.y == Row * 2)
                {
                    Vector2[] grids = {
                    bottomLeftHex,
                    bottomHex,
                };

                    return grids;
                }
                else
                {
                    Vector2[] grids = {
                    topHex,
                    bottomHex,
                    bottomLeftHex,
                    topLeftHex
                };

                    return grids;
                }
            }
            else if (gridPos.y == 1)
            {
                Vector2[] grids = {
                topHex,
                topRightHex,
                topLeftHex
            };

                return grids;
            }
            else if (gridPos.y == 2)
            {
                Vector2[] grids = {
                topHex,
                topRightHex,
                bottomRightHex,
                bottomLeftHex,
                topLeftHex
            };

                return grids;
            }
            else if (gridPos.y == Row * 2)
            {
                Vector2[] grids = {
                bottomRightHex,
                bottomHex,
                bottomLeftHex,
            };

                return grids;
            }
            else if (gridPos.y == (Row * 2) - 1)
            {
                Vector2[] grids = {
                topRightHex,
                bottomRightHex,
                bottomHex,
                bottomLeftHex,
                topLeftHex
            };

                return grids;
            }
            else
            {
                Vector2[] grids = {
                topHex,
                topRightHex,
                bottomRightHex,
                bottomHex,
                bottomLeftHex,
                topLeftHex
            };

                return grids;
            }
        }

        //Find joint hexagons of 2 hexagons
        //Set conditions because hexagons can be on edge of the board
        public List<Vector2> FindSharedHexagons(Vector2 centerHex, Vector2 otherHex)
        {
            List<Vector2> vectors = new List<Vector2>();

            if (centerHex.x == otherHex.x)
            {
                if (centerHex.y > otherHex.y)
                {
                    Vector2 hex1 = centerHex + Vector2.down + Vector2.right;
                    Vector2 hex2 = centerHex + Vector2.down + Vector2.left;

                    vectors.Add(hex1);
                    vectors.Add(hex2);

                    if (hex1.x < 1 || hex1.x > Column)
                    {
                        vectors.Remove(hex1);
                    }
                    if (hex2.x < 1 || hex2.x > Column)
                    {
                        vectors.Remove(hex2);
                    }

                    return vectors;
                }
                else
                {
                    Vector2 hex1 = centerHex + Vector2.up + Vector2.right;
                    Vector2 hex2 = centerHex + Vector2.up + Vector2.left;

                    vectors.Add(hex1);
                    vectors.Add(hex2);

                    if (hex1.x < 1 || hex1.x > Column)
                    {
                        vectors.Remove(hex1);
                    }
                    if (hex2.x < 1 || hex2.x > Column)
                    {
                        vectors.Remove(hex2);
                    }

                    return vectors;
                }

            }
            else if (centerHex.x > otherHex.x)
            {
                if (centerHex.y > otherHex.y)
                {
                    Vector2 hex1 = centerHex + Vector2.up + Vector2.left;
                    Vector2 hex2 = centerHex + Vector2.down * 2;

                    vectors.Add(hex1);
                    vectors.Add(hex2);

                    if (hex1.y < 1 || hex1.y > Row * 2)
                    {
                        vectors.Remove(hex1);
                    }
                    if (hex1.y < 1 || hex1.y > Row * 2)
                    {
                        vectors.Remove(hex2);
                    }

                    return vectors;
                }
                else
                {
                    Vector2 hex1 = centerHex + Vector2.down + Vector2.left;
                    Vector2 hex2 = centerHex + Vector2.up * 2;

                    vectors.Add(hex1);
                    vectors.Add(hex2);

                    if (hex1.y < 1 || hex1.y > Row * 2)
                    {
                        vectors.Remove(hex1);
                    }
                    if (hex1.y < 1 || hex1.y > Row * 2)
                    {
                        vectors.Remove(hex2);
                    }

                    return vectors;
                }
            }
            else
            {
                if (centerHex.y > otherHex.y)
                {
                    Vector2 hex1 = centerHex + Vector2.up + Vector2.right;
                    Vector2 hex2 = centerHex + Vector2.down * 2;

                    vectors.Add(hex1);
                    vectors.Add(hex2);

                    if (hex1.y < 1 || hex1.y > Row * 2)
                    {
                        vectors.Remove(hex1);
                    }
                    if (hex1.y < 1 || hex1.y > Row * 2)
                    {
                        vectors.Remove(hex2);
                    }

                    return vectors;
                }
                else
                {
                    Vector2 hex1 = centerHex + Vector2.down + Vector2.right;
                    Vector2 hex2 = centerHex + Vector2.up * 2;

                    vectors.Add(hex1);
                    vectors.Add(hex2);

                    if (hex1.y < 1 || hex1.y > Row * 2)
                    {
                        vectors.Remove(hex1);
                    }
                    if (hex1.y < 1 || hex1.y > Row * 2)
                    {
                        vectors.Remove(hex2);
                    }

                    return vectors;
                }
            }
        }

    }
}