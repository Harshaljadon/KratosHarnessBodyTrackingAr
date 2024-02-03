using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class UnderRangeStates : MonoBehaviour
{
    List<GameObject> triggerObject;
    [SerializeField][HideInInspector]
    GameObject objectToBeDetect;
    [SerializeField]
    //bool objUnder;
    public event Action<GameObject> objectDected;
    public event Action enableScript;
    public float angle;

    [SerializeField]
    bool forTriPod;

    [SerializeField]
    bool forDAPod;
    bool greaterThanNegative90;

    public bool _GreaterThanNegative90
    {
        get
        {
            if (angle < 0 && triggerObject.Count ==0)
            {
                if (angle > -90)
                {
                     greaterThanNegative90 = true;
                }
                else
                {
                    greaterThanNegative90 = false;

                }
            }
            else
            {
                if (angle > -90)
                {
                    greaterThanNegative90 = false;
                }
                else
                {
                    greaterThanNegative90 = true;

                }
            }
            return greaterThanNegative90;
        }
    }
    [SerializeField]
    Transform iconBase;

    public GameObject[] lookAtUiHolder;
    public modelCatagoryBaseAngle selectedModelCata;

    private void Start()
    {
        triggerObject = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("MainCamera"))
        {
        triggerObject.Add(other.gameObject);

        }
        if (other.gameObject.CompareTag("MainCamera"))
        {
            objectToBeDetect = other.gameObject;
            var playerPos = objectToBeDetect.transform.position;
            var myPos = this.transform.position;
            var myForwardDirect = this.transform.forward;
            AngleBetVect aBV = new AngleBetVect(playerPos, myPos, myForwardDirect);
            angle = aBV.AngleBetween();
            if (modelCatagoryBaseAngle.da01_2_3_hexaPod == selectedModelCata)
            {
                iconBase.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                InvokeEvents();
                StartCoroutine(nameof(OnOffLookAtInfoAndDescriptionpanel));
                return;
            }
            if (modelCatagoryBaseAngle.tripod == selectedModelCata)
            {
                InvokeEvents();

                

                return;
            }
            if (modelCatagoryBaseAngle.other == selectedModelCata)
            {
                if (angle < 0)
                {
                    if (triggerObject.Count != 0)
                    {
                        if (angle > -90)
                        {
                            float rangeNegative0to60 = Mathf.Clamp(angle, -40, 0);
                            iconBase.localRotation = Quaternion.Euler(new Vector3(0, rangeNegative0to60 - 140, 0));

                        }
                        else
                        {
                            float rangeNegative120to180 = Mathf.Clamp(angle, -180, -100);

                            iconBase.localRotation = Quaternion.Euler(new Vector3(0, rangeNegative120to180 + 140, 0));

                        }

                    }
                    else
                    {
                        if (angle > -20 || angle < -160)
                        {
                            iconBase.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
                        }
                        else if (-65 > angle && angle > -90)
                        {
                            iconBase.localRotation = Quaternion.Euler(new Vector3(0, -65, 0));

                        }
                        else if (-90 > angle && angle > -115)
                        {
                            iconBase.localRotation = Quaternion.Euler(new Vector3(0, -115, 0));

                        }
                        else
                        {
                            iconBase.localRotation = Quaternion.Euler(new Vector3(0, angle, 0));

                        }
                    }


                    InvokeEvents();
                }

            }

        }
    }


    IEnumerator OnOffLookAtInfoAndDescriptionpanel()
    {

        var max = lookAtUiHolder.Length;
        for (int i = 0; i < max; i++)
        {
            lookAtUiHolder[i].GetComponent<LookAtCameraUI>().enabled = true;
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < max; i++)
        {
            lookAtUiHolder[i].GetComponent<LookAtCameraUI>().enabled = false;
        }
        yield return new WaitForSeconds(1);
    }
    void InvokeEvents()
    {
        enableScript?.Invoke();
        objectDected?.Invoke(objectToBeDetect);
    }


}

public enum modelCatagoryBaseAngle
{
    tripod,
    da01_2_3_hexaPod,
    other// winches, bracket


}