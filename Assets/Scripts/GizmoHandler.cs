using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AR2
{

    enum GizmoState
    {
        Translate,
        Rotate,
        None
    }
    
    
    public class GizmoHandler : MonoBehaviour
    {
        private GizmoState currentState = GizmoState.None;
        
        
        public Button moveButton;
        public Button rotateButton;

        public GizmoTranslateScript gizmoTranslator;
        public GizmoRotateScript gizmoRotator;


        private void Start()
        {
            SetGizmoState(GizmoState.None);
        }

        public void OnTranslateSelected()
        {
            if (currentState != GizmoState.Translate)
            {
                SetGizmoState(GizmoState.Translate);
            }
            else
            {
                SetGizmoState(GizmoState.None);
            }
        }
        
        public void OnRotateSelected()
        {
            if (currentState != GizmoState.Rotate)
            {
                SetGizmoState(GizmoState.Rotate);
            }
            else
            {
                SetGizmoState(GizmoState.None);
            }
        }

        private void SetGizmoState(GizmoState newState)
        {
            switch (newState)
            {
                case GizmoState.Translate:
                {
                    currentState = GizmoState.Translate;
                    gizmoTranslator.gameObject.SetActive(true);
                    gizmoRotator.gameObject.SetActive(false);
                }
                    break;
                case GizmoState.Rotate:
                {
                    currentState = GizmoState.Rotate;
                    gizmoTranslator.gameObject.SetActive(false);
                    gizmoRotator.gameObject.SetActive(true);
                    
                }
                    break;
                case GizmoState.None :
                {
                    currentState = GizmoState.None;
                    gizmoTranslator.gameObject.SetActive(false);
                    gizmoRotator.gameObject.SetActive(false);
                    
                }
                    break;
            }
        }

        public void EnableGizmos()
        {
            SetGizmoState(currentState);
            moveButton.gameObject.SetActive(true);
            rotateButton.gameObject.SetActive(true);
        }
        
        public void DisableGizmos()
        {
            moveButton.gameObject.SetActive(false);
            rotateButton.gameObject.SetActive(false);
            gizmoTranslator.gameObject.SetActive(false);
            gizmoRotator.gameObject.SetActive(false);
        }
        
        
        
    }
}

