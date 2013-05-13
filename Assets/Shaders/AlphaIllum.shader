Shader "Custom/Illum" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Illum ("Illumin", 2D) = "white" {}
		_EmissionPower ("Emissive Power", Range(0, 1)) = 0
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		sampler2D _Illum;
		fixed4 _Color;
		float _EmissionPower;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Illum;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 i = tex2D(_Illum, IN.uv_Illum) * _Color;
			o.Albedo = c.rgb;
			o.Emission = i.rgb * _EmissionPower;
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Transparent/VertexLit"
}