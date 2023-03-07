Shader "OutlineCamera"
{
    Properties
    {
        _MainTex ("render texture", 2D) = "white" {}
        _outlineTex ("outline texture", 2D) = "white" {}
        _ditherPattern ("dither pattern", 2D) = "gray" {}
        _threshold ("threshold", Range(-0.5, 0.5)) = 0
        _colorScreen ("screen color", Color) = (1, 1, 1)
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
            float _threshold;
            float3 _colorScreen;

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

            float4 frag (Interpolators i) : SV_Target
            {
                float outline = 0;
                float2 uv = i.uv;
                
                // (1.0 / 256) * 512
                float2 ditherUV = (uv / _ditherPattern_TexelSize.zw) * _MainTex_TexelSize.zw;

                float pattern = tex2D(_ditherPattern, ditherUV);
                
                float3 render = tex2D(_MainTex, uv);

                float3 outlineTexture = tex2D(_outlineTex, uv);

                float3 w = float3(0.299, 0.587, 0.144);
                //float grayscale = dot(render, w);

                outline = step(_threshold, outlineTexture);

                return float4(render * saturate(outline + _colorScreen), 1.0);
            }
            ENDCG
        }
    }
}
