using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class ReactionDiffusionTexture : MonoBehaviour
{
   
    //Calculation Variables
    public Vector4 CalculationVariables = new Vector4(1.46f,.03f,.008f,.047f);
    public Vector2Int textureSize = new Vector2Int(960,540);
    
    //Render Textures
    public RenderTexture rawTexture;
    public RenderTexture rawTextureBuffer;
    public RenderTexture displayTexture;
    public RenderTexture displayTextureBuffer;

    //Material Operations
    
    public Material RawDiffusionCalculationMat;
    public Material ThresholdDisplayMat;
    public Material ClearTextureMat;   
    public Material StampImageMat;

    //Image to stamp. It can have color, but its best if its black and white. 
    public Texture stampTexture;

    private void Awake()
    {
        tuneAlgorithm(CalculationVariables);
        syncTextureScale();
    }

    void syncTextureScale()
    {
        RawDiffusionCalculationMat.SetFloat("_TexWidth", textureSize.x);
        RawDiffusionCalculationMat.SetFloat("_TexHeight", textureSize.y);

        StampImageMat.SetFloat("_TexWidth", textureSize.x);
        StampImageMat.SetFloat("_TexHeight", textureSize.y);
    }

    public void tuneAlgorithm(float dA, float dB, float f, float k)
    {
        RawDiffusionCalculationMat.SetFloat("_dA",dA);
        RawDiffusionCalculationMat.SetFloat("_dB",dB);
        RawDiffusionCalculationMat.SetFloat("_f",f);
        RawDiffusionCalculationMat.SetFloat("_k",k);
    }

    public void tuneAlgorithm(Vector4 variables)
    {
        RawDiffusionCalculationMat.SetFloat("_dA", variables.x);
        RawDiffusionCalculationMat.SetFloat("_dB", variables.y);
        RawDiffusionCalculationMat.SetFloat("_f",  variables.z);
        RawDiffusionCalculationMat.SetFloat("_k",  variables.w);
    }

    public RenderTexture[] calculateRawTexture(int bufferMultiplier = 1)
    {
        for (int i = 0; i < bufferMultiplier; i++)
        {
            Graphics.Blit(rawTexture, rawTextureBuffer, RawDiffusionCalculationMat);
            Graphics.Blit(rawTextureBuffer, rawTexture, RawDiffusionCalculationMat);
        }


        Graphics.Blit(rawTextureBuffer, displayTexture, ThresholdDisplayMat);

        Graphics.Blit(displayTexture, displayTextureBuffer, ThresholdDisplayMat);
        Graphics.Blit(displayTextureBuffer, displayTexture, ThresholdDisplayMat);

        RenderTexture[] r = { rawTexture, rawTextureBuffer, displayTexture, displayTextureBuffer};

        return r;
    }

    public void clearRawTexture()
    {
        Graphics.Blit(rawTexture, rawTextureBuffer, ClearTextureMat);
        Graphics.Blit(rawTextureBuffer, rawTexture, ClearTextureMat);
    }

    public void stampImage(float x, float y, float relativeScale)
    {
        stampImage(new Vector2(x,y), relativeScale);
    }

    public void stampImage(Vector2 relativePos, float relativeScale)
    {
        StampImageMat.SetVector("_RelativeStampPosition",relativePos);
        StampImageMat.SetFloat("_StampSize",relativeScale);
        StampImageMat.SetTexture("_ImportImg", stampTexture);
        Graphics.Blit(rawTexture, rawTextureBuffer, StampImageMat);
        Graphics.Blit(rawTextureBuffer, rawTexture, StampImageMat);
    }
}
