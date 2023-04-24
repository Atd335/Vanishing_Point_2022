Shader"DiVittorio_Me/Reaction-Diffusion/Stamp"
{
Properties
{
_MainTex("_MainTex",2D) = "white"{}
_ImportImg("Import Image",2D) = "black"{}
_TexHeight("Texture Height",float) = 256
_TexWidth("Texture Width",float) = 256
_Threshold("Threshold",float) = .5
_RelativeStampPosition("Stamp Position",Vector) = (.5,.5,0,0)
_StampSize("Stamp Size",float) = 50
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

float4 _RelativeStampPosition;

float _TexWidth;
float _TexHeight;
float _Threshold;
float _StampSize;


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
		
    float2 stampSize;
    stampSize.x = w * _StampSize;
    stampSize.y = w * _StampSize;
    
    float2 centerCoords = _RelativeStampPosition.xy;
    
    float2 stampPixelLocation;
    stampPixelLocation.x = w * centerCoords.x;
    stampPixelLocation.y = h * centerCoords.y;
    
    float left = stampPixelLocation.x - (stampSize.x / 2);
    float right = stampPixelLocation.x + (stampSize.x / 2);
    float down = stampPixelLocation.y - (stampSize.y/2);
    float up = stampPixelLocation.y + (stampSize.y/2);
	
    float gtLeft = sign(pixelX - left); //returns 0 if i.uv is less than, and 1 if greater
    gtLeft = clamp(gtLeft,0,1);
	
    float ltRight = sign(right - pixelX); //returns 0 if i.uv is greater than, and 1 if less
    ltRight = clamp(ltRight, 0, 1);
	
    float gtDown = sign(pixelY - down); //returns 0 if i.uv is less than, and 1 if greater
    gtDown = clamp(gtDown, 0, 1);
	
    float ltUp = sign(up - pixelY); //returns 0 if i.uv is greater than, and 1 if less
    ltUp = clamp(ltUp, 0, 1);
	
    float boxCalc = gtLeft + ltRight + gtDown + ltUp;//if in box, this will equal 4
    
    //returns zero if not in box, and one if in box
    float isInBox = floor(boxCalc / 4);
        
    float relX = (pixelX - left) / stampSize.x;
    float relY = (pixelY - down) / stampSize.y;
    
    float2 stampUV = float2(relX,relY);
    float4 stampColor = tex2D(_ImportImg,stampUV);
    
    float bwValue = (stampColor.r + stampColor.g + stampColor.b)/3;
    bwValue = sign(bwValue - _Threshold);
    bwValue = 1 - bwValue;//add black to image
    
    float4 adjustedStampColor = float4(-1, 1, 0, 0) * isInBox * bwValue;

	return color + adjustedStampColor;
}

ENDCG

}
}
}