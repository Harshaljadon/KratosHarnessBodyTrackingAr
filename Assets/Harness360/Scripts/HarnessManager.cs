using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class HarnessManager : MonoBehaviour
{
    public UiManager uiManager;
    public event Action testingActivityEvent;
    public HarnessCaryForwardData _harnessCaryForwardData;
    public HarnessTouchControls _harnessTouchControls;
    //[SerializeField]
    //FilterHarnessManagerQuestion _filterHarnessManagerQuestion;
    [SerializeField]
    GameObject HarnessParent;
    [SerializeField]
    TextMeshProUGUI itemNameText;

    public TextMeshProUGUI ItemNameText
    {
        set { itemNameText = value; }
    }


    // public Harness basicHarness;
    // public Harness harness2;
    [System.Serializable]
    public class ComponentDetails
    {
        public string name;
        public Sprite icon;
    }
    //[System.Serializable]
    //public class HarnesItemInfo
    //{
    //    public HarnessitemTypeEnum type;
    //    public List<ComponentDetails> details = new List<ComponentDetails>();
    //}


    public List<Harness> itemPrefabs ;

    /// <summary>
    /// hold all product item like eyeglass, hemet, mask, shoe, harness, gloves 
    /// </summary>
    [Tooltip("hold all product item like eyeglass, hemet, mask, shoe, harness, gloves")]
    [SerializeField]
    ProductItemCollection allProductitem;
    [SerializeField]
    List<GameObject> itemInstantiatedCollection = new List<GameObject>();
    //public HarnessTheme harnessTheme;

     Harness _currentHarness;
    public Harness CurrentHarness => _currentHarness;
    public Quaternion rotation;
    public Vector3 itemParentScale;
    public float yAxisOffset;
    public GameObject harnessBase;
    private void Awake()
    {
        harnessBase.SetActive(false);
        itemPrefabs = new List<Harness>();
        AssignProductfromScriptableToCollection();
        FilterProductItem();
        //harnessTheme = GetComponent<HarnessTheme>();
        //_filterHarnessManagerQuestion.harnessfilterationAction += _filterHarnessManagerQuestion_harnessfilterationAction;
        //_filterHarnessManagerQuestion.restartHranessQuestionProcess += DestroyAllCreatedharness;
        itemNameText.gameObject.SetActive(false);
        _harnessTouchControls = FindObjectOfType<HarnessTouchControls>();
    }
     int spaceSum, uiLeftRightindex, totalharnessCreated;

    void AssignProductfromScriptableToCollection()
    {
        switch (SceneManag.Instance._currentProductSubCatagory)
        {
            case ProductSubCatagory.EYEGLASS:
                foreach (var item in _harnessCaryForwardData.eyeGlassCollection)
                {
                    allProductitem.eyeGlass.Add(item.gameObject.GetComponent<Harness>());
                }

                break;
            case ProductSubCatagory.HELMET:
                foreach (var item in _harnessCaryForwardData.hemetCollection)
                {
                    allProductitem.helmet.Add(item.gameObject.GetComponent<Harness>());

                }
                break;
            case ProductSubCatagory.MASK:
                foreach (var item in _harnessCaryForwardData.maskCollection)
                {
                    allProductitem.mask.Add(item.gameObject.GetComponent<Harness>());

                }

                break;
            case ProductSubCatagory.HARNESS:
                foreach (var item in _harnessCaryForwardData.collectionHarness)
                {
                    allProductitem.harnesses.Add(item.gameObject.GetComponent<Harness>());

                }

                break;
            case ProductSubCatagory.GLOVES:
                foreach (var item in _harnessCaryForwardData.gloveCollection)
                {
                    allProductitem.gloves.Add(item.gameObject.GetComponent<Harness>());

                }
                break;
            case ProductSubCatagory.SHOE:
                foreach (var item in _harnessCaryForwardData.shoeCollection)
                {
                    allProductitem.shoes.Add(item.gameObject.GetComponent<Harness>());

                }
                break;

            default:
                break;
        }


    }

    private void FilterProductItem()
    {
        _harnessCaryForwardData.productCatogorie = SceneManag.Instance.currentProductCatagory.ToString();
        _harnessCaryForwardData.productSubCatogorie = SceneManag.Instance._currentProductSubCatagory.ToString();
        switch (SceneManag.Instance._currentProductSubCatagory)
        {
            case ProductSubCatagory.EYEGLASS:
                rotation = Quaternion.Euler(0, 0, 0);
                HarnessParent.transform.localScale = itemParentScale;
                HarnessParent.transform.position = new Vector3(HarnessParent.transform.position.x, yAxisOffset, HarnessParent.transform.position.z);
                itemPrefabs = allProductitem.eyeGlass;
                break;
            case ProductSubCatagory.HELMET:

                HarnessParent.transform.position = new Vector3(HarnessParent.transform.position.x, yAxisOffset, HarnessParent.transform.position.z);
                HarnessParent.transform.localScale = itemParentScale;
                rotation = Quaternion.Euler(0, 0, 0);
                itemPrefabs = allProductitem.helmet;
                break;
            case ProductSubCatagory.MASK:

                HarnessParent.transform.position = new Vector3(HarnessParent.transform.position.x, yAxisOffset, HarnessParent.transform.position.z);
                HarnessParent.transform.localScale = itemParentScale;
                rotation = Quaternion.Euler(0, 0, 0);
                itemPrefabs = allProductitem.mask;
                break;
            case ProductSubCatagory.HARNESS:
                HarnessParent.transform.position = new Vector3(HarnessParent.transform.position.x, 0.15f, HarnessParent.transform.position.z);
                rotation = Quaternion.Euler(90, 180, 0);
                itemPrefabs = allProductitem.harnesses;
                HarnessParent.transform.localScale = Vector3.one;
                harnessBase.SetActive(true);

                break;
            case ProductSubCatagory.GLOVES:
                rotation = Quaternion.Euler(0, 0, 0);
                HarnessParent.transform.position = new Vector3(HarnessParent.transform.position.x, yAxisOffset, HarnessParent.transform.position.z);

                HarnessParent.transform.localScale = new Vector3(1,1,1);
                itemPrefabs = allProductitem.gloves;
                break;
            case ProductSubCatagory.SHOE:
                HarnessParent.transform.position = new Vector3(HarnessParent.transform.position.x, yAxisOffset, HarnessParent.transform.position.z);
                rotation = Quaternion.Euler(0, 0, 0);
                HarnessParent.transform.localScale = itemParentScale;

                itemPrefabs = allProductitem.shoes;
                break;

            default:
                break;
        }
    }
    /// <summary>
    /// creating items in start
    /// </summary>
    public void PresentAllSelectedProductItem()
    {
        uiLeftRightindex = 0;
        itemNameText.gameObject.SetActive(true);
        itemNameText.DOFade(1, 1);
        foreach (var itemSortingObj in itemPrefabs)
        {
            var itemInstantiated = Instantiate(itemSortingObj, HarnessParent.transform);
            //itemInstantiated.hrmH.dummyModel.SetActive(true);
            itemInstantiated.dummy.GetComponent<SkinnedMeshRenderer>().enabled = true;
            itemInstantiated.transform.position = HarnessParent.transform.position + new Vector3(spaceSum, 0, 0);
            itemInstantiated.transform.rotation = rotation;
            spaceSum += 2;
            itemInstantiatedCollection.Add(itemInstantiated.gameObject);
        }
        _currentHarness = itemInstantiatedCollection[0].GetComponent<Harness>();
        //_currentHarness.hrmH.dummyModel.gameObject.SetActive(true);
        totalharnessCreated = itemInstantiatedCollection.Count;
        ActiveDeactiveharnessObject(0);
        _harnessCaryForwardData.productItemScriptableIndex = itemInstantiatedCollection[0].GetComponent<HarnessSorting>().itemCollectionId;
        itemNameText.text = itemInstantiatedCollection[0].GetComponent<HarnessSorting>().itemProductCodeName;
        int currentSizeTypeId = (int)itemInstantiatedCollection[0].GetComponent<HarnessSorting>().harnessSizetype;
        //Debug.Log(currentSizeTypeId);
        uiManager.UiharnessSizetypePanel(currentSizeTypeId);
        _harnessCaryForwardData.productItemCode = itemNameText.text;
        testingActivityEvent?.Invoke();
        SceneManag.Instance.allHarnesCreated = true;
    }
    /// <summary>
    /// this will cal use click right left arrow button to change harnes model
    /// </summary>
    /// <param name="value">value is use for to shift left or right -1,1 </param>
    public void CHangeCurrntHarnessUi(int value)
    {
        RestAllY_X_ZaxisRotation();
        if (value == -1 && uiLeftRightindex > 0)
        {
            uiLeftRightindex += value;
            uiLeftRightindex = Mathf.Clamp(uiLeftRightindex, 0, totalharnessCreated);
            HarnessParent.transform.position += new Vector3(2, 0, 0);
        }
        if (value == 1 && uiLeftRightindex < totalharnessCreated -1)
        {
            uiLeftRightindex += value;
            uiLeftRightindex = Mathf.Clamp(uiLeftRightindex, 0, totalharnessCreated);

            HarnessParent.transform.position += new Vector3(-2, 0, 0);
        }
        _currentHarness = itemInstantiatedCollection[uiLeftRightindex].GetComponent<Harness>();
        int id = (int)itemInstantiatedCollection[uiLeftRightindex].GetComponent<HarnessSorting>().harnessSizetype;
        //Debug.Log(id);

        uiManager.UiharnessSizetypePanel(id);
        //_currentHarness.hrmH.dummyModel.gameObject.SetActive(true);

        ActiveDeactiveharnessObject(uiLeftRightindex);
        _harnessCaryForwardData.productItemScriptableIndex = itemInstantiatedCollection[uiLeftRightindex].GetComponent<HarnessSorting>().itemCollectionId;
        itemNameText.text = itemInstantiatedCollection[uiLeftRightindex].GetComponent<HarnessSorting>().itemProductCodeName;
        _harnessCaryForwardData.productItemCode = itemNameText.text;
        testingActivityEvent?.Invoke();

    }


    void RestAllY_X_ZaxisRotation()
    {
        foreach (var item in itemInstantiatedCollection)
        {
            if (SceneManag.Instance.currentProductCatagory == ProductCatagory.BODY)
            {
                item.transform.rotation = rotation;
            }
            if (SceneManag.Instance._currentProductSubCatagory == ProductSubCatagory.EYEGLASS)
            {
                item.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
            if (SceneManag.Instance._currentProductSubCatagory == ProductSubCatagory.HELMET)
            {
                _harnessTouchControls.ResetAllTransFormCompoenet(false);
            }
        }

    }
    //public void DestroyAllCreatedharness()
    //{
    //    spaceSum = 0;
    //    _currentHarness = null;
    //    foreach (var item in harnesInstantiatedCollection)
    //    {
    //        Destroy(item);
    //    }
    //    harnesInstantiatedCollection.Clear();
    //    harnessnameText.DOFade(0,0);

    //    harnessnameText.gameObject.SetActive(false);
    //    HarnessParent.transform.position = new Vector3(0, HarnessParent.transform.position.y, HarnessParent.transform.position.z);

    //}

    void ActiveDeactiveharnessObject(int indexHarness)
    {
        foreach (var item in itemInstantiatedCollection)
        {
            item.SetActive(false);
            item.transform.localScale = new Vector3(1, 1, 1);
        }
        itemInstantiatedCollection[indexHarness].SetActive(true);
    }

    //public void ChooseHarness(int harness)
    //{
    //    if (_currentHarness != null)
    //    {
    //        DestroyImmediate(_currentHarness.gameObject);
    //    }

    //    if (harness < itemPrefabs.Count)
    //    {
    //        _currentHarness = Instantiate(itemPrefabs[harness]) as Harness;
    //    }
    //}


    //public void SetRotation(float rot)
    //{
    //    if (_currentHarness != null)
    //    {
    //        _currentHarness.transform.rotation = Quaternion.Euler(0,rot, 0);
    //    }
    //}
    

    //public int GetColorCountForElement(Element.ElementType elementType)
    //{
    //    return harnessTheme.GetAvaliableColorCount(elementType);
    //}
    
    //public Sprite GetSpriteForColor(Element.ElementType elementType, int i)
    //{
    //    return harnessTheme.GetSpriteForColor(elementType, i);
    //}

    public int GetHarnessCount()
    {
        return itemPrefabs.Count;
    }

    public string GetHarnessName(int index)
    {
        return itemPrefabs[index].name;
    }


    //+++++++++++++++++++++++++++++++
    //public List<HarnesItemInfo> harnessItemInfos = new List<HarnesItemInfo>();


    //public Sprite GetItemIcon(int index, HarnessitemTypeEnum harnessitemTypeEnum)
    //{
    //    Sprite sp = null;

    //    foreach (var item in harnessItemInfos)
    //    {
    //        if (item.type == harnessitemTypeEnum)
    //        {
    //            sp = item.details[index].icon;
    //        }
    //    }

    //    return sp;
    //}

    //public string GetItemName(int index, HarnessitemTypeEnum harnessitemTypeEnum)
    //{
    //    string str = null;

    //    foreach (var item in harnessItemInfos)
    //    {
    //        if (item.type == harnessitemTypeEnum)
    //        {
    //            str = item.details[index].name;
    //        }
    //    }

    //    return str;
    //}

    //public void SetColorVariation(Element.ElementType elementType, int index)
    //{
    //    Material mat = harnessTheme.GetMaterialForElementAtIndex(elementType, index);
        
    //    _currentHarness.SetColorVariation(elementType, mat);
        
    //}

    public Vector3 dumyRoate;
    //internal void SelectItem(HarnessitemTypeEnum buttonType, int buttonIndex, string name, Element.ElementType eeT)
    //{
    //    switch (buttonType)
    //    {
    //        case HarnessitemTypeEnum.Harness:
    //            {
    //                if (_currentHarness?.gameObject)
    //                {
    //                    Destroy(_currentHarness.gameObject);
    //                }

    //                _currentHarness = Instantiate(harnessPrefabs[buttonIndex]).GetComponent<Harness>();
    //                //update harness in scriptable to forward to ar
    //                _currentHarness.GetComponent<Transform>().Rotate(dumyRoate);
    //                _harnessCaryForwardData.harness = harnessPrefabs[buttonIndex].gameObject;
    //                // udate index harness in scriptable harness data
    //                _harnessCaryForwardData.harnesScriptableIndex = buttonIndex;

    //            }
    //            break;

    //        case HarnessitemTypeEnum.None:
    //            {
    //                Material passmaterial = harnessTheme.GetMaterialForElementAtIndex(eeT, buttonIndex);
    //                _currentHarness.GetComponent<Harness>().SetColorVariation(eeT, passmaterial);
    //                //currentActive.SetItems(itemType, buttonName, nameWinchResLoc, winchesGameObj[index]);

    //            }
    //            break;
          

    //        default:
    //            break;
    //    }
    //}
}
/// <summary>
/// collection catagories
/// </summary>
[System.Serializable]
public class ProductItemCollection
{
    public List<Harness> harnesses,eyeGlass,helmet,mask,gloves,shoes;
    
}
