﻿using CalamityYharonChange.Content.Buffs;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Projs.Bosses.Yharon
{
    public class YharonWind : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.width = 408;
            Projectile.height = 408;
            Projectile.scale = 1 / 12.75f;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            if(Projectile.soundDelay <= 0)
            {
                Projectile.soundDelay = 10;
                // 添加音效
            }
            Projectile.damage = 0;
            Projectile.rotation += 0.1f;
            Projectile.rotation %= 6.28f;
            if (Projectile.ai[0]++ > 30)
            {
                Projectile.damage = 600;
                if (Main.masterMode)
                    Projectile.damage /= 6;
                else if (Main.expertMode)
                    Projectile.damage /= 4;
                Projectile.scale += 0.1f / 12.75f;
                int size = (int)(408 * Projectile.scale);
                Projectile.Resize(size, size);
                if(Projectile.scale > 5 / 12.75f)
                {
                    Projectile.Kill();
                    // 这里添加音效
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.Yharon().PlayerFly = 30;
            target.AddBuff(ModContent.BuffType<FlyWind>(),3600);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D projTex = TextureAssets.Projectile[Type].Value; // 获取贴图
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.OrangeRed with { A = 100 };
            Main.EntitySpriteDraw(projTex, drawPos, null, color, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
