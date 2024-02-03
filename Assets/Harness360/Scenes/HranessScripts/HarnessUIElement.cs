using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;

//[Serializable]
//public class AssetReferenceTMP_FontAsset : AssetReferenceT<TMP_FontAsset>
//{
//    public AssetReferenceTMP_FontAsset(string guid) : base(guid)
//    {
//    }
//}
public class HarnessUIElement : MonoBehaviour
{
    public TMP_Text nameTxt;
    public Image image;
    public int buttonIndex;
    public HarnessitemTypeEnum buttonType;
    public Element.ElementType eeT;
    [SerializeField]
    Button button = null;

    //AsyncOperationHandle<TMP_FontAsset> handleOp;
    //public AssetReferenceTMP_FontAsset materialReference;
    HarnessUI harnessUI;

    private void OnEnable()
    {
        //handleOp = materialReference.LoadAssetAsync<TMP_FontAsset>();
        //handleOp.Completed += (op) =>
        //{
        //    nameTxt.font = op.Result;
        //    //downloading.font = op.Result;

        //};
        //Txt.font.atlas.text = textureReference.LoadAssetAsync<Texture>().Result;
    }
    private void Start()
    {
        button.onClick.AddListener(OnPressedCell);
        harnessUI = FindObjectOfType<HarnessUI>();
    }
    void OnPressedCell()
    {
        if (buttonType == HarnessitemTypeEnum.HarnesCustomize)
        {
            // instantiate UI panel to with ui element where HarnessitemTypeEnum is none
            harnessUI.OnSelectHarnesPart(eeT);
            return;
        }
        if (buttonType == HarnessitemTypeEnum.Harness)
        {
            //Instentaite harness
        harnessUI.OnItemSelected(buttonType, buttonIndex, nameTxt.text, eeT);
            return;
        }
        if (buttonType == HarnessitemTypeEnum.None)
        {
            harnessUI.OnItemSelected(buttonType, buttonIndex, nameTxt.text, eeT);
            return;
        }
    }


    public void SetElementValues(HarnessElementCellData data)
    {
        image.sprite = data.icon;
        nameTxt.text = data.Message;
        buttonIndex = data.buttonIndex;
        buttonType = data.buttonTypeEnum;
        eeT = data.eelementType;
    }
}
