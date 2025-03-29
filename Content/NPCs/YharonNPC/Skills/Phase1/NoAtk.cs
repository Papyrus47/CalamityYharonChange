using CalamityYharonChange.Content.Projs;
using CalamityYharonChange.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills.Phase1
{
    /// <summary>
    /// 没有攻击行为的开场
    /// </summary>
    public class NoAtk : BasicPhase1Skills
    {
        public NoAtk(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            NPC.velocity *= 0f;
            NPC.scale = 1;
            NPC.ai[0]++;
            NPC.dontTakeDamage = true;
            if (NPC.alpha > 0)
                NPC.alpha -= 25;
            else
                NPC.alpha = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            _ = AssetPreservation.Extra[2];
            Asset<Texture2D> drawTex = AssetPreservation.Extra[2];
            spriteBatch.Draw(drawTex.Value, NPC.Center - screenPos, null, Color.OrangeRed with { A = 0 } * (1f - NPC.alpha / 255f), 0, drawTex.Size() * 0.5f, NPC.scale * 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(drawTex.Value, NPC.Center - screenPos, null, Color.White with { A = 0 } * (1f - NPC.alpha / 255f), 0, drawTex.Size() * 0.5f, NPC.scale * 0.8f * 2f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false; // 禁止攻击玩家
        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.ai[0] > YharonLimitWing.MoveTime; // 这个技能切换到别的技能的条件
        public override bool ActivationCondition(NPCSkills activeSkill) => false; // 不可以切换到这个技能
    }
}
