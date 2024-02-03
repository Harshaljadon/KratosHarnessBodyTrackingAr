using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{

    public enum ElementType
    {
    //IdPlate,
    Webbing,
    Buckle,
    Stitching,
    ChestStrap,
    SitStrap,
    ShoulderStrap,
    ThighStrap,
    PositioningBelt,
    DRing,
    Loops,
    None
    };
    
    public ElementType item = ElementType.Buckle;
    public List<Material> usableMaterials = new List<Material>();
    public List<Sprite> materialSprites = new List<Sprite>();
    
    
    /// <summary>
    /// -1 means no active item
    /// otherwise represents current active item
    /// </summary>
    private int currentVariation = -1;

    private int currentMaterial = 0;
    
    List<GameObject> variations = new List<GameObject>();
    
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            variations.Add(transform.GetChild(i).gameObject);
            //variations[i].SetActive(false);
        }
        //SetVariation(0);
    }

    public int GetVariationCount()
    {
        return variations.Count;
    }
    
    public void SetVariation(int nextVariation)
    {
        // nothing to change
        if (currentVariation == nextVariation)
        {
            return;
        }

        // hide current
        if (nextVariation == -1)
        {
            variations[currentVariation].SetActive(false);
            currentVariation = nextVariation;
        }
        else
        {
            if (currentVariation == -1)
            {
                currentVariation = nextVariation;
                variations[currentVariation].SetActive(true);
            }
            else
            {
                variations[currentVariation].SetActive(false);
                currentVariation = nextVariation;
                variations[currentVariation].SetActive(true);

            }
        }
        
        SetMaterial(currentMaterial);
    }

    public void SetMaterial(int nextMat)
    {
        currentMaterial = nextMat;
        if (currentVariation != -1)
        {
            variations[currentVariation].GetComponent<MeshRenderer>().material = usableMaterials[currentMaterial];
        }
    }


    public static string GetElementName(ElementType nameof)
    {
        string name = "";

        switch (nameof)
        {
            // case ElementType.IdPlate:
            //     name = "ID Plate";
            //     break;
            case ElementType.Webbing:
                name = "Webbing";
                break;
            case ElementType.Buckle:
                name = "Buckle";
                break;
            // case ElementType.Stitching:
            //     name = "Stitching";
            //     break;
            // case ElementType.SitStrap:
            //     name = "sit Strap";
            //     break;
            case ElementType.ChestStrap:
                name = "Stiches";
                break;
            // case ElementType.ShoulderStrap:
            //     name = "Shoulder Strap";
            //     break;
            case ElementType.ThighStrap:
                name = "Straps";
                break;
            case ElementType.PositioningBelt:
                name = "Positioning Belt";
                break;
            case ElementType.DRing:
                name = "D-Ring";
                break;
            // case ElementType.Loops:
            //     name = "Loops";
            //     break;
            case ElementType.None:
                name = "";
                break;
            default:
                break;
        }

        return name;
    }

    public ElementType GetElementType()
    {
        return item;
    }

    public Sprite GetVariationSprite(int index)
    {
        return variations[index].GetComponent<Variation>().variationIcon;
    }
    
    public int GetMaterialCount()
    {
        return usableMaterials.Count;
    }

    public Sprite GetColorSprite(int index)
    {
        return materialSprites[index];
    }
}
