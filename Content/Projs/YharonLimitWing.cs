using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Projs
{
    public class YharonLimitWing : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.width = 500;
            Projectile.height = 500;
            Projectile.aiStyle = -1;
        }
    }
}
