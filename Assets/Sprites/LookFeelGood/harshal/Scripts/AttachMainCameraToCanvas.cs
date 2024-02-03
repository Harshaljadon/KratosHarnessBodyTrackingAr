using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachMainCameraToCanvas : MonoBehaviour
{
    private Camera MyCamera;
    Canvas myCan;
    private void OnEnable()
    {
        MyCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        myCan = GetComponent<Canvas>();
        myCan.worldCamera = MyCamera;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
