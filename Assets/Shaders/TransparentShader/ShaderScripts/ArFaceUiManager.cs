using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ArFaceUiManager : MonoBehaviour
{
    public Button backScenMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        backScenMenuButton.GetComponent<Button>().onClick.AddListener(() => { BacktoOR_ManualScen(); });

    }

    void BacktoOR_ManualScen()
    {
        SceneManag.Instance.QR_Manual_Scene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
