sampler uImage0 : register(s0);

uniform float uOffset;

float3 colorize(float3 base,float3 col,float saturation,float offset)
{
    float lum = dot(base,float3(0.3,0.6,0.1)) + offset;
    float3 midColor = lerp(float3(0.5,0.5,0.5),col,saturation);
    return lum<0.5?lerp(float3(0.,0.,0.),midColor,lum*2.):lerp(midColor,float3(1.,1.,1.),(lum-0.5)*2.);
}

float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 coord : TEXCOORD0) : COLOR0
{
	
	float4 c = tex2D(uImage0, float2(coord.x , coord.y));
    float4 finalColor;
    finalColor.rgb=colorize(c.rgb,drawColor.rgb,0.8,uOffset);
    finalColor.a = c.a;
    return finalColor*drawColor.a;
}


technique Technique1 
{
	pass Colorize
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
