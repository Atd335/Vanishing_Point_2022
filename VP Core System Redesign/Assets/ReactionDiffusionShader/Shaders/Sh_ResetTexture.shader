Shader"DiVittorio_Me/Reaction-Diffusion/ResetTexture"
{
Properties
{

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
	float4 f = float4(1,0,0,1);
	return f;
}

ENDCG

}
}
}