Shader "UI/TextOutlineUGUI"
{
    Properties
    {
        _MainTex ("Font Texture", 2D) = "white" {}
        _FaceColor ("Text Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 0.1)) = 0.05
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        // ----- Pass 1: Outline -----
        Pass
        {
            Name "OUTLINE"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _OutlineColor;
            float _OutlineThickness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float alpha = 0.0;
                float2 uv = i.uv;

                float2 offsets[8] = {
                    float2( _OutlineThickness,  0),
                    float2(-_OutlineThickness,  0),
                    float2( 0,  _OutlineThickness),
                    float2( 0, -_OutlineThickness),
                    float2( _OutlineThickness,  _OutlineThickness),
                    float2(-_OutlineThickness,  _OutlineThickness),
                    float2( _OutlineThickness, -_OutlineThickness),
                    float2(-_OutlineThickness, -_OutlineThickness)
                };

                for (int j = 0; j < 8; j++)
                {
                    alpha = max(alpha, tex2D(_MainTex, uv + offsets[j]).a);
                }

                fixed4 col = _OutlineColor;
                col.a *= alpha;
                col.rgb *= col.a; // Premultiply Alpha

                return col;
            }
            ENDCG
        }

        // ----- Pass 2: Main Text -----
        Pass
        {
            Name "TEXT"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _FaceColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float alpha = tex2D(_MainTex, i.uv).a;
                fixed4 col = _FaceColor;
                col.a *= alpha;
                col.rgb *= col.a; // Premultiply Alpha

                return col;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}