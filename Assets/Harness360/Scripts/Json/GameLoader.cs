using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public TextAsset jsonTextAsset; // Reference to the JSON text asset
    //public GameData gameData;
    string jsonText, jsonText2;
    FileSelector fileSelector;
    private void Start()
    {
        //jsonText = jsonTextAsset.text;


        //gameData = JsonUtility.FromJson<GameData>(jsonText);

        //foreach (GameObjectData gameObjectData in gameData.gameObjects)
        //{
        //    GameObject prefab = Resources.Load<GameObject>(gameObjectData.name);
        //    Instantiate(prefab, gameObjectData.position, Quaternion.identity);
        //}

        fileSelector = FindObjectOfType<FileSelector>();
        //fileSelector.onFileSelected += ExecuteJsonTask;

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExecuteJsonTask(string _jsonText)
    {
        jsonText2 = _jsonText;
        //Debug.Log(_jsonText);

        //gameData = JsonUtility.FromJson<GameData>(jsonText2);

        //foreach (GameObjectData gameObjectData in gameData.gameObjects)
        //{
        //    GameObject prefab = Resources.Load<GameObject>(gameObjectData.UserName);
        //    Instantiate(prefab, gameObjectData.position, Quaternion.Euler(90,180,0));
        //}
    }
}

