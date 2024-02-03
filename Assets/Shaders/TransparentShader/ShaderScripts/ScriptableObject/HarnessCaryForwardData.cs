using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HarnessDataStore", menuName = "HarnessData")]
public class HarnessCaryForwardData : ScriptableObject
{
    //public ImageUrlQrdataStrore prevouslyData;
    public int productItemScriptableIndex;
    public string productItemCode;

    public string productCatogorie;
    public string productSubCatogorie;

    public float userWeight;
    public string userMailId;
    public string imageUploadUrlQr;

    public GameObject harness;
    public List<HarnessSorting> collectionHarness, eyeGlassCollection, hemetCollection, maskCollection, shoeCollection, gloveCollection;
    
    //public DataHarness[] dH;
    // Add your desired variables and functionality here
    //public string myString;

    public void DoSomething()
    {
        // Add functionality here
    }
}

[System.Serializable]
public class DataHarness
{
    public HarnessitemTypeEnum harnesElementType;
    public SkinnedMeshRenderer[] sMRenderer;

}

[System.Serializable]
public class ImageUrlQrdataStrore
{
    public string productUrl;
    public byte[] fileStr;
    public string fileName;
    public string puId;

}

