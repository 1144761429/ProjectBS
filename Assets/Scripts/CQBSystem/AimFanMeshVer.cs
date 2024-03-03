using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AimFanMeshVer : MonoBehaviour
{
    public Transform player; 
    public float fanAngle = 120f;
    public float maxRange = 100f;
    [SerializeField] private int rayCount = 128;
    public LayerMask Obstacles;
    public float wallHight = 1f;

    void Start()
    {
        CreateFanMesh();
    }
    void Update()
    {
        CreateFanMesh();
    }

    void CreateFanMesh()
    {
        Mesh mesh = new Mesh();

        // Create vertices
        Vector3[] vertices = new Vector3[rayCount + 2];
        vertices[0] = player.position; // Center vertex
        vertices[0].y = wallHight;

        float angleStep = fanAngle / (float)rayCount;
        for (int i = 1; i <= rayCount + 1; i++)
        {
            float angle = -fanAngle / 2 + angleStep * (i - 1);
            Quaternion rotation = Quaternion.AngleAxis(angle, player.up);
            Vector3 direction = rotation * player.forward;
            RaycastHit hit;
            if (Physics.Raycast(player.position, direction, out hit, maxRange, Obstacles))
            {
                // Ray hit something
                vertices[i] = hit.point;
                vertices[i].y = wallHight;
            }
            else
            {
                // Ray did not hit anything within max range
                
                vertices[i] = player.position + direction * maxRange;

                vertices[i].y = wallHight;
            }
        }

        // Create triangles
        int[] triangles = new int[rayCount * 3];
        for (int i = 0, vert = 1; i < triangles.Length; i += 3, vert++)
        {
            triangles[i] = 0;
            triangles[i + 1] = vert;
            triangles[i + 2] = vert + 1;
        }

        // Assign vertices and triangles to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Optional: Add normals and uv's if you need them
        // mesh.RecalculateNormals();

        // Assign the mesh to the MeshFilter
        GetComponent<MeshFilter>().mesh = mesh;
    }
}

