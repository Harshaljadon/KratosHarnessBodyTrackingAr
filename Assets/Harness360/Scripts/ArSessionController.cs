using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ArSessionController : MonoBehaviour
{

    public HarnessManager harnessManager;
    public GameObject arSessionOrigin;
    public GameObject arCamera;

    public GameObject sceneBackground;
    public GameObject sceneParticle;
    
    private ARSession session;
    public GameObject sessionPrefab;
    public karam.PlaceOnPlane placeOnPlane;
    public HarnessDragRotator dragRotator;

    private void Awake()
    {
        arCamera.transform.position = new Vector3(0,0,-1.5f);
        arCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void StartSession()
    { 
        if(harnessManager.CurrentHarness == null)
            return;
        
        arSessionOrigin.transform.position = Vector3.zero;
        arSessionOrigin.transform.rotation = Quaternion.Euler(Vector3.zero);
        
        arCamera.transform.position = new Vector3(0,0,-1.5f);
        arCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
    
        sceneBackground.gameObject.SetActive(false);
        sceneParticle.gameObject.SetActive(false);
        
        harnessManager.CurrentHarness.gameObject.SetActive(false);
        
        StartCoroutine(DoStart());
        dragRotator.canRotate = false;
        
        
    }

    public void StopSession()
    {
        if (session != null)
        {
        Destroy(session.gameObject);
        }
        dragRotator.canRotate = true;
        
        arSessionOrigin.transform.position = Vector3.zero;
        arSessionOrigin.transform.rotation = Quaternion.Euler(Vector3.zero);
        
        arCamera.transform.position = new Vector3(0,0,-1.5f);
        arCamera.transform.rotation = Quaternion.Euler(Vector3.zero);

        harnessManager.CurrentHarness.gameObject.transform.position = Vector3.zero;
        harnessManager.CurrentHarness.gameObject.SetActive(true);
        
        
        sceneBackground.gameObject.SetActive(true);
        sceneParticle.gameObject.SetActive(true);

        // stop touch 
        placeOnPlane.objectPlaced = false;

    }
    
    
    IEnumerator DoStart()
    {
        if (session)
        {
            Destroy(session.gameObject);
            yield return null; 
        }
        else
        {
            yield return null;
        }
        

        if (sessionPrefab != null)
        {
            session = Instantiate(sessionPrefab).GetComponent<ARSession>();
        }

    }
}
