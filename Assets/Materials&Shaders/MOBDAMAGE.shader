Shader "CustomRenderTexture/MOBDAMAGE"
{
    Properties
    {
        _MainTex ("Screen", 2D) = "black" {}
        _OverlayColor ("Overlay Color", Color) = (0, 0, 0, 1) // Full black
        _OverlayAlpha ("Overlay Alpha", Range(0,1)) = 1.0
        [HideInInspector] _texcoord("", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 100

        Pass
        {
            Name "MOBDAMAGEPass"
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

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            float4 _OverlayColor;
            float _OverlayAlpha;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 overlay = _OverlayColor;
                overlay.a *= _OverlayAlpha;

                return lerp(sceneColor, overlay, overlay.a);
            }
            ENDHLSL
        }
    }

    FallBack Off
}