using CalamityYharonChange.Content.NPCs.YharonNPC;
using CalamityYharonChange.Content.Particles;
using CalamityYharonChange.Core.Particles;

namespace CalamityYharonChange.Content.Projs.Bosses.Yharon
{
    public class YharonDustBoom : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 6;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.2f;
        }
        public override void AI()
        {
            int size = (int)(100 * Projectile.scale);
            if (Projectile.ai[2] == 0)
            {
                Projectile.ai[2] = Projectile.timeLeft;
                YharonDustBoomParticles.FixedLength = size;
            }
            YharonDustBoomParticles.FixedPos = Projectile.Center;
            Projectile.alpha = 0;
            if (Projectile.timeLeft > Projectile.ai[2] * 0.5f)
            {
                for (int i = 0; i < 3; i++)
                {
                    ParticlesSystem.AddParticle(BasicParticle.DrawLayer.AfterDust, new YharonDustBoomParticles()
                    {
                        position = Projectile.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(10, 20) * 20,
                        color = Color.Lerp(Color.Red, Color.OrangeRed, Main.rand.NextFloat()) with { A = 0 },
                        scale = new Vector2(Projectile.scale * 0.1f),
                        maxTime = 300,
                    });
                }
            }
            Projectile.Resize(size, size);
        }
        public override bool? CanDamage() => false;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            YharonDustBoomParticles.FixedLength = 0;
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, YharonNPC.YharonRoarWave, Projectile.damage + 1, 0f, Projectile.owner, 0, Projectile.ai[1]);
            Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Main.rand.NextVector2Unit(), 10, 60, 60));
        }
    }
}
