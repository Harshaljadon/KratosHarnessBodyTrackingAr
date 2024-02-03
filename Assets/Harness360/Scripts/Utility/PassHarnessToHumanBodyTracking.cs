using UnityEngine.XR.ARFoundation.Samples;
using UnityEngine;
using System.Collections.Generic;

public class PassHarnessToHumanBodyTracking : MonoBehaviour
{
    public HarnessCaryForwardData harnessData;
    public HumanBodyTracker hBT;
    List<GameObject> prefenSkeletonData;

    private void Awake()
    {
        prefenSkeletonData = new List<GameObject>();
        hBT = FindObjectOfType<HumanBodyTracker>();
        hBT.harnessIndex = harnessData.productItemScriptableIndex;
        foreach (var item in harnessData.collectionHarness)
        {
            prefenSkeletonData.Add(item.gameObject);
        }
        hBT.SkeletonPrefebCollection = prefenSkeletonData;
        //Passharness();

    }
    public void Passharness()
    {
    }
}
