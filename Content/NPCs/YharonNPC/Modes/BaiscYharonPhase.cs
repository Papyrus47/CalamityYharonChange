using CalamityYharonChange.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Modes
{
    public abstract class BaiscYharonPhase : NPCModes
    {
        /// <summary>
        /// 获取YharonNPC
        /// </summary>
        public YharonNPC YharonNPC => NPC.ModNPC as YharonNPC;
        public BaiscYharonPhase(NPC npc) : base(npc)
        {
        }
    }
}
