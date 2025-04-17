sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

uniform float uYOffset;
uniform float4 uColor;
uniform float2 uUVScale;
uniform float uWidth;


float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 coord : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(uImage0, coord);
	float4 noise= tex2D(uImage1, coord*uUVScale);
    return c * drawColor + noise*uColor*smoothstep(uWidth,0.0,abs(coord.y-uYOffset));
}
float4 PixelShaderFunction1(float4 drawColor : COLOR0,float2 coord : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(uImage0, coord);
	float4 noise= tex2D(uImage1, coord*uUVScale);
    return noise*uColor*c;
}
technique Technique1 
{
	pass BlockNoise0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
	pass BlockNoise1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction1();
	}
}
