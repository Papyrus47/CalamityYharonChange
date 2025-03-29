using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills.Phase1
{
    public abstract class BasicPhase1Skills : BasicYharonSkills
    {
        public BasicPhase1Skills(NPC npc) : base(npc)
        {
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Asset<Texture2D> drawTex = AssetPreservation.Extra[2];
            spriteBatch.Draw(drawTex.Value, NPC.Center - screenPos, null, Color.OrangeRed with { A = 0 }, 0, drawTex.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
