using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexagon.GameScene
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;

        private void Start()
        {
            Instance = this;
        }
    }
}