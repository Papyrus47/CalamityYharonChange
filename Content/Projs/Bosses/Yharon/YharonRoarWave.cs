using CalamityMod;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.DraedonsArsenal;

namespace CalamityYharonChange.Content.Projs.Bosses.Yharon
{
    public class YharonRoarWave : BaseMassiveExplosionProjectile, ILocalizedModType, IModType
    {
        public new string LocalizationCategory => "Projectiles.Misc";
        public override int Lifetime => 60;
        public override bool UsesScreenshake => true;
        public override float GetScreenshakePower(float pulseCompletionRatio) => CalamityUtils.Convert01To010(pulseCompletionRatio) * 8f; // 震屏力度
        public override Color GetCurrentExplosionColor(float pulseCompletionRatio) => Color.OrangeRed * 2;
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = Lifetime;
        }
        public override void AI()
        {
            if (MaxRadius == 0)
                MaxRadius = 800;
            base.AI();
        }
    }
}
