using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class HumanBodyTracker : MonoBehaviour
    {

        [SerializeField]
        GameObject comeNearPanel;
        [SerializeField]
        GameObject goAwayPanel;
        [SerializeField]
        [Tooltip("The Skeleton prefab to be controlled.")]
        List<GameObject> m_SkeletonPrefab;

        [SerializeField]
        [Tooltip("Harness Index Selection")]
        int m_Index;
        [SerializeField]
        [Tooltip("The ARHumanBodyManager which will produce body tracking events.")]
        ARHumanBodyManager m_HumanBodyManager;

        [SerializeField]
        [Tooltip("DIstance between harness user and camera")] 
        float m_distanceBetweenHarnessAndCamera;

        [SerializeField]
        [Tooltip("Set REQUIRED DISTANCE")]
        float distanceGreater;

        //[SerializeField]
        //[Tooltip("Set REQUIRED DISTANCE")]
        //float distanceLesser;

        public bool bodyRemoved;

        [SerializeField]
        [Tooltip("Bool To Restart session")]
        bool restartSession = false;

        public bool RestartSession
        {
            get { return restartSession; }
            set { restartSession = value; }
        }

        //public bool debugFirstAdded, debugUpdated;
        /// <summary>
        /// distanceBetweenHarnessAndCamera
        /// </summary>
        public float DistanceBetweenHarnessAndCamera
        {
            get { return m_distanceBetweenHarnessAndCamera; }
            set { m_distanceBetweenHarnessAndCamera = value; }
        }

        /// <summary>
        /// Get/Set the <c>ARHumanBodyManager</c>.
        /// </summary>
        public ARHumanBodyManager humanBodyManager
        {
            get { return m_HumanBodyManager; }
            set { m_HumanBodyManager = value; }
        }
        /// <summary>
        /// Get/Set selected harness index from harness managr collection
        /// </summary>
        public int harnessIndex
        {
            get { return m_Index; }
            set { m_Index = value; }
        }

        /// <summary>
        /// set harness collection
        /// </summary>
        public List<GameObject> SkeletonPrefebCollection
        {
            set { m_SkeletonPrefab = value; }
        }

        /// <summary>
        /// Get/Set the skeleton prefab.
        /// </summary>
        public GameObject skeletonPrefab
        {
            get { return m_SkeletonPrefab[m_Index]; }
            set { m_SkeletonPrefab[m_Index] = value; }
        }

        Dictionary<TrackableId, BoneController> m_SkeletonTracker = new Dictionary<TrackableId, BoneController>();

        void OnEnable()
        {
            Debug.Assert(m_HumanBodyManager != null, "Human body manager is required.");
            m_HumanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
            restartSession = false;
        }

        void OnDisable()
        {
            restartSession = true;
            if (m_HumanBodyManager != null)
                m_HumanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
            ResetingTracablesObject();
        }

            BoneController boneController;
        ARHumanBody lastHumanBodyTracked;
        //GameObject newSkeletonGO;
        void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
        {

            if (m_SkeletonPrefab == null)
                return;



            foreach (var humanBody in eventArgs.added)
            {
                // Calculate the distance between the camera and the detected body
                m_distanceBetweenHarnessAndCamera = Vector3.Distance(Camera.main.transform.position, humanBody.transform.position);
                //// Check if the distance is within the detection range  2.9 > 3  < 3.1

                if (distanceGreater >= m_distanceBetweenHarnessAndCamera)
                {
                    // ui text get away
                    //ComeNear();
                    //newSkeletonGO.SetActive(false);
                    return;
                }

                //if (distanceLesser <= m_distanceBetweenHarnessAndCamera)
                //{
                //    //GoAway();
                //    //newSkeletonGO.SetActive(false);
                //    // get near
                //    return;
                //}

                if (!m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    var newSkeletonGO = Instantiate(m_SkeletonPrefab[m_Index], humanBody.transform);

                    //debugFirstAdded = true;
                    //RightPosition();
                    //newSkeletonGO.SetActive(true);

                    Debug.Log($"Adding a new skeleton [{humanBody.trackableId}].");
                    boneController = newSkeletonGO.GetComponent<BoneController>();
                    m_SkeletonTracker.Add(humanBody.trackableId, boneController);  
                }

                boneController.InitializeSkeletonJoints();
                boneController.ApplyBodyPose(humanBody);
            }

            foreach (var humanBody in eventArgs.updated)
            {
                m_distanceBetweenHarnessAndCamera = Vector3.Distance(Camera.main.transform.position, humanBody.transform.position);
                //// Check if the distance is within the detection range  2.9 > 3  < 3.1
                if (distanceGreater >= m_distanceBetweenHarnessAndCamera)
                {
                    //ComeNear();
                    //newSkeletonGO.SetActive(false);
                    // ui text get away
                    return;
                }

                //if (distanceLesser <= m_distanceBetweenHarnessAndCamera)
                //{
                //    //GoAway();
                //    //newSkeletonGO.SetActive(false);
                //    // get near
                //    return;
                //}
                if (!m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    var newSkeletonGO = Instantiate(m_SkeletonPrefab[m_Index], humanBody.transform);

                    //RightPosition();
                    newSkeletonGO.SetActive(true);

                    //debugUpdated = true;
                    Debug.Log($"Adding a new skeleton [{humanBody.trackableId}].");
                    boneController = newSkeletonGO.GetComponent<BoneController>();
                    m_SkeletonTracker.Add(humanBody.trackableId, boneController);
                    boneController.InitializeSkeletonJoints();
                }

                if (m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {

                    //RightPosition();
                    //newSkeletonGO.SetActive(true);
                    lastHumanBodyTracked = humanBody;
                    boneController.ApplyBodyPose(humanBody);
                }
            }

            foreach (var humanBody in eventArgs.removed)
            {
                    bodyRemoved = true;
                //RightPosition();
                //Debug.Log($"Removing a skeleton [{humanBody.trackableId}].");
                if (m_SkeletonTracker.TryGetValue(humanBody.trackableId, out boneController))
                {
                    Destroy(boneController.gameObject);
                    m_SkeletonTracker.Remove(humanBody.trackableId);
                }
            }


        }

        void ComeNear()
        {
            comeNearPanel.SetActive(false);
            goAwayPanel.SetActive(true);
        }

        void GoAway()
        {
            comeNearPanel.SetActive(true);
            goAwayPanel.SetActive(false);
        }

        void RightPosition()
        {
            comeNearPanel.SetActive(false);
            goAwayPanel.SetActive(false);
        }


        void ResetingTracablesObject()
        {
            foreach (var item in m_SkeletonTracker)
            {
                if (item.Value != null)
                {
                Destroy(item.Value.gameObject);
                }
            }
            if (lastHumanBodyTracked != null)
            {

            m_SkeletonTracker.Remove(lastHumanBodyTracked.trackableId);
            }
            m_SkeletonTracker.Clear();


            //foreach (var item in humanBodyManager.trackables)
            //{
            //    item.gameObject.SetActive(true);
            //}
        }

    }
}