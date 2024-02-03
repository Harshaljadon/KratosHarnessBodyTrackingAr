Shader "Custom/NewSurfaceShader"
{
   Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _GrayColor ("Gray Color", Color) = (0.5, 0.5, 0.5, 0.5)
        _ScanRegion ("Scan Region", Vector) = (0, 0, 400, 400)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _GrayColor;
        float4 _ScanRegion;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Add the gray overlay effect to the specified scan region
            clip(IN.uv_MainTex.x < _ScanRegion.x || IN.uv_MainTex.x > _ScanRegion.z || IN.uv_MainTex.y < _ScanRegion.y || IN.uv_MainTex.y > _ScanRegion.w);
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _GrayColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
