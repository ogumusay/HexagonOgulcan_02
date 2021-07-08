using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.GameObjects;
using Hexagon.State;


namespace Hexagon.Board
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField]
        private BoardSettings _boardSettings;

        private GameObject _hexagonContainer, _hexagonBombContainer;

        private List<HexagonObject> hexagonList = new List<HexagonObject>();
        public static List<AbstractSelectableGameObject> GameObjectsToDestroy = new List<AbstractSelectableGameObject>();

        private void Awake() 
        {
            CreateHexagons(_boardSettings.Row, _boardSettings.Column);
        }

        //Create hexagons in loops by columns and rows
        private void CreateHexagons(int row, int column)
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
                        HexagonObject hexagon = Instantiate(_boardSettings.HexagonObjectPrefab, Vector3.zero, Quaternion.identity);

                        hexagon.PositionOnGrid = new Vector2(i + 1, j * 2 + 1);
                        hexagon.SetRandomColor(_boardSettings.Colors);
                        
                        hexagon.transform.position = hexagon.GridToWorldPosition();
                        hexagon.transform.parent = _hexagonContainer.transform;
                        
                        hexagonList.Add(hexagon);
                    }
                }
                else
                {
                    for (int j = 0; j < row; j++)
                    {
                        HexagonObject hexagon = Instantiate(_boardSettings.HexagonObjectPrefab, Vector3.zero, Quaternion.identity);

                        hexagon.PositionOnGrid = new Vector2(i + 1, j * 2 + 2);
                        hexagon.SetRandomColor(_boardSettings.Colors);

                        hexagon.transform.position = hexagon.GridToWorldPosition();
                        hexagon.transform.parent = _hexagonContainer.transform;
                        
                        hexagonList.Add(hexagon);                    
                    }
                }
            }
        } 

    }
}
