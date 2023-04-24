Shader "Vanishing_Point/OverlayTextureShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OverlayTexture_1("OverlayTexture 1", 2D) = "white" {}
        _OverlayTexture_2("OverlayTexture 2", 2D) = "white" {}
        _OverlayTexture_3("OverlayTexture 3", 2D) = "white" {}

        _OverlayColor_1("Overlay Color 1", Color) = (1,1,1,1)
        _OverlayColor_2("Overlay Color 2", Color) = (1,1,1,1)
        _OverlayColor_3("Overlay Color 3", Color) = (1,1,1,1)

        _DetectColor_1("Detect Color 1", Color) = (1,1,1,1)
        _DetectColor_2("Detect Color 2", Color) = (1,1,1,1)
        _DetectColor_3("Detect Color 3", Color) = (1,1,1,1)
        
        _PerlinTex ("Noise Texture", 2D) = "white" {}
        _PerlinAmount("Noise Magnitude",float) = .1
        _PerlinSize("Noise Scale",float) = 2.5
        _PerlinIntensity("Noise Intensity",float) = .5
        _PerlinOffsetX("Offset X",float) = 1
        _PerlinOffsetY("Offset Y",float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _PerlinTex;
            sampler2D _OverlayTexture_1;
            sampler2D _OverlayTexture_2;
            sampler2D _OverlayTexture_3;

            fixed4 _OverlayColor_1;
            fixed4 _OverlayColor_2;
            fixed4 _OverlayColor_3;

            fixed4 _DetectColor_1;
            fixed4 _DetectColor_2;
            fixed4 _DetectColor_3;

            float _PerlinAmount;
            float _PerlinSize;
            float _PerlinIntensity;
            float _PerlinOffsetX;
            float _PerlinOffsetY;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 ovTex_1 = tex2D(_OverlayTexture_1,i.uv);
                fixed4 ovTex_2 = tex2D(_OverlayTexture_2,i.uv);
                fixed4 ovTex_3 = tex2D(_OverlayTexture_3,i.uv);
                
                fixed4 ovCol_1 = _DetectColor_1;
                fixed4 ovCol_2 = _DetectColor_2;
                fixed4 ovCol_3 = _DetectColor_3;

                //f = greyscale, so 1 means white and zero means black. 
                float f = (col.r + col.g + col.b) / 3;
                if (f == 1) { col.rgb = 0; col.a = 0; }
                
                //it's black if f ==0
                //bool ov1 = ovCol_1 == col;
                bool ov1 = f==0;
                
                //It's red if the color detected is red, and its not black
                //bool ov2 = ovCol_2 == col;
                bool ov2 = ovCol_2 == col && f != 0;
                
                //it's cyan ('interact') if its cyan and not black
                //bool ov3 = ovCol_3 == col;
                bool ov3 = ovCol_3 == col && f != 0;

                //if the color detected is not white, set it to white
                if (f != 1) { col.rgb = 1; }
                
                //apply the overlay color multiplied by the corresponding overlay texture
                //if its a culling color, turn the opacity to zero.
                
                //let's jiggle the blue texture (ovTex_1)
                float2 offset = float2(_PerlinOffsetX, _PerlinOffsetY);
                float4 n4 = tex2D(_PerlinTex, (i.uv + offset) / _PerlinSize);
                float n = (n4.r + n4.g + n4.b) / 3;
                n = lerp(0, _PerlinIntensity, n);
                n *= _PerlinAmount;
    
                float4 plat = tex2D(_OverlayTexture_1, i.uv+n);

                if (ov1) { col = _OverlayColor_1 * plat; }
                if (ov2) { col = _OverlayColor_2  * ovTex_2; }
                if (ov3) { col = _OverlayColor_3  * ovTex_3; }
                if (!ov2 && !ov1 && !ov3) { col.a = 0; }
                
    
                return col;
            }
            ENDCG
        }
    }
}
