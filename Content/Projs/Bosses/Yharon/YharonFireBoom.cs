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
    /// 火焰爆炸特效
    /// </summary>
    public class YharonFireBoom : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 10;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.01f;
        }
        public override void AI()
        {
            int size = (int)(200 * Projectile.scale);
            if (Projectile.timeLeft > Projectile.ai[2] * 0.5f)
                Projectile.scale = MathHelper.SmoothStep(Projectile.ai[0], Projectile.scale, 0.93f);
            else
                Projectile.scale = MathHelper.SmoothStep(1f, Projectile.scale, 0.97f);
            if (Projectile.ai[2] == 0)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                for (int i = 0; i < 25; i++)
                {
                    PartclesSystem.AddPartcle(BasicPartcle.DrawLayer.AfterDust, new YharonBoomDust()
                    {
                        position = Projectile.Center,
                        velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(10, 20),
                        scale = new Vector2(1f),
                    });
                    PartclesSystem.AddPartcle(BasicPartcle.DrawLayer.AfterProj, new YharonBoomExtra98()
                    {
                        position = Projectile.Center,
                        velocity = Main.rand.NextVector2Unit() * 10f,
                        color = Color.Lerp(Color.Red, Color.OrangeRed, Main.rand.NextFloat()) with { A = 0 }
                    });
                }
                Projectile.ai[2] = Projectile.timeLeft;
            }
            Projectile.Resize(size, size);
        }
        public override bool? CanDamage() => Projectile.timeLeft < 30;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D projTex = TextureAssets.Projectile[Type].Value; // 获取贴图
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(projTex, drawPos, null, Color.Orange with { A = 0 } * 0.1f, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * 1.1f, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos, null, Color.Orange with { A = 0 } * 1.2f, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos, null, Color.Red with { A = 0 }, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * 0.9f, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos, null, Color.Red with { A = 0 }, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * 0.9f, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos, null, Color.Red with { A = 0 }, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * 0.9f, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos, null, Color.White with { A = 0 }, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * 0.3f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
