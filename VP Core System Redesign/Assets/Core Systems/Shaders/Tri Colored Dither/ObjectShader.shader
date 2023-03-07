Shader "ObjectShader"
{
    Properties 
    {
        _surfaceColor ("surface color", Color) = (0.4, 0.1, 0.9)

        _resolution ("dither resolution", Float) = 256
        _MainTex ("render texture", 2D) = "white"{}
        _ditherPattern ("dither pattern", 2D) = "gray" {}

        _ditherColor ("dither color", Color) = (0, 0, 0)
        _ditherColor2 ("dither color 2", Color) = (0, 0, 0)
        _ditherColor3 ("dither color 3", Color) = (0, 0, 0)

        _specthreshold ("specular threshold", Range(-0.5, 0.5)) = 0
        _specOpacity ("specular opacity", Range(0, 1)) = 0

        _threshold ("dither threshold 1", Range(-0.5, 0.5)) = 0
        _threshold2 ("dither threshold 2", Range(-0.5, 0.5)) = 0
        _threshold3 ("dither threshold 3", Range(-0.5, 0.5)) = 0

        _fogMin ("fog minimum", Range(.5, 1)) = 0
        _fogMax ("fog maximum", Range(.5, 1)) = 0

        _skyColor ("sky color", Color) = (0, 0, 0)
        _groundColor ("ground color", Color) = (0, 0, 0)
        

    }
    SubShader
    {
        // this tag is required to use _LightColor0
        Tags { "LightMode"="ForwardBase" }



        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _CameraDepthTexture;

            // might be UnityLightingCommon.cginc for later versions of unity
            #include "Lighting.cginc"

            // #define DIFFUSE_MIP_LEVEL 5
            // #define SPECULAR_MIP_STEPS 4
            #define MAX_SPECULAR_POWER 256
            
            float3 _surfaceColor;

            float _resolution;
            sampler2D _MainTex;
            sampler2D _ditherPattern; float4 _ditherPattern_TexelSize;

            float3 _ditherColor;
            float3 _ditherColor2;
            float3 _ditherColor3;
            
            float _specthreshold;
            float _specOpacity;

            float _threshold;
            float _threshold2;
            float _threshold3;

            float _fogMin;
            float _fogMax;

            float3 _skyColor;
            float3 _groundColor;
            

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float2 uv : TEXCOORD0;
                float3 objectWorldPos : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float4 color: COLOR;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;

                o.normal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.objectWorldPos = mul(unity_ObjectToWorld, v.vertex);

                o.screenPos = ComputeScreenPos(o.vertex);

                //o.color = v.color;
                o.uv = v.uv;

                //o.color.xyz = v.normal * 0.5 + 0.5;
               // o.color.w = 1.0;

                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float3 color = 0;
                float2 uv = i.uv * 2 - 1;
                float3 normal = normalize(i.normal);
                //float mip = (1 - _gloss) * SPECULAR_MIP_STEPS;
                float3 w = float3(0.299, 0.587, 0.144);

                //normals gradients for sky & ground overlays
                float3 topNormals = saturate(float3(i.normal.y, i.normal.y, i.normal.y) * _skyColor);
                float3 bottomNormals = saturate(float3(-i.normal.y, -i.normal.y, -i.normal.y) * _groundColor);

                //calculate screen space at which we dither
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float2 ditherUV = screenUV * floor(_resolution);
                float screenpattern = tex2D(_ditherPattern, ditherUV);

                //direction from which the light is going to the object
                float3 lightDirection = _WorldSpaceLightPos0;
                float3 lightColor = _LightColor0; // includes intensity

                //direction from which we are viewing object, inverted to get reflection
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.objectWorldPos);
                float3 viewReflection = reflect(-viewDirection, normal);

                //float3 indirectSpecular = texCUBElod(_IBL, float4(viewReflection, mip));
                float3 halfDirection = normalize(viewDirection + lightDirection);

                float directDiffuse = max(0, dot(normal, lightDirection)); //first diffuse band


                float specular = max(0, dot(normal, halfDirection));

                float specularBand = step(screenpattern, specular - _specthreshold);
                float3 specularBandRender = saturate(specularBand.rrr);
                
                float3 diffuse = saturate(directDiffuse * lightColor);

                //This is for rendering the noise on mesh uvs
                // float meshpattern = tex2D(_ditherPattern, uv);
                // float ditherShadingSurface = step(meshpattern, grayscale + _threshold);
                // float ditherSurfaceRender = saturate(ditherShadingSurface + _ditherColor + _shadowOpacity * 2);

                float ditherBand1 = step(screenpattern, directDiffuse + _threshold);
                float3 ditherBandRender1 = saturate(ditherBand1.rrr + _ditherColor);


                float ditherBand2 = step(screenpattern, directDiffuse + _threshold2);
                float3 ditherBandRender2 = saturate(ditherBand2.rrr + _ditherColor2);

                float ditherBand3 = step(screenpattern, directDiffuse + _threshold3);
                float3 ditherBandRender3 = saturate(ditherBand3.rrr + _ditherColor3);

                float3 ditherBands = saturate(ditherBandRender1 * ditherBandRender2 * ditherBandRender3);
                
                color = ditherBands;

                ditherBands = saturate(ditherBands + saturate(topNormals + bottomNormals));

                //return float4(ditherBands, 1.0);

                //float uvedge = step(.9, i.vertex.x);
                
                //return float4(i.color);
                //float2 uv = i.uv * 2 - 1;


                float3 fog = (1 - Linear01Depth(tex2D(_CameraDepthTexture, screenUV)));
                fog = smoothstep(_fogMin, _fogMax, fog);
                //return float4(fog, 1.0);
                fog = saturate(fog + _skyColor);

                //return float4(fog, 1.0);
                //fog = fog * _skyColor;
                //return float4(color, 1.0);
                
                color = ditherBands * _surfaceColor + (specularBandRender * _specOpacity);

                return float4(color * fog, 1.0);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
