using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace CalamityYharonChange.Content.Configs
{
    public class YharonConfig : ModConfig
    {
        public static YharonConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
        }
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public UIConfig uiConfig = new();
        public class UIConfig
        {
            public float ReadUIX;
            public float ReadUIY;
        }
    }
}
