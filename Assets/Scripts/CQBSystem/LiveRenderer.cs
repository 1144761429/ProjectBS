using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveRenderer : MonoBehaviour
{
    public Camera visionCamera;
    public RenderTexture visionTexture;
    public Material visionMaterial;

    void Start()
    {
        if (visionCamera.targetTexture != visionTexture)
        {
            visionCamera.targetTexture = visionTexture;
        }
        visionMaterial.mainTexture = visionTexture;
    }

    void Update()
    {
    }
}