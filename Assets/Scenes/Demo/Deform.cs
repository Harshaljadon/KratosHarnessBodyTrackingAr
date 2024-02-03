using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deform : MonoBehaviour
{
    public GameObject targetObject; // The game object to wrap around

    private MeshFilter meshFilter;
    private Mesh deformedMesh;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        // Create a new mesh to store the deformed vertices and triangles
        deformedMesh = new Mesh();
        meshFilter.mesh = deformedMesh;

        // Deform the mesh
        DeformMesh();
    }

    private void DeformMesh()
    {
        // Get the meshes of both game objects
        Mesh targetMesh = targetObject.GetComponent<MeshFilter>().sharedMesh;
        Mesh sourceMesh = meshFilter.sharedMesh;

        // Create arrays to store the new vertices and triangles
        Vector3[] newVertices = new Vector3[sourceMesh.vertices.Length];
        int[] newTriangles = new int[targetMesh.triangles.Length];

        // Generate new vertices based on the target object's mesh
        for (int i = 0; i < targetMesh.vertices.Length; i++)
        {
            if (i >= sourceMesh.vertices.Length)
            {
                // If the source mesh doesn't have enough vertices, duplicate the last vertex
                newVertices[i] = newVertices[i - 1];
            }
            else
            {
                // Calculate the position of the new vertex based on the target object's mesh
                Vector3 targetVertex = targetObject.transform.TransformPoint(targetMesh.vertices[i]);
                newVertices[i] = transform.InverseTransformPoint(targetVertex);
            }
        }

        // Generate new triangles based on the target object's mesh
        for (int i = 0; i < targetMesh.triangles.Length; i++)
        {
            if (i >= sourceMesh.triangles.Length)
            {
                // If the source mesh doesn't have enough triangles, duplicate the last triangle
                newTriangles[i] = newTriangles[i - 1];
            }
            else
            {
                // Calculate the index of the new triangle vertex based on the target object's mesh
                int targetVertexIndex = targetMesh.triangles[i];
                newTriangles[i] = targetVertexIndex;
            }
        }

        // Assign the new vertices and triangles to the deformed mesh
        deformedMesh.Clear();
        deformedMesh.vertices = newVertices;
        deformedMesh.triangles = newTriangles;
        deformedMesh.RecalculateNormals();
    }
}
