using CalamityYharonChange.Core.Particles;

namespace CalamityYharonChange.Content.Particles
{
    //From MEAC
    public class LightDust : BasicParticle
    {
        public override Asset<Texture2D> Texture => ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Ball");
        public LightDust()
        {
            maxTime = 30;
        }
        public float ai0 = 0.3f;
        public float alpha = 1f;
        public override void PostUpdate()
        {
            base.PostUpdate();
            float t = maxTime * 1 / 6f;
            if (lifetime > t)
            {
                ai0 -= 0.3f / (maxTime - t);
                velocity *= 0.94f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            return false;
        }
        public override void Draw()
        {
            Color c = color;
            c.A = 0;
            for (int i = 0; i < 5; i++)
            {
                Main.spriteBatch.Draw(Texture.Value, position - Main.screenPosition - velocity * i / 3, null, c * alpha * (1 - i / 5f) * 1.2f, velocity.ToRotation(), Texture.Value.Size() / 2, new Vector2(1.2f, 0.8f) * ai0 * scale, SpriteEffects.None, 0);
            }
        }
    }
}
