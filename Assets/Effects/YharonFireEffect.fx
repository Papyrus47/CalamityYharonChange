sampler uImage0 : register(s0);
texture2D tex;
sampler2D uImage1 = sampler_state
{
    Texture = <tex>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
float2 uChange;
float4 uColor;

float4 Fire(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float4 color1 = tex2D(uImage1, coords * 3 + uChange);
    color1 *= uColor * color.a;
    color *= color1;
    return color;
}

technique Technique1
{
    pass Fire
    {
        PixelShader = compile ps_2_0 Fire();
    }
}
