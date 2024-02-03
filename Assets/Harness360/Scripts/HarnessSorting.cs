using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HarnessSorting : MonoBehaviour
{
    public QuestionEnum harnessQuestionEnum;
    public int itemCollectionId, currentHarnessSizeId;
    public string itemProductCodeName;

    public GameObject[] harnessVariousSize;
    HarnessMasserManagerUI harnessMasserManagerUI;
    public HarnessSizetype harnessSizetype;

    public HarnessSize myHarnessSizeSet = HarnessSize.Small;

    private void OnEnable()
    {
        if (SceneManag.Instance.allHarnesCreated && SceneManager.GetActiveScene().buildIndex != 4)
        {

        SceneManag.Instance.harnessSize = myHarnessSizeSet;
        }
    }

    private void Start()
    {
            //Debug.Log("start again");
        foreach (var item in harnessVariousSize)
        {
            item.SetActive(false);
        }
        harnessMasserManagerUI = FindObjectOfType<HarnessMasserManagerUI>();
        if (harnessMasserManagerUI != null)
        {
        harnessMasserManagerUI.HarnessSorting = this;

        }
        int harnessSizeId = (int)SceneManag.Instance.harnessSize;
        if (SceneManag.Instance.manualHarnesSelection)
        {
            harnessVariousSize[harnessSizeId].SetActive(true);
            //Debug.Log(harnessSizeId);
            //DebugOnOffHarness(harnessSizeId);

        }
        else if (harnessMasserManagerUI != null)
        {
            //Debug.Log("non manual");
            //harnessVariousSize[harnessSizeId].SetActive(true);
            Invoke(nameof(DelaySlectHarnessSize), .5f);
        }
    }

    void DelaySlectHarnessSize()
    {
            harnessMasserManagerUI.QrOrDeepLinkingApprochToSelectHarnessSize();
        SceneManag.Instance.manualHarnesSelection = true;
    }

    private void OnDestroy()
    {
        if (harnessMasserManagerUI != null)
        {
            harnessMasserManagerUI.HarnessSorting = null;
        }
        CancelInvoke(nameof(CheckHarnessActiveness));
    }

    public void DebugOnOffHarness(int id)
    {
        CancelInvoke(nameof(CheckHarnessActiveness));
        currentHarnessSizeId = id;
        if (harnessVariousSize.Length == 0)
        {
            return;
        }
        if (harnessVariousSize[id].activeInHierarchy)
        {
            return;
        }
        foreach (var item in harnessVariousSize)
        {
            item.SetActive(false);
        }
        //Debug.Log("passed");
        harnessVariousSize[id].SetActive(true);
        myHarnessSizeSet = (HarnessSize)id;
        if (harnessMasserManagerUI != null)
        {

        harnessMasserManagerUI.ChangeHarnessSizeSound();
        }

        Invoke(nameof(CheckHarnessActiveness), 2);
    }

    /// <summary>
    /// check all harness are off if of then on that on which is id passed
    /// </summary>
    void CheckHarnessActiveness()
    {
        if (harnessVariousSize[currentHarnessSizeId].activeInHierarchy == false)
        {
        harnessVariousSize[currentHarnessSizeId].SetActive(true);

        }

    }
}

[System.Serializable]
public enum HarnessSizetype
{
    //twoTypeSize = 2, threeTypeSize = 3, universal = 5
    twoTypeSize = 0, threeTypeSize = 1, universal = 2
}
