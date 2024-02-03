using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using SimpleFileBrowser;
using System;
using TMPro;

public class FileSelector : MonoBehaviour
{
    public TextMeshProUGUI selectedFileText;
    //public UnityEvent<string> onFileSelected;

    public event Action<string> onFileSelected;




    private void Awake()
    {
        // Subscribe to the file selection event
        FileBrowser.SetFilters(true, new FileBrowser.Filter("JSON Files", ".txt"));
        FileBrowser.SetDefaultFilter(".txt");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
    }

    private void Start()
    {
        // Register the button click event
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OpenFileBrowser);
    }

    private void OpenFileBrowser()
    {
        // Open the file browser
        FileBrowser.ShowLoadDialog(OnFileSelected, null,FileBrowser.PickMode.Files, true, null, "Select JSON File", "Select");
    }

    private void OnFileSelected(string[] filePaths)
    {
        if (filePaths.Length > 0)
        {
            string filePath = filePaths[0];

            // Read the selected JSON file
            string jsonText = System.IO.File.ReadAllText(filePath);
            Debug.Log(jsonText);
            selectedFileText.text = filePath; // Display the selected file path (optional)

            // Invoke the file selected event
            onFileSelected?.Invoke(jsonText);
        }
    }
}
