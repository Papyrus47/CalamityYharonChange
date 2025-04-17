using CalamityYharonChange.Content.Particles;
using CalamityYharonChange.Core.Particles;
using CalamityYharonChange.Graphics;

namespace CalamityYharonChange.Content.Projs
{
    public class FireBurst_Rectangle : ModProjectile
    {
        //ai2:宽度
        //ai1:长度
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
            Projectile.timeLeft = 60;
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
        float process = 0;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)//预警
            {
                if (timer++ < 16)
                {
                    alpha.LerpValue(0.8f, 0.1f);
                }
                else if (timer < 25)
                {
                    alpha.LerpValue(0f, 0.1f);
                }
                process += 1 / 25f;
                if (timer > 25) // 切换
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
                        for (int i = 0; i < 8; i++)
                        {
                            GradientColor c = new GradientColor();
                            c.colorList.Add((new Color(1f, 1f, 0.5f), 0f));
                            c.colorList.Add((Color.Yellow, 0.1f));
                            c.colorList.Add((Color.Red, 0.6f));
                            Vector2 v = Main.rand.NextVector2Unit();
                            Vector2 pos = Projectile.Center + (new Vector2(Projectile.ai[1] / 3, Projectile.ai[2] / 2) * Main.rand.NextVector2Square(-1, 1f)).RotatedBy(Projectile.rotation);
                            Vector2 vel = Projectile.rotation.ToRotationVector2();

                            Flame f = Flame.AddFlame(pos, vel * Main.rand.Next(10, 40), c, Main.rand.Next(40, 50), Main.rand.Next(20, 30));
                            f.gravity = -0.3f;

                            pos = Projectile.Center + (new Vector2(Projectile.ai[1] / 3, Projectile.ai[2] / 2) * Main.rand.NextVector2Square(-1, 1f)).RotatedBy(Projectile.rotation);

                            ParticlesSystem.AddParticle(BasicParticle.DrawLayer.AfterDust, new LightDust()
                            {
                                position = pos,
                                velocity = vel.RotatedByRandom(0.1f) * Main.rand.Next(10, 20),
                                scale = new Vector2(1.5f, 1.5f),
                                color = new Color(1f, 0.7f, 0f)
                            });
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
            float point = 0;
            Vector2 dir = Projectile.rotation.ToRotationVector2() * Projectile.ai[1] / 2;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - dir, Projectile.Center + dir, Projectile.ai[2], ref point);
        }


        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] == 0)
            {
                Color color = new Color(0.8f, 0.6f, 0.1f) * alpha;
                List<CustomVertexInfo> vertices = new();

                SetVertex(color, vertices);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Range").Value;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

                Vector2 pos = Projectile.Center + Projectile.rotation.ToRotationVector2() * Projectile.ai[1] * (MathF.Pow(process, 1.5f) - 0.5f) * 0.7f - Main.screenPosition;
                Helper.DrawOnCenter("CalamityYharonChange/Assets/Images/LightV", pos, new Color(0.8f, 0.6f, 0.1f, 0f) * 0.5f, Projectile.rotation, new Vector2(Projectile.ai[1] / 5, Projectile.ai[2]) / 256f);
            }
            if (Projectile.ai[0] == 1)
            {
                Color color = new Color(1f, 0.1f, 0.1f) * alpha;
                List<CustomVertexInfo> vertices = new();

                SetVertex(color, vertices, true);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

                SetShaderParameters(new Vector2(1f, 1f) * new Vector2(Projectile.ai[2] / 600, Projectile.ai[1] / 8000), 1f, 0.025f);
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

            }
            return false;
        }

        private void SetVertex(Color color, List<CustomVertexInfo> vertices, bool wcs = false)
        {
            Vector2 sc = wcs ? Vector2.Zero : Main.screenPosition;

            Vector2[] vec = { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(1, 1) };
            Vector2[] coord = { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };

            for (int i = 0; i < 4; i++)
            {
                Vector2 pos = Projectile.Center + (vec[i] * new Vector2(Projectile.ai[1], Projectile.ai[2]) / 2).RotatedBy(Projectile.rotation) - sc;
                vertices.Add(new(pos, color, new Vector3(coord[i], 0)));
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
            shader.Parameters["_Decrease"].SetValue(0.1f);
            shader.Parameters["_Offset"].SetValue(new Vector2(0, -Projectile.timeLeft * speed));
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
