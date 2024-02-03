using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Transform groundPlane;
    public Transform gameCamera;
    
    void Update()
    {
        if (groundPlane.gameObject.activeSelf)
        {
            Vector3 targetPostition = new Vector3( gameCamera.position.x, 
                groundPlane.transform.position.y, 
                gameCamera.position.z ) ;
            groundPlane.transform.LookAt( targetPostition ) ;
        }
    }
}
