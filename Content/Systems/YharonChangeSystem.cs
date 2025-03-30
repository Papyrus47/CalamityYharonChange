using CalamityYharonChange.Content.NPCs.YharonNPC;
using CalamityYharonChange.Content.UIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace CalamityYharonChange.Content.Systems
{
    public class YharonChangeSystem : ModSystem
    {
        /// <summary>
        /// 检测世界是否存在YharonBoss
        /// </summary>
        public static int YharonBoss;
        /// <summary>
        /// 这是Boss战固定的初始位置
        /// </summary>
        public static Vector2 YharonFixedPos;
        public override void PostUpdateEverything()
        {
            if (YharonBoss == -1)
            {
                YharonFixedPos = default;
            }
            bool CleanYharonBoss = true;
            foreach(NPC npc in Main.npc)
            {
                if(npc.active && npc.type == ModContent.NPCType<YharonNPC>())
                {
                    CleanYharonBoss = false;
                }
            }
            if (CleanYharonBoss)
                YharonBoss = -1;
        }
    }
}
