using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;

public class HarnessUI : MonoBehaviour
{
    public HarnessManager harnessManager;
    public HarnessTheme harnessTheme;
    public HarnessUiAddElement harnessUiAddElement;
    public GameObject HarnessPanelSelection;
    public Button ARSceneButton;
    public GameObject[] onOffUiPanel;

    [SerializeField]
    TextMeshProUGUI title;

    private void Start()
    {
        ARSceneButton.GetComponent<Button>().onClick.AddListener(() => { ArScene_Final_End_Ar(); });
        SetTitleName();
    }

    void SetTitleName()
    {
        switch (SceneManag.Instance._currentProductSubCatagory)
        {
            case ProductSubCatagory.EYEGLASS:
                title.text = "Select Safety Eyeglasses";
                break;
            case ProductSubCatagory.HELMET:
                title.text = "Select Helmet";

                break;
            case ProductSubCatagory.MASK:

                title.text = "Select Mask";
                break;
            case ProductSubCatagory.HARNESS:

                title.text = "Select Harness";
                break;
            case ProductSubCatagory.GLOVES:

                title.text = "Select Gloves";
                break;
            case ProductSubCatagory.SHOE:

                title.text = "Select Shoe";
                break;

            default:
                break;
        }
    }

    void ArScene_Final_End_Ar()
    {
        //SceneManag.Instance.ARHarnessScene();
        SceneManag.Instance.Load_AR_Scene();
    }

    public void OnSelectHarnes()
    {
        OnStateSelected(harnessManager.itemPrefabs, HarnessitemTypeEnum.Harness, Element.ElementType.None);

    }

    public void OnSelectHarnesCustomize()
    {
        if (harnessManager.CurrentHarness != null)
        {
        OnStateSelected(harnessManager.CurrentHarness.GetComponent<Harness>().harnessInfo,HarnessitemTypeEnum.HarnesCustomize,Element.ElementType.None);

        }

    }

    public void OnSelectHarnesPart(Element.ElementType eeT)
    {
        foreach (var elementType in harnessTheme.allVariations)
        {
            if (elementType.Type == eeT )
            {
                OnStateSelected(elementType.AvaliableMatrials, HarnessitemTypeEnum.None, eeT);
            }
        }
        //OnStateSelected();

    }


    void OnStateSelected<T>(List<T> collectionitem, HarnessitemTypeEnum harnessitemTypeEnum, Element.ElementType eET)
    {
        OnOffPanel(false);

        HarnessPanelSelection.SetActive(true);
        SetPanelHeading(harnessitemTypeEnum);
        int elementCount = collectionitem.Count;

        switch (harnessitemTypeEnum)
        {
            case HarnessitemTypeEnum.Harness:
                CreateUiButtonOnPanelharnessManager(elementCount, harnessitemTypeEnum, eET);
                break;
            case HarnessitemTypeEnum.HarnesCustomize:
                CreateUiButtonOnPanelharnessThem(elementCount, harnessitemTypeEnum, eET);
                break;
            case HarnessitemTypeEnum.None:
                CreateUiButtonOnPanelharnessThem(elementCount, harnessitemTypeEnum, eET);
                break;

            default:
                break;
        }

    }

    void CreateUiButtonOnPanelharnessManager(int count, HarnessitemTypeEnum harnessitemTypeEnum, Element.ElementType eET )
    {

        //var cellData = Enumerable.Range(0, count)
        //.Select(i => new HarnessElementCellData
        //{
        //  icon = harnessManager.GetItemIcon(i, harnessitemTypeEnum),
        //  Message = harnessManager.GetItemName(i, harnessitemTypeEnum),
        //  buttonIndex = i,
        //  buttonTypeEnum = harnessitemTypeEnum,
        //  eelementType = eET
        //})
        //.ToList();
        //harnessUiAddElement.UpdateHarnessPanelCellData(cellData);
    }
    void CreateUiButtonOnPanelharnessThem(int count, HarnessitemTypeEnum harnessitemTypeEnum, Element.ElementType eET)
    {
        if (harnessitemTypeEnum == HarnessitemTypeEnum.HarnesCustomize)
        {
            var cellData = Enumerable.Range(0, count)
            .Select(i => new HarnessElementCellData
            {
                icon = harnessTheme.GetVariationIcon(harnessManager.CurrentHarness.GetComponent<Harness>().harnessInfo[i].Type),
                Message = harnessManager.CurrentHarness.GetComponent<Harness>().harnessInfo[i].Type.ToString(),
                buttonIndex = i,
                buttonTypeEnum = harnessitemTypeEnum,
                //eelementType = harnessTheme.GetAllVariationTypeEnum(i)
                eelementType = harnessManager.CurrentHarness.GetComponent<Harness>().harnessInfo[i].Type
            })
            .ToList();
            harnessUiAddElement.UpdateHarnessPanelCellData(cellData);
        }
        else
        {
            var cellData = Enumerable.Range(0, count)
            .Select(i => new HarnessElementCellData
            {
                icon = harnessTheme.GetSpriteForColor(eET, i),
                Message = harnessTheme.GetNameForElemnt(eET,i),
                buttonIndex = i,
                buttonTypeEnum = harnessitemTypeEnum,
                eelementType = eET
            })
            .ToList();
            harnessUiAddElement.UpdateHarnessPanelCellData(cellData);
        }

    }

    #region PanelHeading
    public TMP_Text panelHeadingText;

    /// <summary>
    /// Set Panelheading when user select Button
    /// </summary>
    /// <param name="harnessitemTypeEnum"></param>
    void SetPanelHeading(HarnessitemTypeEnum harnessitemTypeEnum)
    {
        switch (harnessitemTypeEnum)
        {
            case HarnessitemTypeEnum.Harness:
                {
                    panelHeadingText.text = "Select Harness";
                }
                break;
            case HarnessitemTypeEnum.HarnesCustomize:
                {
                    panelHeadingText.text = "Select component to customize";
                }
                break;
            case HarnessitemTypeEnum.Buckel:
                {
                    panelHeadingText.text = "Select Buckel";
                }
                break;
            case HarnessitemTypeEnum.D_Ring:
                {
                    panelHeadingText.text = "Select D-Ring";
                }
                break;

            case HarnessitemTypeEnum.Loops:
                {
                    panelHeadingText.text = "Select Loops";
                }
                break;
            case HarnessitemTypeEnum.Shoulder_Strap:
                {
                    panelHeadingText.text = "Select Shoulder-Strap";
                }
                break;
            case HarnessitemTypeEnum.Sit_Strap:
                {
                    panelHeadingText.text = "Select Sit-Strap";
                }
                break;
            case HarnessitemTypeEnum.Stitching:
                {
                    panelHeadingText.text = "Select Stitching";
                }
                break;
            case HarnessitemTypeEnum.Thigh_Strap:
                {
                    panelHeadingText.text = "Select Thigh-Strap";
                }
                break;
            default:
                break;
        }

    }
    #endregion

    internal void OnItemSelected(HarnessitemTypeEnum buttonType, int buttonIndex, string name, Element.ElementType eet)
    {
        //harnessManager.SelectItem(buttonType, buttonIndex, name, eet);

        ////// based on item selected change UI for next
        //switch (buttonType)
        //{
        //    case HarnessitemTypeEnum.Harness:
        //        {
        //            //UpdateMenu(HarnessitemTypeEnum.Harness);
        //            OnItemsBack();
        //            OnOffPanel(true);
        //        }
        //        break;
        //    case HarnessitemTypeEnum.None:
        //        {
        //            //UpdateMenu(HarnessitemTypeEnum.HarnesCustomize);
        //            OnItemsBack();
        //            OnOffPanel(true);
        //        }
        //        break;
        //    default:
        //        break;
        //}

        OnItemsBack();
        OnOffPanel(true);
    }



    void OnOffPanel(bool onOffValue)
    {
        foreach (var item in onOffUiPanel)
        {
            item.SetActive(onOffValue);
        }
    }

    public void OnItemsBack()
    {
        OnOffPanel(true);
        harnessUiAddElement.ClearAllHarnessUi();
        HarnessPanelSelection.SetActive(false);
    }

    //private void UpdateMenu(HarnessitemTypeEnum buttonType)
    //{
    //    throw new NotImplementedException();
    //}
}
