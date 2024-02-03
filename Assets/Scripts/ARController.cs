using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


namespace AR2
{
    public class ARController : MonoBehaviour
    {
        public GameObject arSessionOrigin;
        public GameObject arCamera;
        private ARSession session;
        public GameObject sessionPrefab;
        private ARCameraBackground arCameraBackground;
        
        // these empty transform stores position and rotaion for diffrent modes
        public Transform arPos;
        public Transform nonArPos;


        private void Awake()
        {

            arCameraBackground = arCamera.GetComponent<ARCameraBackground>();
            //arCamera.transform.position = arPos.position;
            //arCamera.transform.rotation = arPos.rotation;
        }

        private void Start()
        {
            arCameraBackground.enabled = false;
            arCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        }

        public void SetArLocation()
        {
            arSessionOrigin.transform.position = arPos.position;
            arSessionOrigin.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        public void SetNonArLocation()
        {
            arSessionOrigin.transform.position = nonArPos.position;
            arSessionOrigin.transform.rotation = nonArPos.rotation;
        }
        
        public void SetArMode(bool isAROn)
        {
            ChangeARSession(isAROn);
        }

        private void ChangeARSession(bool newState)
        {
            if (newState)
            {
                StartSession();
            }
            else
            {
                StopSession();
            }
        }

        public void StartSession()
        {
            //SceneUI.HideUI();

            arSessionOrigin.transform.position = arPos.position;
            arSessionOrigin.transform.rotation = Quaternion.Euler(Vector3.zero);

            arCameraBackground.enabled = true;
            arCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;

            // arCamera.transform.position = Vector3.zero;
            // arCamera.transform.rotation = Quaternion.Euler(Vector3.zero);

            StartCoroutine(DoStart());

        }

        public void StopSession()
        {
            Destroy(session.gameObject);

            arSessionOrigin.transform.position = nonArPos.position;
            arSessionOrigin.transform.rotation = nonArPos.rotation;

            ////arCamera.transform.position = nonArPos.position;
            ////arCamera.transform.rotation = nonArPos.rotation;

            arCamera.transform.localPosition = Vector3.zero;
            arCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
            
            arCameraBackground.enabled = false;
            arCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;

            //SceneUI.ShowUI();
        }

        IEnumerator DoStart()
        {
            if (session)
            {
                Destroy(session.gameObject);
                yield return null;
            }
            else
            {
                yield return null;
            }

            if (sessionPrefab != null)
            {
                session = Instantiate(sessionPrefab).GetComponent<ARSession>();
            }
        }
    }
}
