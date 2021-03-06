using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.GameObjects;
using Hexagon.State;
using Hexagon.UserInput;
using Hexagon.Game;
using System.Linq;
using System;



namespace Hexagon.Board
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField]
        private BoardData _boardData;

        private GameObject _hexagonContainer, _hexagonBombContainer;

        public static event Action OnNoMoveLeft;

        private int _bombAmount = 0;
        private int _bombShowUpScore;

        private void Awake()
        {
            InputEvents.OnSwipe += StartCoroutineRotate;

            _bombShowUpScore = _boardData.BombShowUpScore;

            CreateBoard(_boardData.Row, _boardData.Column);
        }

        //Create hexagons in loops by columns and rows
        private void CreateBoard(int row, int column)
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
                        AbstractSelectableGameObject hexagon = Instantiate(_boardData.HexagonObjectPrefab, Vector3.zero, Quaternion.identity);

                        hexagon.PositionOnGrid = new Vector2(i + 1, j * 2 + 1);
                        hexagon.SetRandomColor(_boardData.Colors);

                        hexagon.transform.position = hexagon.GridToWorldPosition();
                        hexagon.transform.parent = _hexagonContainer.transform;

                        _boardData.GameObjectList.Add(hexagon);
                    }
                }
                else
                {
                    for (int j = 0; j < row; j++)
                    {
                        AbstractSelectableGameObject hexagon = Instantiate(_boardData.HexagonObjectPrefab, Vector3.zero, Quaternion.identity);

                        hexagon.PositionOnGrid = new Vector2(i + 1, j * 2 + 2);
                        hexagon.SetRandomColor(_boardData.Colors);

                        hexagon.transform.position = hexagon.GridToWorldPosition();
                        hexagon.transform.parent = _hexagonContainer.transform;

                        _boardData.GameObjectList.Add(hexagon);
                    }
                }
            }
        }

        private IEnumerator Start()
        {
            StateManager.CurrentState = StateManager.State.WAITING;

            yield return new WaitForSeconds(_boardData.StartDelay);

            yield return FindAllMatches();
        }

        //Find all 3 same color hexagons on board
        private IEnumerator FindAllMatches()
        {
            StateManager.CurrentState = StateManager.State.CHECKING;

            //Every hex in board send rays to check same color hexagons nearby
            foreach (var gameObject in _boardData.GameObjectList)
            {
                gameObject.SendRays();
            }

            //If 3 or more hexagons to destroy, destroy them
            if (_boardData.GameObjectsToDestroy.Count > 2)
            {
                DestroyGameObjects();

                yield return new WaitForSeconds(1f);

                //After destroy them, fill the empty spaces
                yield return FillTheEmptySpaces();
            }
            else
            {
                //If no possible move left in game, end the game
                if (PossibleMoveLeft())
                {
                    StateManager.CurrentState = StateManager.State.EMPTY;
                }
                else
                {
                    StateManager.CurrentState = StateManager.State.GAME_OVER;

                    yield return new WaitForSeconds(0.3f);

                    OnNoMoveLeft?.Invoke();
                }
            }

        }

        public void DestroyGameObjects()
        {
            StateManager.CurrentState = StateManager.State.DESTROYING_OBJECTS;

            if (_boardData.SelectedGameObjects.Count > 0)
            {
                foreach (var hexagon in _boardData.SelectedGameObjects)
                {
                    hexagon.SetColor();
                }

                _boardData.SelectedGameObjects.Clear();
            }

            //Remove destroyed ones and show VFX
            foreach (var gameObject in _boardData.GameObjectsToDestroy)
            {
                _boardData.GameObjectList.Remove(gameObject);

                Destroy(gameObject.gameObject);
            }

        }

        public IEnumerator FillTheEmptySpaces()
        {
            StateManager.CurrentState = StateManager.State.FILLING;

            /*  Shift down existing ones
                Find how many empty spaces of underneath of existing ones and --
                --decrease their y position of gridPositions amount of empty spaces
             */
            ShifDownObjects();

            AddHexagonBomb();

            //Create new hexagons above shifted ones
            CreateNewObjects();

            _boardData.GameObjectsToDestroy.Clear();

            yield return new WaitForSeconds(1f);

            yield return FindAllMatches();
        }

        private void CreateNewObjects()
        {
            for (int i = 1; i <= _boardData.Column; i++)
            {
                List<AbstractSelectableGameObject> emptySpacesByColumns = _boardData.GameObjectsToDestroy.Where(hex => hex.PositionOnGrid.x == i).ToList();

                if (emptySpacesByColumns.Count > 0)
                {
                    int newRow = i % 2 == 0 ? _boardData.Row * 2 : (_boardData.Row * 2) - 1;

                    //If bombAmount is bigger than 0, instantiate 'bomb hexagon' instead of 'normal hexagon'
                    for (int j = 0; j < emptySpacesByColumns.Count; j++)
                    {
                        AbstractSelectableGameObject newGameObject;

                        if (_bombAmount <= 0)
                        {
                            newGameObject = Instantiate(_boardData.HexagonObjectPrefab, new Vector3((i - 1) * 1.75f, newRow + 4, 0f), Quaternion.identity, _hexagonContainer.transform);
                            newGameObject.PositionOnGrid = new Vector2(i, newRow);
                            newGameObject.SetRandomColor(_boardData.Colors);
                        }
                        else
                        {
                            newGameObject = Instantiate(_boardData.HexagonBombObjectPrefab, new Vector3((i - 1) * 1.75f, newRow + 4, 0f), Quaternion.identity, _hexagonBombContainer.transform);
                            newGameObject.PositionOnGrid = new Vector2(i, newRow);
                            newGameObject.SetRandomColor(_boardData.Colors);

                            _bombAmount--;
                        }



                        _boardData.GameObjectList.Add(newGameObject);

                        newRow -= 2;
                    }
                }

            }
        }

        private void ShifDownObjects()
        {
            for (int i = 1; i <= _boardData.Column; i++)
            {
                List<AbstractSelectableGameObject> objectList = _boardData.GameObjectList.Where(gameObject => gameObject.PositionOnGrid.x == i).ToList();

                foreach (var gameObject in objectList)
                {
                    List<Vector2> emptyGrids =
                        _boardData.GameObjectsToDestroy.Where(grid => grid.PositionOnGrid.x == i & grid.PositionOnGrid.y < gameObject.PositionOnGrid.y)
                                            .Select(grid => grid.PositionOnGrid).ToList();

                    if (emptyGrids.Count > 0)
                    {
                        gameObject.PositionOnGrid = new Vector2(i, gameObject.PositionOnGrid.y - (emptyGrids.Count * 2));
                    }
                }
            }
        }

        private IEnumerator Rotate(bool isClockwise)
        {
            if (_boardData.SelectedGameObjects.Count > 0 && StateManager.CurrentState == StateManager.State.EMPTY)
            {
                StateManager.CurrentState = StateManager.State.ROTATING;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 tempGridPos = _boardData.SelectedGameObjects[0].PositionOnGrid;

                    if (isClockwise)
                    {
                        _boardData.SelectedGameObjects[0].PositionOnGrid = _boardData.SelectedGameObjects[1].PositionOnGrid;
                        _boardData.SelectedGameObjects[1].PositionOnGrid = _boardData.SelectedGameObjects[2].PositionOnGrid;
                        _boardData.SelectedGameObjects[2].PositionOnGrid = tempGridPos;
                    }
                    else
                    {
                        _boardData.SelectedGameObjects[0].PositionOnGrid = _boardData.SelectedGameObjects[2].PositionOnGrid;
                        _boardData.SelectedGameObjects[2].PositionOnGrid = _boardData.SelectedGameObjects[1].PositionOnGrid;
                        _boardData.SelectedGameObjects[1].PositionOnGrid = tempGridPos;
                    }

                    yield return new WaitForSeconds(0.3f);

                    //Check every rotation if there is 3 or more same color hexagon
                    foreach (var hex in _boardData.SelectedGameObjects)
                    {
                        hex.SendRays();
                    }


                    //if there is 3 or more same color hexagon
                    if (_boardData.GameObjectsToDestroy.Count > 0)
                    {
                        yield return new WaitForSeconds(0.7f);

                        DestroyGameObjects();

                        yield return new WaitForSeconds(0.3f);

                        //If there is 'hexagon bomb' on board, decrease its countdown
                        if (_hexagonBombContainer.transform.childCount > 0)
                        {
                            foreach (Transform bomb in _hexagonBombContainer.transform)
                            {
                                bomb.GetComponent<HexagonBombObject>().CountDown();
                            }
                        }

                        if (StateManager.CurrentState != StateManager.State.GAME_OVER)
                        {
                            yield return FillTheEmptySpaces();
                        }
                        else
                        {
                            yield break;
                        }

                        //Destroyed same color hexagons, dont rotate anymore
                        break;
                    }
                }

                StateManager.CurrentState = StateManager.State.EMPTY;
            }
        }

        private void StartCoroutineRotate(bool isClockwise)
        {
            StartCoroutine(Rotate(isClockwise));
        }

        private void AddHexagonBomb()
        {
            if (ScoreManager.TotalScore >= _bombShowUpScore)
            {
                _bombAmount++;
                _bombShowUpScore += _boardData.BombShowUpEveryScore;
            }
        }

        private void OnDestroy()
        {
            InputEvents.OnSwipe -= StartCoroutineRotate;

            _boardData.ClearLists();
        }

        /*  ->Go to every hexagon on board
                    ->Find 6 adjacent hexagons of current hexagon
                        ->If there is one in 6 hexagons, with same color as the current hexagon, find joint hexagons of these two
                            ->Find joint hexagons' 6 adjacent hexagons
                                ->If there is same color hexagon in 6 hexagons, as the current hexagon RETURN TRUE       
                -> RETURN FALSE 

            */
        private bool PossibleMoveLeft()
        {
            for (int i = 1; i <= _boardData.Column; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 2; j <= _boardData.Row * 2; j = j + 2)
                    {
                        AbstractSelectableGameObject centerHexagon = _boardData.GameObjectList.Where(hex => hex.PositionOnGrid == new Vector2(i, j)).FirstOrDefault();

                        foreach (var grid in _boardData.FindAdjacentHexagons(new Vector2(i, j)))
                        {
                            AbstractSelectableGameObject otherHexagon = _boardData.GameObjectList.Where(hex => hex.PositionOnGrid == grid).FirstOrDefault();

                            if (otherHexagon.Color == centerHexagon.Color)
                            {
                                foreach (var sharedHex in
                                        _boardData.FindSharedHexagons(centerHexagon.PositionOnGrid, otherHexagon.PositionOnGrid))
                                {

                                    foreach (var hexGrid in _boardData.FindAdjacentHexagons(sharedHex))
                                    {
                                        if (hexGrid == centerHexagon.PositionOnGrid || hexGrid == otherHexagon.PositionOnGrid)
                                        {
                                            continue;
                                        }

                                        AbstractSelectableGameObject hex = _boardData.GameObjectList.Where(hex => hex.PositionOnGrid == hexGrid).
                                                        FirstOrDefault();

                                        if (hex != null)
                                        {
                                            if (hex.Color == centerHexagon.Color)
                                            {
                                                return true;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 1; j <= (_boardData.Row * 2) - 1; j = j + 2)
                    {

                        AbstractSelectableGameObject centerHexagon = _boardData.GameObjectList.Where(hex => hex.PositionOnGrid == new Vector2(i, j)).FirstOrDefault();
                        List<AbstractSelectableGameObject> adjacentHexagons = new List<AbstractSelectableGameObject>();

                        foreach (var grid in _boardData.FindAdjacentHexagons(new Vector2(i, j)))
                        {
                            AbstractSelectableGameObject otherHexagon = _boardData.GameObjectList.Where(hex => hex.PositionOnGrid == grid).FirstOrDefault();

                            if (otherHexagon.Color == centerHexagon.Color)
                            {
                                foreach (var sharedHex in
                                        _boardData.FindSharedHexagons(centerHexagon.PositionOnGrid, otherHexagon.PositionOnGrid))
                                {

                                    foreach (var hexGrid in _boardData.FindAdjacentHexagons(sharedHex))
                                    {
                                        if (hexGrid == centerHexagon.PositionOnGrid || hexGrid == otherHexagon.PositionOnGrid)
                                        {
                                            continue;
                                        }

                                        AbstractSelectableGameObject hex = _boardData.GameObjectList.Where(hex => hex.PositionOnGrid == hexGrid).
                                                        FirstOrDefault();

                                        if (hex != null)
                                        {
                                            if (hex.Color == centerHexagon.Color)
                                            {
                                                return true;
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void OnApplicationQuit()
        {
            StateManager.CurrentState = StateManager.State.DESTROYING_SCENE;
        }
    }
}