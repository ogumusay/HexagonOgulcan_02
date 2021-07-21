using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hexagon.Board;
using System;


namespace Hexagon.GameObjects
{
    public class HexagonBombObject : HexagonObject
    {
        [SerializeField] Text countdownText;
        public int countdown = 7;

        public static event Action OnBombExplodes;

        private void Awake()
        {
            countdownText.text = countdown.ToString();
        }

        //If countdown hits 0, end game
        public void CountDown()
        {
            countdown--;
            countdownText.text = countdown.ToString();
            if (countdown <= 0)
            {   
                OnBombExplodes?.Invoke();
            }
        }
    }
}