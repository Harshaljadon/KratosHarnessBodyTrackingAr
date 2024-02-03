using System;
using UnityEngine;


public class HarnessScreenShotHandler : MonoBehaviour
{
    int ssCounter = 0;

    public void TakeScreenShot()
    {
        if (PlayerPrefs.HasKey("ssCounter"))
        {
            ssCounter = PlayerPrefs.GetInt("scCounter");
        }
        else
        {
            PlayerPrefs.SetInt("scCounter", 0);
        }

        string path = Application.persistentDataPath + "/" + "Harness_ScreenShot" + ssCounter + ".png";
        ssCounter++;
        PlayerPrefs.SetInt("scCounter", ssCounter);
        ScreenCapture.CaptureScreenshot("Harness_ScreenShot" + ssCounter + ".png");
    }

}