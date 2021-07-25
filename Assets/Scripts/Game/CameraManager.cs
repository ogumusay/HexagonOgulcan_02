using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.Board;


namespace Hexagon.Game
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private BoardData _boardData;
        [SerializeField] private Camera _camera;

        private void Start()
        {
            //Set camera position and size
            float camXPos = ((_boardData.Column - 1) * 1.75f) / 2;
            float camYPos = ((_boardData.Row * 2) - 1) / 2;

            _camera.transform.position = new Vector3(camXPos, camYPos, _camera.transform.position.z);
            _camera.orthographicSize = _boardData.Row >= _boardData.Column ? camYPos + 8 : camXPos * 2;
        }

    }
}