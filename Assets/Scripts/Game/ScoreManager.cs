using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hexagon.GameObjects;
using Hexagon.Board;
using System;

namespace Hexagon.Game
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private BoardData _boardData;
        [SerializeField] private Text _scoreText;
        [SerializeField] private GameObject _gameOverUI;

        public static int TotalScore { get; private set;}

        private void OnEnable()
        {
            AbstractSelectableGameObject.OnObjectDestroy += EarnScore;
        }

        //Earn score for each destroyed blocks and show it on UI text
        public void EarnScore(AbstractSelectableGameObject gameObject, SelectableGameObjectData data)
        {
            TotalScore += data.ScoreValue;

            _scoreText.text = TotalScore.ToString();
        }

        private void OnDisable()
        {
            AbstractSelectableGameObject.OnObjectDestroy -= EarnScore;
            TotalScore = 0;
        }
    }
}