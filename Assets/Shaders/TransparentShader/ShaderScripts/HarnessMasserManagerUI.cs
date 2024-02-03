using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation.Samples;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class HarnessMasserManagerUI : MonoBehaviour
{
    public TextMeshProUGUI heightresultText, torsoResultText,thighResultTxt, chestResultTxt, pelvicResultTxt, userWeight, harnessSizeText, harnessSizeTextCodeName, harnessWorkStationDetail;  //measureMeterTitle
    public TextMeshProUGUI[] iconWorkStationName;
    //public TextMeshProUGUI[] measureNumber;
    public Slider weightUser;
    public Slider DebugSlider;

    public Transform headTransform, bottomTransform,hipTrans, neckTrans, lefthandTransform;

    int humanBodyWeight, round_off_weight, frameCount = 0;

    float height_dis, chestCircumference_In_Inch, pelvic_Circumference_In_Inch,torso_dis, thighGirthInches;
    public float thresholdDistance = 10.0f, distanceVariation, heightRoundOffNumber;
    float  torsoRoundOffNumber, chestCircumference_In_InchRoundOff, pelvic_Circumference_In_InchRoundOff, thighGirthInches_InchRoundOff;
    float aspectRatio, refAspectRatio, screenHeight, screenWidth, scaleFactor, height_Top_xAxisValue, height_TOP_yAsixvalue, resolutionYAxis, resolutionXAxis,lefthandX,lefthandY;

    public RectTransform height_torso_measurBarAnim, horizontal_leftsideMeasurbar_Icon, heightTOP_Pointer, heightBOTTOM_Pointer, torsoNECKPointer, torsoHIPKPointer;//horizintalMeasurBarxAxisSxaleUpAnime
    public RectTransform moveSelectionGroupHolder, bottomPanelHarnessWorkDetailScaleUpDown, weightUserParent;
    RectTransform rectTransform;

    public Image[] horizontalIcons, workStationIcon;
   
    bool hightcalulated, crossOutsideBoundry, crossInsideboundry, firstFramePassed, heightTextReUpdate, toggleMoveGrouprectAnchor;
    //public Vector3 _offsetHead, _offseHip;

    Vector2 previousPosition;

    public Button harnessSizeSelectorParent, heightMeasurementButton;

    public SelfRegester sR;

    HumanBodyTracker _humanBodyTracker;
    /// <summary>
    /// ProductCodeData scriptable object
    /// </summary>
    public HarnessCaryForwardData _harnessCaryForwardData;
    /// <summary>
    /// ProductCodeData scriptable object
    /// </summary>
    public ProductCodeData productCodeData;
    //public int count;

    public Canvas canvas;
    public CanvasScaler canvasScaler;

    // ui follow human body to check inbound or out bound of frame
    public GameObject debugUiPoss, leftHandUi;

    public GameObject guideUiAnimation, qrBarCode ;

    public ARController _aRController;


    public BoneController boneController;

    Camera mainCamera;
    [SerializeField]
     HarnessSorting _harnessSorting;

    public AudioSource audioSource;
    public AudioClip harnessSizeChangeNotification;


    public HarnessSorting HarnessSorting
    {
        set { _harnessSorting = value; WorkStationDetail_Ui_Update(); }
    }

    bool toggleScaleUpDown;
    public bool restartingDone, takingSnap;
    public string[] currentHarnessSizeCodename ;
    void WorkStationDetail_Ui_Update()
    {
        if (_harnessSorting != null)
        {
            foreach (var item in productCodeData.wrokStationDetail.harnessWorkStationCatagory)
            {
                if (item.HarnessNameCode == _harnessSorting.itemProductCodeName)
                {
                    currentHarnessSizeCodename = item.harnessSizeCodename.sizeCodeName;

                    harnessWorkStationDetail.text = item.HarnessWrokDes;
                    for (int i = 0; i < 5; i++)
                    {
                        iconWorkStationName[i].text = SeperateUnderscoreEnume(item.workStationCatagories[i].ToString());
                        workStationIcon[i].sprite = productCodeData.wrokStationDetail.icons[((int)item.workStationCatagories[i])];
                    }
                }
            }
        }
    }

    public void ProductCodeUrl()
    {
        foreach (var item in productCodeData.wrokStationDetail.harnessWorkStationCatagory)
        {
            if (_harnessSorting != null)
            {
                if (item.HarnessNameCode == _harnessSorting.itemProductCodeName)
                {
                    currentHarnessSizeCodename = item.harnessSizeCodename.sizeCodeName;
                    foreach (var productCode in item.productUrls)
                    {
                        //Debug.Log(harnessSizeTextCodeName.text);

                        if (productCode.codeNumber == harnessSizeTextCodeName.text)
                        {
                            productCodeData.wrokStationDetail.currentUrl = productCode.productUrl;
                            break;

                        }
                    }
                }
            }
        }
    }

    string SeperateUnderscoreEnume(string enumName)
    {
        var removeUnderScore = enumName.Replace("_", " ");
        return removeUnderScore;
    }

    public void QrBarCodeGuideAnimeScaleUpDown()
    {
        ToggleScaleUpDownQrCode();
        Invoke(nameof(ToggleScaleUpDownQrCode),8);
    }



    void ToggleScaleUpDownQrCode()
    {
        toggleScaleUpDown = !toggleScaleUpDown;
        if (toggleScaleUpDown)
        {
            bottomPanelHarnessWorkDetailScaleUpDown.DOScale(1, 1);
            qrBarCode.transform.DOScale(1, 1);
            weightUserParent.DOScale(0, 1);
            guideUiAnimation.transform.DOScale(0, 1);
        }
        else
        {
            bottomPanelHarnessWorkDetailScaleUpDown.DOScale(0, 1);
            guideUiAnimation.transform.DOScale(1, 1);
            weightUserParent.DOScale(1, 1);
            qrBarCode.transform.DOScale(0, 1);

        }
    }

    private void Start()
    {
        //currentHarnessSizeCodename = new string[5];
        bottomPanelHarnessWorkDetailScaleUpDown.DOScale(0, 0);
        //harnessSizeSelectorParent.GetComponent<Image>().DOFade(0, 0);
        moveSelectionGroupHolder.DOAnchorPosX(0, 0);
        _humanBodyTracker = FindObjectOfType<HumanBodyTracker>();
        height_torso_measurBarAnim.anchoredPosition = new Vector2(1031, 0);
        horizontal_leftsideMeasurbar_Icon.anchoredPosition = new Vector2(-1050, 0);

        //horizintalMeasurBarxAxisSxaleUpAnime.localScale = new Vector3(0, 1 , 1);

        screenHeight = Screen.height;
        screenWidth = Screen.width;

        float refWidth = 2048f; // reference resolution - width - set in Canvas Scaler 
        float refHeight = 1536f;

        aspectRatio = (float)screenWidth / (float)screenHeight;
        refAspectRatio = (float)refWidth / (float)refHeight;
        scaleFactor = refAspectRatio / aspectRatio;
        mainCamera = Camera.main;


        if (canvasScaler.matchWidthOrHeight == 1) canvasScaler.referenceResolution = new Vector2(canvasScaler.referenceResolution.y * aspectRatio, canvasScaler.referenceResolution.y);
        else if (canvasScaler.matchWidthOrHeight == 0) canvasScaler.referenceResolution = new Vector2(canvasScaler.referenceResolution.x, canvasScaler.referenceResolution.x / aspectRatio);
        else if (canvasScaler.matchWidthOrHeight == 0.5f)
        {
            resolutionXAxis = Mathf.Lerp(canvasScaler.referenceResolution.y * aspectRatio, canvasScaler.referenceResolution.x, 0.5f);
            resolutionYAxis = Mathf.Lerp(canvasScaler.referenceResolution.y, canvasScaler.referenceResolution.x / aspectRatio, 0.5f);
            canvasScaler.referenceResolution = new Vector2(resolutionXAxis, resolutionYAxis);


            debugUiPoss.GetComponent<RectTransform>().anchoredPosition = new Vector2(resolutionXAxis/2, resolutionYAxis/2);
            leftHandUi.GetComponent<RectTransform>().anchoredPosition = new Vector2(resolutionXAxis/2, resolutionYAxis/2);
            previousPosition = debugUiPoss.GetComponent<RectTransform>().anchoredPosition;
        }

        harnessSizeSelectorParent.onClick.AddListener(() => { MoveHarnessSizeGroupPanel(); });
        heightMeasurementButton.onClick.AddListener(() => { OnOffHeightMeasue(); });
        sR.trigger += SR_trigger;
    }

    bool onoffToggleHeightMeasurement;
    void OnOffHeightMeasue()
    {
        onoffToggleHeightMeasurement = !onoffToggleHeightMeasurement;
        if (onoffToggleHeightMeasurement)
        {
            weightUser.transform.parent.gameObject.SetActive(true);
            height_torso_measurBarAnim.gameObject.SetActive(true);
        }
        else
        {
            weightUser.transform.parent.gameObject.SetActive(false);
            height_torso_measurBarAnim.gameObject.SetActive(false);


        }
    }

    private void SR_trigger()
    {
        restartingDone = false;
        lefthandTransform = GameObject.Find("LeftHand").GetComponent<Transform>();
        int id = (int)SceneManag.Instance.harnessSize;
        switch (_harnessSorting.harnessSizetype)
        {
            case HarnessSizetype.twoTypeSize:
                if (id == 1)
                {
                    harnessSizeText.text = "Size : S-L";

                }
                if (id == 3)
                {
                    harnessSizeText.text = "Size : L-XXL";

                }
                break;
            case HarnessSizetype.threeTypeSize:
                if (id == 0)
                {
                    harnessSizeText.text = "Size : S-M";

                }
                if (id == 2)
                {
                    harnessSizeText.text = "Size : M-L";

                }
                if (id == 3)
                {
                    harnessSizeText.text = "Size : L-XXL";

                }
                break;
            case HarnessSizetype.universal:
                //harnessSizeText.text = "Size : " + ((HarnessSize)id).ToString();
                harnessSizeText.text = "Size : Adjusting";
                break;
        }
    }

    //public void AdjustHarnessSize(int id)
    //{
    //        //Debug.Log(id);
    //    if (_harnessSorting != null)
    //    {
    //        _harnessSorting.DebugOnOffHarness(id);
    //        MoveHarnessSizeGroupPanel();
    //        harnessSizeText.text = "Size : " + ((HarnessSize)id).ToString();
    //        harnessSizeText2.text = harnessSizeText.text;
    //    }
    //}

    /// <summary>
    /// wave hand to change harness size
    /// </summary>
    /// <param name="id"></param>

    public void AdjustHarnessSizeHandInteraction(int id)
    {
        //Debug.Log(id);
        audioSource.PlayOneShot(harnessSizeChangeNotification);
        if (_harnessSorting != null)
        {
            switch (_harnessSorting.harnessSizetype)
            {
                case HarnessSizetype.twoTypeSize:
                    if (id == 1 || id == 0)
                    {
                        harnessSizeText.text = "Size : S-L";

                    }
                    if (id == 3)
                    {
                        harnessSizeText.text = "Size : L-XXL";

                    }
                    break;
                case HarnessSizetype.threeTypeSize:
                    if (id == 1 || id == 0)
                    {
                        harnessSizeText.text = "Size : S-M";

                    }
                    if (id == 2)
                    {
                        harnessSizeText.text = "Size : M-L";

                    }
                    if (id == 3)
                    {
                        harnessSizeText.text = "Size : L-XXL";

                    }
                    break;
                case HarnessSizetype.universal:
                    //harnessSizeText.text = "Size : " + ((HarnessSize)id).ToString();
                    harnessSizeText.text = "Size : Adjusting";
                    break;
            }

            //harnessSizeText.text = "Size : " + ((HarnessSize)id).ToString();
            harnessSizeTextCodeName.text = currentHarnessSizeCodename[id];
            //harnessSizeTextCodeName.text = productCodeData.wrokStationDetail.harnessWorkStationCatagory
            _harnessSorting.DebugOnOffHarness(id);
            //MoveHarnessSizeGroupPanel();
        }
    }

    void MoveHarnessSizeGroupPanel()
    {
        toggleMoveGrouprectAnchor = !toggleMoveGrouprectAnchor;
        if (toggleMoveGrouprectAnchor)
        {
        moveSelectionGroupHolder.DOAnchorPosX(900, 2);
            moveSelectionGroupHolder.GetComponent<Image>().raycastTarget = !toggleMoveGrouprectAnchor;
        }
        else
        {
            moveSelectionGroupHolder.GetComponent<Image>().raycastTarget = !toggleMoveGrouprectAnchor;
        moveSelectionGroupHolder.DOAnchorPosX(0, 2);

        }
    }


    void Update()
    {
        //CheckAssignTransform();
        if (sR.HarnesPrefebinsticated == null)
        {
            hightcalulated = false;

            return;
        }


        MeasureHeight();
        TorsoMeasure();
        if (height_dis > 1 && !hightcalulated)
        {
            hightcalulated = true;
            Invoke(nameof(MeasurMeterAnime), 2f);

            MeasureChest_Pelvic_thigh_from_Equation(false);
            if (SceneManag.Instance.manualHarnesSelection)
            {
            //harnessSizeText.text = "Size : " + SceneManag.Instance.harnessSize.ToString();
            harnessSizeTextCodeName.text = currentHarnessSizeCodename[(int)SceneManag.Instance.harnessSize];
                //SceneManag.Instance.manualHarnesSelection = false;
            }
        }


        Vector3 headworldPosition = headTransform.transform.position;
        Vector3 lefthandWorldposition = lefthandTransform.transform.position;
        Vector3 bottomworldPosition = bottomTransform.transform.position;
        Vector3 neckWorldPosition = neckTrans.transform.position;
        Vector3 hipWorldPosition = hipTrans.transform.position;




        //takes one of your reference values and accounts for the player's selected aspect ratio. Only supports scaled by totally width or totally height

        Vector2 headWorldToViewportPos = Camera.main.WorldToViewportPoint(headworldPosition);
        Vector2 lefthandWorldToViewportPos = Camera.main.WorldToViewportPoint(lefthandWorldposition);
        Vector2 bottomWorldToViewportPos = Camera.main.WorldToViewportPoint(bottomworldPosition);
        Vector2 neckWorldToViewportPos = Camera.main.WorldToViewportPoint(neckWorldPosition);
        Vector2 hipWorldToViewportPos = Camera.main.WorldToViewportPoint(hipWorldPosition);


        //headWorldToScreenPoint.y *= scaleFactor;

        //float height_TOP_yAsixvalue = headWorldToScreenPoint.y;
        height_TOP_yAsixvalue = canvasScaler.referenceResolution.y * headWorldToViewportPos.y;
        lefthandX = canvasScaler.referenceResolution.x * lefthandWorldToViewportPos.x;
        lefthandY = canvasScaler.referenceResolution.y * lefthandWorldToViewportPos.y;
        height_Top_xAxisValue = canvasScaler.referenceResolution.x * headWorldToViewportPos.x;


        //Debug.Log("pass");

        float height_BOTTOM_yAsixvalue = canvasScaler.referenceResolution.y *  bottomWorldToViewportPos.y  ;
        float height_NECK_yAsixvalue = canvasScaler.referenceResolution.y * neckWorldToViewportPos.y ;
        float height_HIP_yAsixvalue = canvasScaler.referenceResolution.y *  hipWorldToViewportPos.y ;

        var xHeightCommonRectPos = heightTOP_Pointer.anchoredPosition.x;
        var xTorsoCommonRectPos = torsoNECKPointer.anchoredPosition.x;

        heightTOP_Pointer.anchoredPosition = new Vector2(xHeightCommonRectPos, height_TOP_yAsixvalue);
        heightBOTTOM_Pointer.anchoredPosition = new Vector2(xHeightCommonRectPos, height_BOTTOM_yAsixvalue);
        torsoNECKPointer.anchoredPosition = new Vector2(xTorsoCommonRectPos, height_NECK_yAsixvalue);
        torsoHIPKPointer.anchoredPosition = new Vector2(xTorsoCommonRectPos, height_HIP_yAsixvalue);


        debugUiPoss.GetComponent<RectTransform>().anchoredPosition = new Vector2(height_Top_xAxisValue, resolutionYAxis/2);

        if (lefthandTransform != null)
        {
        leftHandUi.GetComponent<RectTransform>().anchoredPosition = new Vector2(lefthandX, lefthandY);

        }

    }


    private void LateUpdate()
    {
        //var height_foot = height_dis * 3.281f;
        var height_foot = height_dis; //* 3.281f
        //var torso_foot = torso_dis * 3.281f;
        var torso_foot = torso_dis; //* 3.281f

        if (sR.HarnesPrefebinsticated == null)
        {
            firstFramePassed = false;
            heightTextReUpdate = false;
            CancelInvoke(nameof(CheckAfterSomeTimeUiPos));
        //Debug.Log("calls");
            return;
        }

        if (!heightTextReUpdate && height_dis > 1)
        {
            torsoResultText.text = "Torso\n" + torso_foot.ToString("F2") + " m";
            heightresultText.text = "Heigh\nt " + height_foot.ToString("F2") + " m";
            heightTextReUpdate = true;
            InvokeRepeating(nameof(CheckAfterSomeTimeUiPos), 1, 3);
        }

        if (boneController == null)
        {
            boneController = FindObjectOfType<BoneController>();
        }
    }

    /// <summary>
    /// this will call every frame when slider slide and change value
    /// </summary>
    public void DebugSpineSliderRtotationZAxis()
    {
        if (boneController != null)
        {
        boneController.ZRot = DebugSlider.value;

        }
    }


    void CheckAfterSomeTimeUiPos()
    {
        var debugRefAnc = debugUiPoss.GetComponent<RectTransform>().anchoredPosition;
        if (debugRefAnc.x > 0 || debugRefAnc.x < resolutionXAxis)
        {
            crossInsideboundry = true;
        }
        if (crossInsideboundry)
        {


            if (!firstFramePassed)
            {
                firstFramePassed = true;
                return;
            }


 

            if (debugRefAnc.x < 50 || debugRefAnc.x > screenWidth - 50 && !restartingDone)
            {
                restartingDone = true;
                Debug.Log("Reseting outOf bound");
                debugUiPoss.GetComponent<RectTransform>().anchoredPosition = new Vector2(resolutionXAxis / 2, resolutionYAxis / 2);
                _aRController.ReStartArSession();
            
            }
        }
    }


    public static Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
        screenPoint.z = 0;

        Vector2 screenPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
        {
            return screenPos;
        }

        return screenPoint;
    }

    void MeasurMeterAnime()
    {
        HeightTorsoMeasurBar();
        PelivMeasurBar();
        ChestMeasureBar();
        ThighMeasureBar();
    }
    // CALLED EACH FRAME RATE
    private void MeasureHeight()
    {
        if (headTransform != null && bottomTransform != null)
        {
            height_dis = Vector3.Distance(headTransform.position, bottomTransform.position);
            heightRoundOffNumber = RoundOffinPontOneDigit(height_dis); //+ 0.1f
        }

        //var height_foot = height_dis * 3.281f;
        var height_foot = height_dis ;
        heightresultText.text = "Height\n " + height_foot.ToString("F2") + " m";
    }

    void TorsoMeasure()
    {
        if (hipTrans != null && neckTrans != null)
        {
            torso_dis = Vector3.Distance(hipTrans.position, neckTrans.position);

        }
    }

    float RoundOffinPontOneDigit(float value)
    {
        int decimalPrecision = 2; // Set the number of decimal places you want to display
        return Mathf.Round(value * Mathf.Pow(10, decimalPrecision)) / Mathf.Pow(10, decimalPrecision);

    }

    void MeasureChest_Pelvic_thigh_from_Equation(bool weightSliderPassingvalue)
    {
        if (!weightSliderPassingvalue)
        {
            round_off_weight = Mathf.RoundToInt(_harnessCaryForwardData.userWeight);

        }
        else
        {
            round_off_weight = Mathf.RoundToInt(weightUser.value);

        }
        //int round_off_weight = Mathf.RoundToInt(_harnessCaryForwardData.userWeight);
        humanBodyWeight = round_off_weight;
        float heightCm = height_dis * 100;
        float chestCircumference = 0.531f * heightCm + 0.021f * humanBodyWeight - 11.645f; //equation is the "Chest Girth Equation" developed by Dr. Andrew Jackson.
        //chestCircumference_In_Inch = chestCircumference / 2.54f; // 2.54 divide cm to convert into inch
        chestCircumference_In_Inch = chestCircumference / 100f; // 2.54 divide cm to convert into inch
        //y.text ="CHEST :" + "\n"+ chestCircumference_In_Inch.ToString("F2") + " Inch";
        //chestCircumference_In_InchRoundOff = RoundOffinPontOneDigit(chestCircumference_In_Inch);

        float CalculatePelvicCircumference = 0.376f * heightCm + 0.114f * humanBodyWeight - 22.239f; // "Pelvic Girth Equation" developed by Dr. Andrew Jackson.

        //pelvic_Circumference_In_Inch = CalculatePelvicCircumference / 2.54f; // 2.54 divide cm to convert into inch
        pelvic_Circumference_In_Inch = CalculatePelvicCircumference / 100f; // 2.54 divide cm to convert into inch
        //z.text = "PELVIC :" + "\n" + pelvic_Circumference_In_Inch.ToString("F2") + " Inch";
        //pelvic_Circumference_In_InchRoundOff = RoundOffinPontOneDigit(pelvic_Circumference_In_Inch);

        float thighGirthCm = (0.24f * heightCm) + (0.18f * humanBodyWeight) - 8.44f;
        //thighGirthInches = (thighGirthCm / 2.54f) * 0.8f;
        thighGirthInches = thighGirthCm / 100f;
        //thighGirthInches_InchRoundOff = RoundOffinPontOneDigit(thighGirthInches);
    }


    

    public void HeightTorsoMeasurBar() => MeasureBar(0);

    public void PelivMeasurBar() => MeasureBar(1);

    public void ChestMeasureBar() => MeasureBar(2);

    public void ThighMeasureBar() => MeasureBar(3);

    void MeasureBar(int id)
    {
        
        switch (id)
        {
            case 0:
                HeightTorsoPointerPos();
                //var height_foot = height_dis * 3.281f;
                var height_foot = height_dis; //* 3.281f
                heightresultText.text = "Height\n " + height_foot.ToString("F2") + " m";
                //var torso_foot = torso_dis * 3.281f;
                var torso_foot = torso_dis; //* 3.281f
                torsoResultText.text = "Torso\n " + torso_foot.ToString("F2") + " m";
                break;
            case 1:
                chestResultTxt.text ="Chest " + chestCircumference_In_Inch.ToString("F2") + " m.";
                break;
            case 2:

                pelvicResultTxt.text = "Pelvic  " + pelvic_Circumference_In_Inch.ToString("F2") + " m.";
                break;
            case 3:
                thighResultTxt.text = "Thigh  " + thighGirthInches.ToString("F2") + " m.";
                StartCoroutine(BottomHorizontalBarAnime());

                break;
            default:
                break;
        }

    }

    IEnumerator BottomHorizontalBarAnime()
    {
        horizontal_leftsideMeasurbar_Icon.DOAnchorPos(new Vector2(0, 0), 1);

        yield return new WaitForSeconds(1);

        foreach (var item in horizontalIcons)
        {
        item.GetComponent<Image>().DOFade(0, 1);
        }
        yield return new WaitForSeconds(1.2f);
        foreach (var item in horizontalIcons)
        {
            item.GetComponent<RectTransform>().GetChild(0).GetComponentInChildren<RectTransform>().DOScale(Vector3.one, 1);
        }
        var weightKG_To_Pound = _harnessCaryForwardData.userWeight * 2.205f; // convert kg to pound
        int weight = Mathf.RoundToInt(weightKG_To_Pound);

        //userWeight.text = weight.ToString() + "lb";
        //Debug.Log("calls");
        userWeight.text = _harnessCaryForwardData.userWeight.ToString() + "Kg";

    }


    int i = 0;

    void HeightTorsoPointerPos()
    {


        height_torso_measurBarAnim.DOAnchorPos(new Vector2(30, 0), 1, false);
     
    }

    /// <summary>
    /// filter from slider 
    /// </summary>
    /// <param name="id"> id is 0,1,2,3,4</param>
    void SizeMeasureHarnesstype(int id)
    {
        //harnessSorting = FindObjectOfType<HarnessSorting>();
        if (_harnessSorting == null)
        {
            return;
        }
        switch (_harnessSorting.harnessSizetype)
        {
            case HarnessSizetype.twoTypeSize:
                switch (id)
                {
                    case 1:
                    case 0:
                        //harnessSizeText.text = "Size : S-L";
                        SliderAdjustHarnessSize(1);

                        break;
                    case 3:
                    case 2:
                    case 4:
                        //harnessSizeText.text = "Size : L-XXL";
                        SliderAdjustHarnessSize(3);
                        //Debug.Log("text");
                        break;
                }
                break;
            case HarnessSizetype.threeTypeSize:
                switch (id)
                {
                    case 0:
                    case 1:
                        //harnessSizeText.text = "Size : S-M";
                        SliderAdjustHarnessSize(0);
                        break;
                    case 2:
                        //harnessSizeText.text = "Size : M-L";
                        SliderAdjustHarnessSize(2);
                        break;
                    case 3:
                    case 4:
                        //harnessSizeText.text = "Size : L-XXL";
                        SliderAdjustHarnessSize(3);
                        break;
                }
                break;
            case HarnessSizetype.universal:
                //harnessSizeText.text = "Size : " + ((HarnessSize)id).ToString();
                SliderAdjustHarnessSize(id);
                break;

        }
        
    }

    /// <summary>
    /// this will call from slider 
    /// </summary>
    public void UpdateUserWeightzUISlider()
    {
        var weightKG_To_Pound = weightUser.value * 2.205f; // convert kg to pound
        int weight = Mathf.RoundToInt(weightKG_To_Pound);

        //userWeight.text = weight.ToString() + "lb";
        userWeight.text = weightUser.value.ToString() + "Kg";
        MeasureChest_Pelvic_thigh_from_Equation(true);


        chestResultTxt.text ="Chest " + chestCircumference_In_Inch.ToString("F2") + " m.";


        pelvicResultTxt.text = "Pelvic  " + pelvic_Circumference_In_Inch.ToString("F2") + " m.";

        thighResultTxt.text = "Thigh  " + thighGirthInches.ToString("F2") + "m.";

        WeightheightRelation(weightUser.value);
    }

    /// <summary>
    /// When button press to select harness size type
    /// </summary>
    /// <param name="id">id is use to select size 0,1,2,3,4 => s,m,l,xl,2xl</param>
    public void SliderAdjustHarnessSize(int id)
    {
        if (_harnessSorting != null)
        {
            switch (_harnessSorting.harnessSizetype)
            {
                case HarnessSizetype.twoTypeSize:
                    if (id == 1)
                    {
                        harnessSizeText.text = "Size : S-L";

                    }
                    if (id == 3)
                    {
                        harnessSizeText.text = "Size : L-XXL";

                    }
                    break;
                case HarnessSizetype.threeTypeSize:
                    if (id == 0)
                    {
                        harnessSizeText.text = "Size : S-M";

                    }
                    if (id == 2)
                    {
                        harnessSizeText.text = "Size : M-L";

                    }
                    if (id == 3)
                    {
                        harnessSizeText.text = "Size : L-XXL";

                    }
                    break;
                case HarnessSizetype.universal:
                    harnessSizeText.text = "Size : Adjusting"; //((HarnessSize)id).ToString()

                    break;
            }
            SceneManag.Instance.harnessSize = (HarnessSize)id;
            //harnessSizeText.text = "Size : " + ((HarnessSize)id).ToString();
            harnessSizeTextCodeName.text = currentHarnessSizeCodename[id];
            //Debug.Log(id);
            //MoveHarnessSizeGroupPanel();
            _harnessSorting.DebugOnOffHarness(id);
        }
    }

    public void ChangeHarnessSizeSound()
    {

            audioSource.PlayOneShot(harnessSizeChangeNotification);
    }

    public void QrOrDeepLinkingApprochToSelectHarnessSize()
    {
        WeightheightRelation(_harnessCaryForwardData.userWeight);
        weightUser.value = _harnessCaryForwardData.userWeight;
    }
    //public float weihtKgDebug, endResultWeight;
    //[ContextMenu("Do Something")]
    //void TestingWeight()
    //{
    //    WeightheightRelation(weihtKgDebug);
    //}

    /// <summary>
    /// Provide harnes on the basses of weight and height
    /// </summary>
    /// <param name="weight"> slider will provide value </param>
    void WeightheightRelation(float weight)
    {
        weight = Mathf.Clamp(weight, 54.1f, 200);
        //endResultWeight = weight;
        if (54 < weight && weight < 63)
        {
            if (1.47f < heightRoundOffNumber && heightRoundOffNumber < 1.87f)
            {
                // small
                SizeMeasureHarnesstype(0);
                //SliderAdjustHarnessSize(0);
            }
            else
            {
                SizeMeasureHarnesstype(1);
                //SliderAdjustHarnessSize(1);
                //medium
            }
        }

        if (63 < weight && weight < 108)
        {
            if (1.52f < heightRoundOffNumber && heightRoundOffNumber < 1.87f)
            {
                //SliderAdjustHarnessSize(1);
                SizeMeasureHarnesstype(1);
                // medium
            }
            else if (heightRoundOffNumber < 1.52f)
            {
                SizeMeasureHarnesstype(0);
                //SliderAdjustHarnessSize(0);
                //small
            }
        }

        if (108 < weight && weight < 136)
        {
            if (1.52f < heightRoundOffNumber && heightRoundOffNumber < 1.87f)
            {
                SizeMeasureHarnesstype(1);
                //SliderAdjustHarnessSize(1);
                // medium
            }
            else if(1.87f < heightRoundOffNumber && heightRoundOffNumber < 1.93f)
            {
                SizeMeasureHarnesstype(2);
                //SliderAdjustHarnessSize(2);
                // large
            }
        }

        if (136 < weight && weight < 154)
        {
            if (1.57f < heightRoundOffNumber && heightRoundOffNumber < 1.93f)
            {
                SizeMeasureHarnesstype(2);
                //SliderAdjustHarnessSize(2);
                // large
            }
            else if(1.93f < heightRoundOffNumber && heightRoundOffNumber < 1.98f)
            {
                SizeMeasureHarnesstype(3);
                //SliderAdjustHarnessSize(3);
                // X large
            }
            else if (1.57f > heightRoundOffNumber)
            {
                SizeMeasureHarnesstype(1);
                //SliderAdjustHarnessSize(1);
                // medium
            }
        }

        if (154 < weight && weight < 181)
        {
            if (1.62f < heightRoundOffNumber && heightRoundOffNumber < 1.98f)
            {
                SizeMeasureHarnesstype(3);
                //SliderAdjustHarnessSize(3);
                //x large
            }
            else if (1.98f < heightRoundOffNumber && heightRoundOffNumber < 2.03f)
            {
                SizeMeasureHarnesstype(4);
                //SliderAdjustHarnessSize(4);
                // 2X large
            }
            else if (1.62f > heightRoundOffNumber)
            {
                SizeMeasureHarnesstype(2);
                //SliderAdjustHarnessSize(2);
                // large
            }
        }


       
    }


}
