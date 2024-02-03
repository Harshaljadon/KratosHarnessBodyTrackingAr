using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRegesterAndDeregesterToUiPanelCollec : MonoBehaviour
{
    UiPannelCollection collection;
    private void Awake()
    {
         collection = FindObjectOfType<UiPannelCollection>();
    }

    private void OnEnable()
    {
        if (collection != null)
        {
        collection.AddDescriptionPanel(this.gameObject);

        }
    }


    private void OnDisable()
    {
        if (collection != null)
        {

        collection.RemoveDescriptionPanel(this.gameObject);
        }
    }
}
