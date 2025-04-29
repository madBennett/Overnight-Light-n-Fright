Shader "CustomRenderTexture/MOBDAMAGE"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _FlashColor ("Flash Color", Color) = (1, 0, 0, 0.5)
        _Intensity ("Flash Intensity", Range(0, 1)) = 0.8
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _FlashColor;
            float _Intensity;

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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseColor = tex2D(_MainTex, i.uv);
                fixed4 overlay = _FlashColor;
                overlay.a *= _Intensity;

                return lerp(baseColor, overlay, overlay.a);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
