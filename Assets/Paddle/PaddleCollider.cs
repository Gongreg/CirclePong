using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleCollider : MonoBehaviour
{
    [SerializeField, Range(0, 360)]
    private int length = 5;


    [SerializeField, Range(0, 10)]
    int divisionsPer1Length = 2;

    private Vector3[] vertices;

    private float previousDistance;

    [SerializeField, Range(0, 5f)]
    private float width = 1f;

    private void OnValidate()
    {
        GenerateMesh();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }

    }

    private int[] GenerateTriangles()
    {
        int boxCount = length * (divisionsPer1Length + 1);
        int[] triangles = new int[boxCount * 6];

        for (int triangleIndex = 0, box = 0, verticeIndex = 0; box < boxCount; box++, verticeIndex += 2)
        {
            triangles[triangleIndex++] = verticeIndex;
            triangles[triangleIndex++] = verticeIndex + 1;
            triangles[triangleIndex++] = verticeIndex + 2;
            triangles[triangleIndex++] = verticeIndex + 2;
            triangles[triangleIndex++] = verticeIndex + 1;
            triangles[triangleIndex++] = verticeIndex + 3;
        }

        return triangles;
    }

    private void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "PaddleColliderMesh";
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3 center = transform.localPosition;

        float halfWidth = width / 2f;
        float halfLenght = length / 2f;
        float divisionLength = 1 / (float)(divisionsPer1Length + 1);

        float circleRadius = center.y;

        int verticesForParts = length * 2 + 2;
        int verticesForDivisions = divisionsPer1Length * 2 * length;

        vertices = new Vector3[verticesForParts + verticesForDivisions];

        int index = 0;
        float partPosition = center.x - halfLenght;
        vertices[index++] = new Vector3(partPosition, center.y - halfWidth, center.z);
        vertices[index++] = new Vector3(partPosition, center.y + halfWidth, center.z);

        for (int part = 0; part < length; part++)
        {
            float firstPositionX = partPosition + part;

            for (int division = 1; division <= divisionsPer1Length; division++)
            {
                float divisionPositionX = firstPositionX + divisionLength * division;
                vertices[index++] = new Vector3(divisionPositionX, center.y - halfWidth, center.z);
                vertices[index++] = new Vector3(divisionPositionX, center.y + halfWidth, center.z);
            }

            float lastPositionX = firstPositionX + 1;

            vertices[index++] = new Vector3(lastPositionX, center.y - halfWidth, center.z);
            vertices[index++] = new Vector3(lastPositionX, center.y + halfWidth, center.z);

        }

        mesh.vertices = vertices;
        mesh.triangles = GenerateTriangles();
    }

    // Start is called before the first frame update
    void Awake()
    {
        previousDistance = transform.localPosition.y;

        GenerateMesh();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
