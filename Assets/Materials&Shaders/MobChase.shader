Shader "CustomRenderTexture/MOBCHASE"
{
    Properties
    {
        // Motion Lines Properties
        _MainTex ("Screen", 2D) = "black" {}
        _Colour("Colour", Color) = (1,1,1,1)
        _SpeedLinesTiling("Speed Lines Tiling", Float) = 200
        _SpeedLinesRadialScale("Speed Lines Radial Scale", Range(0,10)) = 0.1
        _SpeedLinesPower("Speed Lines Power", Float) = 1
        _SpeedLinesRemap("Speed Lines Remap", Range(0,1)) = 0.8
        _SpeedLinesAnimation("Speed Lines Animation", Float) = 3
        _MaskScale("Mask Scale", Range(0,2)) = 1
        _MaskHardness("Mask Hardness", Range(0,1)) = 0
        _MaskPower("Mask Power", Float) = 5
        [HideInInspector] _texcoord("", 2D) = "white" {}

        // Snow Properties
        _NoiseSpeed("Noise Speed", Float) = 1.0
        _NoiseTimeScale("Noise Time Scale", Float) = 1.0
        _NoiseOpacity("Noise Opacity", Range(0,1)) = 0.5
        _NoiseScale("Noise UV Scale", Float) = 5.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 100

        Pass
        {
            Name "MOBCHASEPass"
            Tags { "LightMode" = "UniversalForward" }

            Cull Off
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Motion Lines Variables
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;
            float _SpeedLinesRadialScale;
            float _SpeedLinesTiling;
            float _SpeedLinesAnimation;
            float _SpeedLinesPower;
            float _SpeedLinesRemap;
            float _MaskScale;
            float _MaskHardness;
            float _MaskPower;
            float4 _Colour;

            // Snow Variables
            float _NoiseSpeed;
            float _NoiseTimeScale;
            float _NoiseOpacity;
            float _NoiseScale;

            // --- Simplex Noise Helper Functions ---
            float3 mod2D289(float3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
            float2 mod2D289(float2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
            float3 permute(float3 x) { return mod2D289(((x * 34.0) + 1.0) * x); }
            float snoise(float2 v)
            {
                const float4 C = float4(0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439);
                float2 i = floor(v + dot(v, C.yy));
                float2 x0 = v - i + dot(i, C.xx);
                float2 i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
                float4 x12 = x0.xyxy + C.xxzz;
                x12.xy -= i1;
                i = mod2D289(i);
                float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0)) + i.x + float3(0.0, i1.x, 1.0));
                float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
                m = m * m; m = m * m;
                float3 x = 2.0 * frac(p * C.www) - 1.0;
                float3 h = abs(x) - 0.5;
                float3 ox = floor(x + 0.5);
                float3 a0 = x - ox;
                m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);
                float3 g;
                g.x = a0.x * x0.x + h.x * x0.y;
                g.yz = a0.yz * x12.xz + h.yz * x12.yw;
                return 130.0 * dot(m, g);
            }
            // --------------------------------------

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // Base Scene Color
                float2 uv_MainTex = i.uv;
                float4 SceneColour7 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv_MainTex);

                // Snow Effect
                float2 snowUV = i.uv * _NoiseScale;
                float timeFactor = frac(_Time.y * _NoiseSpeed * _NoiseTimeScale);
                float2 timedUV = snowUV + float2(timeFactor, timeFactor);
                float n = frac(sin(dot(timedUV, float2(12.9898, 78.233))) * 43758.5453);
                float4 snowColor = float4(n, n, n, _NoiseOpacity);
                float4 sceneWithSnow = lerp(SceneColour7, snowColor, _NoiseOpacity);

                // Motion Lines Calculation
                float2 CenteredUV15_g1 = (i.uv - float2(0.5, 0.5));
                float2 break17_g1 = CenteredUV15_g1;
                float2 appendResult23_g1 = float2(
                    (length(CenteredUV15_g1) * _SpeedLinesRadialScale * 2.0),
                    (atan2(break17_g1.x, break17_g1.y) * (1.0 / 6.28318548202515) * _SpeedLinesTiling)
                );
                float2 appendResult58 = float2((-_SpeedLinesAnimation * _Time.y), 0.0);

                float simplePerlin2D10 = snoise(appendResult23_g1 + appendResult58);
                simplePerlin2D10 = simplePerlin2D10 * 0.5 + 0.5;
                float temp_output_1_0_g6 = _SpeedLinesRemap;
                float SpeedLines21 = saturate((pow(simplePerlin2D10, _SpeedLinesPower) - temp_output_1_0_g6) / (1.0 - temp_output_1_0_g6));
                
                // Motion Mask
                float2 texCoord60 = i.uv * float2(2, 2) + float2(-1, -1);
                float temp_output_1_0_g5 = _MaskScale;
                float lerpResult71 = lerp(0.0, _MaskScale, _MaskHardness);
                float Mask24 = pow((1.0 - saturate(((length(texCoord60) - temp_output_1_0_g5) / ((lerpResult71 - 0.001) - temp_output_1_0_g5)))), _MaskPower);
                float MaskedSpeedLines29 = (SpeedLines21 * Mask24);

                // Final Composition
                float3 ColourRGB38 = _Colour.rgb;
                float ColourA40 = _Colour.a;
                float4 lerpResult2 = lerp(
                    sceneWithSnow,
                    float4((MaskedSpeedLines29 * ColourRGB38), 0.0),
                    (MaskedSpeedLines29 * ColourA40)
                );

                return lerpResult2;
            }
            ENDHLSL
        }
    }
    CustomEditor "ASEMaterialInspector"
}
