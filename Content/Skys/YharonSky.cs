using CalamityYharonChange.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;

namespace CalamityYharonChange.Content.Skys
{
    /// <summary>
    /// 类似炼狱的火焰背景
    /// </summary>
    public class YharonSky : CustomSky
    {
        public struct FireBall
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Scale;
            public bool Active;
            public void Update()
            {
                Scale -= 0.05f;
                Velocity = Velocity.RotatedByRandom(0.2);
                if(Scale <= 0.1f)
                {
                    Active = false;
                }
            }
            public FireBall(Vector2 position, Vector2 velocity, float scale)
            {
                Position = position;
                Velocity = velocity;
                Scale = scale;
                Active = true;
            }
            public FireBall()
            {
                Position = default;
                Velocity = default;
                Scale = 1f;
                Active = false;
            }
        }
        public static FireBall[] fireBalls = new FireBall[300];
        public static Asset<Texture2D> BallTex => AssetPreservation.Extra[2];
        public static BasicSkillNPC YharonNPC;
        public static int Timeleft = 100; //弄一个计时器，让天空能自己消失
        public override void Update(GameTime gameTime)//天空激活时的每帧更新函数
        {
            if (!Main.gamePaused)//游戏暂停时不执行
            {
                if (Timeleft > 0)
                    Timeleft--;//只要激活时就会减少，这样就会在外部没赋值时自己消失了
                else
                {
                    if (SkyManager.Instance[nameof(YharonSky)].IsActive())
                    {
                        SkyManager.Instance.Deactivate(nameof(YharonSky));//消失
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (YharonNPC != null)
            {
                if (YharonNPC.NPC.CanBeChasedBy()) // NPC 存活
                {
                    if(minDepth < 9 && maxDepth > 9) // 绘制在背景景物后面，防止遮挡，当然你想的话，也可以去掉这个条件
                    {
                        for (int i = 0; i < fireBalls.Length; i++)
                        {
                            if (fireBalls[i].Active)
                            {
                                fireBalls[i].Update();
                                spriteBatch.Draw(BallTex.Value, fireBalls[i].Position, null, Color.White, 0f, Vector2.Zero, fireBalls[i].Scale, SpriteEffects.None, 0f);
                            }
                            else
                            {
                                fireBalls[i] = new(new Vector2(Main.rand.Next(Main.screenWidth), Main.screenHeight), -Vector2.UnitY.RotatedByRandom(0.4), 2.3f);
                            }
                        }
                    }
                    spriteBatch.Draw(TextureAssets.BlackTile.Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.OrangeRed * 0.3f);
                }
            }
        }
        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
        }

        public override void Deactivate(params object[] args)
        {

        }

        public override void Reset()
        {

        }

        public override bool IsActive() => Timeleft > 0;
    }
}
