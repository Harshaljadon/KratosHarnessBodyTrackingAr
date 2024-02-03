using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class ProcessDeepLinkMngr : MonoBehaviour
{
    public static ProcessDeepLinkMngr Instance { get; private set; }
    public string deeplinkURL, replacedUrl, deepLinkingJsontext;
    public ApiResponse productCodes = new ApiResponse();

    public HarnessCaryForwardData _harnessCaryForwardDataScriptable;
    public ProductCodeData _ProductCodeDataScriptable;
    public TextMeshProUGUI productCodeTextDebug;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void onDeepLinkActivated(string url)
    {
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
        deeplinkURL = url;
        //replacedUrl = url.Replace("arrestoar://", "https://uatapi.arresto.in/");
        replacedUrl = url.Replace("arrestoar://", "https://api.arresto.in/");
        StartCoroutine(FatchindJsonFromUrl(replacedUrl));

        // Decode the URL to determine action. 
        // In this example, the app expects a link formatted like this:
        // unitydl://mylink?scene1
        //string sceneName = url.Split("?"[0])[1];
        //bool validScene;
        //switch (sceneName)
        //{
        //    case "scene1":
        //        validScene = true;
        //        break;
        //    case "scene2":
        //        validScene = true;
        //        break;
        //    default:
        //        validScene = false;
        //        break;
        //}
        //if (validScene) SceneManager.LoadScene(sceneName);
    }

    IEnumerator FatchindJsonFromUrl(string jsonUrl)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(jsonUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch JSON file: " + webRequest.error);
                yield break;
            }

            deepLinkingJsontext = webRequest.downloadHandler.text;

            productCodes = JsonUtility.FromJson<ApiResponse>(deepLinkingJsontext);

            if (productCodes.message == "Data not found")
            {

                StopCoroutine(nameof(FatchindJsonFromUrl));
            }

            if (productCodes.data.data.Count != 0)
            {
                if ( productCodes.data.data[0] != null)
                {
                    productCodeTextDebug.text = productCodes.data.data[0].product_code.ToUpper();
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
            //Debug.Log(jsonText);
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