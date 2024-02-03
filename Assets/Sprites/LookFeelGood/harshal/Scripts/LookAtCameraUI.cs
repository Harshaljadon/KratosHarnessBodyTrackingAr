using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtCameraUI : MonoBehaviour
{
    Camera targetCamera; // Reference to the camera the UI should look at
    private RectTransform uiElement; // Reference to the UI element you want to attach to the camera

    public bool LookAtForOneSec;
    bool occuerAferSecs;

    private void OnEnable()
    {
        if (targetCamera == null)
        {
        targetCamera = Camera.main;

        }
        uiElement = this.GetComponent<RectTransform>();

    }

    IEnumerator LookAtOnOffInSec()
    {

        yield return new WaitForSeconds(2);
        occuerAferSecs = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (targetCamera != null && !occuerAferSecs)
        {
            uiElement.LookAt(targetCamera.transform.position);
            uiElement.Rotate(0, 180, 0); // Optionally, rotate the UI element by 180 degrees to make it face the camera correctly
        }
        if (LookAtForOneSec)
        {
            StartCoroutine(nameof(LookAtOnOffInSec));
        }
    }
}
