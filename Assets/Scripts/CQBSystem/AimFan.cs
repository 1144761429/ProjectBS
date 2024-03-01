using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFan : MonoBehaviour
{
    public Transform player; // The starting point of the raycast
    public float maxRange = 100f; // Maximum range of the weapon
    public float spreadAngle = 10f; // The spread angle of the weapon
    public int rayCount = 32; // How many rays to cast within the spread cone
    public LayerMask hitLayers; // Layers that the ray should collide with
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = rayCount + 1; // +1 for the origin
        // Optionally, configure LineRenderer appearance here
    }

    void Update()
    {
        DrawSpreadIndicator();
    }

    void DrawSpreadIndicator()
    {
        Vector3[] points = new Vector3[2*rayCount + 2];
        points[0] = player.position; // The first point is the origin

        float stepAngle = spreadAngle / (float)(rayCount - 1);
        for (int i = 0; i < rayCount; i++)
        {
            float angle = -spreadAngle * 0.5f + stepAngle * i; // Rotate around the forward axis
            Quaternion rotation = Quaternion.AngleAxis(angle, player.up);
            Vector3 direction = rotation * player.forward;
            RaycastHit hit;

            points[2 * i] = player.position;
            if (Physics.Raycast(player.position, direction, out hit, maxRange, hitLayers))
            {
                // Ray hit something
                points[2*i + 1] = hit.point;
            }
            else
            {
                // Ray did not hit anything within max range
                points[2*i + 1] = player.position + direction * maxRange;
            }

        }

        // Set the positions on the LineRenderer
        lineRenderer.SetPositions(points);
    }
}
