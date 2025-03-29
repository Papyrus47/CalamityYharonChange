using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Projs
{
    public class YharonBoomProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.scale = 2;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 2)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Type])
                {
                    if (Projectile.ai[0] != 1)
                        Projectile.Kill();
                    else if (Projectile.frame >= Main.projFrames[Type] * 2)
                        Projectile.Kill();
                }
                else if (Projectile.frame == 1 && Projectile.ai[0] != 1)
                    SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] == 1)
            {
                Vector2 drawPos = Projectile.Center - Main.screenPosition;
                Texture2D projTex = AssetPreservation.Extra[2].Value; // 获取贴图
                Color color = Color.OrangeRed with { A = 0 } * (1f - (float)Projectile.frame / (Main.projFrames[Type] * 2));
                const float scale = 0.8f;
                const float colorSclae = 0.5f;
                Main.EntitySpriteDraw(projTex, drawPos, null, color * colorSclae, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * scale, SpriteEffects.None, 0f);
                return false;
            }    
            return base.PreDraw(ref lightColor);
        }
    }
}
