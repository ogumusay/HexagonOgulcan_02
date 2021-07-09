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
        [SerializeField]
        public int Row, Column;

        [SerializeField]
        public float StartDelay;

        [SerializeField]
        public HexagonObject HexagonObjectPrefab;

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

    }
}