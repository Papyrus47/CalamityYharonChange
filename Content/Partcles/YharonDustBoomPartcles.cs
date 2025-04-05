using CalamityYharonChange.Core.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CalamityYharonChange.Content.Partcles
{
    public class YharonDustBoomPartcles : BasicPartcle
    {
        public int alpha;
        public bool InBoom;
        public static Vector2 FixedPos;
        public static float FixedLength;
        public override Asset<Texture2D> Texture => AssetPreservation.Extra[2];
        public override void PostUpdate()
        {
            Vector2 vel = FixedPos - position;
            vel = vel.RotatedBy(0.01);
            if (FixedLength == 0 && !InBoom)
            {
                InBoom = true;
                position = FixedPos;
                velocity = -vel * 0.03f;
            }
            if (InBoom)
            {
                scale += new Vector2(0.01f);
                alpha += 10;
                velocity += velocity * 0.2f;
                color = Color.Lerp(color, color * 1.2f, 0.4f) with { A = 0 };
                if (alpha > 255)
                    ShouldRemove = true;
            }
            else
            {
                if (vel.Length() > FixedLength)
                    velocity = vel * 0.2f;
                else if (vel.Length() < FixedLength * 0.4f)
                    velocity = vel.RotatedBy(MathHelper.PiOver2);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D texture = AssetPreservation.Extra[2].Value;
            Vector2 scale1 = !InBoom ? scale : new(scale.X * (2 + velocity.Length() * 0.2f), scale.Y * 0.1f);
            sb.Draw(texture, position - Main.screenPosition, null, color * (1 - alpha / 255f), velocity.ToRotation(), texture.Size() * 0.5f, scale1, SpriteEffects.None, 0f);

            return false;
        }
    }
}
