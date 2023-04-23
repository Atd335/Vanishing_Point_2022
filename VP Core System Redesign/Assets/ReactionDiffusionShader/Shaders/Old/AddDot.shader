Shader"DiVittorio_Me/Reaction-Diffusion/AddDot"
{
Properties
{
_MainTex("_MainTex",2D) = "white"{}
_TexHeight("Texture Height",float) = 256
_TexWidth("Texture Width",float) = 256
_DotPosition("DotPosition",Vector) = (0,0,0,0)
_DotRadius("DotRadius",float) = 50
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
float _TexWidth;
float _TexHeight;

float4 _DotPosition;
float _DotRadius;

v2f vert(appdata v)
{
v2f o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.uv = v.uv;
return o;
}

float random(float2 uv)
{
	return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
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
	
	float2 centerPoint = float2(_DotPosition.x,_DotPosition.y);
	float radius = _DotRadius;
		
	float4 color = tex2D(_MainTex, i.uv);
	float4 additionColor = float4(0,0,0,0);
	
	if (distance(pixelPos, centerPoint) < radius)
	{
		additionColor = float4(-1,1,0,0);
	}
	return color+additionColor;
}

ENDCG

}
}
}