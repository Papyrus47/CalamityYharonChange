using CalamityYharonChange.Content.NPCs.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Projs.Bosses.Yharon
{
    public class YharonFire : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            int size = (int)(100 * Projectile.scale);
            Projectile.alpha = 0;
            if (Projectile.timeLeft < 30)
                Projectile.scale *= 0.9f;
            if (Projectile.localAI[0]-- <= 0)
            {
                Projectile.localAI[0] = 6;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<YharonFireDust>(), Main.rand.NextVector2Unit() * Main.rand.NextFloat(15,23) * (1f - Projectile.localAI[1]), 0, Color.White, Projectile.scale);
            }
            Projectile.Resize(size, size);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
