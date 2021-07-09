using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Hexagon.GameObjects
{
    public class HexagonBombObject : HexagonObject
    {
        [SerializeField] Text countdownText;
        public int countdown = 7;

        protected override void Awake()
        {
            countdownText.text = countdown.ToString();
            base.Awake();
        }

        //If countdown hits 0, end game
        public void CountDown()
        {
            countdown--;
            countdownText.text = countdown.ToString();
            if (countdown <= 0)
            {
                
            }
        }
    }
}