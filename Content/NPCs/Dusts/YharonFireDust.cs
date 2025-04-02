using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.Dusts
{
    public class YharonFireDust : ModDust
    {
        public override bool Update(Dust dust)
        {
            dust.velocity *= 0.9f;
            dust.position += dust.velocity;
            dust.scale *= 0.99f * (1f - dust.fadeIn);
            dust.fadeIn += 0.001f;
            if(dust.alpha < 255)
                dust.alpha++;
            if (dust.scale < 0.1f)
                dust.active = false;
            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            if (!CalamityYharonChange.renderHelperSystem.drawRenders.Contains(DrawYharonFireSystem.Instance))
                CalamityYharonChange.renderHelperSystem.drawRenders.Enqueue(DrawYharonFireSystem.Instance);
            DrawYharonFireSystem.fires.Add(dust.dustIndex);
            return false;
        }
        public static void DrawSelf(Dust dust)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D texture = AssetPreservation.Extra[2].Value;
            sb.Draw(texture, dust.position - Main.screenPosition, null, dust.GetAlpha(Color.White), 0, texture.Size() * 0.5f, dust.scale, SpriteEffects.None, 0f);
        }
    }
}
