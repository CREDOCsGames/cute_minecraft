// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TranslucentCrystals/Crystal"
{
	Properties
	{
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TranslucencyMask("Translucency Mask", 2D) = "white" {}
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		_TranslucencyMaskPower("Translucency Mask Power", Range( 0 , 1)) = 1
		_AlbedoMask("Albedo Mask", 2D) = "white" {}
		_ColorTint1("Color Tint 1", Color) = (1,1,1,1)
		_ColorTint2("Color Tint 2", Color) = (1,1,1,1)
		_Normal("Normal", 2D) = "bump" {}
		_Emission("Emission", 2D) = "white" {}
		_EmissionColor("Emission Color", Color) = (1,1,1,1)
		_EmissionPower("Emission Power", Range( 0 , 10)) = 2
		[Toggle]_RampEnabled("Ramp Enabled", Int) = 0
		[Toggle]_RampInverted("Ramp Inverted", Int) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_RampMask("Ramp Mask", 2D) = "white" {}
		_MetallicSmoothness("MetallicSmoothness", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1

        _NoiseTex ("Noise", 2D) = "white" {}
        _Cuton ("Cut on", Range(0, 1)) = 0
        _EdgeWidth ("Edge Width", Range(0, 1)) = 0.05
        [HDR] _EdgeColor ("Edge Color", Color) = (1,1,1,1)

        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _RAMPINVERTED_ON
		#pragma shader_feature _RAMPENABLED_ON
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
    float2 uv_NoiseTex;
};

		struct SurfaceOutputStandardCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			fixed3 Translucency;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _ColorTint1;
		uniform float4 _ColorTint2;
		uniform sampler2D _AlbedoMask;
		uniform float4 _AlbedoMask_ST;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform sampler2D _Ramp;
		uniform sampler2D _RampMask;
		uniform float4 _RampMask_ST;
		uniform float _EmissionPower;
		uniform float4 _EmissionColor;
		uniform sampler2D _MetallicSmoothness;
		uniform float4 _MetallicSmoothness_ST;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform sampler2D _TranslucencyMask;
		uniform float4 _TranslucencyMask_ST;
		uniform float _TranslucencyMaskPower;

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			UNITY_GI(gi, s, data);
		}
half _Cuton;
half _EdgeWidth;
fixed4 _EdgeColor;
fixed4 noisePixel, pixel;
sampler2D _NoiseTex;
UNITY_INSTANCING_BUFFER_START(Props)
UNITY_INSTANCING_BUFFER_END(Props)
		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_AlbedoMask = i.uv_texcoord * _AlbedoMask_ST.xy + _AlbedoMask_ST.zw;
			float4 lerpResult17 = lerp( _ColorTint1 , _ColorTint2 , tex2D( _AlbedoMask, uv_AlbedoMask ).r);
			o.Albedo = lerpResult17.rgb;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float4 tex2DNode5 = tex2D( _Emission, uv_Emission );
			float2 uv_RampMask = i.uv_texcoord * _RampMask_ST.xy + _RampMask_ST.zw;
			float4 tex2DNode22 = tex2D( _RampMask, uv_RampMask );
			#ifdef _RAMPINVERTED_ON
				float staticSwitch26 = ( 1.0 - tex2DNode22.r );
			#else
				float staticSwitch26 = tex2DNode22.r;
			#endif
			float2 appendResult23 = (float2(staticSwitch26 , 0.0));
			#ifdef _RAMPENABLED_ON
				float4 staticSwitch20 = ( tex2DNode5 * tex2D( _Ramp, appendResult23 ) * _EmissionPower );
			#else
				float4 staticSwitch20 = ( tex2DNode5 * _EmissionColor * _EmissionPower );
			#endif
			o.Emission = staticSwitch20.rgb;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 tex2DNode12 = tex2D( _MetallicSmoothness, uv_MetallicSmoothness );
			o.Metallic = ( tex2DNode12.r * _Metallic );
			o.Smoothness = ( tex2DNode12.a * _Smoothness );
			float2 uv_TranslucencyMask = i.uv_texcoord * _TranslucencyMask_ST.xy + _TranslucencyMask_ST.zw;
			float3 temp_cast_2 = (( tex2D( _TranslucencyMask, uv_TranslucencyMask ).r * _TranslucencyMaskPower )).xxx;
			o.Translucency = temp_cast_2;
			o.Alpha = 1;
	
			noisePixel = tex2D(_NoiseTex, i.uv_NoiseTex);
			clip(noisePixel.r >= (1 - _Cuton) ? 1 : -1);
			o.Emission = noisePixel.r >= ((1 - _Cuton) * (_EdgeWidth + 1.0)) ? 0 : _EdgeColor;
}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}