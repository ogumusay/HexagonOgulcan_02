using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.Board;


namespace Hexagon.Game
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private BoardSettings _boardSettings;
        [SerializeField] private Camera _camera;

        private void Start()
        {
            //Set camera position and size
            float camXPos = ((_boardSettings.Column - 1) * 1.75f) / 2;
            float camYPos = ((_boardSettings.Row * 2) - 1) / 2;

            _camera.transform.position = new Vector3(camXPos, camYPos, _camera.transform.position.z);
            _camera.orthographicSize = _boardSettings.Row >= _boardSettings.Column ? camYPos + 8 : camXPos * 2;
        }

    }
}