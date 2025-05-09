Shader "Unlit/HauntedShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _GhostColor ("Ghost Color", Color) = (.5, .5, .5, 1)
        _FlickerSpeed ("Flicker Speed", Range(0.1, 10)) = 1.0
        _DistortionStrength ("Distortion Strength", Range(0.0, 0.1)) = 0.05
        _GlowIntensity ("Glow Intensity", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Declare the texture and sampler for _MainTex
            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Properties for the haunted effect
            float _FlickerSpeed;
            float _DistortionStrength;
            float4 _GhostColor;
            float _GlowIntensity;

            // Random function for jittering effect
            float rand(float2 seed)
            {
                return frac(sin(dot(seed, float2(12.9898, 78.233))) * 43758.5453);
            }

            // Vertex Shader
            v2f vert(appdata v)
            {
                v2f o;

                // Time-based distortion effect
                float2 distortion = float2(
                    rand(v.vertex.xy + _Time.y * _FlickerSpeed) * 2.0 - 1.0,
                    rand(v.vertex.yx + _Time.y * _FlickerSpeed) * 2.0 - 1.0
                );
                distortion *= _DistortionStrength;

                // Apply distortion to vertex position
                v.vertex.xy += distortion;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            // Fragment Shader
            half4 frag(v2f i) : SV_Target
            {
                // Sample the texture
                half4 color = tex2D(_MainTex, i.uv);
                
                // Apply the ghostly flickering color
                color.rgb += _GhostColor.rgb * (0.5 + sin(_Time.y * _FlickerSpeed) * 0.5);
                
                // Apply glow effect (faint, spectral glow)
                color.rgb += _GlowIntensity * sin(_Time.y * 0.5);
                
                return color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}
