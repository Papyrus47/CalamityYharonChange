﻿using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
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
    /// 弹幕地狱-第一个AI
    /// </summary>
    public class FirstAttack_ProjHell : BasicPhase1Skills
    {
        public FirstAttack_ProjHell(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            NPC.ai[0]++;
            NPC.velocity *= 0;
            NPC.dontTakeDamage = true;
            if ((int)NPC.ai[0] == 1) // 产生扩散波,并且上Debuff
            {
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, YharonNPC.YharonRoarWave, 0, 0f, Target.whoAmI);
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, YharonNPC.YharonRoarWave, 0, 0f, Target.whoAmI);

                Target.AddBuff(ModContent.BuffType<Dragonfire>(), 300);
            }
            int type = YharonNPC.YharonBoomProj;
            if ((int)NPC.ai[0] % 60 == 0)
            {
                int lenght = (int)(NPC.ai[0] / 60) + 1; // 距离
                float Const = 20 + lenght * 20;
                for (int i = 0; i < Const; i++)
                {
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + Vector2.UnitX.RotatedBy(i / Const * MathHelper.TwoPi) * lenght * 98f * 2f, Vector2.Zero, type, NPC.GetProjectileDamage(type), 0f, Target.whoAmI);
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + Vector2.UnitX.RotatedBy(i / Const * MathHelper.TwoPi) * (lenght * 98f * 2f), Vector2.Zero, type, 0, 0f, Target.whoAmI, 1);
                }
            }
            if ((int)NPC.ai[0] % 20 == 0)
            {
                type = YharonNPC.FlareBomb; // 火球
                float Const = 15;
                for (int i = 0; i < Const; i++)
                {
                    var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.UnitX.RotatedBy(i / Const * MathHelper.TwoPi + NPC.ai[0] % 6) * 6f, type, NPC.GetProjectileDamage(type), 0f, Target.whoAmI,-1);
                    proj.timeLeft *= 5;
                }
            }
            //if((int)NPC.ai[0] % 120 == 40)
            //{
            //    int lenght = (int)(NPC.ai[0] / 120) + 1; // 距离
            //    float Const = 20 + lenght * 20;
            //    for (int i = 0; i < Const; i++)
            //    {
            //        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + Vector2.UnitX.RotatedBy(i / Const * MathHelper.TwoPi) * (lenght * 98f * 2f), Vector2.Zero, type, 0, 0f, Target.whoAmI, 1);
            //    }
            //}
            //if (NPC.ai[0] / 60 > 9)
            //{
            //    NPC.ai[0] = 0;
            //}
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawBall(spriteBatch, screenPos);
            return false;
        }
        public override bool SwitchCondition(NPCSkills changeToSkill) => (NPC.ai[0] - 20) / 60 > 9 && base.SwitchCondition(changeToSkill);
        public override bool ActivationCondition(NPCSkills activeSkill) => true; // 只要上个技能满足切换条件就允许切换该技能
    }
}
