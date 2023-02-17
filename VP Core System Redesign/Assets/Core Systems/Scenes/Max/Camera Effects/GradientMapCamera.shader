Shader "GradientMapCamera"
{
    Properties
    {
        _MainTex ("render texture", 2D) = "white" {}
        //_outlineTex ("outline texture", 2D) = "white" {}
        _ditherPattern ("dither pattern", 2D) = "gray" {}

        _ditherIntensity ("dither intensity", Range(0.0, 1.0)) = 0

        _colorGradient ("color gradient", 2D) = "gray" {}

        _saturation ("gradient saturation", Range(0.0, 1.0)) = 0

        _brightness ("brightness", Range(-1.0, 1.0)) = 0

        _contrast ("value contrast", Range(-4.0, 4.0)) = 1.0

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
            sampler2D _colorGradient;
            float _ditherIntensity;
            float _threshold;
            float _saturation;
            float _contrast;
            float _brightness;

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

            half3 AdjustContrast(half3 color, half contrast) {
                return saturate(lerp(half3(0.5, 0.5, 0.5), color, contrast));
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float outline = 0;
                float2 uv = i.uv;
                
                float2 ditherUV = (uv / _ditherPattern_TexelSize.zw) * _MainTex_TexelSize.zw;

                float pattern = tex2D(_ditherPattern, ditherUV);

                float3 camRender = tex2D(_MainTex, uv);

                float3 w = float3(0.299, 0.587, 0.144);
                float grayscale = dot(camRender, w);


                grayscale = AdjustContrast(grayscale, _contrast) + _brightness + (pattern * _ditherIntensity);

                float processed = clamp(grayscale, 0.05, 0.95);

                float3 gradientmap = tex2D(_colorGradient, float2(processed, 0.5));

                float3 saturationAdjusted = lerp(float3(grayscale, grayscale, grayscale), gradientmap, _saturation);

                return float4(saturationAdjusted + _colorScreen, 1.0);

            }
            ENDCG
        }
    }
}
