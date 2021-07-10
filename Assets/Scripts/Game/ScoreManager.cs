using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hexagon.GameObjects;


namespace Hexagon.Game
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        [SerializeField] private GameObject _gameOverUI;

        private int _totalScore = 0;

        private void OnEnable()
        {
            AbstractSelectableGameObject.OnObjectDestroy += EarnScore;
        }

        //Earn score for each destroyed blocks and show it on UI text
        public void EarnScore(int score)
        {
            _totalScore += score;

            _scoreText.text = _totalScore.ToString();
        }
    }
}