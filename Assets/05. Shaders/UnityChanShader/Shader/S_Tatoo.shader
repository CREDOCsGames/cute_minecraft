// Made with Amplify Shader Editor v1.9.4.4
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New Amplify Shader"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_BaseColor("Base Color", Color) = (0.5471698,0.5471698,0.5471698,0)
		_Precision("Precision", Float) = 0.31
		_OpacityRange("OpacityRange", 2D) = "white" {}
		_OpacityRangeMultiply("OpacityRangeMultiply", Float) = 3
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _BaseColor;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform float _Precision;
		uniform sampler2D _OpacityRange;
		uniform float4 _OpacityRange_ST;
		uniform float _OpacityRangeMultiply;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float temp_output_3_0 = step( tex2D( _MainTexture, uv_MainTexture ).b , _Precision );
			o.Albedo = ( _BaseColor * temp_output_3_0 ).rgb;
			o.Alpha = 1;
			float2 uv_OpacityRange = i.uv_texcoord * _OpacityRange_ST.xy + _OpacityRange_ST.zw;
			clip( ( temp_output_3_0 * ( tex2D( _OpacityRange, uv_OpacityRange ) * _OpacityRangeMultiply ) ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19404
Node;AmplifyShaderEditor.SamplerNode;1;-912,160;Inherit;True;Property;_MainTexture;MainTexture;0;0;Create;True;0;0;0;False;0;False;-1;8bf41584f63c1ce438646a267dcf1b8f;8bf41584f63c1ce438646a267dcf1b8f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-752,400;Inherit;False;Property;_Precision;Precision;3;0;Create;True;0;0;0;False;0;False;0.31;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1056,544;Inherit;True;Property;_OpacityRange;OpacityRange;4;0;Create;True;0;0;0;False;0;False;-1;8c4a7fca2884fab419769ccc0355c0c1;8c4a7fca2884fab419769ccc0355c0c1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-1040,784;Inherit;False;Property;_OpacityRangeMultiply;OpacityRangeMultiply;5;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;3;-512,240;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-704,624;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;7;-666.2551,-73.73044;Inherit;False;Property;_BaseColor;Base Color;2;0;Create;True;0;0;0;False;0;False;0.5471698,0.5471698,0.5471698,0;0.5471698,0.5471698,0.5471698,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-352,480;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-336,-32;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;New Amplify Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;3
WireConnection;3;1;4;0
WireConnection;13;0;10;0
WireConnection;13;1;14;0
WireConnection;12;0;3;0
WireConnection;12;1;13;0
WireConnection;8;0;7;0
WireConnection;8;1;3;0
WireConnection;0;0;8;0
WireConnection;0;10;12;0
ASEEND*/
//CHKSM=ED3F3C31628F351AB26D3D9E96DFDB471BB416CD