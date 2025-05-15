Shader "Custom/FloatEffectWithTexture"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _FloatStrength ("Float Strength", Float) = 0.5
        _FloatSpeed ("Float Speed", Float) = 0.1
    }
    SubShader
    {
        Tags {"Queue"="Overlay" "RenderType"="Opaque"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex; // To hold the chest's texture
            float _FloatStrength;
            float _FloatSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.color = v.color; // Retain the original color of the object

                // Modify the Y position to simulate floating.
                float offset = sin(_Time.y * _FloatSpeed) * _FloatStrength;
                v.vertex.y += offset;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Sample the texture for the chest and return the texture color
                half4 texColor = tex2D(_MainTex, i.texcoord);
                return texColor * i.color; // Apply original color along with texture
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
