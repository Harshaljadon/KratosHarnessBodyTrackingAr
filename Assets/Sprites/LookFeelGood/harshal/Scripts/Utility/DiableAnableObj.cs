using System.Collections.Generic;
using UnityEngine;

public class DiableAnableObj 
{
    public void AnableObj( List<GameObject> anableGameobject)
    {
        byte length = (byte)anableGameobject.Count;
        for (int i = 0; i < length; i++)
        {
            anableGameobject[i].SetActive(true);
        }
    }

    public void DiableObj( List<GameObject> disableGameObject)
    {
        byte length = (byte)disableGameObject.Count;
        for (int i = 0; i < length; i++)
        {
            disableGameObject[i].SetActive(false);
        }
    }
}
