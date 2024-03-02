Shader "Custom/VisionShader"
{
    Properties
    {
        _VisionTex ("Vision Texture", 2D) = "white" {}
        _DimColor ("Dim Color", Color) = (0,0,0,0.5)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _VisionTex;
            float4 _VisionTex_ST;
            fixed4 _DimColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_VisionTex, i.uv);
                return lerp(_DimColor, col, col.a);
            }
            ENDCG
        }
    }
}