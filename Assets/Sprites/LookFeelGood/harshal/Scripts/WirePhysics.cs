using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePhysics : MonoBehaviour
{
    public Transform startPoint; // Starting point of the wire
    public Transform endPoint; // Ending point of the wire
    public int segments = 10; // Number of segments to divide the wire into
    public float segmentLength = 1f; // Length of each segment
    public float springForce = 10f; // Force applied to maintain segment length
    public float damping = 0.5f; // Damping factor for the spring force
    private LineRenderer lineRenderer; // Reference to the Line Renderer component
    private GameObject[] segmentsObjects; // Array to store the segment game objects
    private SpringJoint[] segmentJoints; // Array to store the Spring Joint components

    void Start()
    {
        var totalLength = Vector3.Distance(startPoint.position, endPoint.position);
        segments = Mathf.CeilToInt(totalLength / segmentLength);
        lineRenderer = GetComponent<LineRenderer>();


        lineRenderer.positionCount = segments + 1;
        segmentsObjects = new GameObject[segments];
        segmentJoints = new SpringJoint[segments];

        for (int i = 0; i < segments; i++)
        {
            segmentsObjects[i] = new GameObject("Segment_" + i);
            segmentsObjects[i].transform.position = Vector3.Lerp(startPoint.position, endPoint.position, (float)i / segments);
            segmentsObjects[i].transform.SetParent(transform);

            segmentJoints[i] = segmentsObjects[i].AddComponent<SpringJoint>();
            segmentJoints[i].connectedBody = i == 0 ? startPoint.GetComponent<Rigidbody>() : segmentsObjects[i - 1].GetComponent<Rigidbody>();
            segmentJoints[i].spring = springForce;
            segmentJoints[i].damper = damping;
            segmentJoints[i].autoConfigureConnectedAnchor = false;
            segmentJoints[i].connectedAnchor = Vector3.up * segmentLength;
        }
    }

    void Update()
    {
        float totalLength = Vector3.Distance(startPoint.position, endPoint.position);

        int segmentCount = Mathf.CeilToInt(totalLength / segmentLength);

        lineRenderer.positionCount = segmentCount + 1;
        for (int i = 0; i <= segments; i++)
        {
            lineRenderer.SetPosition(i, segmentsObjects[i < segments ? i : segments - 1].transform.position);
        }
    }

}
