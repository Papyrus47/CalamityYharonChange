using CalamityYharonChange.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills.General
{
    public class DashSystem : BasicYharonSkills
    {
        public List<Dash> DashSkills;
        public int CurrentDashSkillIndex;
        public DashSystem(NPC npc,List<Dash> dashes) : base(npc)
        {
            DashSkills = dashes;
        }
        public override void AI()
        {
            if(CurrentDashSkillIndex < DashSkills.Count)
            {
                DashSkills[CurrentDashSkillIndex].AI();
                if(CurrentDashSkillIndex + 1 < DashSkills.Count && DashSkills[CurrentDashSkillIndex].SwitchCondition(null))
                {
                    DashSkills[CurrentDashSkillIndex].OnSkillDeactivate(null);
                    CurrentDashSkillIndex++;
                    DashSkills[CurrentDashSkillIndex].OnSkillActive(null);
                }
            }
        }
        public override bool SwitchCondition(NPCSkills changeToSkill)
        {
            return CurrentDashSkillIndex == DashSkills.Count - 1 && DashSkills[CurrentDashSkillIndex].SwitchCondition(changeToSkill);
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            CurrentDashSkillIndex = 0;
        }
        public override bool ActivationCondition(NPCSkills activeSkill) => true;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => DashSkills[CurrentDashSkillIndex].PreDraw(spriteBatch, screenPos, drawColor);

        public override void FindFrame(int frameHeight)
        {
            DashSkills[CurrentDashSkillIndex].FindFrame(frameHeight);
        }
    }
}
