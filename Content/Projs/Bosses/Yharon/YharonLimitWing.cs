using CalamityYharonChange.Content.NPCs.YharonNPC;
using CalamityYharonChange.Content.NPCs.YharonNPC.Modes;
using CalamityYharonChange.Content.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Projs.Bosses.Yharon
{
    public class YharonLimitWing : ModProjectile
    {
        public static readonly int MoveTime = 120; // 2秒移动时间
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 100000000;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1;
        }
        public override void AI()
        {
            if (YharonChangeSystem.YharonBoss == -1 || YharonChangeSystem.YharonBoss >= 0 && (Main.npc[YharonChangeSystem.YharonBoss].ModNPC as YharonNPC).CurrentMode is not YharonPhase1) // 清除弹幕
            {
                Projectile.ai[2]++;
                if (Projectile.ai[2] > 60f)
                    Projectile.Kill();
                return;
            }
            Projectile.scale = 1;
            Projectile.timeLeft = 60; // 保持不消失
            if (Projectile.ai[1] < MoveTime)
                Projectile.ai[1]++;
            Projectile.Center = YharonChangeSystem.YharonFixedPos + Vector2.UnitX * (Projectile.ai[1] / MoveTime * 45f + 15f) * 16f * Projectile.ai[0]; // 产生时候移动
            Projectile.position.Y += 4000; // 向下移动250格
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Main.instance.LoadProjectile(Type);
            Texture2D projTex = TextureAssets.Projectile[Type].Value; // 获取贴图
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.OrangeRed with { A = 0 } * (1f - Projectile.ai[2] / 60f) * (Projectile.ai[1] / MoveTime);
            Color color1 = Color.White with { A = 0 } * (1f - Projectile.ai[2] / 60f) * (Projectile.ai[1] / MoveTime);

            #region 竖着的
            _ = TextureAssets.Extra[193].Value; // 预加载
            // 193 作为激光升起来的贴图
            Texture2D lineTex = TextureAssets.Extra[193].Value;
            Vector2[] poses = new Vector2[4]
            {
                drawPos + new Vector2(Projectile.width,0), // 右下角
                drawPos + new Vector2(-Projectile.width,0), // 左下角
                drawPos + new Vector2(Projectile.width,-8000), // 右上角
                drawPos + new Vector2(-Projectile.width,-8000) // 左上角
            };
            CustomVertexInfo[] customVertexInfos = new CustomVertexInfo[4];
            float timer = Main.GlobalTimeWrappedHourly % 20;
            customVertexInfos[0] = new CustomVertexInfo(new Vector2(poses[0].X, poses[0].Y), color, new Vector3(timer, 0,0));  // 左下角
            customVertexInfos[1] = new CustomVertexInfo(new Vector2(poses[1].X, poses[1].Y), color, new Vector3(1 + timer, 1, 0));  // 右下角
            customVertexInfos[2] = new CustomVertexInfo(new Vector2(poses[2].X, poses[2].Y), color, new Vector3(timer, 0,0));  // 左上角
            customVertexInfos[3] = new CustomVertexInfo(new Vector2(poses[3].X, poses[3].Y), color, new Vector3(1 + timer, 1, 0));  // 右上角
            GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;
            graphicsDevice.Textures[0] = lineTex;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            //var origin = graphicsDevice.RasterizerState;
            //RasterizerState rasterizerState = new()
            //{
            //    CullMode = CullMode.None,
            //    FillMode = FillMode.WireFrame
            //};
            //graphicsDevice.RasterizerState = rasterizerState;
            //graphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, customVertexInfos, 0, 2);

            graphicsDevice.Textures[0] = TextureAssets.Extra[197].Value;
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, customVertexInfos, 0, 2);
            //graphicsDevice.RasterizerState = origin;
            #endregion
            #region 下面横着
            poses = new Vector2[4]
            {
                drawPos + new Vector2(0,Projectile.height), // 右下角
                drawPos + new Vector2(0,-Projectile.height), // 左下角
                drawPos + new Vector2((Projectile.ai[1] / MoveTime * 45f + 15f) * 32f * -Projectile.ai[0],Projectile.height), // 右上角
                drawPos + new Vector2((Projectile.ai[1] / MoveTime * 45f + 15f) * 32f * -Projectile.ai[0],-Projectile.height) // 左上角
            };
            customVertexInfos[0] = new CustomVertexInfo(new Vector2(poses[0].X, poses[0].Y), color, new Vector3(timer, 0, 0));  // 左下角
            customVertexInfos[1] = new CustomVertexInfo(new Vector2(poses[1].X, poses[1].Y), color, new Vector3(1 + timer, 1, 0));  // 右下角
            customVertexInfos[2] = new CustomVertexInfo(new Vector2(poses[2].X, poses[2].Y), color, new Vector3(timer, 0, 0));  // 左上角
            customVertexInfos[3] = new CustomVertexInfo(new Vector2(poses[3].X, poses[3].Y), color, new Vector3(1 + timer, 1, 0));  // 右上角
            graphicsDevice.Textures[0] = lineTex;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            //var origin = graphicsDevice.RasterizerState;
            //RasterizerState rasterizerState = new()
            //{
            //    CullMode = CullMode.None,
            //    FillMode = FillMode.WireFrame
            //};
            //graphicsDevice.RasterizerState = rasterizerState;
            //graphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, customVertexInfos, 0, 2);

            graphicsDevice.Textures[0] = TextureAssets.Extra[197].Value;
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, customVertexInfos, 0, 2);
            #endregion
            #region 上面横着
            poses = new Vector2[4]
            {
                drawPos + new Vector2(0,Projectile.height - 8000), // 右下角
                drawPos + new Vector2(0,-Projectile.height - 8000), // 左下角
                drawPos + new Vector2((Projectile.ai[1] / MoveTime * 45f + 15f) * 32f * -Projectile.ai[0],Projectile.height - 8000), // 右上角
                drawPos + new Vector2((Projectile.ai[1] / MoveTime * 45f + 15f) * 32f * -Projectile.ai[0],-Projectile.height - 8000) // 左上角
            };
            customVertexInfos[0] = new CustomVertexInfo(new Vector2(poses[0].X, poses[0].Y), color, new Vector3(timer, 0, 0));  // 左下角
            customVertexInfos[1] = new CustomVertexInfo(new Vector2(poses[1].X, poses[1].Y), color, new Vector3(1 + timer, 1, 0));  // 右下角
            customVertexInfos[2] = new CustomVertexInfo(new Vector2(poses[2].X, poses[2].Y), color, new Vector3(timer, 0, 0));  // 左上角
            customVertexInfos[3] = new CustomVertexInfo(new Vector2(poses[3].X, poses[3].Y), color, new Vector3(1 + timer, 1, 0));  // 右上角
            graphicsDevice.Textures[0] = lineTex;
            graphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            //var origin = graphicsDevice.RasterizerState;
            //RasterizerState rasterizerState = new()
            //{
            //    CullMode = CullMode.None,
            //    FillMode = FillMode.WireFrame
            //};
            //graphicsDevice.RasterizerState = rasterizerState;
            //graphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, customVertexInfos, 0, 2);

            graphicsDevice.Textures[0] = TextureAssets.Extra[197].Value;
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, customVertexInfos, 0, 2);

            #endregion
            const float scale = 1.6f;
            const float colorSclae = 1;
            Main.EntitySpriteDraw(projTex, drawPos, null, color * colorSclae, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos + new Vector2(0, -8000), null, color * colorSclae, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos, null, color * colorSclae, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos + new Vector2(0, -8000), null, color * colorSclae, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos, null, color * colorSclae, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos + new Vector2(0, -8000), null, color * colorSclae, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos, null, color1 * colorSclae, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * scale, SpriteEffects.None, 0f);
            Main.EntitySpriteDraw(projTex, drawPos + new Vector2(0, -8000), null, color1 * colorSclae, Projectile.rotation, projTex.Size() * 0.5f, Projectile.scale * scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            
            
            return false;
        }
    }
}
