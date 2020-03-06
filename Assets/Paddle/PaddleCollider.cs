using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PaddleCollider : MonoBehaviour
{
    [SerializeField, Range(0, 360f)]
    private float lengthInDegrees = 5;

    [SerializeField, Range(1, 360f)]
    int divisionPerNDegrees = 4;

    private Vector3[] vertices;

    private float previousDistance;

    [SerializeField, Range(0, 5f)]
    private float width = 1f;

    [SerializeField, Range(0, 10f)]
    private float radius = 3f;

    private void OnValidate()
    {
        GenerateMesh();
    }

    private void OnDrawGizmos()
    {
        //return;

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

    private int[] GenerateTriangles(int partCount)
    {
        int[] triangles = new int[partCount * 6];

        int triangleIndex = 0, verticeIndex = 0;
        for (int part = 0; part < partCount; part++)
        {
            triangles[triangleIndex] = verticeIndex;
            triangles[triangleIndex + 1] = verticeIndex + 1;
            triangles[triangleIndex + 2] = verticeIndex + 2;
            triangles[triangleIndex + 3] = verticeIndex + 2;
            triangles[triangleIndex + 4] = verticeIndex + 1;
            triangles[triangleIndex + 5] = verticeIndex + 3;

            verticeIndex += 2;
            triangleIndex += 6;
        }

        return triangles;
    }

    private int addVertices(int index, float rotation)
    {
        Vector3 normal = Quaternion.Euler(0, 0, rotation) * Vector3.up;

        Vector3 vertice = normal * radius;
        vertices[index] = vertice;
        vertices[index + 1] = normal * (radius + width);

        return index + 2;
    }

    private void GenerateMesh()
    {
        Assert.AreNotEqual(0, divisionPerNDegrees);

        Mesh mesh = new Mesh();
        mesh.name = "PaddleColliderMesh";
        GetComponent<MeshFilter>().mesh = mesh;

        float diameter = radius * 2;
        float circumference = diameter * Mathf.PI;

        float lengthOnCircumference = 360 * (lengthInDegrees / circumference);

        int partCount = 0;

        if (lengthInDegrees < divisionPerNDegrees)
        {
            partCount = 1;
        }
        else
        {
            //If there is a remainder left 
            int additionalPart = lengthInDegrees % divisionPerNDegrees > 0 ? 1 : 0;
            partCount = (int)Mathf.Floor(lengthInDegrees / divisionPerNDegrees) + additionalPart;
        }

        float partLength = lengthOnCircumference / (float)partCount;

        int verticeCount = partCount * 2 + 2;

        vertices = new Vector3[verticeCount];

        float parentRotation = transform.parent.eulerAngles.z;
        float leftRotation = parentRotation + lengthInDegrees / 2;
        float rightRotation = parentRotation - lengthInDegrees / 2;

        int index = 0;
        index = addVertices(index, leftRotation);
        index = addVertices(index, rightRotation);

        //float halfWidth = width / 2f;
        //float halfLenght = length / 2f;

        // int index = 0;
        // float partPosition = center.x - halfLenght;

        // vertices[index++] = new Vector3(partPosition, center.y - halfWidth, center.z);
        // vertices[index++] = new Vector3(partPosition, center.y + halfWidth, center.z);

        // for (int part = 0; part < length; part++)
        // {
        //     float firstPositionX = partPosition + part;

        //     for (int division = 1; division <= divisionsPer1Length; division++)
        //     {
        //         float divisionPositionX = firstPositionX + divisionLength * division;
        //         vertices[index++] = new Vector3(divisionPositionX, center.y - halfWidth, center.z);
        //         vertices[index++] = new Vector3(divisionPositionX, center.y + halfWidth, center.z);
        //     }

        //     float lastPositionX = firstPositionX + 1;

        //     vertices[index++] = new Vector3(lastPositionX, center.y - halfWidth, center.z);
        //     vertices[index++] = new Vector3(lastPositionX, center.y + halfWidth, center.z);

        // }

        mesh.vertices = vertices;
        mesh.triangles = GenerateTriangles(partCount);
    }

    // Start is called before the first frame update
    void Awake()
    {
        GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
