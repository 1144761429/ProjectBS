using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FOVCone : MonoBehaviour
{
    /// <summary>
    /// The angles that the cone covers.
    /// </summary>
    [Range(0, 360)] [SerializeField] private float viewAngle;

    /// <summary>
    /// The maximum distance of the cone.
    /// </summary>
    [SerializeField] private float viewDistance;
    
    /// <summary>
    /// How detail the cone are.
    /// The higher this value is, the more triangles will be used for the cone.
    /// </summary>
    ///
    /// <remarks>
    /// The cone is actually a shape that is constructed of many triangles.
    /// </remarks>
    [Range(0, 2)] [SerializeField] private float resolution;

    public bool IsDebugMode;
    
    private void LateUpdate()
    {
        DrawCone();
    }

    // TODO: add comments
    private void DrawCone()
    {
        int triangleAmount = Mathf.RoundToInt(viewAngle * resolution);
        float triangleDegree = viewAngle / triangleAmount;

        for (int i = 0; i <= triangleAmount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + triangleDegree * i;

            if (IsDebugMode)
            {
                print("drawing");
                Vector3 position = transform.position;
                Debug.DrawLine(position, position + DirectionFromAngle(angle) * viewDistance, Color.red);
            }
        }
    }

    private Vector3 DirectionFromAngle(float angleInDeg)
    {
        //angleInDeg += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }
}
