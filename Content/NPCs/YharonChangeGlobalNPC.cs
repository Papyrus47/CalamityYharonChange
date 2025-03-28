using CalamityYharonChange.Content.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs
{
    public class YharonChangeGlobalNPC : GlobalNPC
    {
        public override void ResetEffects(NPC npc)
        {
            if (npc.type == ModContent.NPCType<YharonNPC.YharonNPC>())
            {
                if(YharonChangeSystem.YharonBoss != -1)
                    npc.active = false; // 避免召唤更多Boss
                else
                    YharonChangeSystem.YharonBoss = npc.whoAmI; // 记录Boss的whoAmI
            }
        }
    }
}
