Shader "OutLine/SolidColor"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1) // 主颜色
        _EnableOutline ("Enable Outline", Float) = 0 // 是否开启轮廓
        _OutlineColor ("Outline Color", Color) = (0,1,0,1) // 轮廓颜色
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.02 // 轮廓宽度
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        // 轮廓Pass（只在开启时渲染）
        Pass
        {
            Name "OUTLINE"
            Cull Front
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ENABLEOUTLINE_ON
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
            };
            
            float _OutlineWidth;
            float4 _OutlineColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                #ifdef _ENABLEOUTLINE_ON
                    v.vertex.xyz += v.normal * _OutlineWidth;
                #endif
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                #ifdef _ENABLEOUTLINE_ON
                    return _OutlineColor;
                #else
                    discard;
                    return 0;
                #endif
            }
            ENDCG
        }
        
        // 主颜色Pass
        Pass
        {
            Name "MAINCOLOR"
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
            };
            
            float4 _MainColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                return _MainColor;
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}