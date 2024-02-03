using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableAllDescPanelOnClickInfoPanel : MonoBehaviour
{
    public GameObject rootUnderRangeStates;
     UnderRangeStates uRS;
    public Button buttonInfoPanel;
    public GameObject descPanel;
    UiPannelCollection uPC;
    public float leftRightPos;

    private void OnEnable()
    {
        uRS = rootUnderRangeStates.GetComponent<UnderRangeStates>();
        var greaterOrLessAngleResult = uRS._GreaterThanNegative90;
        if (descPanel != null && leftRightPos != 0)
        {
            if (greaterOrLessAngleResult)
            {
                descPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-leftRightPos, 0, 0);
            }
            else
            {
                descPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(leftRightPos, 0, 0);
            }
        }

        //Debug.Log(greaterOrLessAngleResult);
    }
    // Start is called before the first frame update
    void Start()
    {


        uPC = FindObjectOfType<UiPannelCollection>();
        buttonInfoPanel.onClick.AddListener(() =>
        {
            if (!descPanel.activeInHierarchy)
            {
            uPC.DisableAllregiterPanel();
            descPanel.SetActive(true);

            }
            else
            {
                uPC.DisableAllregiterPanel();

                descPanel.SetActive(false);

            }
            //this.gameObject.SetActive(false);
        });

        
    }


    //void TakeAction()
    //{
    //    StartCoroutine(nameof(OnOffObj));
    //}

    //IEnumerator OnOffObj()
    //{
    //    uPC.DisableAllregiterPanel();
    //    yield return new WaitForEndOfFrame();
    //    descPanel.SetActive(true);
    //}
}
