using CalamityYharonChange.Core.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Partcles
{
    public class YharonBoomExtra98 : BasicPartcle
    {
        public override Asset<Texture2D> Texture => TextureAssets.Extra[98];
        public YharonBoomExtra98()
        {
            TimeLeftMax = 20;
            scale = new Vector2(2f,2f);
        }
        public override void PostUpdate()
        {
            base.PostUpdate();
            if (TimeLeft <= TimeLeftMax * 0.5f)
                scale.X += (10f - scale.X) * 0.2f;
            scale.Y -= 0.1f;
            rotation = velocity.ToRotation();
        }
        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Texture.Value;
            spriteBatch.Draw(texture, position - Main.screenPosition, null, color, rotation, new Vector2(texture.Width / 2, texture.Height), scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
