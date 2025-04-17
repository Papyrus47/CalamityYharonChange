namespace CalamityYharonChange.Content.Projs
{
    public class MoonHaloLightning : ModProjectile
    {
        //ai0:width
        public override string Texture => "Terraria/Images/Item_0";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 1000;
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20;
            //Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            nodes = new Vector2[40];
        }
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        public Vector2[] nodes;
        public override void AI()
        {
            if (Projectile.timeLeft == 20 || Projectile.timeLeft % 4 == 0)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    Vector2 vert = Helper.VerticalVec(Projectile.velocity);
                    int l = 60;//单节的垂直长度
                    int r = 40;//最大偏移
                    nodes[i] = Projectile.Center + Projectile.velocity * i * l + vert * 0.5f * Main.rand.Next(-r, r);
                }
            }
            if (Projectile.timeLeft > 15)
            {
                Projectile.ai[0].LerpValue(20, 0.2f);
            }
            if (Projectile.timeLeft < 5)
            {
                Projectile.ai[0].LerpValue(0, 0.2f);
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(0.5f, 0.4f, 0f);
            List<CustomVertexInfo> vertices = new();
            for (int i = 0; i < nodes.Length - 1; ++i)
            {
                if (nodes[i + 1] == Vector2.Zero)
                {
                    break;
                }
                var normalDir = Vector2.Normalize(Helper.VerticalVec(nodes[i] - nodes[i + 1]));
                var factor = i / (float)nodes.Length;
                var w = 1f;
                float width = Projectile.ai[0];
                vertices.Add(new(nodes[i] + normalDir * width - Main.screenPosition, new Vector3((float)Math.Sqrt(factor), 1, w), color));
                vertices.Add(new(nodes[i] + normalDir * -width - Main.screenPosition, new Vector3((float)Math.Sqrt(factor), 0, w), color));
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
