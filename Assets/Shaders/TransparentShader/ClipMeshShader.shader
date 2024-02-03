Shader "Custom/ClipMeshShader"
{
     Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Alpha ("Alpha", Range(0, 1)) = 1.0
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
       _Plane0 ("Top", Vector) = (0,0,0,0)
       _Plane1 ("Bottom", Vector) = (0,0,0,0)
       _Plane2 ("Left", Vector) = (0,0,0,0)
       _Plane3 ("Right", Vector) = (0,0,0,0)
       _Plane4 ("Front", Vector) = (0,0,0,0)
       _Plane5 ("Back", Vector) = (0,0,0,0)
        _Transparency ("Transparency", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"  }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite off
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha:fade fullforwardshadows keepalpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
       float4 _Plane0;
       float4 _Plane1;
       float4 _Plane2;
       float4 _Plane3;
       float4 _Plane4;
       float4 _Plane5;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        half _Transparency;
        float _Alpha;


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
       float p0 = dot(_Plane0.xyz, IN.worldPos) + _Plane0.w;
          float p1 = dot(_Plane1.xyz, IN.worldPos) + _Plane1.w;
          float p2 = dot(_Plane2.xyz, IN.worldPos) + _Plane2.w;
          float p3 = dot(_Plane3.xyz, IN.worldPos) + _Plane3.w;
          float p4 = dot(_Plane4.xyz, IN.worldPos) + _Plane4.w;
          float p5 = dot(_Plane5.xyz, IN.worldPos) + _Plane5.w;
          if(p0<0 && p1<0 && p2<0 && p3<0 && p4<0 && p5<0)
              clip(-1);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            c.a = _Alpha;

            o.Alpha = c.a * _Transparency;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
