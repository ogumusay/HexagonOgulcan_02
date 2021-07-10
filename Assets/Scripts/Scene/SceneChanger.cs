using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hexagon.State;

namespace Hexagon.GameScene
{
    public enum GameScene
    {
        Game
    }

    [CreateAssetMenu(menuName = "Hexagon/Scene/Scene Changer")]
    public class SceneChanger : ScriptableObject
    {
        public void RestartGame()
        {
            GameSceneManager.Instance.StartCoroutine(LoadGameSceneAsync(GameScene.Game));
        }

        private IEnumerator LoadGameSceneAsync(GameScene scene)
        {
            StateManager.CurrentState = StateManager.State.DESTROYING_SCENE;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)scene);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}