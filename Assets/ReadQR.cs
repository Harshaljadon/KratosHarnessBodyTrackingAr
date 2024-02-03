using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.iOS;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


public class ReadQR : MonoBehaviour
{
    public TextMeshProUGUI resultText; // Reference to the UI Text element

    public bool isScanning = true, cameraOn; // Flag to control scanning

    // Specify the region where QR code should be detected
    public int scanRegionWidth = 400; // Width of the scan region
    public int scanRegionHeight = 400; // Height of the scan region

    //public GameLoader _gameLoaderFromJson;
    public GameData gameData = new GameData();
    public ApiResponse productCodes = new ApiResponse();

    //public string jsonText, deviceNameCamera, matchingApiId = "uatapi.arresto.in", matchingApiId2;
    public string jsonText, deviceNameCamera, matchingApiId, matchingApiId2;



    public RawImage backgound;
    private WebCamTexture backCam;
    //public AspectRatioFitter fit;
    //private Texture defaultBackground;

    FileSelector fileSelector;

    public HarnessCaryForwardData _harnessCaryForwardDataScriptable;
    public ProductCodeData _ProductCodeDataScriptable;

    internal void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
    }

    internal void PermissionCallbacks_PermissionGranted(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");
    }

    internal void PermissionCallbacks_PermissionDenied(string permissionName)
    {
        Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
    }

    AsyncOperation asyncOperation;

    private void Start()
    {
        //CallCamera();
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            CallCamera();
            InvokeRepeating(nameof(ScaneBarCode), 1, 1);
            //Debug.Log("webcam found");
        }
        else
        {
            asyncOperation = Application.RequestUserAuthorization(UserAuthorization.WebCam);
            asyncOperation.completed += ReadQR_completed;
        }

        fileSelector = FindObjectOfType<FileSelector>();
        if ( fileSelector != null)
        {
        fileSelector.onFileSelected += FileSelector_onFileSelected;

        }
    }

    private void FileSelector_onFileSelected(string obj)
    {
        jsonText = obj;
        productCodes =  JsonConvert.DeserializeObject<ApiResponse>(obj);
    }

    private void ReadQR_completed(AsyncOperation obj)
    {
        CallCamera();
        InvokeRepeating(nameof(ScaneBarCode), 1, 1);

    }

    void CallCamera()
    {
        WebCamDevice[] externalDevices = WebCamTexture.devices;

        //+++++++++++++++++
        //defaultBackground = backgound.texture;


        for (int i = 0; i < externalDevices.Length; i++)
        {
#if UNITY_EDITOR
            // Code to run in the Unity editor
            backCam = new WebCamTexture(deviceNameCamera);
#else
            if (!externalDevices[i].isFrontFacing)
            {
            
                deviceNameCamera = externalDevices[i].name;
                backCam = new WebCamTexture(externalDevices[i].name);

            }
            Debug.Log("DECODED TEXT FROM QR: " );
#endif

        }

        //if (backCam == null)
        //{
        //    backCam = new WebCamTexture(deviceNameCamera);
        //    return;
        //}
        backCam.Play();
        cameraOn = backCam.isPlaying;
        backgound.texture = backCam;
        resultText.text = "";
        //++++++++++++++++++

        // Set the RawImage to display the camera feed
        backgound.texture = backCam;



        //string a = "https://aspl-bucket.s3.us-west-2.amazonaws.com/gameobject.json";
        //StartCoroutine(LoadGameData(a));

    }



    private void ScaneBarCode()
    {
        //Debug.Log("Repeat");

        if (backCam != null && isScanning)
        {
            try
            {
                //+++++
                //float ratio = (float)backCam.width / (float)backCam.height;
                //fit.aspectRatio = ratio;
                float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
                backgound.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
                //++++
                IBarcodeReader barcodeReader = new BarcodeReader();

                // Calculate the center region to scan for QR code
                int centerX = backCam.width / 2 - scanRegionWidth / 2;
                int centerY = backCam.height / 2 - scanRegionHeight / 2;

                // Decode the current frame from the center region
                var result = DecodeQRCodeInRegion(backCam, centerX, centerY, scanRegionWidth, scanRegionHeight);

                int orientation = -backCam.videoRotationAngle;

                backgound.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);


                if (result != null)
                {
                    //Debug.Log("DECODED TEXT FROM QR: " + result.Text);

                    // Display the result on the screen using the UI Text element
                    resultText.text =  "Loading...";
                    // Define a regular expression pattern to match URLs
                    string pattern = @"^(https?://)?([\w.-]+)";

                    // Use Regex.Match to find the first match in the input string
                    Match match = Regex.Match(result.Text, pattern);
                    if (match.Success)
                    {
                        var extractedURL = match.Groups[2].Value;
                    //Debug.Log("DECODED TEXT FROM QR: " + extractedURL);
                        var matched = string.Equals(extractedURL, matchingApiId );
                        if (!matched)
                        {
                            resultText.text = "Wrong Qr ";
                            return;
                        }
                        //Debug.Log(extractedURL);
                    }
                    StartCoroutine(LoadGameData(result.Text));
                    // Set the flag to stop scanning further
                    isScanning = false;
                }
                else
                {
                    // Clear the text if no QR code is detected
                    resultText.text = "";
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }
    }


    private IEnumerator LoadGameData(string jsonUrl)
    {
        //Debug.Log(jsonUrl);


        using (UnityWebRequest webRequest = UnityWebRequest.Get(jsonUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch JSON file: " + webRequest.error);
                yield break;
            }

            jsonText = webRequest.downloadHandler.text;
            //Debug.Log(jsonText);

            productCodes = JsonUtility.FromJson<ApiResponse>(jsonText);
            if (productCodes.message == "Data not found")
            {
                Debug.Log("Data not found");
                resultText.text = "Data not found, Please use other Qr code";
                isScanning = true;
                StopCoroutine(nameof(LoadGameData));
            }

            if (productCodes.data.data.Count != 0)
            {
                //gameData = JsonUtility.FromJson<GameData>(jsonText);
                if (productCodes.data.data[0] != null)
                {
                    string productType, productSubType;
                    _harnessCaryForwardDataScriptable.userMailId = productCodes.data.user.email;
                    if (_ProductCodeDataScriptable.GetEquipmentType(productCodes.data.data[0].product_code.ToUpper(), out productType, out productSubType))
                    {
                        _harnessCaryForwardDataScriptable.productItemCode = productCodes.data.data[0].product_code.ToUpper();
                        float qrQuestionWeight;
                        if (float.TryParse(productCodes.data.weight, out qrQuestionWeight))
                        {
                            _harnessCaryForwardDataScriptable.userWeight = qrQuestionWeight;

                        }
                        else
                        {
                            _harnessCaryForwardDataScriptable.userWeight = 95f;
                        }
                        //_harnessCaryForwardDataScriptable.productCatogorie = gameData.gameObjects[0].ProductCatagory; // end result ar
                        //    FACE, BODY, HAND, LEG, EQUIPMENT
                        _harnessCaryForwardDataScriptable.productCatogorie = productType; // end result ar
                        SceneManag.Instance.QR_UpdateProudctCatagory(_harnessCaryForwardDataScriptable.productCatogorie);

                        _harnessCaryForwardDataScriptable.productSubCatogorie = productSubType; //collection slection
                                                                                                //    EYEGLASS, HELMET, MASK, HARNESS, GLOVES,SHOE, CONFIND_SPACE, VERTICAL,OVER_ROOF, OVER_HEAD
                        SceneManag.Instance.QR_UpdateSub_ProudctCatagory(_harnessCaryForwardDataScriptable.productSubCatogorie);

                        switch (SceneManag.Instance._currentProductSubCatagory)
                        {
                            case ProductSubCatagory.EYEGLASS:
                                GetProductItemFrommCollection(_harnessCaryForwardDataScriptable.eyeGlassCollection);
                                break;
                            case ProductSubCatagory.HELMET:
                                GetProductItemFrommCollection(_harnessCaryForwardDataScriptable.hemetCollection);

                                break;
                            case ProductSubCatagory.MASK:
                                GetProductItemFrommCollection(_harnessCaryForwardDataScriptable.maskCollection);

                                break;
                            case ProductSubCatagory.HARNESS:
                    //Debug.Log(productCodes.data.data[0].product_code.ToUpper());
                                GetProductItemFrommCollection(_harnessCaryForwardDataScriptable.collectionHarness);

                                break;
                            case ProductSubCatagory.GLOVES:
                                GetProductItemFrommCollection(_harnessCaryForwardDataScriptable.gloveCollection);

                                break;
                            case ProductSubCatagory.SHOE:
                                GetProductItemFrommCollection(_harnessCaryForwardDataScriptable.shoeCollection);

                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        // trafer to blank scene where coming soon show for harness
                        SceneManag.Instance.commingSoonHarnessCode = productCodes.data.data[0].product_code.ToUpper();
                        SceneManag.Instance.ComingSoonProductScene();
                    }
                }
            }

          
        }
    }

    void GetProductItemFrommCollection(List<HarnessSorting> harnessSortings)
    {
        var uppercaseCodename = productCodes.data.data[0].product_code.ToUpper();
        foreach (var item in harnessSortings)
        {
                //Debug.Log(item.itemProductCodeName + " " + gameData.gameObjects[0].itemProductCodeName);
            if (item.itemProductCodeName == uppercaseCodename)
            {
                _harnessCaryForwardDataScriptable.productItemScriptableIndex = item.itemCollectionId;

                SceneManag.Instance.Load_AR_Scene();
                break;
            }
        }
    }

    private Result DecodeQRCodeInRegion(WebCamTexture camTexture, int x, int y, int width, int height)
    {
        Texture2D regionTexture = new Texture2D(width, height);
        regionTexture.SetPixels(camTexture.GetPixels(x, y, width, height));
        regionTexture.Apply();

        IBarcodeReader barcodeReader = new BarcodeReader();
        Color32[] pixels = regionTexture.GetPixels32();
        return barcodeReader.Decode(pixels, width, height);
    }




    [System.Serializable]
    public class GameObjectData
    {
        public string UserMailId;
        public string ProductCatagory;  // FACE, BODY, HAND, LAG, EQUIPMENT
        public string ProductSubCatagory; // EYEGLASS, HELMET, MASK, HARNESS, GLOVES,SHOE, CONFIND_SPACE, VERTICAL,OVER_ROOF, OVER_HEAD
        public string itemProductCodeName;
        public string Weight;
        //public Vector3 position;
    }

    [System.Serializable]
    public class GameData
    {
        public GameObjectData[] gameObjects;
        //public Welcome[] gameOb;
    }
    /// <summary>
    /// FROM QR SCANNER TO MODE SELECTION QR or Manual
    /// </summary>
    public void BackToModeScene()
    {
        SceneManag.Instance.QR_Manual_Scene();
    }



    [System.Serializable]
    public class ProductData
    {
        public int product_id;
        public int category_id;
        public string product_code;
        public string product_type;
    }

    [System.Serializable]
    public class UserData
    {
        public int id;
        public string name;
        public string email;
        public string mobile_country_code;
        public string mobile;
        public string profile_image;
    }

    [System.Serializable]
    public class UserProfile
    {
        public int id;
        public int user_id;
        public string weight; // Use the appropriate data type for weight
        public List<ProductData> data;
        public UserData user;
    }

    [System.Serializable]
    public class ApiResponse
    {
        public string status;
        public string message;
        public UserProfile data;
    }

}


