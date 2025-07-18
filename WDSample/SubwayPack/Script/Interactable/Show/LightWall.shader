Shader "LightWall/GlowingWhiteWall"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range(1, 10)) = 2
        _GlowFalloff ("Glow Falloff", Range(0.1, 5)) = 1
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        
        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
        };
        
        fixed4 _MainColor;
        float _GlowIntensity;
        float _GlowFalloff;
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // 纯白色基础颜色
            o.Albedo = _MainColor.rgb;
            
            // 计算基于视角的发光强度
            float3 viewDir = normalize(_WorldSpaceCameraPos - IN.worldPos);
            float glow = pow(saturate(dot(viewDir, IN.worldNormal)), _GlowFalloff);
            
            // 纯白光发射
            o.Emission = _MainColor.rgb * glow * _GlowIntensity;
            
            o.Metallic = 0;
            o.Smoothness = 0;
            o.Alpha = _MainColor.a;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}