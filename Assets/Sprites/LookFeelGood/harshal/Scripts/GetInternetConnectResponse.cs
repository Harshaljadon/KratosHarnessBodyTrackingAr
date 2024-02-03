using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;


public class GetInternetConnectResponse : MonoBehaviour
{
    bool connectedInternet;

    public static GetInternetConnectResponse Instance;

    public bool ConnectedInternet { get {
            CheckConnectionProcess();
            return connectedInternet;
        }
        private set => connectedInternet = value; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CheckConnectionProcess();
        DontDestroyOnLoad(this.gameObject);
    }

    public void CheckConnectionProcess()
    {
        //Debug.Log("checking");

        CheckNetworking a = new CheckNetworking();
        string HtmlText = a.GetHtmlFromUri("http://google.com");
        if (HtmlText == "")
        {
            //No connection
            connectedInternet = false;
        }
        else if (!HtmlText.Contains("schema.org/WebPage"))
        {
            //Redirecting since the beginning of googles html contains that 
            //phrase and it was not found
            connectedInternet = false;
        }
        else
        {
            //success
            connectedInternet = true;
        }
    }
  
}
