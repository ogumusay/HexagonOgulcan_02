using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hexagon.State;
using Hexagon.GameObjects;
using Hexagon.Board;

namespace Hexagon.Game
{
    public class SelectionController : MonoBehaviour
    {
        [SerializeField] private SelectionControllerSettings _selectionControllerSettings;

        private void Update()
        {
            ObjectSelectionProcess();
        }

        private void SelectObject()
        {
            Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero);

            if (hit.collider != null)
            {
                AbstractSelectableGameObject selectableGameObject = hit.collider.GetComponent<AbstractSelectableGameObject>();

                if (selectableGameObject != null)
                {
                    _selectionControllerSettings.SelectObjects(selectableGameObject);
                }
            }
        }

        private void ObjectSelectionProcess()
        {

            if (StateManager.CurrentState == StateManager.State.EMPTY)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    SelectObject();
                }
            }
        }

    }
}