Shader "Unlit/JitterEffect"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _JitterStrength ("Jitter Strength", Range(0.0, 1.0)) = 0.1
        _JitterSpeed ("Jitter Speed", Range(0.0, 3.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Declare the texture and sampler
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

            // Shader properties
            float _JitterStrength;
            float _JitterSpeed;

            // Random function for jittering
            float rand(float2 seed)
            {
                return frac(sin(dot(seed, float2(12.9898, 78.233))) * 43758.5453);
            }

            // Vertex Shader
            v2f vert(appdata v)
            {
                v2f o;

                // Time-based random jitter effect
                float jitter = rand(v.vertex.xy + _Time.y * _JitterSpeed) * 2.0 - 1.0; // Generates noise in range [-1, 1]
                jitter *= _JitterStrength; // Apply jitter strength

                // Apply jitter to vertex position
                v.vertex.xy += jitter;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            // Fragment Shader
            half4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv); // Sample the texture
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}
