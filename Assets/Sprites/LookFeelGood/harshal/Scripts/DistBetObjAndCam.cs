using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class DistBetObjAndCam : MonoBehaviour
{
    private GameObject MyCamera;
    [SerializeField]
    float distance;
    UnderRangeStates uRS;
    

    public float Distance { get => distance; private set => distance = value; }

    private void OnEnable()
    {
        uRS = GetComponent<UnderRangeStates>();
        uRS.objectDected += URS_objectDectedPassObject;
        
    }

    private void OnDisable()
    {
        uRS.objectDected -= URS_objectDectedPassObject;
    }

    private void URS_objectDectedPassObject(GameObject obj)
    {
        MyCamera = obj;
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (MyCamera != null )
        {
            distance = Vector3.Distance(this.transform.position, MyCamera.transform.position);

        }
    }
}
