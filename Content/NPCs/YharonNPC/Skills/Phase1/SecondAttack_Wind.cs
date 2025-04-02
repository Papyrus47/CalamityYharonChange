using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityYharonChange.Core.SkillsNPC;
using CalamityYharonChange.Content.UIs;
using CalamityMod.NPCs.Yharon;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills.Phase1
{
    public class SecondAttack_Wind : BasicPhase1Skills
    {
        public SecondAttack_Wind(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            SkillTimeUI.Active = true;
            NPC.dontTakeDamage = false;
            Vector2 vel = (Target.Center - NPC.Center).SafeNormalize(default);
            NPC.spriteDirection = NPC.direction = (vel.X > 0).ToDirectionInt();
            NPC.velocity += vel;
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.velocity *= 0f;
            if (NPC.spriteDirection == -1)
                NPC.rotation += MathHelper.Pi;
            SkillTimeUI.SkillTimeMax = 180;
            SkillTimeUI.SkillName = TheUtility.RegisterTextBySkill("Wind").Value;
            SkillTimeUI.SkillTime = (int)NPC.ai[0];
            if ((int)NPC.ai[0]++ == 180)
            {
                SoundEngine.PlaySound(Yharon.RoarSound, NPC.Center);
                int yharonWind = YharonNPC.YharonWind;
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), Target.Bottom, Vector2.Zero, yharonWind, 0, 0f, Target.whoAmI);
                for(int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), Target.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(200,500), Vector2.Zero, yharonWind, 0, 0f, Target.whoAmI);
                }
            }
        }
        public override bool ActivationCondition(NPCSkills activeSkill) => true;
        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.ai[0] > 200 && base.SwitchCondition(changeToSkill);
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            SkillTimeUI.Active = true;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[0] < 180)
            {
                NPC.frame.Y = ((int)NPC.ai[0] / 8 % 5) * frameHeight;
            }
            else
            {
                if(NPC.frameCounter++ > 4)
                {
                    NPC.frameCounter = 0;
                    if (NPC.localAI[0] < 2)
                    {
                        NPC.frame.Y = frameHeight * (Main.npcFrameCount[NPC.type] - (int)(2 - NPC.localAI[0]));
                        NPC.localAI[0]++;
                    }
                    else
                    {
                        NPC.frame.Y = 0;
                    }
                }
            }
        }
    }
}
