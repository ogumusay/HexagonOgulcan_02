using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.GameObjects;


namespace Hexagon.Board
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField]
        private BoardSettings _boardSettings;

        private GameObject _hexagonContainer, _hexagonBombContainer;

        private List<HexagonObject> hexagonList = new List<HexagonObject>();

        private void Awake() 
        {
            CreateHexagons();
        }

        //Create hexagons in loops by columns and rows
        private void CreateHexagons()
        {
            //Create containers
            _hexagonContainer = new GameObject("Hexagons");
            _hexagonBombContainer = new GameObject("HexagonBombs");

            for (int i = 0; i < _boardSettings.Column; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < _boardSettings.Row; j++)
                    {
                        HexagonObject hexagon = Instantiate(_boardSettings.HexagonObjectPrefab, Vector3.zero, Quaternion.identity);

                        hexagon.PositionOnGrid = new Vector2(i + 1, j * 2 + 1);
                        hexagon.transform.position = hexagon.GridToWorldPosition();
                        hexagon.transform.parent = _hexagonContainer.transform;
                        
                        hexagonList.Add(hexagon);
                    }
                }
                else
                {
                    for (int j = 0; j < _boardSettings.Row; j++)
                    {
                        HexagonObject hexagon = Instantiate(_boardSettings.HexagonObjectPrefab, Vector3.zero, Quaternion.identity);

                        hexagon.PositionOnGrid = new Vector2(i + 1, j * 2 + 2);
                        hexagon.transform.position = hexagon.GridToWorldPosition();
                        hexagon.transform.parent = _hexagonContainer.transform;
                        
                        hexagonList.Add(hexagon);                    
                    }
                }
            }
        }

    }
}
