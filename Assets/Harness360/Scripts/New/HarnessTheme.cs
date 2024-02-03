using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct MaterialInfo
{
    public string name;
    public Material material;
    public Sprite sprite;
}

[Serializable]
public struct ColorVariationInfo
{
    public Element.ElementType Type;
    public Sprite icon;
    public List<MaterialInfo> AvaliableMatrials;
}

public class HarnessTheme : MonoBehaviour
{
    [SerializeField] public List<ColorVariationInfo> allVariations = new List<ColorVariationInfo>();


    public int GetAvaliableColorCount(Element.ElementType elementType)
    {
        int count = 0;
        
        foreach (var variation in allVariations)
        {
            if (variation.Type == elementType)
            {
                count = variation.AvaliableMatrials.Count;
            }
        }

        return count;
    }


    public Sprite GetSpriteForColor(Element.ElementType elementType, int i)
    {
        Sprite sp = null;
        
        foreach (var variation in allVariations)
        {
            if (variation.Type == elementType)
            {
                sp = variation.AvaliableMatrials[i].sprite;
                //sp = variation.icon;
            }
        }

        return sp;
    }


    //public Sprite GetIcon(Element.ElementType elementType, int i)
    //{
    //    Sprite sp = null;

    //    foreach (var variation in allVariations)
    //    {
    //        if (variation.Type == elementType)
    //        {
    //            sp = variation.icon;
    //        }
    //    }

    //    return sp;
    //}


    public string GetNameForElemnt(Element.ElementType elementType, int i)
    {
        string sp = null;

        foreach (var variation in allVariations)
        {
            if (variation.Type == elementType)
            {
                sp = variation.AvaliableMatrials[i].name;
            }
        }

        return sp;
    }

    public Material GetMaterialForElementAtIndex(Element.ElementType elementType, int index)
    {
        Material mat = null;
        
        foreach (var variation in allVariations)
        {
            if (variation.Type == elementType)
            {
                mat = variation.AvaliableMatrials[index].material;
            }
        }

        return mat;
    }

    public Sprite GetVariationIcon(Element.ElementType eeT)
    {
        Sprite sp = null;

        foreach (var variation in allVariations)
        {
            if (variation.Type == eeT)
            {
                sp = variation.icon;
            }
        }

        return sp;
    }

    public string GetallVariationstypeName(int index)
    {
        var nameEnum = allVariations[index].Type.ToString();
        return nameEnum;
    }

    public Sprite GetAllVariationTypeIcon(int index)
    {
        var icon = allVariations[index].icon;
        return icon;
    }

    public Element.ElementType GetAllVariationTypeEnum(int index)
    {
        var enumMum = allVariations[index].Type;
        return enumMum;
}
}
