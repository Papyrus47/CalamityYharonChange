using CalamityMod;
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
    public class Phase1FinallyAttack : Dash
    {
        public Phase1FinallyAttack(NPC npc) : base(npc,30,20)
        {
        }
        public override void AI()
        {
            NPC.dontTakeDamage = false;
            NPC.ai[2]++;
            SkillTimeUI.SkillTimeMax = 600;
            SkillTimeUI.SkillTime = (int)NPC.ai[2];
            SkillTimeUI.Active = true;
            SkillTimeUI.SkillName = TheUtility.RegisterTextBySkill("HellFire").Value;
            if (NPC.life < NPC.lifeMax * 0.001f)
                NPC.life = (int)(NPC.lifeMax * 0.001f);
            if (NPC.ai[2] <= 120 && NPC.life < NPC.lifeMax * 0.01f)
                NPC.life = (int)(NPC.lifeMax * 0.01f);
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
                return;
            }
            base.AI();
            if(DashState == DashMode.End)
            {
                DashState = DashMode.Wait;
                NPC.ai[1] = 1;
            }
        }
        public override bool CompulsionSwitchSkill(NPCSkills activeSkill) => YharonNPC.musicSupport.Bar >= (int)((YharonNPC.MusicTimerPhase1 - 120) / YharonNPC.musicSupport.Unit);
    }
}
