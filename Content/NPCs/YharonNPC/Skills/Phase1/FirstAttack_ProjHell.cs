using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Boss;
using CalamityYharonChange.Content.NPCs.Dusts;
using CalamityYharonChange.Content.Projs;
using CalamityYharonChange.Content.Projs.Bosses.Yharon;
using CalamityYharonChange.Core.NPCAI;
using CalamityYharonChange.Core.SkillsNPC;
using Microsoft.CodeAnalysis;
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
        public Vector2 FixedPos;
        public FirstAttack_ProjHell(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            NPC.ai[0]++;
            NPC.velocity *= 0;
            NPC.dontTakeDamage = true;
            LimitEdge();
            if ((int)NPC.ai[0] == 1)
            {
                FixedPos = NPC.Center;
                new ShootRing().ApplyExtraAI(YharonNPC.extraAIs, NPC);
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<LimitArea>(), 0, 0f, Target.whoAmI,NPC.whoAmI,16 * 90);

            }
            if ((int)NPC.ai[0] == 300)
            {
                new ShootRing() { counts = 4 }.ApplyExtraAI(YharonNPC.extraAIs, NPC);
            }
            if ((int)NPC.ai[0] == 1) // 产生扩散波,并且上Debuff
            {
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, YharonNPC.YharonRoarWave, 0, 0f, Target.whoAmI);
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, YharonNPC.YharonRoarWave, 0, 0f, Target.whoAmI);

                Target.AddBuff(ModContent.BuffType<Dragonfire>(), 300);
            }
            if ((int)NPC.ai[0] % 20 == 0)
            {
                int type = YharonNPC.FlareBomb; // 火球
                float Const = 10;
                for (int i = 0; i < Const; i++)
                {
                    var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.UnitX.RotatedBy(i / Const * MathHelper.TwoPi + NPC.ai[0] % 6) * 6f, type, NPC.GetProjectileDamage(type), 0f, Target.whoAmI, -1);
                    proj.timeLeft *= 5;
                }
                if ((int)(NPC.ai[0] - 30) % 12 == 0)
                {
                    var proj1 = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (Target.Center - NPC.Center).SafeNormalize(default) * 8f, ModContent.ProjectileType<DarkRedFlareBomb>(), NPC.GetProjectileDamage(type), 0f, Target.whoAmI, -1);
                    proj1.timeLeft *= 5;
                }
            }
        }

        
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawBall(spriteBatch, screenPos);
            return false;
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
        }
        public override bool SwitchCondition(NPCSkills changeToSkill) => (NPC.ai[0] - 30) / 60 > 9 && base.SwitchCondition(changeToSkill);
        public override bool ActivationCondition(NPCSkills activeSkill) => true; // 只要上个技能满足切换条件就允许切换该技能
    }

    /// <summary>
    /// 环状AOE
    /// </summary>
    public class ShootRing : ExtraAI
    {
        private int timer;
        public int counts = 6;
        private int distance;
        public override void AI()
        {
            FireBurstRing.width = 400;
            if (++timer % 120 == 0)
            {
                Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<FireBurstRing>(), npc.GetProjectileDamage(YharonNPC.YharonNormalBoomProj), 0f, Main.myPlayer, 0, distance);
                distance += FireBurstRing.width;
            }
            if (timer >= 120 * counts)
            {
                Remove();
            }
        }
    }
}
