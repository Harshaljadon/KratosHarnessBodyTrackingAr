using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class HarnessUiAddElement : MonoBehaviour
{
    public GameObject panelItemPrefeb;
    public float divYPosPanel = 2f;
    RectTransform panelItem;
    public RectTransform contentPanel;
    public void UpdateHarnessPanelCellData(List<HarnessElementCellData> data)
    {
        //loadingStatus.SetActive(true);

        //panelItemAddressableAsset.LoadAssetAsync<GameObject>().Completed += (op) =>
        //{

        //    if (op.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        loadingStatus.SetActive(false);


        //        Addressables.Release(op);
        //    }

        //};
        ClearAllHarnessUi();
        panelItem = panelItemPrefeb.GetComponent<RectTransform>();
        var count = data.Count;
        for (int i = 0; i < count; i++)
        {
            RectTransform uiItem = Instantiate(panelItem, contentPanel);
            HarnessUIElement element = uiItem.GetComponent<HarnessUIElement>();
            element.SetElementValues(data[i]);
        }


        // update Scrollbar
        RectTransform rt = contentPanel.GetComponent<RectTransform>();
        int childsInVerticle = contentPanel.childCount / 3;

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((panelItem.rect.height + 10f) * (childsInVerticle + 1f)));// gap and spacing added
        rt.DOAnchorPosY(-(rt.rect.height / divYPosPanel), 0, false);//= new Vector2(rt.anchoredPosition3D.x, (-rt.anchoredPosition3D.y / 2f) ) ;

    }
    public void ClearAllHarnessUi()
    {
        foreach (Transform child in contentPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
