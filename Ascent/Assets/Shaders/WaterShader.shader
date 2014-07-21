Shader "Ascent/WaterShader"
{
    Properties 
    {
		_Color ("Main Color", Color) = (1,1,1,1)
		_ColorTint ("Tint", Color) = (1.0, 0.6, 0.6, 1.0)
        _MainTex ("Texture", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
    }
  
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Off
        CGPROGRAM
		// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
		#pragma exclude_renderers gles
  
        #pragma surface surf Lambert vertex:vert finalcolor:mycolor

  
        struct Input 
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 customColor;
        };

		struct VertexOutput
		{
			float4 position;
			float2 tex;
			float4 reflectionPosition;
			float4 refractionPosition;
		};

		fixed4 _ColorTint;
		float4 _Color;
		half _Scale;
		half _Speed;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		float _Amount;

		void vert (inout appdata_full v, out Input o) 
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
		}

		void mycolor (Input IN, SurfaceOutput o, inout fixed4 color)
		{
			color *= _ColorTint;
		}
  
        void surf (Input IN, inout SurfaceOutput o) 
        {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
        }
  
        ENDCG
    }
  
    FallBack "Diffuse"
}