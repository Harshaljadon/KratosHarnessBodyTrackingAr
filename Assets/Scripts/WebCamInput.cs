using UnityEngine;
using UnityEngine.UI;

public class WebCamInput : MonoBehaviour
{
    [SerializeField] string webCamName;
    [SerializeField] Vector2 webCamResolution;// = new Vector2(1920, 1080);
    [SerializeField] Texture staticInput;

    // Provide input image Texture.
    public Texture inputImageTexture{
        get{
            if(staticInput != null) return staticInput;
            return inputRT;
        }
    }

    public WebCamTexture webCamTexture;
    RenderTexture inputRT;

    AsyncOperation asyncOperationHandcameraCallBack;


    void Start()
    {
        webCamResolution = new Vector2(Screen.width, Screen.height);
        //CallCamera();
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            HandTraceCamera();

            //Debug.Log("webcam found");
        }
        else
        {
            asyncOperationHandcameraCallBack = Application.RequestUserAuthorization(UserAuthorization.WebCam);
            asyncOperationHandcameraCallBack.completed += AsyncOperationHandcameraCallBack_completed;
        }
    }

    private void AsyncOperationHandcameraCallBack_completed(AsyncOperation obj)
    {
        HandTraceCamera();
    }

    void HandTraceCamera()
    {
        //webCamResolution = new Vector2(Screen.height, Screen.width);
        WebCamDevice[] externalDevices = WebCamTexture.devices;
//#if UNITY_EDITOR
//        webCamName = externalDevices[0].name;

//#elif !UNITY_EDITOR


//#endif

        for (int i = 0; i < externalDevices.Length; i++)
        {
            if (!externalDevices[i].isFrontFacing)
            {
                webCamName = externalDevices[i].name;


            }
        }
        //backCam = new WebCamTexture(externalDevices[i].name);

        if (staticInput == null)
        {
            webCamTexture = new WebCamTexture(webCamName, (int)webCamResolution.x, (int)webCamResolution.y); //, (int)webCamResolution.x, (int)webCamResolution.y
            webCamTexture.Play();
        }
        inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 24);
        //inputRT = webCamTexture.
    }
    public int orient;

    public RawImage displayObScreen;

    void Update()
    {
        if(staticInput != null) return;
        if(!webCamTexture.didUpdateThisFrame) return;

        var aspect1 = (float)webCamTexture.width / webCamTexture.height;
        var aspect2 = (float)inputRT.width / inputRT.height;
        var aspectGap = aspect2 / aspect1;

        //var vMirrored = webCamTexture.videoVerticallyMirrored;
        var vMirrored = webCamTexture.videoVerticallyMirrored; // Change to horizontally mirrored
        var scale = new Vector2(aspectGap, vMirrored ? -1 : 1);
        //var scale = new Vector2(vMirrored ? -1 * a : 1 * a, aspectGap * b);        // Adjust scaling
        var offset = new Vector2((1 - aspectGap) / 2, vMirrored ? 1 : 0);
        //var offset = new Vector2(vMirrored ? 1 : 0, (1 - aspectGap) / 2);
            orient = -webCamTexture.videoRotationAngle;
         

        Graphics.Blit(webCamTexture, inputRT, new Vector2(scale.x, scale.y), offset);


        //displayObScreen.texture = webCamTexture;

        float scaleY = webCamTexture.videoVerticallyMirrored ? -1f : 1f;
        //displayObScreen.rectTransform.localScale = new Vector3(1f, scaleY, 1f);




        //Graphics.Blit(webCamTexture, inputRT);
    }
    //bool toogle;
    //public void TogglecamRotation()
    //{
    //     toogle = !toogle;

    //}

    void OnDestroy(){
        if (webCamTexture != null) Destroy(webCamTexture);
        if (inputRT != null) Destroy(inputRT);
    }
}
