  Shader"DiVittorio_Me/Glitch/Vertex" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Amount ("Magnitude", float) = 0.5
      _OffsetPos ("Offset Position", Vector) = (0,0,0,0)
      _OffsetScale ("Offset Scale", Vector) = (1,1,1,1)
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
struct Input
{
    float2 uv_MainTex;
};
float _Amount;
float4 _OffsetPos;
float4 _OffsetScale;
float random(float4 v)
{
return frac(sin(dot(v, float4(12.9898, 78.233, 56.123123, 50.11212))) * 1225458.5453123);
}
float4 randomFloat4(float4 v)
{
float rx = random(float4(v.x, v.y, v.z, v.w));
float ry = random(float4(rx, v.y, v.z, v.w));
float rz = random(float4(v.x, v.y, ry, v.w));
float rw = random(float4(v.x, v.y, v.z, rz));
float4 rnd4 = float4(rx, ry, rz, rw);
return rnd4;
}

void vert(inout appdata_full v)
{
    v.vertex += randomFloat4(v.vertex)*_Amount;
    v.vertex.xyz += _OffsetPos;
    v.vertex.xyz *= _OffsetScale;
    
}
sampler2D _MainTex;
void surf(Input IN, inout SurfaceOutput o)
{
    o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
}
      ENDCG
    } 
Fallback"Diffuse"
  }