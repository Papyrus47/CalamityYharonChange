using CalamityYharonChange.Content.Particles;
using CalamityYharonChange.Core.Particles;
using CalamityYharonChange.Graphics;

namespace CalamityYharonChange.Content.Projs
{
    public class FireBurst : ModProjectile
    {
        //ai1:当前宽度
        //ai2:目标宽度
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
            Projectile.timeLeft = 120;
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
                if (timer++ < 20)
                {
                    alpha.LerpValue(1f, 0.1f);
                    Projectile.ai[2].LerpValue(960, 0.1f);

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
                if (Projectile.timeLeft < 20)
                {
                    alpha.LerpValue(0f, 0.1f);
                    threshold.LerpValue(-0.5f, 0.05f);
                }
                else
                {
                    if (alpha > 0.8)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            GradientColor c = new GradientColor();
                            c.colorList.Add((Color.White, 0f));
                            c.colorList.Add((Color.Yellow, 0.1f));
                            c.colorList.Add((Color.Red, 0.5f));
                            Vector2 v = (Projectile.rotation).ToRotationVector2().RotatedByRandom(MathHelper.Pi / 3);
                            Flame.AddFlame(Projectile.Center + v * Main.rand.Next(1000), v * Main.rand.Next(20, 50), c, 30, Main.rand.Next(30, 50));

                            if (Main.rand.NextBool(3))
                                ParticlesSystem.AddParticle(BasicParticle.DrawLayer.AfterDust, new LightDust()
                                {
                                    position = Projectile.Center + v * Main.rand.Next(1600),
                                    velocity = v * Main.rand.Next(8, 14),
                                    scale = new Vector2(1.2f),
                                    color = new Color(1f, 0.7f, 0f)
                                });
                        }
                    }
                    alpha.LerpValue(1f, 0.1f);
                    threshold.LerpValue(-1f, 0.2f);
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

            return Helper.AngleBetween(v1, Projectile.rotation.ToRotationVector2()) < MathHelper.Pi / 3;
        }
        public override bool PreDraw(ref Color lightColor)
        {

            if (Projectile.ai[0] == 0)
            {
                Color color = new Color(0.8f, 0.6f, 0.1f) * alpha;
                List<CustomVertexInfo> vertices = new();

                SetVertex(100, color, vertices);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Range").Value;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
            }
            if (Projectile.ai[0] == 1)
            {
                Color color = new Color(1f, 0.1f, 0.1f) * alpha;
                List<CustomVertexInfo> vertices = new();

                SetVertex(300, color, vertices, true);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

                SetShaderParameters(new Vector2(0.8f, 1.5f), 0.65f, 0.025f);
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                //SetShaderParameters(new Vector2(1f, 1f), 0.5f, 0.04f);
                //Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

            }
            return false;
        }
        private void SetVertex(int counts, Color color, List<CustomVertexInfo> vertices, bool wcs = false)
        {
            Vector2 sc = wcs ? Vector2.Zero : Main.screenPosition;
            float radius = 2000;
            float startAngle = Projectile.rotation - MathHelper.TwoPi / 6;
            float angle = MathHelper.TwoPi / 3;
            for (int i = 0; i <= counts; i++)
            {
                float uvx = (float)i / counts;
                vertices.Add(new(Projectile.Center + (startAngle + i * angle / counts).ToRotationVector2() * radius - sc, color, new Vector3(uvx, 1, 0)));
                vertices.Add(new(Projectile.Center - sc, color, new Vector3(uvx, 0, 0)));
            }
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
            shader.Parameters["_Decrease"].SetValue(0.5f);
            shader.Parameters["_Offset"].SetValue(new Vector2(0, Projectile.timeLeft * speed));
            shader.CurrentTechnique.Passes[0].Apply();

            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Range").Value;
            Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Perlin1").Value;
            Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Cloud1").Value;
            Main.graphics.GraphicsDevice.Textures[3] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Color_Fire").Value;

            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;
        }
    }
}
