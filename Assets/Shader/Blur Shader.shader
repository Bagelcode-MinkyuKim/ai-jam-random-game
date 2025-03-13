Shader "UI/BlurForUGUI"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
        _Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        LOD 200
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _BlurSize;
            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 offset = _MainTex_TexelSize.xy * _BlurSize;

                // 가우시안 가중치
                fixed weightCenter = 0.227027f;
                fixed weight1 = 0.1945946f;
                fixed weight2 = 0.1216216f;
                fixed weight3 = 0.054054f;

                fixed4 col = tex2D(_MainTex, uv) * weightCenter;
                col += tex2D(_MainTex, uv + offset * float2(1, 0)) * weight1;
                col += tex2D(_MainTex, uv - offset * float2(1, 0)) * weight1;
                col += tex2D(_MainTex, uv + offset * float2(2, 0)) * weight2;
                col += tex2D(_MainTex, uv - offset * float2(2, 0)) * weight2;
                col += tex2D(_MainTex, uv + offset * float2(3, 0)) * weight3;
                col += tex2D(_MainTex, uv - offset * float2(3, 0)) * weight3;
                col += tex2D(_MainTex, uv + offset * float2(0, 1)) * weight1;
                col += tex2D(_MainTex, uv - offset * float2(0, 1)) * weight1;
                col += tex2D(_MainTex, uv + offset * float2(0, 2)) * weight2;
                col += tex2D(_MainTex, uv - offset * float2(0, 2)) * weight2;
                col += tex2D(_MainTex, uv + offset * float2(0, 3)) * weight3;
                col += tex2D(_MainTex, uv - offset * float2(0, 3)) * weight3;

                col *= i.color;
                return col;
            }
            ENDCG
        }
    }
}