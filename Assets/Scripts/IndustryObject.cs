using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AR2
{
    public class IndustryObject : MonoBehaviour
    {
        public List<GameObject> objectsToHideInCameraShoot = new List<GameObject>();
        public List<GameObject> objectsToHideInAR = new List<GameObject>();

        public void ShowObjects()
        {
            foreach (var item in objectsToHideInCameraShoot)
            {
                item.SetActive(true);
            }
        }

        public void HideObjects()
        {
            foreach (var item in objectsToHideInAR)
            {
                item.SetActive(false);
            }

        }

        internal void HideAllObjects()
        {
            foreach (var item in objectsToHideInCameraShoot)
            {
                item.SetActive(false);
            }
        }
        
        public void OnArActive()
        {
            // var quaternion = new Quaternion {eulerAngles = arRotation};
            // industryRoot.rotation = quaternion;
            //
            //
            // if (rotateImage)
            // {
            //     var planeRotation = new Quaternion {eulerAngles = (arRotation + new Vector3(270,0,0))};
            //     ImagePlane.transform.rotation = planeRotation;
            // }
            
        }

        public void OnArDeactivate()
        {
            // var quaternion = new Quaternion {eulerAngles = nonARRotation};
            // industryRoot.rotation = quaternion;

            //
            // if (rotateImage)
            // {
            //     var planeRotation = new Quaternion {eulerAngles = (nonARRotation + new Vector3(270,0,0))};
            //     ImagePlane.transform.rotation = planeRotation;
            // }
            //
            // ImagePlane.SetActive(false);
            //
            // var loadedTexture = ImagePlane.GetComponent<MeshRenderer>().material.mainTexture;
            //
            // if (loadedTexture)
            // {
            //     Destroy(loadedTexture);
            // }
            
        }
        
    }
}
