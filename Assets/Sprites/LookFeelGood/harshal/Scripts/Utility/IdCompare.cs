using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AR2;

public class IdCompare 
{
    int idRecive;
    public byte caseMatch;
    IdHolder idCollection;
    GameObject containAssembly;
    public IdCompare( IdHolder _idholder, GameObject container)
    {
        containAssembly = container;
        this.idCollection = _idholder;
    }

    public bool IdComparation(int id)
    {
        byte childCount = (byte)containAssembly.transform.childCount;
        idRecive = id;
        byte length = (byte)idCollection.data.Length;
        if (childCount == 0)
        {
            //base not install
            for (int i = 0; i < length; i++)
            {
                if (idRecive == idCollection.data[i].id)
                {
                    caseMatch = (byte)i;
                    return true;
                }
            }
        }
        else
        {
            // base installed but bracket not install
            //ConfinedPod cp = containAssembly.GetComponentInChildren<ConfinedPod>();
            //byte size = (byte)cp.brackets[0].childCount;
            //if (size == (byte)0)
            //{
            //    length -= 1;
            //    for (int i = 1; i < length; i++)
            //    {
            //        if (idRecive == idCollection.data[i].id)
            //        {
            //            caseMatch = (byte)i;
            //            return true;
            //        }
            //    }

            //}

        }

        return false;
    }
}
