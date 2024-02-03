using System;
using System.Collections;
using UnityEngine;


public class ScreenShotHandler : MonoBehaviour
{
    int ssCounter = 0;

    public void TakeScreenShotwithDelay(float delay)
    {
        Invoke(nameof(TakeScreenShot), delay);        
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("ssCounter"))
        {
            ssCounter = PlayerPrefs.GetInt("scCounter");
        }
        else
        {
            PlayerPrefs.SetInt("scCounter", 0);
        }

        //string path = Application.persistentDataPath + "/" + "Harness_ScreenShot" + ssCounter + ".png";
        //ssCounter++;
        //PlayerPrefs.SetInt("scCounter", ssCounter);
        //ScreenCapture.CaptureScreenshot("Harness_ScreenShot" + ssCounter + ".png");
    }

    private void TakeScreenShot()
    {
        StartCoroutine(TakeScreenshotAndSave());
    }

    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // set name
        string name = "Harness_ScreenShot" + ssCounter + ".png";
        ssCounter++;
        PlayerPrefs.SetInt("scCounter", ssCounter);

        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "HarnessAR", name, (success, path) => Debug.Log("Media save result: " + success + " " + path));

        Debug.Log("Permission result: " + permission);

        // To avoid memory leaks
        Destroy(ss);
    }

}