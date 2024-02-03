using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotator : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float rotationSpeed;
    public float rotationDamping;
    public Transform dragItem;

    public bool canRotate = false;
    
    private float _rotationVelocity;
    private bool _dragged;

    public void SetDragItem(GameObject item)
    {
        dragItem = item.transform;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragged = true;
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        if (dragItem == null || !canRotate)
        {
            return;
        }
        
        _rotationVelocity = eventData.delta.x * rotationSpeed;
        dragItem.Rotate(Vector3.down, -_rotationVelocity, Space.Self);
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        _dragged = false;
    }
 
    private void Update()
    {
        if (dragItem == null || !canRotate)
        {
            return;
        }
        
        if( !_dragged && !Mathf.Approximately( _rotationVelocity, 0 ) )
        {
            float deltaVelocity = Mathf.Min(
                Mathf.Sign(_rotationVelocity) * Time.deltaTime * rotationDamping,
                Mathf.Sign(_rotationVelocity) * _rotationVelocity
            );
            _rotationVelocity -= deltaVelocity;
            dragItem.Rotate(Vector3.down, -_rotationVelocity, Space.Self);
        }
    }
}