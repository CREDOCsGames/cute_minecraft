Shader"ASESampleShaders/SimpleDissolve"
{
	Properties
	{
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
		#include "SimpleDissolve.cginc"
		#pragma target 3.0
		        #pragma surface surf Standard addshadow fullforwardshadows

        #ifndef SHADER_API_D3D11
            #pragma target 3.0
        #else
            #pragma target 4.0
        #endif


		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}