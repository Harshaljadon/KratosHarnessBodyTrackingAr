using UnityEngine;

public class FollowWithOffset : MonoBehaviour
{
    public Transform target; // Reference to the target game object to follow
    public Vector3  desiredPosition; // Offset from the target game object
    public float followRange = 10f, distance, speed; // Range within which to start following the target
    private bool isFollowing = false; // Flag to indicate if the script is currently following the target


    private void LateUpdate()
    {
        if (target != null)
        {
             distance = Vector3.Distance(transform.position, target.position);
            if (distance > followRange && !isFollowing)
            {
                // Start following the target
                isFollowing = true;
            }
            else if (distance <= followRange && isFollowing)
            {
                // Stop following the target
                isFollowing = false;
            }

            if (isFollowing)
            {
                // Calculate the desired position with the offset
                 desiredPosition = new Vector3(target.position.x, transform.position.y, target.position.z); //+ offset

                // Smoothly move towards the desired position
                //transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
                //var a  = Mathf.Lerp(transform.position.z, desiredPosition.z, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime* speed);
                //transform.position = desiredPosition;

                //transform.position = new Vector3(transform.position.x, transform.position.y, a);
                //transform.rotation = target.rotation;
            }
           


        }
    }
}
