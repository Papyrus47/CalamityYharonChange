using CalamityMod;
using CalamityMod.NPCs;
using CalamityYharonChange.Content.NPCs.YharonNPC.Skills.General;
using CalamityYharonChange.Content.Projs.Bosses.Yharon;
using CalamityYharonChange.Content.UIs;
using CalamityYharonChange.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills.Phase1
{
    public class Phase1FinallyAttack : BasicPhase1Skills
    {
        public Phase1FinallyAttack(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            NPC.rotation = NPC.rotation.AngleLerp(0, 0.1f);
            NPC.ai[2]++;
            NPC.velocity *= 0.9f;
            CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();
            calamityGlobalNPC.AITimer = calamityGlobalNPC.KillTime;
            SkillTimeUI.SkillTimeMax = 600;
            SkillTimeUI.SkillTime = (int)NPC.ai[2];
            SkillTimeUI.Active = true;
            SkillTimeUI.SkillName = TheUtility.RegisterTextBySkill("HellFire").Value;
            if (NPC.life < NPC.lifeMax * 0.001f)
            {
                NPC.life = (int)(NPC.lifeMax * 0.001f);
                NPC.dontTakeDamage = true;
            }
            if ((int)NPC.ai[2] == 120)
            {
                NPC.ai[0] = NPC.ai[1] = 0;
                var Calamity = NPC.Calamity();
                Calamity.AITimer = Calamity.KillTime; // 解除动态减伤
            }
            else if ((int)NPC.ai[2] == 600)
                NPC.ai[0] = NPC.ai[1] = 0;
            
            if (NPC.ai[2] >= 600 && NPC.life > NPC.lifeMax * 0.001f)
            {
                if (NPC.ai[0] > 1)
                {
                    NPC.velocity.Y = -10 - (NPC.ai[0]++);
                    SkillTimeUI.Active = false;
                    if (NPC.Distance(Target.Center) > 1000)
                        NPC.active = false;
                    return;
                }
                NPC.velocity *= 0.5f;
                if (NPC.velocity.Length() < 3 && NPC.ai[0] < 1)
                {
                    NPC.ai[0] = 2;
                    for (int i = -50; i <= 50; i++)
                    {
                        var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(360) / 100f * i) * 10, YharonNPC.YharonFire, 114514, 0f, Target.whoAmI);
                        proj.extraUpdates = 10;
                        proj.timeLeft = 30000;
                        proj.localAI[1] = Math.Abs(i / 50f);
                        (proj.ModProjectile as YharonFire).IsKillPlayer = true;
                    }
                }
            }
        }
        public override bool ActivationCondition(NPCSkills activeSkill) => false;
        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.life < NPC.lifeMax * 0.001f;
        public override bool CompulsionSwitchSkill(NPCSkills activeSkill) => YharonNPC.musicSupport.Bar >= YharonNPC.MusicTimerPhase1 && YharonNPC.musicSupport.OnBeat(4,1);
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ > 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= frameHeight * (Main.npcFrameCount[NPC.type] - 2))
                {
                    NPC.ai[1]++;
                    NPC.frame.Y = 0;
                }
            }
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            NPC.dontTakeDamage = false;
        }
    }
}
