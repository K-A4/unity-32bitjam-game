// Upgrade NOTE: commented out 'half4 unity_LightmapST', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Unlit/SH_Unlit"
{
    Properties
    {
        _Color("_Color", Color) = (1.0,1.0,1.0,1.0)
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("_Scae ", float) = 1.0
        _Power("Power ", float) = 1.0
        _Affine("Affine", Range(0, 1.0)) = 1.0
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
            #pragma multi_compile multi_compile_fog LIGHTMAP_ON LIGHTMAP_OFF
            //#pragma multi_compile_fog

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
            #ifdef LIGHTMAP_ON
                        //half4 unity_LightmapST;
                        //sampler2D_half unity_Lightmap;
            #endif
            sampler2D _MainTex;
            float _Scale;
            float _Power;
            float _Affine;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex = (floor(o.vertex * _Scale) / _Scale * _Affine) + ((1 - _Affine) * o.vertex);
                float2 uvTransformed = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = uvTransformed * (o.vertex.w) * _Affine + ((1 - _Affine) * uvTransformed);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                float2 uv = _Affine * (i.uv / i.vertex.w) + ((1 - _Affine) * i.uv);
                fixed4 col = tex2D(_MainTex, uv) * _Color;
#ifdef LIGHTMAP_ON
                col.rgb *= (DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv[1]))) * _Power;
#endif
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
            Fallback "Diffuse"

}
