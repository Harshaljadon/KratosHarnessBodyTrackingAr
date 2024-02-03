using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnchorType
{
    Start,
    Mid,
    End
}

public class Anchor : MonoBehaviour
{
    public AnchorType anchorType;

    public GameObject tensioner;
    public GameObject extermity;

    public Transform startPoint;
    public Transform endPoint;

    public List<GameObject> variationMeshes = new List<GameObject>();

    public int currentVariation { get; private set; }

    private void Awake()
    {
        if (anchorType == AnchorType.Start)
        {
            // enable tensionar
            if (tensioner != null)
            {
                tensioner.SetActive(true);
                startPoint.position = tensioner.transform.GetChild(0).position;
            }
            if (extermity != null)
            {
                extermity.SetActive(false);
            }
        }
        else
        {
            // enable extremity
            if (tensioner != null)
            {
                tensioner.SetActive(false);
            }
            if (extermity != null)
            {
                extermity.SetActive(true);

                startPoint.position = extermity.transform.GetChild(0).position;
            }
        }
    }

    public void SetVariation(bool next)
    {
        if(next)
        {
            if(currentVariation < variationMeshes.Count - 1)
            {
                currentVariation++;
            }
        }
        else
        {
            if (currentVariation > 0)
            {
                currentVariation--;
            }

        }

        for (int i = 0; i < variationMeshes.Count; i++)
        {
            variationMeshes[i].SetActive(false);
        }

        variationMeshes[currentVariation].SetActive(true);

    }


}
