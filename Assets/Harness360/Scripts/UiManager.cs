using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.UI.Extensions.Examples;
using System.Collections;
using System;

public class UiManager : MonoBehaviour
{
    public event Action<string, int> triggerTestingEvent;  
    public HarnessManager harnessManager;
    public HarnessDragRotator dragRotator;
    public AudioSource audioPlayer;
    
    public Button  subProductBenifiteButton, activityTestingButtonPrefeb, harnessScalingbutton;


    public RectTransform splashPanel, menuPanel, topPanel ; //harnessChoosePanel leftMenuPanelItem leftProfilePanelItemDetail outAreaPanelLeftMenu outAreaPanelLeftProfile
    public RectTransform  viewPanel, settingsPanel, Benifite_Group_Panel; //arPanel


    public FilterHarnessManagerQuestion _filterHarnessManagerQuestion;
    public Image leftButtonImage, rightButtonImage;

    public Image[] commonUiFadeOut;
    public GameObject[] harnessTypeSizepanel;
    //public Image[] fadeInUI;

    //[SerializeField]
    //RectTransform closeButton;
    //[SerializeField]
    //RectTransform settingButton;
    public Sprite elementSprite;
    
    private bool isSplashClicked = false, toogleScaleActivityGroupButtonpanel, toggleScaleHarnessSizepanel;
    bool canVibrate = true;
    private bool canPlaySound = true, isArON = false;
    
    //[SerializeField] private UnityEngine.UI.Extensions.Examples.ElementScrollView elementsScrollView;
    //[SerializeField] private UnityEngine.UI.Extensions.Examples.ElementScrollView variationsScrollView;
    //[SerializeField] private UnityEngine.UI.Extensions.Examples.ElementScrollView colorsScrollView;

    public ScreenShotHandler screenShotHandler;

    //[SerializeField] Animator scalingArAnime;

    [SerializeField] ActivityTest activityTest;
    public TextMeshProUGUI productItemName;

    private void Awake()
    {
        //arPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(false);
        topPanel.gameObject.SetActive(false);
        //leftMenuPanelItem.gameObject.SetActive(false);
        //leftProfilePanelItemDetail.gameObject.SetActive(false);
        //closeButton.gameObject.SetActive(false);
        //settingButton.gameObject.SetActive(false);
        menuPanel.anchoredPosition = new Vector2(0, -110f);
        topPanel.anchoredPosition = new Vector2(0, 1262f);
        //leftMenuPanelItem.anchoredPosition = new Vector2(-1785f, 0f);
        //leftProfilePanelItemDetail.anchoredPosition = new Vector2(-1785f, 0f);
        //closeButton.anchoredPosition = new Vector2(0, 219);
        //settingButton.anchoredPosition = new Vector2(0, 219);
        
        viewPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        Benifite_Group_Panel.gameObject.SetActive(false);
        Benifite_Group_Panel.DOScale(Vector3.zero, 1);
        //_filterHarnessManagerQuestion.harnessfilterationAction += PlayStartAnim;
        //_filterHarnessManagerQuestion.restartHranessQuestionProcess += _filterHarnessManagerQuestion_restartHranessQuestionProcess;
        
    }

    /// <summary>
    /// id for active the size type panel
    /// </summary>
    /// <param name="id"></param>
    public void UiharnessSizetypePanel(int id)
    {
        foreach (var item in harnessTypeSizepanel)
        {
            item.SetActive(false);
        }

        harnessTypeSizepanel[id].SetActive(true);
    }

    void AcivityFadeOutAllUI()
    {
        foreach (var item in activityTest.subProdocutItem)
        {
            foreach (var itemActivityUiData in item._activityDatas)
            {
                foreach (var itemUI_ImageGroup in itemActivityUiData.imageUiFadeInOut)
                {
                    foreach (var uiImage in itemUI_ImageGroup.GetComponentsInChildren<Image>())
                    {
                        uiImage.DOFade(0, 0);
                    }
                }
                foreach (var itemUI_Text in itemActivityUiData.textUiFadeInOut)
                {
                    itemUI_Text.DOFade(0, 0);
                }
            }
        }
    }

    private void Start()
    {
        leftButtonImage.DOFade(0, 0);
        rightButtonImage.DOFade(0, 0);
        leftButtonImage.GetComponent<Button>().interactable = false;
        rightButtonImage.GetComponent<Button>().interactable = false;
        HideSplash();
        subProductBenifiteButton.onClick.AddListener(ScaleUpDownBenifitePane);
        AcivityFadeOutAllUI();

        if (SceneManag.Instance._currentProductSubCatagory == ProductSubCatagory.HARNESS )
        {
            harnessScalingbutton.gameObject.SetActive(true);
            harnessScalingbutton.transform.GetChild(0).DOScale(0, 0);
            subProductBenifiteButton.gameObject.SetActive(false);
        }
        else
        {
            harnessScalingbutton.gameObject.SetActive(false);
            subProductBenifiteButton.gameObject.SetActive(true);

        }
        harnessScalingbutton.onClick.AddListener(() => { HarnessSizeScaleUpDownPanel(); });
    }

    void HarnessSizeScaleUpDownPanel()
    {
        toggleScaleHarnessSizepanel = !toggleScaleHarnessSizepanel;
        if (toggleScaleHarnessSizepanel)
        {

        harnessScalingbutton.transform.GetChild(0).DOScale(1, 2);
        }
        else
        {
        harnessScalingbutton.transform.GetChild(0).DOScale(0, 2);

        }
    }
    /// <summary>
    /// this will call from harness size button to slect size
    /// </summary>
    /// <param name="id">id is for s,m,l,xl,xxl => 0,1,2,3,4</param>
    public void SizeButtonToSelectharness(int id)
    {
        SceneManag.Instance.SizeSelection(id);
        harnessManager.CurrentHarness.gameObject.GetComponent<HarnessSorting>().DebugOnOffHarness(id);
    }

    void ScaleUpDownBenifitePane()
    {
        toogleScaleActivityGroupButtonpanel = !toogleScaleActivityGroupButtonpanel;
        if (toogleScaleActivityGroupButtonpanel)
        {
            CancelInvoke(nameof(Benifite_Group_PanelActivenes));
            Benifite_Group_Panel.gameObject.SetActive(true);

            var checkChildCount = Benifite_Group_Panel.GetChild(0).childCount;
            //Debug.Log(checkChildCount);
            if (checkChildCount== 0)
            {
                foreach (var item in activityTest.subProdocutItem)
                {
                    if (item._productSubCatagory == SceneManag.Instance._currentProductSubCatagory)
                    {
                        int size = item._activityDatas.Length;
                        for (int i = 0; i < size; i++)
                        {
                            CreatebuttonAndSetvalue(item, i);
                        }
                    }
                }
            }
            Benifite_Group_Panel.transform.DOScale(Vector3.one, 1);
            harnessManager._harnessTouchControls.ResetAllTransFormCompoenet(true);
        }
        else
        {
            Invoke(nameof(Benifite_Group_PanelActivenes),1);

            Benifite_Group_Panel.transform.DOScale(Vector3.zero, 1);

        }
    }

    void CreatebuttonAndSetvalue(SubProdocutItem item, int i)
    {
        var buttonCreated = Instantiate(activityTestingButtonPrefeb, Benifite_Group_Panel.GetChild(0).transform);
        buttonCreated.name = item._activityDatas[i].testingName;
        var componentRef = buttonCreated.GetComponent<SubrProducttestActivity>();
        componentRef.NameAssign = item._activityDatas[i].testingName;
        componentRef.activityId = item._activityDatas[i].testingId;
        buttonCreated.GetComponent<Button>().onClick.AddListener(() => { TaskToExcuteAnimation(componentRef.activityId, componentRef.NameAssign);
           
        });
    }
    int currentButtonIdPressed;
    void TaskToExcuteAnimation(int id, string activityName)
    {
        currentButtonIdPressed = id;
        StartCoroutine(Setup_Activity_ui_HideUnHideUi(0,false,id));
        triggerTestingEvent?.Invoke(activityName, id);
    }


    IEnumerator Setup_Activity_ui_HideUnHideUi(int fadingValue, bool raycastenable, int testIdButton)
    {
        //AcivityFadeOutAllUI();
        yield return new WaitForSeconds(.1f);

        foreach (var item in commonUiFadeOut)
        {
            item.DOFade(fadingValue, 1);
            item.raycastTarget = raycastenable;
            
        }
        productItemName.DOFade(fadingValue, 1);
        if (!raycastenable)
        {
            Benifite_Group_Panel.transform.DOScale(Vector3.zero, 1);
            toogleScaleActivityGroupButtonpanel = !toogleScaleActivityGroupButtonpanel;
        }

        yield return new WaitForSeconds(.1f);
        foreach (var item in activityTest.subProdocutItem)
        {
            if (item._productSubCatagory == SceneManag.Instance._currentProductSubCatagory)
            {
                if (!raycastenable)
                {
                    fadingValue = 1;
                }
                else
                {
                    fadingValue = 0;

                }

                // thing whichi want to show
                foreach (var uiItem in item._activityDatas)
                {
                    if (uiItem.testingId == testIdButton)
                    {
                        foreach (var itemImage in uiItem.imageUiFadeInOut)
                        {
                            foreach (var itemImagesChiled in itemImage.GetComponentsInChildren<Image>())
                            {

                                itemImagesChiled.DOFade(fadingValue, 1);
                            }
                        }
                        foreach (var itemText in uiItem.textUiFadeInOut)
                        {
                            itemText.DOFade(fadingValue, 1);
                        }
                        //uiItem.raycastTarget = raycastenable;
                    }
                }

            }
        }


    }

    /// <summary>
    /// this will call  when animation complete
    /// </summary>
    public void ReturnBackToinitialState()
    {
        StartCoroutine(Setup_Activity_ui_HideUnHideUi(1, true, currentButtonIdPressed));
    }

    void Benifite_Group_PanelActivenes()
    {

        Benifite_Group_Panel.gameObject.SetActive(false);
        harnessManager._harnessTouchControls.ResetAllTransFormCompoenet(false);

    }


    void LeftRightActiveArrow()
    {
        leftButtonImage.GetComponent<Button>().interactable = true;
        rightButtonImage.GetComponent<Button>().interactable = true;
        leftButtonImage.DOFade(1, 1);
        rightButtonImage.DOFade(1, 1);
    }
    private void _filterHarnessManagerQuestion_restartHranessQuestionProcess()
    {
        menuPanel.DOAnchorPos(new Vector2(0f, -110f), 0.5f, true);
        topPanel.DOAnchorPos(new Vector2(0f, 1025f), 0.5f, true);
    }

    void PlayStartAnim()
    {
        menuPanel.gameObject.SetActive(true);
        topPanel.gameObject.SetActive(true);
        //closeButton.gameObject.SetActive(true);
        //settingButton.gameObject.SetActive(true);
        menuPanel.DOAnchorPos(new Vector2(0f, 220f), 0.5f, true);
        topPanel.DOAnchorPos(new Vector2(0f, 0f), 0.5f, true);

        //closeButton.DOAnchorPos(new Vector2(-44f, -72f), 0.5f, true);
        //settingButton.DOAnchorPos(new Vector2(5f, -72f), 0.5f, true);
        LeftRightActiveArrow();
    }

    //public void LeftMenuPanelAnimStart()
    //{

    //    leftMenuPanelItem.gameObject.SetActive(true);
    //    leftMenuPanelItem.DOAnchorPos(new Vector2(0, 0f), 0.5f, true);
    //    outAreaPanelLeftMenu.gameObject.SetActive(true);
    //}

    //public void leftProfilePanelItemDetailUI()
    //{
    //    leftProfilePanelItemDetail.gameObject.SetActive(true);
    //    leftProfilePanelItemDetail.DOAnchorPos(new Vector2(0, 0f), 0.5f, true);
    //    outAreaPanelLeftProfile.gameObject.SetActive(true);

    //}

    //public void LeftProfilePanelAnimStartBack()
    //{
    //    leftProfilePanelItemDetail.DOAnchorPos(new Vector2(-1785f, 0f), 0.5f, true);
    //    outAreaPanelLeftProfile.gameObject.SetActive(false);

    //}

    //public void LeftMenuPanelAnimStartBack()
    //{
    //    leftMenuPanelItem.DOAnchorPos(new Vector2(-1785f, 0f), 0.5f, true);
    //    outAreaPanelLeftMenu.gameObject.SetActive(false);

    //}

    
    public void HideSplash()
    {
        if (!isSplashClicked)
        {
            isSplashClicked = true;
            splashPanel.GetComponent<Image>().DOFade(0, 1f);
            var collectionChildImageSplash = splashPanel.GetComponentsInChildren<Image>();
            var collectionChildTextMeshProSplash = splashPanel.GetComponentsInChildren<TextMeshProUGUI>();
            //scalingArAnime.SetBool("Scaling", true);
            foreach (var item in collectionChildImageSplash)
            {
                item.DOFade(0, 1f);
            }

            foreach (var item in collectionChildTextMeshProSplash)
            {
                item.DOFade(0, 1f);
            }
            //var a = FindObjectOfType
            //splashPanel.GetChild(0).GetChild(0).GetComponent<Image>().DOFade(0, 1f);
         
            menuPanel.gameObject.SetActive(true);
            topPanel.gameObject.SetActive(true);
            dragRotator.gameObject.SetActive(false);

            //Invoke(nameof(DeactiveateSplash), 1.01f);
            Invoke(nameof(PlayStartAnim), .3f);
            //Invoke(nameof(LeftRightActiveArrow), .3f);
            Invoke(nameof(CreateAllHarness), .4f);
        }
    }

    void CreateAllHarness()
    {
            harnessManager.PresentAllSelectedProductItem();

    }
    void DeactiveateSplash()
    {
        splashPanel.gameObject.SetActive(false);
        //_filterHarnessManagerQuestion.RestartQuestioning();

    }

    /// <summary>
    /// FROM MANUAL TO MODE SELECTION QR or Manual
    /// </summary>
    public void BackToModeSceneFromManual()
    {
        SceneManag.Instance.QR_Manual_Scene();
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++====================================
    public void OnVibrate()
    {
        if (canVibrate)
        {
            //Handheld.Vibrate();
        }

        if (canPlaySound)
        {
            audioPlayer.Play();
        }
    }

    public void OnChooseHarness(int harnessType)
    {
        // harnessManager.ChooseHarness(harnessType);
        // harnessChoosePanel.gameObject.SetActive(false);
        // menuPanel.gameObject.SetActive(true);
        // dragRotator.gameObject.SetActive(true);
        // dragRotator.SetDragItem(harnessManager.CurrentHarness.gameObject);
    }
    

    /// <summary>
    /// harness button in main menu
    /// </summary>
    public void OnSelectHarnessButton()
    {
        //elementsScrollView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
        //elementsScrollView.gameObject.SetActive(true);
        //variationsScrollView.gameObject.SetActive(false);
        //colorsScrollView.gameObject.SetActive(false);

        //RectTransform elementRect = elementsScrollView.GetComponent<RectTransform>();
        
        //elementRect.anchoredPosition = Vector2.zero;
        //elementRect.DOAnchorPos(new Vector2(0f, 160f), 0.5f, true);
        //elementRect.localScale = new Vector3(1,0,0);
        //elementRect.DOScale(Vector3.one, 0.5f);
        
        // int elementCount = harnessManager.CurrentHarness.GetAvailableElementCount();
        int elementCount = harnessManager.GetHarnessCount();
        
        // var cellData = Enumerable.Range(0, elementCount)
        //     .Select(i => new UnityEngine.UI.Extensions.Examples.ElementCellDto {elementType = harnessManager.CurrentHarness.GetElementType(i),
        //                                                                             icon = elementSprite,
        //                                                                             Message = harnessManager.CurrentHarness.GetElementName(i), 
        //                                                                             buttonType = ButtonType.element})
        //     .ToList();
        
        var cellData = Enumerable.Range(0, elementCount)
            .Select(i => new UnityEngine.UI.Extensions.Examples.ElementCellDto {elementType = Element.ElementType.None,
                icon = elementSprite,
                Message = harnessManager.GetHarnessName(i), 
                buttonType = ButtonType.harnessVariation})
            .ToList();

        
        //elementsScrollView.UpdateData(cellData);
    }
    
    public void OnSelectColor() // main menu button
    {
        if(harnessManager.CurrentHarness == null)
            return;
        
        //elementsScrollView.gameObject.SetActive(true);
        //variationsScrollView.gameObject.SetActive(false);
        //colorsScrollView.gameObject.SetActive(false);
        
        
        int elementCount = harnessManager.CurrentHarness.GetAvailableElementCount();
        
        var cellData = Enumerable.Range(0, elementCount)
            .Select(i => new UnityEngine.UI.Extensions.Examples.ElementCellDto {elementType = harnessManager.CurrentHarness.GetElementType(i),
                icon = elementSprite,
                Message = harnessManager.CurrentHarness.GetElementName(i), 
                buttonType = ButtonType.colorMenuButton})
            .ToList();

        //elementsScrollView.UpdateData(cellData);
    }

    // public void OnHarnessButtonPressed()
    // {
    //     variationsScrollView.gameObject.SetActive(true);
    //     colorsScrollView.gameObject.SetActive(false);
    //
    //     int variationCount = harnessManager.GetHarnessCount();
    //
    //     var cellData = Enumerable.Range(0, variationCount)
    //         .Select(i => new UnityEngine.UI.Extensions.Examples.ElementCellDto {elementType = elementType,
    //             icon = harnessManager.CurrentHarness.GetElementVariationSprite(elementType,i), 
    //             buttonType = ButtonType.harnessVariation})
    //         .ToList();
    //     
    //     variationsScrollView.UpdateData(cellData);
    // }

    /// <summary>
    /// choose element, whose available colors need to show 
    /// </summary>
    /// <param name="elementType"></param>
    public void OnColorButtonPressed(Element.ElementType elementType) // 
    {
        //variationsScrollView.gameObject.SetActive(false);
        //colorsScrollView.gameObject.SetActive(true);

        //int variationCount = harnessManager.GetColorCountForElement(elementType);

        //var cellData = Enumerable.Range(0, variationCount)
        //    .Select(i => new UnityEngine.UI.Extensions.Examples.ElementCellDto {elementType = elementType,
        //        icon = harnessManager.GetSpriteForColor(elementType, i), 
        //        buttonType = ButtonType.colorVariation})
        //    .ToList();
        
        //colorsScrollView.UpdateData(cellData);
        
        OnVibrate();
    }
    
 

    public void OnHarnessPressed(int index)
    {
        //harnessManager.ChooseHarness(index);

        // // Hide panel with animation
        // RectTransform elementRect = elementsScrollView.GetComponent<RectTransform>();
        // elementRect.DOAnchorPos(new Vector2(0f, 0), 0.3f, true);
        // elementRect.DOScale(new Vector3(1,0,0), 0.3f);
        
        dragRotator.canRotate = true;
        dragRotator.gameObject.SetActive(true);
        dragRotator.SetDragItem(harnessManager.CurrentHarness.gameObject);
        OnVibrate();
    }
    
    
    public void OnMeshVariationPressed(Element.ElementType elementType, int index)
    {
        harnessManager.CurrentHarness.SetElementVariation(elementType, index);
    }
    
    public void OnColorVariationPressed(Element.ElementType elementType, int index)
    {
        //harnessManager.SetColorVariation(elementType, index);
        OnVibrate();
    }
    

    public void OnView()
    {
        viewPanel.anchoredPosition = new Vector2(0f, -2600f);
        viewPanel.gameObject.SetActive(true);
        viewPanel.DOAnchorPos(new Vector2(0f, 0), 0.5f, true);
    }

    public void HideView()
    {
        viewPanel.DOAnchorPos(new Vector2(0f, -2600f), 0.2f, true);
    }

    public void OnAR()
    {
        isArON = true;
        menuPanel.gameObject.SetActive(false);
        topPanel.gameObject.SetActive(false);
        //closeButton.gameObject.SetActive(false);
        //harnessChoosePanel.gameObject.SetActive(false);
        
        //elementsScrollView.gameObject.SetActive(false);
        //variationsScrollView.gameObject.SetActive(false);
        //colorsScrollView.gameObject.SetActive(false);
        
        //arPanel.gameObject.SetActive(true);
    }

    public void TakeScreenShot()
    {
        //HideUi();
        TakingScreenShot();
        //Invoke(nameof(UnHide), 0.025f);
    }

    void TakingScreenShot()
    {
        screenShotHandler.TakeScreenShotwithDelay(0f);
    }
    //void HideUi()
    //{
    //    arPanel.gameObject.SetActive(false);

    //}

    //void UnHide()
    //{
    //    arPanel.gameObject.SetActive(true);

    //}

    public void CloseAR()
    {
        isArON = false;
        menuPanel.gameObject.SetActive(true);
        topPanel.gameObject.SetActive(true);
        //closeButton.gameObject.SetActive(true);
        //arPanel.gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        if (!isArON)
        {

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        else
        {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);

        }
    }
    public void OnSettings()
    {
        settingsPanel.anchoredPosition = new Vector2(0f, -2600f);
        settingsPanel.gameObject.SetActive(true);
        settingsPanel.DOAnchorPos(new Vector2(0f, 0), 0.5f, true);
    }

    public void HideSettings()
    {
        settingsPanel.DOAnchorPos(new Vector2(0f, -2600f), 0.2f, true);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void ChooseVibration(Toggle toggle)
    {
        if (toggle.isOn)
        {
            canVibrate = true;
        }
        else
        {
            canVibrate = false;
        }
    }
    
    public void ChooseSound(Toggle toggle)
    {
        if (toggle.isOn)
        {
            canPlaySound = true;
        }
        else
        {
            canPlaySound = false;
        }
    }
    
}

[System.Serializable]
public class ActivityTest
{
    public SubProdocutItem[] subProdocutItem;
}

[System.Serializable]
public class SubProdocutItem
{
    public string subProductName;
    public ProductSubCatagory _productSubCatagory;
    public ActivityData[] _activityDatas;
}

[System.Serializable]
public struct ActivityData
{
    public string testingName;
    public int testingId;
    public GameObject[] imageUiFadeInOut;
    public TextMeshProUGUI[] textUiFadeInOut;
}

