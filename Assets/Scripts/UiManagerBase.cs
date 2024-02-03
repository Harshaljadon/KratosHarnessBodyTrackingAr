using System;
using UnityEngine;

namespace AR2
{
    public class UiManagerBase : MonoBehaviour
    {

        public GameObject InfoText;
        // public Color32 selectedColor;

        public virtual void ShowUI()
        {

        }

        public virtual void HideUI(bool addInfotext)
        {
            InfoText.SetActive(addInfotext);
        }
    }

}
