using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    public static SceneController Instance => _instance;

    private string _sceneName; // field

    public string SceneName => _sceneName;


    private void Awake()
    {
        if (_instance != null && _instance != this) 
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad( this.gameObject );
        }
    }


    private void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string scnName)
    {
        _sceneName = scnName;
        SceneManager.LoadSceneAsync(_sceneName);
    }
    
}