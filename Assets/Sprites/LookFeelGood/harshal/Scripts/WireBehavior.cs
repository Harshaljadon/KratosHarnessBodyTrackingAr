using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireBehavior : MonoBehaviour
{
    public Transform startPoint; // The starting point of the wire
    public Transform endPoint; // The end point of the wire
    public int initialSegments = 5; // Initial number of line segments
    public float segmentLength = 0.1f; // Length of each line segment

    private LineRenderer lineRenderer; // Reference to the Line Renderer component

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = initialSegments;

        // Set the initial positions of the line renderer
        UpdateLineRenderer();
    }


    void UpdateLineRenderer()
    {
        float totalLength = Vector3.Distance(startPoint.position, endPoint.position);
        int segmentCount = Mathf.CeilToInt(totalLength / segmentLength);

        lineRenderer.positionCount = segmentCount + 1; // Add 1 to account for the start point
        Vector3[] positions = new Vector3[segmentCount + 1];

        for (int i = 0; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            positions[i] = Vector3.Lerp(startPoint.position, endPoint.position, t);
        }

        lineRenderer.SetPositions(positions);
    }
    void Update()
    {
        UpdateLineRenderer();
    }

}
