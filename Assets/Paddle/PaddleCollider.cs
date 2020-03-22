using UnityEngine;
using UnityEngine.Assertions;

public class PaddleCollider : MonoBehaviour
{
    [SerializeField, Range(0.01f, 360f)]
    private float lengthInDegrees = 5;

    [SerializeField, Range(1, 360f)]
    int divisionPerNDegrees = 4;

    [SerializeField, Range(0.01f, 5f)]
    private float width = 1f;

    [SerializeField, Range(0.01f, 10f)]
    private float radius = 3f;


    private Vector3[] vertices;
    private Vector2[] points = new Vector2[4];

    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null) { return; }
            GenerateMesh();
        };

    }
    private int[] GenerateTriangles(int partCount)
    {
        PolygonCollider2D polC = gameObject.GetComponent<PolygonCollider2D>();
        polC.pathCount = partCount;
        int[] colliderVerticeOrder = new int[4] { 0, 1, 3, 2 };

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


            for (int i = 0; i < 4; i++)
            {
                int colliderIndex = colliderVerticeOrder[i];

                points[i] = new Vector2(
                    vertices[verticeIndex + colliderIndex].x,
                    vertices[verticeIndex + colliderIndex].y
                );
            }

            polC.SetPath(part, points);

            verticeIndex += 2;
            triangleIndex += 6;
        }

        return triangles;
    }


    private int AddVertices(int index, float rotation)
    {
        Vector3 normal = Quaternion.Euler(0, 0, rotation) * Vector3.up;

        normal = transform.InverseTransformPoint(normal);
        Vector3 vertice = normal * radius;
        vertices[index] = vertice;
        vertices[index + 1] = normal * (radius + width);

        return index + 2;
    }

    private int GetPartCount()
    {
        int partCount = 0;

        if (lengthInDegrees < divisionPerNDegrees)
        {
            partCount = 1;
        }
        else
        {
            //If there is a remainder left, we treat the remainder as a separate part
            int additionalPart = lengthInDegrees % divisionPerNDegrees > 0 ? 1 : 0;

            partCount = (int)Mathf.Floor(lengthInDegrees / divisionPerNDegrees) + additionalPart;
        }

        return partCount;
    }

    private void GenerateMesh()
    {
        Assert.AreNotEqual(0, divisionPerNDegrees);

        int partCount = GetPartCount();
        float partLength = lengthInDegrees / (float)partCount;

        int verticeCount = partCount * 2 + 2;
        vertices = new Vector3[verticeCount];

        float parentRotation = transform.eulerAngles.z;
        float leftRotation = parentRotation + lengthInDegrees / 2;

        int index = 0;
        for (int part = 0; part < partCount; part++)
        {
            index = AddVertices(index, leftRotation - part * partLength);
        }

        float rightRotation = parentRotation - lengthInDegrees / 2;
        index = AddVertices(index, rightRotation);


        Mesh mesh = new Mesh();
        mesh.name = "PaddleColliderMesh";
        GetComponent<MeshFilter>().mesh = mesh;
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
