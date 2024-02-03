using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelfRegester : MonoBehaviour
{
    public event Action trigger;
    public GameObject HarnesPrefebinsticated,model;

    //private void Start()
    //{
    //    Screen.sleepTimeout = SleepTimeout.NeverSleep;
    //}
    private void OnEnable()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void OnDisable()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    public void AssignPrefeb(GameObject harnes, GameObject mode)
    {
        if (harnes == null || mode == null)
        {
            return;
        }
        HarnesPrefebinsticated = harnes;
        model = mode;
        trigger.Invoke();
        if (model != null)
        {
        bool isActive = model.activeInHierarchy;

            if (isActive)
            {
                OnOFFModel();
            }
        }
    }

    public void DeAssignPrefeb()
    {

    }
    public void OnOFFModel()
    {
        //return;
        bool isActive = model.activeInHierarchy;
        model.SetActive(!isActive);
    }

}
