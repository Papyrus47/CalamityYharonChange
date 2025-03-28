using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Systems
{
    public class YharonChangeSystem : ModSystem
    {
        /// <summary>
        /// 检测世界是否存在YharonBoss
        /// </summary>
        public static int YharonBoss;
        public override void PreUpdateEntities()
        {
            YharonBoss = -1;
        }
    }
}
