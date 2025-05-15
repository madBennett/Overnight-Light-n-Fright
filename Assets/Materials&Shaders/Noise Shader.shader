//Madison Bennett
//CS 596

Shader "Unlit/Noise Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AnimateXY("AnimateXY", Vector) = (0,-0.5,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _AnimateXY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Offset by set vector per amount of time
                i.uv += frac(_AnimateXY.xy * _Time.y);

                // sample the texture and apply a noise function to it
                fixed4 col = tex2D(_MainTex, i.uv) + frac(sin(dot(i.uv,float2(12.9898,78.233)))*43758.5453);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
