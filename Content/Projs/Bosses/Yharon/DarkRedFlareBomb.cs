using CalamityMod.Projectiles.Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Projs.Bosses.Yharon
{
    public class DarkRedFlareBomb : FlareBomb
    {
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.DarkOrange;
            lightColor.A = (byte)Projectile.alpha;
            Texture2D value = TextureAssets.Projectile[base.Projectile.type].Value;
            int num = value.Height / Main.projFrames[base.Projectile.type];
            int y = num * base.Projectile.frame;
            Main.spriteBatch.Draw(value, base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY), new Rectangle(0, y, value.Width, num), lightColor, base.Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), base.Projectile.scale, SpriteEffects.None, 0f);
            lightColor = Color.Orange;
            lightColor.A = 0;
            Main.spriteBatch.Draw(value, base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY), new Rectangle(0, y, value.Width, num), lightColor, base.Projectile.rotation, new Vector2((float)value.Width / 2f, (float)num / 2f), base.Projectile.scale * 0.5f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
