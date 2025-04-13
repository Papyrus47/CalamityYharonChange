using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Boss;
using CalamityYharonChange.Content.NPCs.Dusts;
using CalamityYharonChange.Content.Projs;
using CalamityYharonChange.Content.Projs.Bosses.Yharon;
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
        public int Timer,Timer1;
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
                Timer = Timer1 = 0;
                FixedPos = NPC.Center;
                YharonNPC.ExtraAI += Shoot;
            }
            if((int)NPC.ai[0] == 300)
                YharonNPC.ExtraAI += Shoot1;
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
        public void Shoot()
        {
            Timer++;
            if (Timer % 120 == 0)
            {
                int lenght = (int)(Timer / 60); // 距离
                float Const = 20 + lenght * 20;
                for (int i = 0; i < Const; i++)
                {
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), FixedPos + (Vector2.UnitX.RotatedBy(i / Const * MathHelper.TwoPi) * lenght * 98f * 2), Vector2.Zero, YharonNPC.YharonNormalBoomProj, NPC.GetProjectileDamage(YharonNPC.YharonNormalBoomProj), 0f, Target.whoAmI);
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), FixedPos + (Vector2.UnitX.RotatedBy(i / Const * MathHelper.TwoPi) * lenght * 98f * 2), Vector2.Zero, YharonNPC.YharonNormalBoomProj, 0, 0f, Target.whoAmI, 1);
                }
            }
            if (Timer / 120 > 7)
            {
                YharonNPC.ExtraAI -= Shoot;
                Timer = 0;
            }
        }
        public void Shoot1()
        {
            Timer1++;
            if (Timer1 % 120 == 0)
            {
                int lenght = (int)(Timer1 / 60); // 距离
                float Const = 20 + lenght * 20;
                for (int i = 0; i < Const; i++)
                {
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), FixedPos + (Vector2.UnitX.RotatedBy(i / Const * MathHelper.TwoPi) * lenght * 98f * 2), Vector2.Zero, YharonNPC.YharonNormalBoomProj, NPC.GetProjectileDamage(YharonNPC.YharonNormalBoomProj), 0f, Target.whoAmI);
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), FixedPos + (Vector2.UnitX.RotatedBy(i / Const * MathHelper.TwoPi) * lenght * 98f * 2), Vector2.Zero, YharonNPC.YharonNormalBoomProj, 0, 0f, Target.whoAmI, 1);
                }
            }
            if (Timer1 / 120 > 7)
            {
                YharonNPC.ExtraAI -= Shoot1;
                Timer1 = 0;
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
}
