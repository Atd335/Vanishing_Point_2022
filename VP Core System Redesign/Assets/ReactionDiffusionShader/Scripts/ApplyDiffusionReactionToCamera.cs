using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyDiffusionReactionToCamera : MonoBehaviour
{
    public Material mat;

    public Color color = Color.white;
    public float intensity = 1;

    private void OnValidate()
    {
        bool hasNoise = this.gameObject.GetComponent<NoiseJitter>() != null;

        if (hasNoise)
        {
            this.gameObject.GetComponent<NoiseJitter>().camMat = mat;
        }
        else
        {
            this.gameObject.AddComponent<NoiseJitter>();
            this.gameObject.GetComponent<NoiseJitter>().camMat = mat;
        }
    }



    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetColor("_Color", color);
        mat.SetFloat("_MaskColorIntensity", intensity);
        Graphics.Blit(source,destination, mat);
    }
}

