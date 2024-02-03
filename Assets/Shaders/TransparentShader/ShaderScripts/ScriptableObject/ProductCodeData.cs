using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductCodeData", menuName = "ScriptableObjects/ProductCodeData")]
public class ProductCodeData : ScriptableObject
{


    [Serializable]
    public struct ProductCodeMapping
    {
        public string equipmentTypeName;
        public ProductType productType;
        public ProductFilter productFilter;
    }

    [Serializable]
    public struct ProductFilter
    {

        public ProductTypeCATAGORIES productTypeCATAGORIES;
        public string[] productCode;
    }
    //string productTypeString;
    //string productSubTypeString;

    public List<ProductCodeMapping> mappings = new List<ProductCodeMapping>();
    public WrokStationDetail wrokStationDetail;
    public bool GetEquipmentType(string productCodePassed, out string productTypeString, out string productSubTypeString)
    {
        productTypeString = string.Empty;
        productSubTypeString = string.Empty;
        foreach (var mapping in mappings)
        {
            foreach (var item in mapping.productFilter.productCode)
            {
                if (item == productCodePassed)
                {
                    productTypeString = mapping.productType.ToString();
                    productSubTypeString = mapping.productFilter.productTypeCATAGORIES.ToString();
                    return true;

                }
            }

        }
        return false; // Handle the case where the product code is not found.
    }

    [Serializable]
    public struct WrokStationDetail
    {
        public string currentUrl;
        public Sprite[] icons;
        public HarnessWorkStationCatagory[] harnessWorkStationCatagory;

    }

    [System.Serializable]
    public struct HarnessSizeCodename
    {
        public string[] sizeCodeName;
 

    }

    [Serializable]
    public struct HarnessWorkStationCatagory
    {
        public string HarnessNameCode;
        public string HarnessWrokDes;
        public ProductUrl[] productUrls;
        public WorkStationCatagory[] workStationCatagories;
        public HarnessSizeCodename harnessSizeCodename;

    }

    [Serializable]
    public enum ProductType
    {
        FACE, BODY, HAND, LEG, EQUIPMENT
    }

    [Serializable]
    public enum ProductTypeCATAGORIES
    {
        EYEGLASS, HELMET, MASK, HARNESS, GLOVES, SHOE, CONFIND_SPACE, VERTICAL, OVER_ROOF, OVER_HEAD
    }

    [Serializable]
    public enum WorkStationCatagory
    {
        Cherry_Picker, Plan_Incline, Work_Station_Holding, Fixed_Ladder, Confined_Space,
        Double_Rope_Decent, Work_Station_Prevention, Rescue_Evacuation, Self_Evacuation,
        Lattice_Mettalic_Structure,Horizontal_Plane, Scaffolding, Door_Framework, KratosLogo
    }

    [Serializable]
    public struct ProductUrl
    {
        public String codeNumber;
        public String productUrl;
    }
}
