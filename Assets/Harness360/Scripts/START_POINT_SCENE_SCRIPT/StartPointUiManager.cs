
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StartPointUiManager : MonoBehaviour
{
    public RectTransform[] productUI;
    public Image fadeOutFrontBG;
    public RectTransform splashPanel;
    [SerializeField] Animator scalingArAnime;
    public GameObject _bgKratos, splashPanelParent;
    public TextMeshProUGUI title;

    // Start is called before the first frame update
    void Start()
    {
        title.DOFade(0, 0);
        _bgKratos.SetActive(false);

        fadeOutFrontBG.gameObject.SetActive(false);
        foreach (var item in productUI)
        {
            item.localScale = new Vector3(0, 0, 0);

        }
        if (SceneManag.Instance.splashingDone)
        {
            splashPanelParent.gameObject.SetActive(false);
            ScaleUp_Fade();
            return;
        }
    }

    public void SlashHit()
    {
        splashPanel.GetComponent<Image>().DOFade(0, 1f);
        var collectionChildImageSplash = splashPanel.GetComponentsInChildren<Image>();
        var collectionChildTextMeshProSplash = splashPanel.GetComponentsInChildren<TextMeshProUGUI>();
        scalingArAnime.SetBool("Scaling", true);
        foreach (var item in collectionChildImageSplash)
        {
            item.DOFade(0, 1f);
        }

        foreach (var item in collectionChildTextMeshProSplash)
        {
            item.DOFade(0, 1f);
        }
        Invoke(nameof(ScaleUp_Fade), 1.2f);
        SceneManag.Instance.splashingDone = true;
    }

    void ScaleUp_Fade()
    {
        _bgKratos.SetActive(true);
        fadeOutFrontBG.gameObject.SetActive(true);
        fadeOutFrontBG.DOFade(0, .5f);
        title.DOFade(1, 1.5f);
        foreach (var item in productUI)
        {
            item.DOScale(Vector3.one, 1);

        }
        splashPanel.gameObject.SetActive(false);
    }

    //public void HarnessModulScen()
    //{
    //    SceneManag.Instance.SlectionProductItemFaceBodyModeScene();
    //}

    //public void FaceModulScen()
    //{
    //    SceneManag.Instance.FaceAr();
    //}



    /// <summary>
    /// pass ids for product 
    /// product index for FACE = 0, BODY = 1, HAND = 2, LAG = 3, EQUIPMENT = 4
    /// </summary>
    /// <param name="productId">face,body,hand, leg and equipment end result for AR scene</param>

    public void SelectProductModul(int productId)
    {
        SceneManag.Instance.SelectedProduct(productId);
    }
    /// <summary>
    /// pass ids for sub product item for human body
    /// EYEGLASS =0, HELMET = 1, MASK = 2, HARNESS = 3, GLOVES =4,SHOE =5,CONFIND_SPACE = 6, VERTICAL =7,OVER_ROOF = 8, OVER_HEAD = 9
    /// </summary>
    /// <param name="productSubId">eyeglass, helmet, mask, harness,gloves, shoes</param>
    public void SelectModuleSubProduct(int productSubId)
    {
        SceneManag.Instance.SelectedSubProduct(productSubId);
        SceneManag.Instance.QR_Manual_Scene();

    }
}
