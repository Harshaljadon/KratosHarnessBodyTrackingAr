using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<RectTransform> panels = new List<RectTransform>();

    private int _currentPanelIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(false);
        }

        _currentPanelIndex = 0;
        panels[_currentPanelIndex].gameObject.SetActive(true);
    }

    public void OnStartPressed()
    {
        // animate
       ShowNext();
    }
    
    public void OnEndPressed()
    {
        SceneController.Instance.LoadScene("MainScene");
    }

    public void ShowNext()
    {
        if (_currentPanelIndex < panels.Count)
        {
            //hide current one
            panels[_currentPanelIndex].gameObject.SetActive(false);
            //
            _currentPanelIndex++;
            //show next panel
            panels[_currentPanelIndex].gameObject.SetActive(true);
        }
    }

    public void ShowPrevious()
    {
        if (_currentPanelIndex > 0)
        {
            //hide current one
            panels[_currentPanelIndex].gameObject.SetActive(false);
            //
            _currentPanelIndex--;
            //show next panel
            panels[_currentPanelIndex].gameObject.SetActive(true);
        }
    }
    
}
