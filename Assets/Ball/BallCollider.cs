using UnityEngine;
using UnityEngine.Assertions;

public class BallCollider : MonoBehaviour
{
    [SerializeField, Range(0.01f, 10f)]
    private float radius = 5;

    [SerializeField, Range(3, 60)]
    int nGon = 4;

    private Vector3[] vertices;

    private void OnValidate()
    {

        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null) { return; }
            GenerateMesh();
        };

    }

    private int[] GenerateTriangles()
    {
        int triangleCount = nGon - 2;
        int[] triangles = new int[triangleCount * 3];

        int triangleIndex = 0, verticeIndex = 0;
        for (int part = 0; part < triangleCount; part++)
        {
            triangles[triangleIndex] = verticeIndex + 1;
            triangles[triangleIndex + 1] = 0;
            triangles[triangleIndex + 2] = verticeIndex + 2;
            verticeIndex += 1;
            triangleIndex += 3;
        }

        return triangles;
    }

    private Vector3 GetVertice(float rotation)
    {
        Vector3 normal = Quaternion.Euler(0, 0, rotation) * Vector3.up;

        Vector3 vertice = normal * radius;
        return vertice;
    }

    private void GenerateMesh()
    {
        vertices = new Vector3[nGon];

        float corner = 360 / nGon;

        for (int vertice = 0; vertice < nGon; vertice++)
        {
            vertices[vertice] = GetVertice(corner * vertice);
        }

        Mesh mesh = new Mesh();
        mesh.name = "BallColliderMesh";
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices;
        mesh.triangles = GenerateTriangles();
        PolygonCollider2D polC = gameObject.GetComponent<PolygonCollider2D>();

        Vector2[] points = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            points[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        polC.SetPath(0, points);
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
