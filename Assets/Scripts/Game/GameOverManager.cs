using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.Board;
using Hexagon.GameObjects;
using Hexagon.State;


namespace Hexagon.Game
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverUI;

        private void Awake()
        {
            BoardManager.OnNoMoveLeft += EndGame;
            HexagonBombObject.OnBombExplodes += EndGame;
        }

        private void EndGame()
        {
            StateManager.CurrentState = StateManager.State.GAME_OVER;

            _gameOverUI.SetActive(true);
        }

        private void OnDisable()
        {
            BoardManager.OnNoMoveLeft -= EndGame;
            HexagonBombObject.OnBombExplodes -= EndGame;
        }
    }
}