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
            YharonBoss = -1; // 记录Boss的whoAmI
        }
    }
}
