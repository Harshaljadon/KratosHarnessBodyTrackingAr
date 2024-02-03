using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;

public class ObjectFollower : MonoBehaviour
{
    public Transform target; // The target object to follow
    public bool followLocalScale, followRotattion = true, rotated;
    public Vector3 offset, offsetRotation, rotVectAngle ;
    public Quaternion angleRotQuat;
    public BoneController boneController;

    [SerializeField]
    [Range(130,240)]
    float rangeA;
    [SerializeField]
    [Range(130, 240)]
    float rangeB;

    [SerializeField]
    [Range(-15, 1)]
    float spine5Range;

    //private void Start()
    //{
    //    boneController = FindObjectOfType<BoneController>();
    //}

    private void Update()
    {
        // Check if the target is assigned
        if (target != null)
        {
            // Create a Quaternion representing the desired rotation offset
            Quaternion rotationOffset = Quaternion.Euler(offsetRotation);
            // Update the position and rotation of the object to match the target
            transform.position = target.position + offset;
            if (followRotattion)
            {
            transform.rotation = target.rotation * rotationOffset;
                angleRotQuat = transform.rotation;
                rotVectAngle = transform.rotation.eulerAngles;
            }
            if (followLocalScale)
            {

            transform.localScale = target.localScale;
            }

            if (boneController != null)
            {
                // negetive value
                if (rotVectAngle.y > 130f && rotVectAngle.y < 220f) //-.99 to -.8f,  130, 220
                {
                    
                    boneController.Spine5AdjustZaxisRotation = -15f;
                    boneController.Spine6AdjustZaxisRotation = -7.5f;
                    rotated = true;
                }
                else
                {
                    boneController.Spine5AdjustZaxisRotation = 0f;
                    boneController.Spine6AdjustZaxisRotation = 0f;
                    rotated = false;
                    // positive value

                }
            }
        }
    }
}
