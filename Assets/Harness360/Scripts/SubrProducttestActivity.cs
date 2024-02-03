using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubrProducttestActivity : MonoBehaviour
{
    [SerializeField]
    string nameAssign;
    public int activityId;

    public string NameAssign
    {
        get { return nameAssign; }
        set { nameAssign = value;
            SetTextNameButton();
        }
    }

    //public int ActivityId
    //{
    //    get { return activityId; }
    //    set { activityId = value; }
    //}

    void SetTextNameButton()
    {
        buttonName.text = nameAssign;

    }



    [SerializeField]
    TextMeshProUGUI buttonName;
    // Start is called before the first frame update
    //void Start()
    //{
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
