Shader "CustomRenderTexture/MOBDAMAGE"
{
    Properties
    {
        _FlashColor ("Flash Color", Color) = (1, 1, 1, 1)
        _Intensity ("Flash Intensity", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Cull Off
            Lighting Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _FlashColor;
            float _Intensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 flash = _FlashColor;
                flash.a *= _Intensity;
                return flash;
            }
            ENDCG
        }
    }

    FallBack Off
}
