sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float4x4 uTransform;
float _Threshold1;
float _Edge;
float _MainColorScale;
float2 _Offset;
float2 _UVScale;
float _Decrease;
struct VSInput 
{
	float2 Pos : POSITION0;
    float4 Color : COLOR0;
	float2 Texcoord : TEXCOORD0;
};

struct PSInput 
{
	float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
	float2 Texcoord : TEXCOORD0;
};
PSInput VertexShaderFunction(VSInput input) 
{
	PSInput output;
	output.Texcoord = input.Texcoord;
	output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    output.Color = input.Color;
	return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{

	float2 coord = input.Texcoord; //纹理坐标
	float4 edge = tex2D(uImage0,coord); //边缘范围图
	edge.rgb *= 2 * input.Color.rgb * edge.rgb;

	float2 offset1;
	offset1.x = tex2D(uImage2,coord * _UVScale).r;
	offset1.y = -offset1.x;
    float2 offset2;
	offset2.x = tex2D(uImage2,coord * _UVScale + offset1 * 0.1 + _Offset).r;
	offset2.y = offset2.x;
	float4 main = tex2D(uImage1,(coord+_Offset + offset2 * 0.05) * _UVScale); //主体形状
	
	float threshold = coord.y * _Decrease + _Threshold1;

	main -= threshold;

	main*=_MainColorScale;

	main.rgb -= edge.rrr * _Edge; 

    main.rgb *= main.rgb * main.rgb;

	main.rgb = tex2D(uImage3,float2(main.r,0));//着色

	return (main+edge)*input.Color.a;
}

technique Technique1 {
	pass P0 {
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}