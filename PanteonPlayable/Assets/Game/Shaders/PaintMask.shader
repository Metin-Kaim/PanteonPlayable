Shader "Custom/PaintMask_GrayEdge"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _MaskTex("Mask Texture", 2D) = "white" {}
        _EdgeColor("Edge Color", Color) = (0.3,0.3,0.3,1)
        _EdgeWidth("Edge Width", Range(0.001,0.01)) = 0.003
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
            #include "UnityCG.cginc"

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

            sampler2D _MaskTex;
            float4 _BaseColor;
            float4 _EdgeColor;
            float _EdgeWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 paintCol = tex2D(_MaskTex, i.uv);

                // Eðer boyanmamýþsa baseColor
                if (paintCol.a < 0.001)
                    return _BaseColor;

                // Kenar tespiti: komþular alpha == 0 ise
                float left = tex2D(_MaskTex, i.uv + float2(-_EdgeWidth, 0)).a;
                float right = tex2D(_MaskTex, i.uv + float2(_EdgeWidth, 0)).a;
                float up = tex2D(_MaskTex, i.uv + float2(0, _EdgeWidth)).a;
                float down = tex2D(_MaskTex, i.uv + float2(0, -_EdgeWidth)).a;

                bool isEdge = (left < 0.001 || right < 0.001 || up < 0.001 || down < 0.001);

                if (isEdge)
                    return _EdgeColor; // Kenar rengi sabit gri
                else
                    return paintCol;   // Ýç kýsým boyanýn rengi
            }
            ENDCG
        }
    }
}
