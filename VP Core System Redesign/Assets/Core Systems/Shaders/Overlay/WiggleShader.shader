Shader"Unlit/WiggleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _Amount("Noise Magnitude",float) = .1
        _PerlinSize("Noise Scale",float) = 2.5
        _PerlinIntensity("Noise Intensity",float) = .5
        _PerlinOffsetX("Offset X",float) = 1
        _PerlinOffsetY("Offset Y",float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float _Amount;
            float _PerlinSize;
            float _PerlinIntensity;
            float _PerlinOffsetX;
            float _PerlinOffsetY;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                float2 offset = float2(_PerlinOffsetX,_PerlinOffsetY);
                float4 n4 = tex2D(_NoiseTex, (i.uv+offset) / _PerlinSize);
                float n = (n4.r + n4.g + n4.b)/3;
                n = lerp(0,_PerlinIntensity,n);
                n*=_Amount;
                fixed4 col = tex2D(_MainTex, i.uv + (n));
    
                return col;
            }
            ENDCG
        }
    }
}
