using CalamityYharonChange.Content.NPCs.Dusts;
using CalamityYharonChange.Content.UIs;
using CalamityYharonChange.Core.SkillsNPC;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills
{
    public abstract class BasicYharonSkills : NPCSkills
    {
        public YharonNPC YharonNPC => ModNPC as YharonNPC;
        public Player Target => YharonNPC.TargetPlayer;
        public BasicYharonSkills(NPC npc) : base(npc)
        {
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            SkillTimeOut = false;
            NPC.ai[0] = NPC.ai[1] = NPC.ai[2] = NPC.ai[3] = 0;
            NPC.localAI[0] = NPC.localAI[1] = NPC.localAI[2] = NPC.localAI[3] = 0;
        }
        public override void OnSkillDeactivate(NPCSkills changeToSkill)
        {
            base.OnSkillDeactivate(changeToSkill);
            SkillTimeOut = false;
            NPC.ai[0] = NPC.ai[1] = NPC.ai[2] = NPC.ai[3] = 0;
            NPC.localAI[0] = NPC.localAI[1] = NPC.localAI[2] = NPC.localAI[3] = 0;
            SkillTimeUI.Active = false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.instance.LoadNPC(NPC.type);
            Texture2D tex = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, drawColor * (1f - (float)NPC.alpha / 255f), NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
        public virtual void DrawBall(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            _ = AssetPreservation.Extra[2];
            Asset<Texture2D> drawTex = AssetPreservation.Extra[2];
            spriteBatch.Draw(drawTex.Value, NPC.Center - screenPos, null, Color.OrangeRed with { A = 0 } * (1f - NPC.alpha / 255f), 0, drawTex.Size() * 0.5f, NPC.scale * 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(drawTex.Value, NPC.Center - screenPos, null, Color.White with { A = 0 } * (1f - NPC.alpha / 255f), 0, drawTex.Size() * 0.5f, NPC.scale * 0.8f * 2f, SpriteEffects.None, 0f);
        }
        public virtual void DrawAfterimage(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            _ = TextureAssets.Npc[NPC.type].Value;
            Texture2D tex = TextureAssets.Npc[NPC.type].Value;
            float drawConst = NPCID.Sets.TrailCacheLength[NPC.type];
            for(int i = 1; i < drawConst; i++)
            {
                spriteBatch.Draw(tex, NPC.oldPos[i] + NPC.frame.Size() * 0.25f * NPC.scale - screenPos, NPC.frame, Color.White * (1f - i / drawConst), NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
        }
        public void LimitEdge(float limit = 90)
        {
            if (Target.Center.Distance(NPC.Center) > 16 * limit)
            {
                Target.Center -= (NPC.Center - Target.Center).SafeNormalize(default);
                Target.velocity = (NPC.Center - Target.Center).SafeNormalize(default) * 5;
            }
            for (int i = 0; i < 360; i++)
            {
                Vector2 pos = NPC.Center + Vector2.UnitX.RotatedBy(i / 360f * MathHelper.TwoPi) * (limit * 16 + 100);
                Dust dust = Dust.NewDustPerfect(pos, ModContent.DustType<YharonFireDust>(), (pos - NPC.Center).SafeNormalize(default) * 5, 100, Color.OrangeRed, 0.6f);
            }
        }
    }
}
