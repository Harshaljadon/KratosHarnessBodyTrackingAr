using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArInteractionPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ArPlaceObject arPlaceObject;
    
    
    void Start()
    {
        if (arPlaceObject == null)
            arPlaceObject = FindObjectOfType<ArPlaceObject>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (arPlaceObject)
        {
            arPlaceObject.isInInteractableZone = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (arPlaceObject)
        {
            arPlaceObject.isInInteractableZone = false;
        }
    }
}
