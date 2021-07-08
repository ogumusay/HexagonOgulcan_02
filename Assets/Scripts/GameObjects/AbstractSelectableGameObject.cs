using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexagon.State;

namespace Hexagon.GameObjects
{
    public abstract class AbstractSelectableGameObject : MonoBehaviour, ISelectable
    {        
        private event Action _onMouseUp;

        private void Awake() 
        {
            _onMouseUp += SelectObject;
        }

        public virtual void SelectObject()
        {
            
        }

        private void OnMouseUp() 
        {
            if (StateManager.CurrentState == StateManager.State.EMPTY)
            {
                _onMouseUp?.Invoke();                
            }
        }
        
        private void OnDestroy() 
        {
            _onMouseUp -= SelectObject;
        }
    }
}
