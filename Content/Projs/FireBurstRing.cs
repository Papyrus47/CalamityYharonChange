using CalamityYharonChange.Content.Particles;
using CalamityYharonChange.Graphics;

namespace CalamityYharonChange.Content.Projs
{
    public class FireBurstRing : ModProjectile
    {
        public static int width = 300;
        //ai2:当前宽度
        //ai1:目标宽度
        public override string Texture => "Terraria/Images/Item_0";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 3000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 80;
            Projectile.aiStyle = -1;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 0;
        }
        float alpha = 0;
        float timer;
        float threshold = 0;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)//预警
            {
                if (timer++ < 16)
                {
                    alpha.LerpValue(0.8f, 0.1f);
                    Projectile.ai[2].LerpValue(Projectile.ai[1], 0.25f);

                }
                else if (timer < 25)
                {
                    alpha.LerpValue(0f, 0.1f);
                }
                if (timer > 25)
                {
                    Projectile.ai[0]++;
                    timer = 0;

                }
            }
            if (Projectile.ai[0] == 1) //攻击
            {
                if (Projectile.timeLeft < 10)
                {
                    alpha.LerpValue(0f, 0.2f);
                    threshold.LerpValue(0f, 0.1f);
                }
                else
                {
                    if (alpha > 0.2)
                    {

                        for (int i = 0; i < 5 * (1 + Projectile.ai[1] / width / 2f); i++)
                        {
                            GradientColor c = new GradientColor();
                            c.colorList.Add((new Color(1f, 1f, 0.5f), 0f));
                            c.colorList.Add((Color.Yellow, 0.1f));
                            c.colorList.Add((Color.Red, 0.6f));
                            Vector2 v = Main.rand.NextVector2Unit();
                            Flame f = Flame.AddFlame(Projectile.Center + v * (Projectile.ai[1] + Main.rand.Next(width)), Vector2.Zero, c, Main.rand.Next(40, 70), Main.rand.Next(20, 30));
                            f.gravity = Main.rand.NextFloat(-0.15f, -0.07f);
                        }
                    }
                    alpha.LerpValue(1f, 0.1f);
                    threshold.LerpValue(-0.5f, 0.2f);
                }
            }
        }
        public override bool CanHitPlayer(Player target)
        {
            return Projectile.ai[0] == 1;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 v1 = targetHitbox.Center.ToVector2() - Projectile.Center;

            return Projectile.ai[2] < v1.Length() && Projectile.ai[2] + width > v1.Length();
        }
        public override bool PreDraw(ref Color lightColor)
        {

            if (Projectile.ai[0] == 0)
            {
                Color color = new Color(0.8f, 0.6f, 0.1f) * alpha;
                List<CustomVertexInfo> vertices = new();
                int counts = 100;
                float radius0 = Projectile.ai[2];
                float radius1 = radius0 + width;
                for (int i = 0; i <= counts; i++)
                {
                    float uvx = (float)i / counts;
                    vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * radius0, color, new Vector3(uvx, 1, 0)));
                    vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * radius1, color, new Vector3(uvx, 0, 0)));
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/RangeV").Value;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
            }
            if (Projectile.ai[0] == 1)
            {
                Color color = new Color(1f, 0.1f, 0.1f) * alpha;
                List<CustomVertexInfo> vertices = new();
                int counts = 300;
                float radius0 = Projectile.ai[2];
                float radius1 = radius0 + width;
                for (int i = 0; i <= counts; i++)
                {
                    float uvx = (float)i / counts;
                    vertices.Add(new(Projectile.Center + (i * MathHelper.TwoPi / counts).ToRotationVector2() * radius0, color, new Vector3(uvx, 1, 0)));
                    vertices.Add(new(Projectile.Center + (i * MathHelper.TwoPi / counts).ToRotationVector2() * radius1, color, new Vector3(uvx, 0, 0)));
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);


                SetShaderParameters(new Vector2(1f * (1 + Projectile.ai[1] / width), 0.75f), 1f, 0.025f);
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                //SetShaderParameters(new Vector2(1f, 1f), 0.5f, 0.04f);
                //Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

            }
            return false;
        }
        private void SetShaderParameters(Vector2 uvScale, float intensity, float speed)
        {
            Effect shader = ModContent.Request<Effect>("CalamityYharonChange/Assets/Effects/FireBurstProj", AssetRequestMode.ImmediateLoad).Value;

            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;

            shader.Parameters["uTransform"].SetValue(model * projection);
            shader.Parameters["_Threshold1"].SetValue(threshold);
            shader.Parameters["_MainColorScale"].SetValue(intensity);
            shader.Parameters["_UVScale"].SetValue(uvScale);
            shader.Parameters["_Edge"].SetValue(0.8f);
            shader.Parameters["_Decrease"].SetValue(0.1f);
            shader.Parameters["_Offset"].SetValue(new Vector2(0, -Projectile.timeLeft * speed));
            shader.CurrentTechnique.Passes[0].Apply();

            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/RangeV").Value;
            Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Perlin1").Value;
            Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Cloud1").Value;
            Main.graphics.GraphicsDevice.Textures[3] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Color_Fire").Value;

            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;
        }
    }
}
