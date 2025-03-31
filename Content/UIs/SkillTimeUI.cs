using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using ReLogic.Graphics;

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
        public static string SkillName;
        /// <summary>
        /// 激活
        /// </summary>
        public static bool Active;
        public override void OnInitialize()
        {
            base.OnInitialize();
            Width.Set(120, 0);
            Height.Set(16, 0);
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (BossBarLoader.CurrentStyle is BossHealthBarManager)
                {
                    Top.Set(Main.screenHeight - 120, 0);
                    Left.Set(Main.screenWidth - 420, 0);
                }
                else
                {
                    Top.Set(Main.screenHeight - 100, 0);
                    Left.Set(Main.screenWidth / 2 - Width.Pixels / 2, 0);
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
                spriteBatch.Draw(texture, rect, null, Color.White);
                Vector2 size = FontAssets.MouseText.Value.MeasureString(SkillName);
               
                spriteBatch.Draw(lineTex, new Rectangle(rect.X, rect.Y, (int)(rect.Width * Math.Min(1, (float)SkillTime / SkillTimeMax)), rect.Height), null, Color.OrangeRed);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Center(), Color.Black, 0f, size * 0.5f + new Vector2(-1, -5), size.Y / rect.Height * 0.5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Center(), Color.Black, 0f, size * 0.5f + new Vector2(1, -5), size.Y / rect.Height * 0.5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Center(), Color.Black, 0f, size * 0.5f + new Vector2(1, -4), size.Y / rect.Height * 0.5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Center(), Color.Black, 0f, size * 0.5f + new Vector2(-1, -6), size.Y / rect.Height * 0.5f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Center(), Color.White, 0f, size * 0.5f + new Vector2(0,-5), size.Y / rect.Height * 0.5f, SpriteEffects.None, 0f);
            }
            else
                SkillName = "";
        }
    }
}
