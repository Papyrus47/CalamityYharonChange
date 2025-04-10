using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Buffs
{
    public class FlyWind : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.Yharon().flyWind = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture + "1").Value;
            spriteBatch.Draw(tex, drawParams.Position, null, drawParams.DrawColor, 0, Vector2.Zero,0.7f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
