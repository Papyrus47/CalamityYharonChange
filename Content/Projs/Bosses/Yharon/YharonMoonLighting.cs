using CalamityYharonChange.Content.Partcles;
using CalamityYharonChange.Core.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Projs.Bosses.Yharon
{
    /// <summary>
    /// ai[0] = 最小范围
    /// ai[1] = 最大范围
    /// </summary>
    public class YharonMoonLighting : ModProjectile
    {
        public class Lighting
        {
            /// <summary>
            /// 拐点
            /// </summary>
            public Vector2[] points;

            public Lighting(int pointsLength)
            {
                this.points = new Vector2[pointsLength];
            }
        }
        public Lighting[] lightings;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000000000;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            Projectile.penetrate = -1;
            //Projectile.Resize((int)Projectile.ai[1] * 2, (int)Projectile.ai[1] * 2);
        }
        public override bool? CanDamage() => true;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => true;
        public override bool CanHitPlayer(Player target)
        {
            float dis = target.Center.Distance(Projectile.Center);
            return dis > Projectile.ai[0] && dis < Projectile.ai[1];
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            GraphicsDevice gd = Main.graphics.GraphicsDevice;
            gd.Textures[0] = TextureAssets.Extra[197].Value;
            gd.SamplerStates[0] = SamplerState.LinearWrap;

            lightings ??= new Lighting[40];
            float length = lightings.Length;
            for (int i = 0; i < length; i++)
            {
                lightings[i] ??= new(25);
                float lightingsLenght = 25f;
                float factor = i / length * 2;
                lightings[i].points[0] = Projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * factor + Main.GlobalTimeWrappedHourly % 0.1f * 2f) * Projectile.ai[0] * 0.7f;
                lightings[i].points[1] = Projectile.Center + Vector2.One.RotatedBy(MathHelper.TwoPi * factor + Main.GlobalTimeWrappedHourly % 0.1f * 2f + Main.rand.NextFloatDirection() * 0.2f) * (Projectile.ai[1] / lightingsLenght + Projectile.ai[0]);
                for (int j = 2; j < lightings[i].points.Length; j++)
                {
                    Vector2 vel = lightings[i].points[1] - lightings[i].points[0];
                    vel = vel.SafeNormalize(default) * (Projectile.ai[1] / lightingsLenght) * 0.9f;
                    vel = vel.RotatedByRandom(0.6);
                    lightings[i].points[j] = lightings[i].points[j - 1] + vel;
                }
            }
            List<List<CustomVertexInfo>> vertexInfo = new(); // 这是每个画的雷电+环绕圈
            Color color = Color.LightBlue with { A = 0 };
            for (int i = 0; i < length; i++)
            {
                if (Projectile.timeLeft % 2 != 0)
                    break;
                List<CustomVertexInfo> vertex = new();
                vertexInfo.Add(vertex);
                for (int j = 1; j < lightings[i].points.Length; j++)
                {
                    Vector2 vel = lightings[i].points[j] - lightings[i].points[j - 1];
                    float fc = (float)j / lightings[i].points.Length;
                    if (fc < 0.3f)
                        fc = 0.3f;
                    //vertex.Add(new CustomVertexInfo(lightings[i].points[j - 1] - Main.screenPosition, Color.Lerp(Color.Red,color,fc), new Vector3(fc, 0.5f, 0)));
                    //gd.DrawUserPrimitives(PrimitiveType.LineStrip, vertex.ToArray(), 0, vertex.Count - 1);
                    vertex.Add(new CustomVertexInfo(lightings[i].points[j - 1] + vel.RotatedBy(MathHelper.PiOver2) * 0.8f * (1f - fc) - Main.screenPosition, color, new Vector3(fc, 0, 0)));
                    vertex.Add(new CustomVertexInfo(lightings[i].points[j - 1] + vel.RotatedBy(-MathHelper.PiOver2) * 0.8f * (1f - fc) - Main.screenPosition, color, new Vector3(fc, 1, 0)));
                }
            }
            List<CustomVertexInfo> insideCircle = new();
            List<CustomVertexInfo> outsideCircle = new();
            vertexInfo.Add(insideCircle);
            vertexInfo.Add(outsideCircle);
            for (float rad = 0; rad <= 6.28f; rad += 0.1f)
            {
                Vector2 vel = rad.ToRotationVector2();
                insideCircle.Add(new CustomVertexInfo(Projectile.Center + vel * Projectile.ai[0] + vel.RotatedBy(MathHelper.PiOver2) * 40 - Main.screenPosition, color, new Vector3(rad / 6.28f, 0, 0)));
                insideCircle.Add(new CustomVertexInfo(Projectile.Center + vel * Projectile.ai[0] + vel.RotatedBy(-MathHelper.PiOver2) * 40 - Main.screenPosition, color, new Vector3(rad / 6.28f, 1, 0)));

                outsideCircle.Add(new CustomVertexInfo(Projectile.Center + vel * Projectile.ai[1] + vel.RotatedBy(MathHelper.PiOver2) * 120 - Main.screenPosition, color, new Vector3(rad / 6.28f, 0, 0)));
                outsideCircle.Add(new CustomVertexInfo(Projectile.Center + vel * Projectile.ai[1] + vel.RotatedBy(-MathHelper.PiOver2) * 120 - Main.screenPosition, color, new Vector3(rad / 6.28f, 1, 0)));
            }

            foreach (List<CustomVertexInfo> vertex in vertexInfo)
            {
                gd.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex.ToArray(), 0, vertex.Count - 2);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
