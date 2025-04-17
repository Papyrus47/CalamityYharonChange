namespace CalamityYharonChange.Content.Projs
{
    public class MoonHalo : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_0";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 1000;
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
            Projectile.tileCollide = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        const float MaxRadius = 480f;
        float alpha = 0;
        float timer;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)//预警
            {
                if (timer++ < 15)
                {
                    alpha.LerpValue(1f, 0.1f);
                    Projectile.ai[2].LerpValue(MaxRadius, 0.2f);

                }
                else if (timer < 30)
                {
                    alpha.LerpValue(0f, 0.1f);
                }
                if (timer > 30)
                {
                    Projectile.ai[0]++;
                    Projectile.ai[1] = -0.3f;
                    timer = 0;
                }
            }
            if (Projectile.ai[0] == 1) //攻击
            {
                if (Projectile.ai[1] < 1.5f)
                    Projectile.ai[1] += 0.015f;
                if (Projectile.timeLeft < 20)
                {
                    alpha.LerpValue(0f, 0.2f);
                }
                else
                {
                    alpha.LerpValue(1f, 0.3f);

                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 dir = Main.rand.NextVector2Unit();
                        Projectile.NewProjectile(null, Projectile.Center + dir * Projectile.ai[2], dir.RotatedByRandom(0.5f), ModContent.ProjectileType<MoonHaloLightning>(), 0, 0, Main.myPlayer);
                    }
                    /*
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 vfxpos = Projectile.Center + Main.rand.NextVector2Unit() * (Main.rand.NextFloat(1f) * Main.rand.NextFloat(1f) * 2000 + Projectile.ai[2] + 80);
                        VisualEffect explode = VisualEffect.Create<Explosion>(vfxpos, Vector2.Zero);
                        explode.drawColor = new Color(0.8f, 1f, 0.1f);
                        explode.scale = 3f;
                    }*/
                }

            }
        }
        public override bool CanHitPlayer(Player target)
        {
            return Projectile.ai[0] == 1;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return (targetHitbox.Center.ToVector2() - Projectile.Center).Length() > Projectile.ai[2];
        }
        public override bool PreDraw(ref Color lightColor)
        {

            if (Projectile.ai[0] == 0)
            {
                Color color = new Color(0.5f, 0.4f, 0f) * alpha;
                List<CustomVertexInfo> vertices = new();
                int counts = 100;
                float radius = Projectile.ai[2];
                float width = 1.2f * radius;
                for (int i = 0; i <= counts; i++)
                {
                    float uvx = i / counts;
                    vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * radius, new Vector3(uvx, 0, 0), color));
                    vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * (radius + width), new Vector3(uvx, 1, 0), color * 0f));
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

                Main.graphics.GraphicsDevice.Textures[0] = Terraria.GameContent.TextureAssets.MagicPixel.Value;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
            }
            if (Projectile.ai[0] == 1)
            {
                Color color = new Color(0.5f, 0.2f, 0.0f, 0f) * alpha;
                List<CustomVertexInfo> vertices = new();
                int counts = 100;
                float radius = Projectile.ai[2];
                float width = 2f * radius;
                for (int i = 0; i <= counts; i++)
                {
                    float uvx = (float)i / counts;
                    vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * radius, new Vector3(uvx, 0, 0), color));
                    vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * (radius + width), new Vector3(uvx, 1, 0), color * 0f));
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

                Main.graphics.GraphicsDevice.Textures[0] = Terraria.GameContent.TextureAssets.MagicPixel.Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/BlockNoise1").Value;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;

                Effect shader = ModContent.Request<Effect>("CalamityYharonChange/Assets/Effects/DrawBlockNoise", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                shader.Parameters["uYOffset"].SetValue(Projectile.ai[1]);
                shader.Parameters["uUVScale"].SetValue(new Vector2(2f, 0.6f));
                shader.Parameters["uColor"].SetValue(new Vector4(1f, 0.7f, 0.1f, 0f) * alpha);
                shader.Parameters["uWidth"].SetValue(0.75f);
                shader.CurrentTechnique.Passes[0].Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                vertices.Clear();
                width = 100 * alpha;
                color.A = (byte)(100 + 155 * alpha);
                for (int i = 0; i <= counts; i++)
                {
                    float uvx = (float)i / counts;
                    vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * (radius - width), new Vector3(uvx, 0, 0), color));
                    vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * (radius + width), new Vector3(uvx, 1, 0), color));
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Laser").Value;

                Effect shader1 = ModContent.Request<Effect>("CalamityYharonChange/Assets/Effects/Colorize", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                shader1.CurrentTechnique.Passes[0].Apply();

                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

            }
            return false;
        }
    }
}
