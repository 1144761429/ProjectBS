using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AimFanScripts : MonoBehaviour
{
    public Transform player;
    public float fanAngle = 120f;
    public float maxRange = 100f;
    [SerializeField] private int rayCount = 240; // a total of this amount of ray will be casted from player
    public LayerMask Obstacles;
    public float wallHight = 1f;
    private float rayPerDegree;
    public GameObject AimFanPrefab;
    public GameObject SpreadFanPrefab;
    public GameObject AimLinePrefab;
    private GameObject AimFan;
    private GameObject SpreadFan;

    public PlayerAllInOne playerAllInOne; // used to get the spread

    private void Start()
    {
        // create three aim indicators
        AimFan = Instantiate(AimFanPrefab);
        SpreadFan = Instantiate(SpreadFanPrefab);

        
        rayPerDegree = rayCount / fanAngle;
        CreateFanMesh();
    }
    private void FixedUpdate()
    {
        CreateFanMesh();
    }

    private void CreateFanMesh()
    {
        float spreadAngle = playerAllInOne.spreadAngle;
        float spreadRayCount = spreadAngle * rayPerDegree;
        int spreadRayStart = (int)((rayCount - spreadRayCount) / 2);

        Mesh aimFanMesh = new Mesh();
        Mesh spreadFanMesh = new Mesh();

        // Create aimFanVertices
        Vector3[] aimFanVertices = new Vector3[rayCount + 2];
        aimFanVertices[0] = player.position; // Center vertex

        // init raycast
        Quaternion rotation;
        Vector3 direction;
        RaycastHit hit;

        float angleStep = fanAngle / (float)rayCount;
        for (int i = 1; i <= rayCount + 1; i++)
        {
            float angle = -fanAngle / 2 + angleStep * (i - 1);

            rotation = Quaternion.AngleAxis(angle, player.up);
            direction = rotation * player.forward;

            if (Physics.Raycast(player.position, direction, out hit, maxRange, Obstacles))
            {
                // Ray hit something
                aimFanVertices[i] = hit.point;
                aimFanVertices[i].y = wallHight; // show the full wall player can see
                //if(i>spreadRayStart)
            }
            else
            {
                // Ray did not hit anything within max range
                aimFanVertices[i] = player.position + direction * maxRange;
                aimFanVertices[i].y = wallHight;
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

        // Assign aimFanVertices and triangles to aimFanMesh
        aimFanMesh.vertices = aimFanVertices;
        aimFanMesh.triangles = triangles;

        // Optional: Add normals and uv's if you need them
        // aimFanMesh.RecalculateNormals();

        // Assign the aimFanMesh to the MeshFilter
        AimFan.GetComponent<MeshFilter>().mesh = aimFanMesh;
        SpreadFan.GetComponent<MeshFilter>().mesh = spreadFanMesh;
    }
}

