using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform cameraTransform;
    public float distance = 5.0f; // The distance in front of the camera.
    public Vector3 offset = Vector3.zero; // The offset from the camera's forward direction.

    SelfRegester selfRegester;

    public GameObject[] rendererCircle;
    public float _distance;
    public Vector3 roboPos;
    Harness harness;
    private void Start()
    {
        selfRegester = FindObjectOfType<SelfRegester>();
    }

    void Update()
    {
        if (cameraTransform == null)
        {
            // Find the main camera if not assigned.
            cameraTransform = Camera.main.transform;
        }

        SetPosRing();

        if (selfRegester.model != null)
        {
            if (harness == null)
            {
                harness = FindObjectOfType<Harness>();
            }
            roboPos = new Vector3(harness.GetComponent<Transform>().position.x, 0, 0);
            var vectorA = new Vector3(this.transform.position.x, 0, 0);
            //var vectorB = new Vector3(selfRegester.GetComponent<Transform>().position.x, 0, 0);
            //_distance = Vector3.Distance(vectorA,vectorB);
            _distance = Vector3.Distance(vectorA, roboPos);
            //Debug.Log(vectorA + " " + roboPos);
            if (_distance < .1f)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (rendererCircle[i].activeInHierarchy)
                    {
                    rendererCircle[i].SetActive(false) ;

                    }

                }
                //foreach (var item in rendererCircle)
                //{
                //}
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    if (!rendererCircle[i].activeInHierarchy)
                    {
                        rendererCircle[i].SetActive(true) ;
                        Debug.Log("FAR");

                    }
                }
                //foreach (var item in rendererCircle)
                //{
                //    item.enabled = true;
                //}
            }
        }

    }

    void SetPosRing()
    {
        // Calculate the target position in front of the camera.
        Vector3 targetPosition = cameraTransform.position + (cameraTransform.forward * distance) + offset;

        // Move the GameObject to the target position.
        transform.position = targetPosition;
        // Calculate the rotation angle around the Y-axis based on the camera's rotation.
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up));

        // Smoothly interpolate the rotation.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1);
    }
}
