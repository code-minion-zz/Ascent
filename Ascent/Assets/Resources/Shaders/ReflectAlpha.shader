Shader "Reflective/Transparent/Diffuse" {
	Properties {
	
//		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_ReflectColor ("Reflection Color", Color) = (1, 1, 1, 1)
		_Alpha ("Alpha", Range (0, 1)) = 1
		_MainTex ("Base Texture (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" {TexGen CubeReflect}
	}
	
	SubShader {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _Cube;
		float	_Alpha;
//		fixed4 _Color;
		fixed4 _ReflectColor;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldRefl;
			INTERNAL_DATA
		};
		
		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 c = tex;
			o.Albedo = c.rgb;
			
			fixed4 reflcol = texCUBE(_Cube, IN.worldRefl);
			reflcol *= tex.a;
			o.Emission = reflcol.rgb * _ReflectColor.rgb;
			c.a = c.a * _Alpha;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap ,IN.uv_BumpMap));
		}
		
		ENDCG
	}
	
	Fallback "Transparent/VertexLit"
} 