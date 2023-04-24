Shader"DiVittorio_Me/Reaction-Diffusion/CalculateTexture"
{
Properties
{
_MainTex("DiffusionCalcuations",2D) = "white"{}
_TexHeight("Texture Height",float) = 256
_TexWidth("Texture Width",float) = 256
_dA("diffusion B",float) = 1
_dB("diffusion A",float) = .5
_f("feed rate",float)    = .0545
_k("kill rate",float)    = .062
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
float _dA;
float _dB;
float _f;
float _k;

v2f vert(appdata v)
{
v2f o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.uv = v.uv;
return o;
}

float2 getAdjustedUV(float2 v, float x, float y)
{
	float2 v2 = float2(v.x + x, v.y + y);
	v2.x = clamp(v2.x, 0, 1);
	v2.y = clamp(v2.y, 0, 1);

	return v2;
}

float laplaceA(v2f i, float a)
{
float pScaleX = 1 / _TexWidth;
float pScaleY = 1 / _TexHeight;
	
float sum = a * -1;

sum += tex2D(_MainTex, getAdjustedUV(i.uv,pScaleX,0)).r * .2f;
sum += tex2D(_MainTex, getAdjustedUV(i.uv,-pScaleX,0)).r * .2f;
sum += tex2D(_MainTex, getAdjustedUV(i.uv, 0, pScaleY)).r * .2f;
sum += tex2D(_MainTex, getAdjustedUV(i.uv, 0, -pScaleY)).r * .2f;
	
sum += tex2D(_MainTex, getAdjustedUV(i.uv, -pScaleX, -pScaleY)).r * .05f;
sum += tex2D(_MainTex, getAdjustedUV(i.uv, -pScaleX, pScaleY)).r * .05f;
sum += tex2D(_MainTex, getAdjustedUV(i.uv, pScaleX, -pScaleY)).r * .05f;
sum += tex2D(_MainTex, getAdjustedUV(i.uv, pScaleX, pScaleY)).r * .05f;

return sum;
}

float laplaceB(v2f i, float b)
{	
	float pScaleX = 1 / _TexWidth;
	float pScaleY = 1 / _TexHeight;
	
	float sum = b * -1;

	sum += tex2D(_MainTex, getAdjustedUV(i.uv, 0, pScaleX)).g * .2f;
	sum += tex2D(_MainTex, getAdjustedUV(i.uv, 0, -pScaleX)).g * .2f;
	sum += tex2D(_MainTex, getAdjustedUV(i.uv, pScaleX, 0)).g * .2f;
	sum += tex2D(_MainTex, getAdjustedUV(i.uv, -pScaleX, 0)).g * .2f;
	
	sum += tex2D(_MainTex, getAdjustedUV(i.uv, -pScaleX, -pScaleY)).g* .05f;
	sum += tex2D(_MainTex, getAdjustedUV(i.uv, -pScaleX, pScaleY)).g * .05f;
	sum += tex2D(_MainTex, getAdjustedUV(i.uv, pScaleX, -pScaleY)).g * .05f;
	sum += tex2D(_MainTex, getAdjustedUV(i.uv, pScaleX, pScaleY)).g * .05f;

	return sum;
}

fixed4 frag	(v2f i) : SV_Target
{	
		float4 textureColor = tex2D(_MainTex, i.uv);
		float4 cell = textureColor;
		
		float a1 = cell.r;
		float b1 = cell.g;

		float a2 = a1 + ((_dA * laplaceA(i, a1)) - (a1 * b1 * b1) + (_f * (1 - a1)));
		float b2 = b1 + ((_dB * laplaceB(i, b1)) + (a1 * b1 * b1) - ((_k + _f) * b1));
	
		return float4(a2, b2, 0, 1);
}

	ENDCG

}
}
}