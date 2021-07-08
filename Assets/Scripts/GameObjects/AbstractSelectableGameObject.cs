using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            _onMouseUp?.Invoke();
        }
        
        private void OnDestroy() 
        {
            _onMouseUp -= SelectObject;
        }
    }
}
