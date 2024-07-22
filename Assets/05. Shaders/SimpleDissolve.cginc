struct Input
{
    float2 uv_MainTex;
    float2 uv_NoiseTex;
};

half _Cuton;
half _EdgeWidth;
fixed4 _EdgeColor;
fixed4 noisePixel, pixel;
sampler2D _NoiseTex;
UNITY_INSTANCING_BUFFER_START(Props)
UNITY_INSTANCING_BUFFER_END(Props)


void surf(Input i, inout SurfaceOutputStandard o)
{
    noisePixel = tex2D(_NoiseTex, i.uv_NoiseTex);
    clip(noisePixel.r >= (1 - _Cuton) ? 1 : -1);
    o.Emission = noisePixel.r >= ((1 - _Cuton) * (_EdgeWidth + 1.0)) ? 0 : _EdgeColor;
}