﻿using CalamityYharonChange.Content.Projs.Bosses.Yharon;
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
            NPC.ai[0]++;
            NPC.dontTakeDamage = true;
            if (NPC.alpha > 0)
                NPC.alpha -= 25;
            else
                NPC.alpha = 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawBall(spriteBatch, screenPos); 
            return false;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false; // 禁止攻击玩家
        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.ai[0] > YharonLimitWing.MoveTime; // 这个技能切换到别的技能的条件
        public override bool ActivationCondition(NPCSkills activeSkill) => false; // 不可以切换到这个技能
    }
}
