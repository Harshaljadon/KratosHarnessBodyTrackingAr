using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HarnessSelectModeUI : MonoBehaviour
{
    public RectTransform QrPanel, manual;
    public Image backImagefade;
    // Start is called before the first frame update  410.7737, 1293
    void Start()
    {
        backImagefade.DOFade(0, 0);
        QrPanel.anchoredPosition = new Vector2(0, 1293);
        manual.anchoredPosition = new Vector2(0, -1231);

        QrPanel.DOAnchorPos(new Vector2(0, 0), 1);
        manual.DOAnchorPos(new Vector2(0, 133.48f), 1);
        Invoke(nameof(FadeInBackBut), 1);

    }

    void FadeInBackBut()
    {
        backImagefade.DOFade(1, 1);
    }

    public void BackToAllModuleScene()
    {
        SceneManag.Instance.ProductModuleScene();
    }

    public void QrMode()
    {
        SceneManag.Instance.QrScene();
    }

    public void HarnessCollectionMode()
    {
        SceneManag.Instance.All_BODY_SAFTY_CollectionMainScene()
;    }
}
