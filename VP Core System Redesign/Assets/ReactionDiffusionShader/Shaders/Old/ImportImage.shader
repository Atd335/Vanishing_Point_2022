Shader"DiVittorio_Me/Reaction-Diffusion/AddImage"
{
Properties
{
_MainTex("_MainTex",2D) = "white"{}
_ImportImg("Import Image",2D) = "white"{}
_TexHeight("Texture Height",float) = 256
_TexWidth("Texture Width",float) = 256
_Threshold("Threshold",float) = .5
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
sampler2D _ImportImg;
float _TexWidth;
float _TexHeight;
float _Threshold;

v2f vert(appdata v)
{
v2f o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.uv = v.uv;
return o;
}

fixed4 frag(v2f i):
SV_Target
{
	float w = _TexWidth;
	float h = _TexHeight;
	
	float uvx = i.uv.x;
	float uvy = i.uv.y;
	
	float pixelX = w * uvx;
	float pixelY = h * uvy;
	float2 pixelPos = float2(pixelX, pixelY);
			
	float4 color = tex2D(_MainTex, i.uv);
    float bwValue = tex2D(_ImportImg, i.uv).r * tex2D(_ImportImg, i.uv).g * tex2D(_ImportImg, i.uv).b;
    bwValue = sign(bwValue - _Threshold);
	
    float4 additionColor = float4(-1,1,0,0);
    additionColor *= (1 - bwValue);
	//get all of the black from an image, turn it green, add it to the color
	
	return color+additionColor;
}

ENDCG

}
}
}