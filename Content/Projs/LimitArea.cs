using CalamityYharonChange.Content.NPCs.YharonNPC;

namespace CalamityYharonChange.Content.Projs
{
    public class LimitArea : ModProjectile
    {
        //ai0:npc
        //ai1:radius
        public override string Texture => "Terraria/Images/Item_0";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 1000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        float alpha = 0;
        float timer;
        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Projectile.Center = Vector2.Lerp(Projectile.Center,npc.Center,0.05f);
            if(npc.type != ModContent.NPCType<YharonNPC>()||!npc.active)
            {
                Projectile.Kill();
            }
            if(Projectile.timeLeft<20)
            {
                alpha.LerpValue(0f, 0.1f);
            }
            else
            {
                alpha.LerpValue(0.75f, 0.05f);
            }
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Color color = new Color(0.5f, 0.2f, 0.0f, 0f) * alpha;
            List<CustomVertexInfo> vertices = new();
            int counts = 100;
            float radius = Projectile.ai[1];
            float width = 0.5f * radius;


            vertices.Clear();
            width = 100 * alpha;
            color.A = (byte)(100 + 155 * alpha);
            for (int i = 0; i <= counts; i++)
            {
                float uvx = (float)i / counts;
                vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * (radius - width), new Vector3(uvx, 0, 0), color));
                vertices.Add(new(Projectile.Center - Main.screenPosition + (i * MathHelper.TwoPi / counts).ToRotationVector2() * (radius + width), new Vector3(uvx, 1, 0), color));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Laser").Value;

            Effect shader1 = ModContent.Request<Effect>("CalamityYharonChange/Assets/Effects/Colorize", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            shader1.CurrentTechnique.Passes[0].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

            return false;
        }


    }
}
