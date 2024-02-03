using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPannelCollection : MonoBehaviour
{
    public List<GameObject> descripPanelCollection = new List<GameObject>();
    DiableAnableObj dNAOBJ;
    // Start is called before the first frame update
    void Start()
    {
        dNAOBJ = new DiableAnableObj();
    }

    public void AddDescriptionPanel(GameObject iconPanelObj)
    {
        descripPanelCollection.Add(iconPanelObj);
    }

    public void RemoveDescriptionPanel(GameObject obj)
    {
        descripPanelCollection.Remove(obj);
    }

    public void DisableAllregiterPanel()
    {
        if (descripPanelCollection.Count != 0)
        {
        dNAOBJ.DiableObj(descripPanelCollection);

        }
    }
}
