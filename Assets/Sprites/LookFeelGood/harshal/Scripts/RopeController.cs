using UnityEngine;
using System.Collections.Generic;

public class RopeController : MonoBehaviour
{


    public Transform startPoint;
    public Transform endPoint;
    public GameObject ropeSegmentPrefab;
    public int segmentCount = 10;
    public float segmentLength = 1f;
    public float springStrength = 100f;
    public float dampingRatio = 1f;

    private LineRenderer lineRenderer;
    private GameObject[] ropeSegments;
    private SpringJoint[] springJoints;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        ropeSegments = new GameObject[segmentCount];
        springJoints = new SpringJoint[segmentCount];

        // Create the rope segments
        for (int i = 0; i < segmentCount; i++)
        {
            ropeSegments[i] = Instantiate(ropeSegmentPrefab, transform);
            ropeSegments[i].transform.localPosition = new Vector3(0f, -i * segmentLength, 0f);

            if (i > 0)
            {
                // Connect the rope segments with spring joints
                springJoints[i] = ropeSegments[i].AddComponent<SpringJoint>();
                springJoints[i].connectedBody = ropeSegments[i - 1].GetComponent<Rigidbody>();
                springJoints[i].spring = springStrength;
                springJoints[i].damper = dampingRatio;
                springJoints[i].autoConfigureConnectedAnchor = false;
                springJoints[i].connectedAnchor = new Vector3(0f, segmentLength, 0f);
            }
        }

        // Set the start and end points
        ropeSegments[0].transform.position = startPoint.position;
        ropeSegments[segmentCount - 1].transform.position = endPoint.position;
    }



    void Update()
    {
        // Calculate the desired rope length
        float desiredLength = Vector3.Distance(startPoint.position, endPoint.position);

        // Compare with the current rope length
        float currentLength = segmentCount * segmentLength;

        if (desiredLength > currentLength)
        {
            // Add segments
            int segmentsToAdd = Mathf.CeilToInt((desiredLength - currentLength) / segmentLength);
            AddSegments(segmentsToAdd);
        }
        else if (desiredLength < currentLength)
        {
            // Remove segments
            int segmentsToRemove = Mathf.CeilToInt((currentLength - desiredLength) / segmentLength);
            RemoveSegments(segmentsToRemove);
        }
    }

    void AddSegments(int count)
    {
        int startIndex = segmentCount;

        // Increase the array sizes
        System.Array.Resize(ref ropeSegments, segmentCount + count);
        System.Array.Resize(ref springJoints, segmentCount + count);

        // Create and connect the new segments
        for (int i = startIndex; i < segmentCount + count; i++)
        {
            ropeSegments[i] = Instantiate(ropeSegmentPrefab, transform);
            ropeSegments[i].transform.localPosition = new Vector3(0f, -i * segmentLength, 0f);

            // Connect the new segment with the previous one
            springJoints[i] = ropeSegments[i].AddComponent<SpringJoint>();
            springJoints[i].connectedBody = ropeSegments[i - 1].GetComponent<Rigidbody>();
            springJoints[i].spring = springStrength;
            springJoints[i].damper = dampingRatio;
            springJoints[i].autoConfigureConnectedAnchor = false;
            springJoints[i].connectedAnchor = new Vector3(0f, segmentLength, 0f);
        }

        // Update the segment count
        segmentCount += count;
    }
    void RemoveSegments(int count)
    {
        // Ensure that the count does not exceed the segment count
        count = Mathf.Min(count, segmentCount - 1);

        // Disconnect and destroy the segments
        for (int i = segmentCount - 1; i > segmentCount - count - 1; i--)
        {
            Destroy(ropeSegments[i]);
            Destroy(springJoints[i]);
        }

        // Decrease the array sizes
        System.Array.Resize(ref ropeSegments, segmentCount - count);
        System.Array.Resize(ref springJoints, segmentCount - count);

        // Update the segment count
        segmentCount -= count;
    }

}


