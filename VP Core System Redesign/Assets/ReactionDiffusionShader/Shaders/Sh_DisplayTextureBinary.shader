Shader"DiVittorio_Me/Reaction-Diffusion/DisplayTextureBinary"
{
Properties
{
_MainTex("DiffusionCalcuations",2D) = "white"{}
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
float _Threshold;
v2f vert(appdata v)
{
v2f o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.uv = v.uv;
return o;
}

fixed4 frag(v2f i):SV_Target
{	
float4 textureColor = tex2D(_MainTex, i.uv);
float g = textureColor.g;
float b = lerp(1,0,g*2);

	b = sign(b - _Threshold);
	b = clamp(b, 0, 1);

	return float4(b,b,b,1);
}

ENDCG

}
}
}