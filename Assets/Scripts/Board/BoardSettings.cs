using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.GameObjects;

namespace Hexagon.Board
{
    [CreateAssetMenu(menuName = "Hexagon/Board/BoardSettings")]
    public class BoardSettings : ScriptableObject
    {
        [SerializeField]
        public int Row, Column;

        [SerializeField]
        public HexagonObject HexagonObjectPrefab;

        [SerializeField]
        public SelectableGameObjectColor[] Colors;
    }
}