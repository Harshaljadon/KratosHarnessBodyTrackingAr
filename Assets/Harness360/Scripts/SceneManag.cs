using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManag : SingletonGeneric<SceneManag>
{

    public int commonProductItemIndex;

    public ProductCatagory currentProductCatagory;
    public ProductSubCatagory _currentProductSubCatagory;
    public HarnessSize harnessSize;
    /// <summary>
    /// commingSoonHarnessCode will use this in commingSoon scene
    /// </summary>
    public string commingSoonHarnessCode;
    /// <summary>
    /// if user select harness manual in manual scene
    /// </summary>
    public bool manualHarnesSelection;

    public bool allHarnesCreated;

    /// <summary>
    /// product index for FACE = 0, BODY = 1, HAND = 2, LAG = 3, EQUIPMENT = 4
    /// </summary>

    public void SelectedProduct(int id)
    {
        currentProductCatagory = (ProductCatagory)id;
    }
    /// <summary>
    /// Individual button like eyeglass, helmet, mask,harness, shoes and glove
    /// EYEGLASS =0, HELMET = 1, MASK = 2, HARNESS = 3, GLOVES =4,SHOE =5,CONFIND_SPACE = 6, VERTICAL =7,OVER_ROOF = 8, OVER_HEAD = 9
    /// </summary>
    /// <param name="id">int value</param>
    public void SelectedSubProduct(int id)
    {
        _currentProductSubCatagory = (ProductSubCatagory)id;
    }

    /// <summary>
    /// Update currentProductCatagory when Qr scan for end result
    /// </summary>
    /// <param name="jsonQrProductName">it hold comman name of product</param>
    public void QR_UpdateProudctCatagory(string jsonQrProductName)
    {
        switch (jsonQrProductName)
        {
            case "FACE":
                currentProductCatagory = ProductCatagory.FACE;
                break;
            case "BODY":
                currentProductCatagory = ProductCatagory.BODY;
                break;
            case "HAND":
                currentProductCatagory = ProductCatagory.HAND;
                break;
            case "LEG":
                currentProductCatagory = ProductCatagory.LEG;
                break;
            case "EQUIPMENT":
                currentProductCatagory = ProductCatagory.EQUIPMENT;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Update currentSubProductCatagory when Qr scan for item slection
    /// </summary>
    /// <param name="jsonQrProductName">it hold comman name of sub product</param>
    public void QR_UpdateSub_ProudctCatagory(string jsonQrProductName)
    {
        switch (jsonQrProductName)
        {
            case "EYEGLASS":
                _currentProductSubCatagory = ProductSubCatagory.EYEGLASS;
                break;
            case "HELMET":
                _currentProductSubCatagory = ProductSubCatagory.HELMET;
                break;
            case "MASK":
                _currentProductSubCatagory = ProductSubCatagory.MASK;
                break;
            case "HARNESS":
                _currentProductSubCatagory = ProductSubCatagory.HARNESS;
                break;
            case "GLOVES":
                _currentProductSubCatagory = ProductSubCatagory.GLOVES;
                break;
            case "SHOE":
                _currentProductSubCatagory = ProductSubCatagory.SHOE;
                break;
            case "CONFIND_SPACE":
                _currentProductSubCatagory = ProductSubCatagory.CONFIND_SPACE;
                break;
            case "VERTICAL":
                _currentProductSubCatagory = ProductSubCatagory.VERTICAL;
                break;
            case "OVER_ROOF":
                _currentProductSubCatagory = ProductSubCatagory.OVER_ROOF;
                break;
            case "OVER_HEAD":
                _currentProductSubCatagory = ProductSubCatagory.OVER_HEAD;
                break;
            default:
                break;
        }
    }

    public bool splashingDone;
    /// <summary>
    /// Ar end result
    /// </summary>
    public void Load_AR_Scene()
    {
        switch (currentProductCatagory)
        {
            case ProductCatagory.FACE:
                FaceAr();
                break;
            case ProductCatagory.BODY:
                ARHarnessScene();
                break;
            case ProductCatagory.HAND:
                AR_Hand();
                break;
            case ProductCatagory.LEG:
                break;
            case ProductCatagory.EQUIPMENT:
                break;
            default:
                //ComingSoonProductScene();
                break;
        }
    }

    public void AR_Hand()
    {
        SceneManager.LoadScene(6);
    }

    /// <summary>
    /// All product or module, start point
    /// </summary>
    public void ProductModuleScene()
    {
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// Qr button or manual select scene 
    /// </summary>
    public void QR_Manual_Scene()
    {
        manualHarnesSelection = false;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Scan qr 
    /// </summary>
    public void QrScene()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// User can select any safet equipment related to body
    /// </summary>
    public void All_BODY_SAFTY_CollectionMainScene()
    {
        manualHarnesSelection = true;
        SceneManager.LoadScene(3);
    }

    //public PassHarnessToHumanBodyTracking _passHarnessToHumanBodyTracking;
    /// <summary>
    /// ar harness final result
    /// </summary>
    public void ARHarnessScene()
    {
        SceneManager.LoadSceneAsync(4, LoadSceneMode.Single).completed += SceneManag_completed;
    }

    private void SceneManag_completed(AsyncOperation obj)
    {
        if (obj.isDone)
        {
            //Invoke(nameof(GetRef), 1);
            //_passHarnessToHumanBodyTracking = FindObjectOfType<PassHarnessToHumanBodyTracking>();
            //_passHarnessToHumanBodyTracking.Passharness();
            //harnessRefPas.gameObject.SetActive(false);
            //harnessRefPas. GetComponent<Harness>(). hrmH.FindSelfRef();
        }
    }

    void FaceAr()
    {
        SceneManager.LoadScene(5);
    }
    /// <summary>
    /// this will call UI_Manager when user click size button and select size button
    /// </summary>
    /// <param name="id">0,1,2,3,4 => s,m,l,xl,xxl</param>
    public void SizeSelection(int id)
    {
        harnessSize = (HarnessSize)id;
    }

    /// <summary>
    /// when product code not match then coming soon scene will open
    /// </summary>
    public void ComingSoonProductScene()
    {
        SceneManager.LoadScene(5);
    }

}

[System.Serializable]
public enum ProductCatagory
{
    FACE, BODY, HAND, LEG, EQUIPMENT

}

[System.Serializable]
public enum ProductSubCatagory
{
    EYEGLASS, HELMET, MASK, HARNESS, GLOVES,SHOE, CONFIND_SPACE, VERTICAL,OVER_ROOF, OVER_HEAD

}

[System.Serializable]
public enum HarnessSize
{
    Small,Medium,Large,XLarge,XXLarge

}