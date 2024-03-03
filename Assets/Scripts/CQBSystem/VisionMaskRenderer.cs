using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FOVPostProcess : MonoBehaviour
{
    public Shader postProcessShader;
    private Material postProcessMaterial;
    public RenderTexture maskTexture; // Assign the mask Render Texture here in the inspector.

    void Start()
    {
        if (postProcessShader != null)
        {
            postProcessMaterial = new Material(postProcessShader);
            Console.WriteLine("MatisReady");
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (postProcessMaterial != null)
        {
            // Assign the textures to the material
            postProcessMaterial.SetTexture("_MainTex", src);
            postProcessMaterial.SetTexture("_MaskTex", maskTexture);

            Console.WriteLine("MrMaintexisMasked");

            // Apply the shader operation
            Graphics.Blit(src, dest, postProcessMaterial);
        }
        else
        {

            Console.WriteLine("MrMaintexisNOTMasked");
            // Fallback, just copy the source render texture to the destination
            Graphics.Blit(src, dest);
        }
    }
}