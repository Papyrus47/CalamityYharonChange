using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using ReLogic.Graphics;
using CalamityYharonChange.Content.Configs;

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
        public static float MoveX;
        public static float MoveY;
        public override void OnInitialize()
        {
            base.OnInitialize();
            Width.Set(300, 0);
            Height.Set(30, 0);
        }
        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                MoveX = YharonConfig.Instance.uiConfig.ReadUIX;
                MoveY = YharonConfig.Instance.uiConfig.ReadUIY;
                Width.Set(300, 0);
                Height.Set(16, 0);
                if (BossBarLoader.CurrentStyle is BossHealthBarManager)
                {
                    Top.Set(Main.screenHeight - 120 + MoveY, 0);
                    Left.Set(Main.screenWidth - 420 + MoveX, 0);
                }
                else
                {
                    Top.Set(Main.screenHeight - 100 + MoveY, 0);
                    Left.Set(Main.screenWidth / 2 - Width.Pixels / 2 + MoveX, 0);
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
                Vector2 size = FontAssets.MouseText.Value.MeasureString(SkillName);

                spriteBatch.Draw(lineTex, rect, null, Color.Black);
                Rectangle rectangle = rect;
                rectangle.X += 2;
                spriteBatch.Draw(texture, rectangle, null, (Color.White * 0.1f) with { A = 50 });
                rectangle = rect;
                rectangle.X -= 2;
                spriteBatch.Draw(texture, rectangle, null, (Color.White * 0.1f) with { A = 50 });
                rectangle = rect;
                rectangle.Y += 2;
                spriteBatch.Draw(texture, rectangle, null, (Color.White * 0.1f) with { A = 50 });
                rectangle = rect;
                rectangle.Y -= 2;
                spriteBatch.Draw(texture, rectangle, null, (Color.White * 0.1f) with { A = 50 });

                spriteBatch.Draw(lineTex, new Rectangle(rect.X, rect.Y, (int)(rect.Width * Math.Min(1, (float)SkillTime / SkillTimeMax)), rect.Height), null, Color.White);

                spriteBatch.Draw(texture, rect, null, Color.Gold);

                Vector2 size1 = size * 0.5f;
                size1.X = size.X;
                size1.Y = 0;

                Color black = Color.Black with { A = 100 };
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Right(), black, 0f, size1 + new Vector2(-1, -5) * 0.4f, rect.Height / size.Y * 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Right(), black, 0f, size1 + new Vector2(1, -5) * 0.4f, rect.Height / size.Y * 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Right(), black, 0f, size1 + new Vector2(1, -4) * 0.4f, rect.Height / size.Y * 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Right(), black, 0f, size1 + new Vector2(-1, -6) * 0.4f, rect.Height / size.Y * 2f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Right(), Color.Gold, 0f, size1 + new Vector2(-1, -5) * 0.1f, rect.Height / size.Y * 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Right(), Color.Gold, 0f, size1 + new Vector2(1, -5) * 0.1f, rect.Height / size.Y * 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Right(), Color.Gold, 0f, size1 + new Vector2(1, -4) * 0.1f, rect.Height / size.Y * 2f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Right(), Color.Gold, 0f, size1 + new Vector2(-1, -6) * 0.1f, rect.Height / size.Y * 2f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(FontAssets.MouseText.Value, SkillName, rect.Right(), Color.White, 0f, size1 + new Vector2(0,-5) * 0.1f, rect.Height / size.Y * 2f, SpriteEffects.None, 0f);
            }
            else
                SkillName = "";
        }
    }
}
