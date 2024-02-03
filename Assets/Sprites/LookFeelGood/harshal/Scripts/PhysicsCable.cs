using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SpringJoint))]
public class PhysicsCable : MonoBehaviour
{


    public Transform startPoint;
    public Transform endPoint;
    public int segments = 10;
    public float stiffness = 100f;
    public float damping = 10f;
    public float lineWidth = 0.1f;
    public float segmentLength = 0.1f; // Length of each line segment

    private LineRenderer lineRenderer;
    private SpringJoint springJoint;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint>();
    }

    private void Start()
    {
        float totalLength = Vector3.Distance(startPoint.position, endPoint.position);
        int segmentCount = Mathf.CeilToInt(totalLength / segmentLength);
        lineRenderer.positionCount = segmentCount + 1; // Add 1 to account for the start point
        // Set up line renderer properties
        //lineRenderer.positionCount = segments + 1;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Set up spring joint properties
        springJoint.connectedBody = startPoint.GetComponent<Rigidbody>();
        springJoint.anchor = Vector3.zero;
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = Vector3.zero;
        springJoint.spring = stiffness;
        springJoint.damper = damping;
        springJoint.enableCollision = true;
    }

    private void Update()
    {
        float totalLength = Vector3.Distance(startPoint.position, endPoint.position);
        int segmentCount = Mathf.CeilToInt(totalLength / segmentLength);

        lineRenderer.positionCount = segmentCount + 1; // Add 1 to account for the start point

        // Update line renderer positions
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(segments, endPoint.position);

        // Update spring joint anchor positions
        springJoint.anchor = transform.InverseTransformPoint(startPoint.position);
        springJoint.connectedAnchor = transform.InverseTransformPoint(endPoint.position);
    }

}
