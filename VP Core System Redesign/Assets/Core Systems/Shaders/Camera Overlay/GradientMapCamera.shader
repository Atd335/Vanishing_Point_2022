Shader"GradientMapCamera"
{
Properties
{
_MainTex ("render texture", 2D) = "white" {}
//_outlineTex ("outline texture", 2D) = "white" {}
_ditherPattern ("dither pattern", 2D) = "gray" {}

_ditherResolution ("dither resolution", Range(0.2, 2.0)) = 1.0

_ditherIntensity ("dither intensity", Range(0.0, 1.0)) = 0

_colorGradient ("color gradient", 2D) = "gray" {}

_saturation ("gradient saturation", Range(0.0, 1.0)) = 0

_brightness ("brightness", Range(-1.0, 1.0)) = 0

_contrast ("value contrast", Range(-4.0, 4.0)) = 1.0

_colorScreen ("screen color", Color) = (1, 1, 1)

_scale ("noise scale", Range(2, 60)) = 15.5

_intensity ("noise intensity", Range(0.001, 0.05)) = 0.006

_speed ("noise speed", Range(0.05, 4)) = 0.2

_SaturationThreshold("saturationThreshold",float) = .5
}

SubShader
{
Cull Off
ZWrite Off
ZTest Always

Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

sampler2D _MainTex; float4 _MainTex_TexelSize;
sampler2D _outlineTex;
sampler2D _ditherPattern; float4 _ditherPattern_TexelSize;
sampler2D _colorGradient;
sampler2D _ColorOverlayPalette;

float _SaturationThreshold;

float _ditherIntensity;
float _ditherResolution;
float _threshold;
float _saturation;
float _contrast;
float _brightness;

float3 _colorScreen;

float _scale;
float _intensity;
float _speed;

struct MeshData
{
float4 vertex : POSITION;
float2 uv : TEXCOORD0;
};

struct Interpolators
{
float4 vertex : SV_POSITION;
float2 uv : TEXCOORD0;
};

Interpolators vert (MeshData v)
{
Interpolators o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.uv = v.uv;
return o;
}

float rand (float2 uv) {
return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453123);
}

float value_noise (float2 uv) {
float2 ipos = floor(uv);
float2 fpos = frac(uv); 
                
float o  = rand(ipos);
float x  = rand(ipos + float2(1, 0));
float y  = rand(ipos + float2(0, 1));
float xy = rand(ipos + float2(1, 1));

float2 smooth = smoothstep(0, 1, fpos);
return lerp( lerp(o,  x, smooth.x), 
    lerp(y, xy, smooth.x), smooth.y);
}

half3 AdjustContrast(half3 color, half contrast) {
return saturate(lerp(half3(0.5, 0.5, 0.5), color, contrast));
}

float colorSaturation(float3 col)
{
return distance(min(min(col.r, col.g), col.b), max(max(col.r, col.g), col.b));
}

float4 frag (Interpolators i) : SV_Target
{
float outline = 0;
float2 uv = i.uv;
                
float2 ditherUV = (uv / _ditherPattern_TexelSize.zw) * _MainTex_TexelSize.zw;
//float2 screenUV = i.screenPos.xy / i.screenPos.w;
ditherUV /= _ditherResolution;

float pattern = tex2D(_ditherPattern, ditherUV);

float4 camRender = tex2D(_MainTex, uv);

float camSaturation = (sign(colorSaturation(camRender.rgb) - _SaturationThreshold));
camSaturation = clamp(camSaturation, 0, 1);
    
float n = (value_noise((uv + floor(_Time.y * _speed)) * _scale) - 0.5) * _intensity;

// add our noise value to our uv coordinates when we sample the texture
float3 camWiggle = tex2D(_MainTex, uv + n);

float3 w = float3(0.299, 0.587, 0.144);
float grayscale = dot(camWiggle, w);
    
grayscale = AdjustContrast(grayscale, _contrast) + _brightness + (pattern * _ditherIntensity);

float processed = clamp(grayscale, 0.05, 0.95);
    grayscale += (1 - grayscale) * lerp(0, .85, camSaturation);
    
float3 gradientmap = tex2D(_colorGradient, float2(processed, 0.5));

float3 saturationAdjusted = lerp(float3(grayscale, grayscale, grayscale), gradientmap, _saturation);

float4 satColorScreen = float4(saturationAdjusted + _colorScreen, 1.0);

    
float4 saturatedColors = ((float4(1, 1, 1, 1) - camRender) * (1-camSaturation)) + (camRender);
//float4 saturatedColors = camRender * camSaturation;
    
    float vibranceModifier = 1;
    vibranceModifier *= camSaturation;
    vibranceModifier += 1;
    
return satColorScreen * saturatedColors * vibranceModifier;

}
ENDCG
}
}
}
