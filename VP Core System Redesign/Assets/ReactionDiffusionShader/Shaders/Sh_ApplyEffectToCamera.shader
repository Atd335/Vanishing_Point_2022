Shader"DiVittorio_Me/Reaction-Diffusion/CameraEffect"
{
Properties
{
_MainTex("Camera Texture",2D) = "white"{}
_MaskTexture("Display Texture",2D) = "white"{}
_NoiseTex("NoiseTexture",2D) = "white"{}

_Threshold("Threshold",float) = .5
_NoiseStrength("Noise Strength", float) = .5
_MaskColorIntensity("Intensity", float) = 1

_NoiseOffset("Noise Offset",Vector) = (0,0,0,0)
_UVOffset("UV Offset", Vector) = (0,0,0,0)

_Color("Color",Color) = (1,1,1,1)
}

SubShader
{


Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

struct appdata
{
float4 vertex : POSITION;
float2 uv : TEXCOORD0;
};

struct v2f
{
float2 uv : TEXCOORD0;
float4 vertex : SV_POSITION;
};

sampler2D _MainTex;
sampler2D _MaskTexture;
sampler2D _NoiseTex;

float4 _UVOffset;
float4 _NoiseOffset;
float4 _Color;

float _Threshold;
float _NoiseStrength;
float _MaskColorIntensity;

bool _InvertMask;

v2f vert(appdata v)
{
v2f o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.uv = v.uv;
return o;
}

fixed4 frag(v2f i):SV_Target
{	
    float bwValue = tex2D(_MaskTexture, i.uv).r * tex2D(_MaskTexture, i.uv).g * tex2D(_MaskTexture, i.uv).b;
    bwValue = sign(bwValue - _Threshold);//returns 0 if black, and 1 if white
    bwValue = clamp(bwValue,0,1);

    
    //im going to use black as the mask
    float4 camColor = tex2D(_MainTex, i.uv);
    
    float3 adjColor = camColor;
        
    float2 uvOffset = _UVOffset;
    float2 adjUV = i.uv + uvOffset;
        
    adjColor = tex2D(_MainTex, adjUV); 
    adjColor *= 1.2f;
    
    float2 noiseUV = _NoiseOffset.xy + i.uv;
    adjColor *= float4(1, 1, 1, 1) - (_NoiseStrength * tex2D(_NoiseTex, noiseUV));
    
    //^^^do whatever shader calculations you want to the adjusted color, these will be the pixels affected by the mask
   
    camColor.rgb = ((adjColor * _Color.rgb * _MaskColorIntensity) * (bwValue)) + (camColor.rgb * (1 - bwValue));
    
    return camColor;
}

ENDCG

}
}
}