using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ImageEffectBasic : MonoBehaviour
{
    public Material effectMaterial;

    public float _ditherResolution = 2f;
    public float _ditherIntensity = .446f;
    public float _saturation = .39f;
    public float _brightness = -.33f;
    public float _contrast = 1.92f;
    public Color _colorScreen = new Color(0.198f, 0.155f, 0.099f,1);
    public float _scale = 54.8f;
    public float _intensity = .001f;
    public float _speed = 1.55f;
    public float _SaturationThreshold = .1f;

    void OnRenderImage (RenderTexture source, RenderTexture destination) {
        effectMaterial.SetFloat("_ditherResolution", _ditherResolution);
        effectMaterial.SetFloat("_ditherIntensity", _ditherIntensity);
        effectMaterial.SetFloat("_saturation", _saturation);
        effectMaterial.SetFloat("_brightness", _brightness);
        effectMaterial.SetFloat("_contrast", _contrast);
        effectMaterial.SetFloat("_scale", _scale);
        effectMaterial.SetFloat("_intensity", _intensity);
        effectMaterial.SetFloat("_speed", _speed);
        effectMaterial.SetFloat("_SaturationThreshold", _SaturationThreshold);

        effectMaterial.SetColor("_colorScreen", _colorScreen);
        Graphics.Blit(source, destination, effectMaterial);
    }
}
