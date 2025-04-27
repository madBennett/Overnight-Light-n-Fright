Shader "Unlit/MobChase"
{
    Properties
    {
        // comment
        _MainTex ("Texture", 2D) = "red" {} // default texture image
        _NoiseSpeed ("Noise Speed", Float) = 1.0 // speed for noise movement
        _NoiseTimeScale ("Noise Time Scale", Float) = 1.0 // time scale modifier for noise speed
        _NoiseOpacity ("Noise Opacity", Range(0,1)) = 0.5 // noise opacity
        _NoiseScale ("Noise UV Scale", Float) = 5.0 // scales UVs for noise variation
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } // render type
        LOD 100 // detail setting level
        Blend SrcAlpha OneMinusSrcAlpha // blending mode for transparency

        Pass
        {
            CGPROGRAM
            #pragma vertex vert // vertex shader function
            #pragma fragment frag // fragment shader function
            #pragma multi_compile_fog

            #include "UnityCG.cginc" // Unity's shader utilities

            // Vertex input structure
            struct appdata
            {
                float4 vertex : POSITION; // vertex position of object
                float2 uv : TEXCOORD0; // texture coordinates
            };

            // Passes data from vertex shader to fragment shader
            struct v2f
            {
                float2 uv : TEXCOORD0; // UV coordinates after modification for rolling effect
                float2 noiseUV : TEXCOORD1; // UV coordinates used for noise effect
                UNITY_FOG_COORDS(2) // coordinates for fog calculations
                float4 vertex : SV_POSITION; // new vertex position
            };

            // Texture samplers
            sampler2D _MainTex; // default texture image
            float4 _MainTex_ST; // texture scaling and transformation

            // Shader properties
            float _NoiseSpeed; // speed for noise movemen
            float _NoiseTimeScale; // time scale modifier for noise speed
            float _NoiseOpacity; // noise opacity
            float _NoiseScale; // scales UVs for noise variation

            // Noise function with time factor to control movement speed
            float noise(float2 p)
            {
                // Uses time-based factor to animate the noise and keep it in a [0,1] range
                float timeFactor = frac(_Time.y * _NoiseSpeed * _NoiseTimeScale);
                // Simple noise generation based on sine wave, dot product, and a large prime number
                return frac(sin(dot(p + timeFactor, float2(12.9898, 78.233))) * 43758.5453); // algorithm given to us in class on WolframAlpha
            }

            // Vertex shader function 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // vertex to clip
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                // Separate UVs for noise
                o.noiseUV = v.uv * _NoiseScale;

                // Calculate fog
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            // Fragment shader function
            fixed4 frag (v2f i) : SV_Target
            {
                // Use the separate noise UVs to create noise effect
                float n = noise(i.noiseUV);

                // Sample the main texture using the transformed UVs
                fixed4 texColor = tex2D(_MainTex, frac(i.uv)); // Ensure UVs stay within [0,1]

                // Grayscale noise color with opacity
                fixed4 noiseColor = fixed4(n, n, n, _NoiseOpacity);

                // Lerp between the texture color and the noise color based on opacity
                fixed4 col = lerp(texColor, noiseColor, _NoiseOpacity);

                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                // Output final color
                return col;
            }
            ENDCG
        }
    }
}
