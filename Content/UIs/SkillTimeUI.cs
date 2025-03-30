using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace CalamityYharonChange.Content.UIs
{
    /// <summary>
    /// 读条UI
    /// </summary>
    public class SkillTimeUI : UIState
    {
        /// <summary>
        /// 请在外面赋值的读条数值
        /// </summary>
        public static int SkillTime;
        /// <summary>
        /// 请在外面赋值的读条最大值
        /// </summary>
        public static int SkillTimeMax;
        /// <summary>
        /// 激活
        /// </summary>
        public static bool Active;
        public override void OnInitialize()
        {
            base.OnInitialize();
            Width.Set(60, 0);
            Height.Set(8, 0);
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (BossBarLoader.CurrentStyle is BossHealthBarManager)
                {
                    Top.Set(Main.screenHeight - 100, 0);
                    Left.Set(Main.screenWidth - 420, 0);
                }
                else
                {
                    Top.Set(Main.screenHeight - 150, 0);
                    Left.Set(Main.screenWidth / 2, 0);
                }
                base.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                _ = ModContent.Request<Texture2D>(TheUtility.GetInstancePartWithName(this));
                Texture2D texture = ModContent.Request<Texture2D>(TheUtility.GetInstancePartWithName(this)).Value;
                Texture2D lineTex = TextureAssets.BlackTile.Value;
                var dimensions = GetDimensions();
                var rect = dimensions.ToRectangle();
                spriteBatch.Draw(texture, rect,null, Color.White);
                spriteBatch.Draw(lineTex, new Rectangle(rect.X, rect.Y, (int)(rect.Width * ((float)SkillTime / SkillTimeMax)), rect.Height), null, Color.OrangeRed);
            }
        }
    }
}
