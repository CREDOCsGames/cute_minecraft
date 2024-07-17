// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASESampleShaders/DissolveEffect"
{
	Properties
	{
		_AlbedoMix("Albedo Mix", Range( 0 , 1)) = 0.5
		_CharcoalMix("Charcoal Mix", Range( 0 , 1)) = 1
		EmberColorTint("Ember Color Tint", Color) = (0.9926471,0.6777384,0,1)
		Albedo("Albedo", 2D) = "white" {}
		Normals("Normals", 2D) = "bump" {}
		BaseEmber("Base Ember", Range( 0 , 1)) = 0
		GlowEmissionMultiplier("Glow Emission Multiplier", Range( 0 , 30)) = 1
		GlowColorIntensity("Glow Color Intensity", Range( 0 , 10)) = 0
		_BurnOffset("Burn Offset", Range( 0 , 5)) = 1
		_CharcoalNormalTile("Charcoal Normal Tile", Range( 2 , 5)) = 5
		_BurnTilling("Burn Tilling", Range( 0.1 , 1)) = 1
		GlowBaseFrequency("Glow Base Frequency", Range( 0 , 5)) = 1.1
		GlowOverride("Glow Override", Range( 0 , 10)) = 1
		Masks("Masks", 2D) = "white" {}
		BurntTileNormals("Burnt Tile Normals", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}

        _Cuton ("Cut on", Range(0, 1)) = 0
        _EdgeWidth ("Edge Width", Range(0, 1)) = 0.05
        [HDR] _EdgeColor ("Edge Color", Color) = (1,1,1,1)
        
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent" "IsEmissive" = "true"  }
        Cull[_Cull]
		ZTest LEqual
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		        #pragma surface surf Standard addshadow fullforwardshadows

        #ifndef SHADER_API_D3D11
            #pragma target 3.0
        #else
            #pragma target 4.0
        #endif

struct Input
{
    float2 uv_MainTex;
    float2 uv_NoiseTex;
};
		sampler2D _MainTex;
		sampler2D _NoiseTex;
		half _EdgeWidth;
		fixed4 _Color;
		fixed4 _EdgeColor;
		uniform sampler2D Normals;
		uniform sampler2D BurntTileNormals;
		uniform float _CharcoalNormalTile;
		uniform float _CharcoalMix;
		uniform sampler2D Masks;
		uniform float _BurnOffset;
		uniform float _BurnTilling;
		uniform sampler2D Albedo;
		uniform float _AlbedoMix;
		uniform float BaseEmber;
		uniform float4 EmberColorTint;
		uniform float GlowColorIntensity;
		uniform float GlowBaseFrequency;
		uniform float GlowOverride;
		uniform float GlowEmissionMultiplier;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

fixed4 noisePixel, pixel;
half _Cuton;


		void surf( Input i , inout SurfaceOutputStandard o )
		{
    float4 tex2DNode83 = tex2D(BurntTileNormals, (i.uv_MainTex * _CharcoalNormalTile));
			float4 appendResult182 = (float4(1.0 , tex2DNode83.g , 0.0 , tex2DNode83.r));
    float2 panner9 = (_BurnOffset * float2(1, 0.5) + (i.uv_MainTex * _BurnTilling));
			float4 tex2DNode98 = tex2D( Masks, panner9 );
			float temp_output_19_0 = ( _CharcoalMix + tex2DNode98.r );
    float3 lerpResult103 = lerp(UnpackNormal(tex2D(Normals, i.uv_MainTex)), UnpackNormal(appendResult182), temp_output_19_0);
			o.Normal = lerpResult103;
    float4 tex2DNode80 = tex2D(Albedo, i.uv_MainTex);
			float4 temp_cast_0 = (0.0).xxxx;
			float4 lerpResult28 = lerp( ( tex2DNode80 * _AlbedoMix ) , temp_cast_0 , temp_output_19_0);
			float4 lerpResult148 = lerp( ( float4(0.718,0.0627451,0,1) * ( tex2DNode83.a * 2.95 ) ) , ( float4(0.647,0.06297875,0,1) * ( tex2DNode83.a * 4.2 ) ) , tex2DNode98.g);
			float4 lerpResult152 = lerp( lerpResult28 , ( ( lerpResult148 * tex2DNode98.r ) * BaseEmber ) , ( tex2DNode98.r * 1.0 ));
			o.Albedo = lerpResult152.rgb;
			float4 temp_cast_2 = (0.0).xxxx;
			float4 temp_cast_3 = (100.0).xxxx;
			float4 clampResult176 = clamp( ( ( tex2DNode98.r * ( ( ( ( EmberColorTint * GlowColorIntensity ) * ( ( sin( ( _Time.y * GlowBaseFrequency ) ) * 0.5 ) + ( GlowOverride * ( tex2DNode98.r * tex2DNode83.a ) ) ) ) * tex2DNode98.g ) * tex2DNode83.a ) ) * GlowEmissionMultiplier ) , temp_cast_2 , temp_cast_3 );
			o.Smoothness = tex2DNode80.a;
	
    pixel = tex2D(_MainTex, i.uv_MainTex) * _Color;

    noisePixel = tex2D(_NoiseTex, i.uv_NoiseTex);
    clip(noisePixel.r >= (1 - _Cuton) ? 1 : -1);
    o.Emission = noisePixel.r >= ((1 - _Cuton) * (_EdgeWidth + 1.0)) ? 0 : _EdgeColor;


}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}