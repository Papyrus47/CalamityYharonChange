using CalamityYharonChange.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills.Phase1
{
    public class Phase1FinallyAttack : BasicPhase1Skills
    {
        public Phase1FinallyAttack(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            base.AI();
        }
        public override bool CompulsionSwitchSkill(NPCSkills activeSkill) => YharonNPC.MusicTimer >= YharonNPC.MusicTimerPhase1 - 120;
    }
}
