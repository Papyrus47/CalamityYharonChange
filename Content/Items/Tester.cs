using CalamityYharonChange.Content.Particles;
using CalamityYharonChange.Content.Projs;
using CalamityYharonChange.Core.Particles;
using CalamityYharonChange.Graphics;

namespace CalamityYharonChange.Content.Items
{
    public class Tester : ModItem
    {
        public override string Texture => "Terraria/Images/Item_3625";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useStyle = 1;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.maxStack = 1;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (!Main.gamePaused && (int)Main.timeForVisualEffects % 2 == 0)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        GradientColor c = new GradientColor();
                        c.colorList.Add((new Color(1f, 0.7f, 0f), 0f));
                        c.colorList.Add((new Color(0.5f, 0f, 1f), 0.6f));
                        Flame f = Flame.AddFlame(Item.Center + (i * MathHelper.TwoPi / 50).ToRotationVector2() * 100 * (j + 1), new Vector2(0, -1f), c, 50, 40);
                        f.gravity = -0.25f;
                    }
                }
                Main.NewText(ParticlesSystem.flames.Count);
            }
            return false;
        }
        public override bool CanUseItem(Player player)
        {
            Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center);

            //Projectile.NewProjectile(null, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<MoonHalo>(), 100, 1, Main.myPlayer);

            //Projectile.NewProjectile(null, Main.MouseWorld, dir.RotatedBy(0f), ModContent.ProjectileType<FireBurst_Rectangle>(), 100, 1, Main.myPlayer, 0, 8000, 300);

            Projectile.NewProjectile(null, Main.MouseWorld, 0 * dir.RotatedBy(0f), ModContent.ProjectileType<LimitArea>(), 100, 1, Main.myPlayer, 0, 900);


            //Projectile.NewProjectile(null, player.Center, dir*1, ModContent.ProjectileType<MoonHaloLightning>(), 100, 1, Main.myPlayer);

            //Projectile.NewProjectile(null, Main.MouseWorld, -dir, ModContent.ProjectileType<FireBurst>(), 100, 1, Main.myPlayer);

            /*
            Projectile.NewProjectile(null, Main.MouseWorld, dir, ModContent.ProjectileType<FireBurstRing>(), 100, 1, Main.myPlayer, 0, 0f);
            Projectile.NewProjectile(null, Main.MouseWorld, dir, ModContent.ProjectileType<FireBurstRing>(), 100, 1, Main.myPlayer, 0, 600f);
            Projectile.NewProjectile(null, Main.MouseWorld, dir, ModContent.ProjectileType<FireBurstRing>(), 100, 1, Main.myPlayer, 0, 1200f);
            */
            return true;
        }
    }
}
