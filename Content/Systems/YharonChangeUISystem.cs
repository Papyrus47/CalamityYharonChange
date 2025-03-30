using CalamityYharonChange.Content.UIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace CalamityYharonChange.Content.Systems
{
    public class YharonChangeUISystem : ModSystem
    {
        public static SkillTimeUI skillTimeUI;
        public static UserInterface skillTimeUserInterface;
        public override void Load()
        {
            skillTimeUI = new();
            skillTimeUI.Initialize();
            skillTimeUserInterface = new UserInterface();
            skillTimeUserInterface.SetState(skillTimeUI);
        }
        public override void Unload()
        {
            skillTimeUserInterface.SetState(null);
            skillTimeUserInterface = null;
            skillTimeUI = null;
        }
        public override void UpdateUI(GameTime gameTime)
        {
            skillTimeUserInterface.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(x => x.Name.Equals("Vanilla: Mouse Text"));
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer("Calamity Yharon Change:Skill Time", () =>
                {
                    skillTimeUserInterface.Draw(Main.spriteBatch, new());
                    return true;
                }, InterfaceScaleType.UI));
            }
        }
    }
}
