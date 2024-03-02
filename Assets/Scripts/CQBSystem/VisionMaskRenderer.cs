using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionMaskGenerator : MonoBehaviour
{
    public Material visionMaterial; // The material that will use the mask texture.
    public int textureSize = 512; // The size of the mask texture.

    private Texture2D visionTexture;

    private void Start()
    {
        GenerateVisionTexture();
    }

    void GenerateVisionTexture()
    {
        visionTexture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);

        // Set the entire texture to transparent.
        Color[] colors = visionTexture.GetPixels();
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.clear; // Set all pixels to transparent.
        }
        visionTexture.SetPixels(colors);
        visionTexture.Apply();

        // Assign the texture to the material.
        visionMaterial.SetTexture("_VisionTex", visionTexture);
    }

    public void UpdateVisionArea(Vector2 center, float radius)
    {
        // Reset the texture to transparent.
        Color[] colors = visionTexture.GetPixels();
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.clear;
        }

        // Draw a filled circle at 'center' with 'radius' in pixels.
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float dx = center.x - x;
                float dy = center.y - y;
                if (dx * dx + dy * dy <= radius * radius)
                {
                    colors[x + y * textureSize] = Color.white; // Vision area.
                }
            }
        }

        visionTexture.SetPixels(colors);
        visionTexture.Apply();
    }
}