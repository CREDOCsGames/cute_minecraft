Shader"Custom/DistortionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Float) = 0.1
        _Alpha("Alpha", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

Blend SrcAlpha OneMinusSrcAlpha

ZWrite Off

AlphaToMask Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

sampler2D _MainTex;
float _DistortionStrength;
float _Alpha;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;

                // Distortion effect
    float wave = sin(_Time.y * 5.0 + v.uv.x * 10.0) * cos(_Time.y * 5.0 + v.uv.y * 10.0);
    o.uv.x += wave * _DistortionStrength;
    o.uv.y += wave * _DistortionStrength;

    return o;
}

half4 frag(v2f i) : SV_Target
{
    half4 col = tex2D(_MainTex, i.uv);
        col.a = _Alpha;
    
    return col;
}
            ENDCG
        }
    }
FallBack"Transparent"
}
