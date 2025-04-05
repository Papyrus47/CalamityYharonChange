using CalamityMod;
using CalamityYharonChange.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Modes
{
    /// <summary>
    /// 一阶段的Mode
    /// </summary>
    public class YharonPhase1 : BaiscYharonPhase
    {
        public YharonPhase1(NPC npc) : base(npc)
        {
        }
        public override void OnEnterMode()
        {
            YharonNPC.NPC.Calamity().AITimer = 0;
            YharonNPC.NPC.Calamity().KillTime = YharonNPC.MusicTimerPhase1 - 120;
        }
    }
}
