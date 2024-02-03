using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation.Samples;


public class ARController : MonoBehaviour
{

    public ARSession sessionPrefab;
    public Button closeScrrenshotButton, snapShootButton, backScenMenuButton;
    public HumanBodyTracker _humanBodyTracker;
    public ARHumanBodyManager humanBodyManager;

    public void SetArMode(bool isAROn)
    {
        //ChangeARSession(isAROn);
    }

    private void Start()
    {
        //_humanBodyTracker = FindObjectOfType<HumanBodyTracker>();
        closeScrrenshotButton.GetComponent<Button>().onClick.AddListener(() => { StartSession(); });
        snapShootButton.GetComponent<Button>().onClick.AddListener(() => { StopSession(); });
        backScenMenuButton.GetComponent<Button>().onClick.AddListener(() => { BacktoOR_ManualScen(); });
        //StartSession();
    }

    void BacktoOR_ManualScen()
    {
        SceneManag.Instance.QR_Manual_Scene();
    }

    private void ChangeARSession(bool newState)
    {
        if (newState)
        {
            StartSession();
        }
        else
        {
            StopSession();
        }
    }

    public void StartSession()
    {
        StartCoroutine(StartARSession());
    }

    public void StopSession()
    {
        StartCoroutine(StopARSession());

    }

    //IEnumerator DoStart()
    //{
    //    if (sessionPrefab)
    //    {
    //        Destroy(sessionPrefab.gameObject);
    //        yield return null;
    //    }
    //    else
    //    {
    //        yield return null;
    //    }

    //    if (sessionPrefab != null)
    //    {
    //        sessionPrefab = Instantiate(sessionPrefab).GetComponent<ARSession>();
    //    }
    //}

    IEnumerator StopARSession()
    {
        yield return new WaitForSeconds(1);
        if (humanBodyManager != null && sessionPrefab != null)
        {
            sessionPrefab.GetComponent<ARSession>().Reset();
            humanBodyManager.enabled = false;
            _humanBodyTracker.enabled = false;
            //_humanBodyTracker.GetComponent<HumanBodyTracker>().RestartSession = true;
            //foreach (var item in humanBodyManager.trackables)
            //{
            //    item.gameObject.SetActive(false);
            //}
        }

        //if (arSession != null)
        //{
        //    arSession.Reset();
        //}


    }
    IEnumerator StartARSession()
    {
        yield return null;

        if (humanBodyManager != null)
        {
            //_humanBodyTracker.RestartSession = false;
            humanBodyManager.enabled = true;
            _humanBodyTracker.enabled = true;
        }
    }

    public void ReStartArSession()
    {
        StartCoroutine(RestartSessionProcess());
    }

    IEnumerator RestartSessionProcess()
    {
        yield return new WaitForSeconds(.1f);
        if (humanBodyManager != null && sessionPrefab != null)
        {
            sessionPrefab.GetComponent<ARSession>().Reset();
            humanBodyManager.enabled = false;
            _humanBodyTracker.enabled = false;
            //_humanBodyTracker.GetComponent<HumanBodyTracker>().RestartSession = true;
            //foreach (var item in humanBodyManager.trackables)
            //{
            //    item.gameObject.SetActive(false);
            //}
        }

        yield return new WaitForSeconds(.5f);
        //yield return null;

        if (humanBodyManager != null)
        {
            //_humanBodyTracker.RestartSession = false;
            humanBodyManager.enabled = true;
            _humanBodyTracker.enabled = true;
        }
    }
}
