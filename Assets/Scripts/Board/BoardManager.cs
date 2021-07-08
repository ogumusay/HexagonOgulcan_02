using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.GameObjects;
using Hexagon.State;
using System.Linq;


namespace Hexagon.Board
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField]
        private BoardSettings _boardSettings;

        private GameObject _hexagonContainer, _hexagonBombContainer;

        private List<AbstractSelectableGameObject> gameObjectList = new List<AbstractSelectableGameObject>();
        public static List<AbstractSelectableGameObject> GameObjectsToDestroy = new List<AbstractSelectableGameObject>();

        private void Awake()
        {
            CreateGameObjects(_boardSettings.Row, _boardSettings.Column);
        }

        //Create hexagons in loops by columns and rows
        private void CreateGameObjects(int row, int column)
        {
            //Create containers
            _hexagonContainer = new GameObject("Hexagons");
            _hexagonBombContainer = new GameObject("HexagonBombs");

            for (int i = 0; i < column; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < row; j++)
                    {
                        AbstractSelectableGameObject hexagon = Instantiate(_boardSettings.HexagonObjectPrefab, Vector3.zero, Quaternion.identity);

                        hexagon.PositionOnGrid = new Vector2(i + 1, j * 2 + 1);
                        hexagon.SetRandomColor(_boardSettings.Colors);

                        hexagon.transform.position = hexagon.GridToWorldPosition();
                        hexagon.transform.parent = _hexagonContainer.transform;

                        gameObjectList.Add(hexagon);
                    }
                }
                else
                {
                    for (int j = 0; j < row; j++)
                    {
                        AbstractSelectableGameObject hexagon = Instantiate(_boardSettings.HexagonObjectPrefab, Vector3.zero, Quaternion.identity);

                        hexagon.PositionOnGrid = new Vector2(i + 1, j * 2 + 2);
                        hexagon.SetRandomColor(_boardSettings.Colors);

                        hexagon.transform.position = hexagon.GridToWorldPosition();
                        hexagon.transform.parent = _hexagonContainer.transform;

                        gameObjectList.Add(hexagon);
                    }
                }
            }
        }

        private IEnumerator Start()
        {
            StateManager.CurrentState = StateManager.State.WAITING;

            yield return new WaitForSeconds(0.8f);

            StartCoroutine(FindAllMatches());
        }

        //Find all 3 same color hexagons on board
        private IEnumerator FindAllMatches()
        {
            StateManager.CurrentState = StateManager.State.CHECKING;

            //Every hex in board send rays to check same color hexagons nearby
            foreach (var gameObject in gameObjectList)
            {
                gameObject.SendRays();
            }

            //If 3 or more hexagons to destroy, destroy them
            if (GameObjectsToDestroy.Count > 2)
            {
                DestroyGameObjects();

                yield return new WaitForSeconds(1f);

                //After destroy them, fill the empty spaces
                yield return FillTheEmptySpaces();
            }

        }

        public void DestroyGameObjects()
        {
            StateManager.CurrentState = StateManager.State.DESTROYING;

            //Remove destroyed ones and show VFX
            foreach (var gameObject in GameObjectsToDestroy)
            {
                gameObjectList.Remove(gameObject);

                Destroy(gameObject.gameObject, 0.1f);
            }

        }

        public IEnumerator FillTheEmptySpaces()
        {
            StateManager.CurrentState = StateManager.State.FILLING;

            /*  Shift down existing ones
                Find how many empty spaces of underneath of existing ones and --
                --decrease their y position of gridPositions amount of empty spaces
             */
            for (int i = 1; i <= _boardSettings.Column; i++)
            {
                List<AbstractSelectableGameObject> objectList = gameObjectList.Where(gameObject => gameObject.PositionOnGrid.x == i).ToList();

                foreach (var gameObject in objectList)
                {
                    List<Vector2> emptyGrids =
                        GameObjectsToDestroy.Where(grid => grid.PositionOnGrid.x == i & grid.PositionOnGrid.y < gameObject.PositionOnGrid.y)
                                            .Select(grid => grid.PositionOnGrid).ToList();

                    if (emptyGrids.Count > 0)
                    {
                        gameObject.PositionOnGrid = new Vector2(i, gameObject.PositionOnGrid.y - (emptyGrids.Count * 2));
                    }
                }

            }

            //Create new hexagons above shifted ones
            for (int i = 1; i <= _boardSettings.Column; i++)
            {
                List<AbstractSelectableGameObject> emptySpacesByColumns = GameObjectsToDestroy.Where(hex => hex.PositionOnGrid.x == i).ToList();

                if (emptySpacesByColumns.Count > 0)
                {
                    int newRow = i % 2 == 0 ? _boardSettings.Row * 2 : (_boardSettings.Row * 2) - 1;

                    //If bombAmount is bigger than 0, instantiate 'bomb hexagon' instead of 'normal hexagon'
                    foreach (var space in emptySpacesByColumns)
                    {
                        AbstractSelectableGameObject newGameObject;


                        newGameObject = Instantiate(_boardSettings.HexagonObjectPrefab, new Vector3((i - 1) * 1.75f, newRow + 4, 0f), Quaternion.identity);
                        newGameObject.PositionOnGrid = new Vector2(i, newRow);
                        newGameObject.SetRandomColor(_boardSettings.Colors);
                        newGameObject.transform.parent = _hexagonContainer.transform;

                        gameObjectList.Add(newGameObject);

                        newRow -= 2;
                    }
                }

            }

            GameObjectsToDestroy.Clear();

            yield return new WaitForSeconds(1f);

            yield return FindAllMatches();
        }
    }
}