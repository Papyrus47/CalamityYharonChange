using CalamityYharonChange.Core.SkillsNPC;
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
        }
        public override void OnSkillDeactivate(NPCSkills changeToSkill)
        {
            base.OnSkillDeactivate(changeToSkill);
            SkillTimeOut = false;
            NPC.ai[0] = NPC.ai[1] = NPC.ai[2] = NPC.ai[3] = 0;
        }
    }
}
