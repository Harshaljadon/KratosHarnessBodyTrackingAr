using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.Collections;
using UnityEngine.Networking;

public class ModelImporter : MonoBehaviour
{
    public GameObject targetObject; // The GameObject to which the imported model will be attached
    public GameObject modelPrefab;
    public GameObject modelInstance;
    private void Start()
    {
        // Register the button click event
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OpenFileBrowser);
    }

    private void OpenFileBrowser()
    {
        // Open the file browser with file pick mode
        // Open the file browser to select an .fbx model
        FileBrowser.SetFilters(true, new FileBrowser.Filter("FBX Files", ".fbx"));
        FileBrowser.SetDefaultFilter(".fbx");
        FileBrowser.ShowLoadDialog(OnFileSelected, null,FileBrowser.PickMode.Files, false, null, "Select .fbx Model", "Select");
    }

    private void OnFileSelected(string[] filePaths)
    {
        if (filePaths.Length > 0)
        {
            string filePath = filePaths[0];

            // Load the .fbx model and instantiate it as a GameObject
            StartCoroutine(LoadModel(filePath));
        }
    }

    private IEnumerator LoadModel(string filePath)
    {
        // Create a UnityWebRequest to load the model from the external location
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle("file://" + filePath);
        yield return request.SendWebRequest();

        if (!request.isNetworkError && !request.isHttpError)
        {
            // Get the downloaded asset bundle
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

            // Instantiate the model as a GameObject
            GameObject modelPrefab = bundle.LoadAsset<GameObject>(bundle.GetAllAssetNames()[0]);
            GameObject modelInstance = Instantiate(modelPrefab);

            // Attach the model to the target object or set its position/rotation as desired
            if (targetObject != null)
            {
                modelInstance.transform.SetParent(targetObject.transform);
            }
            else
            {
                // Set position and rotation if no target object is specified
                modelInstance.transform.position = Vector3.zero;
                modelInstance.transform.rotation = Quaternion.identity;
            }

            bundle.Unload(false);
        }
        else
        {
            Debug.LogError("Failed to load model: " + request.error);
        }
    }
}
