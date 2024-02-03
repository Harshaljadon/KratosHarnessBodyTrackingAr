using UnityEngine;

public class MeshDeformation : MonoBehaviour
{
    public GameObject targetObject;

    public float radius;

    void Update()
    {
        Mesh targetMesh = targetObject.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = targetMesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 localPos = targetObject.transform.InverseTransformPoint(vertices[i]);
            Vector3 direction = (localPos - targetObject.transform.position).normalized;
             radius = 1.0f; // Adjust this value to control the wrapping effect
            Vector3 offset = direction * radius;

            vertices[i] = targetObject.transform.position + offset;
        }

        targetMesh.vertices = vertices;
        targetMesh.RecalculateBounds();
        targetMesh.RecalculateNormals();
    }


}
