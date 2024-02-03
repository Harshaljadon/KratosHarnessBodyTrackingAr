using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommingSoonUiManager : MonoBehaviour
{
    public TextMeshProUGUI productCodeNameText;

    // Start is called before the first frame update
    void Start()
    {
        productCodeNameText.text = SceneManag.Instance.commingSoonHarnessCode;
    }

    public void BackToQR_Manual_Scene()
    {
        SceneManag.Instance.QR_Manual_Scene();
    }
}
