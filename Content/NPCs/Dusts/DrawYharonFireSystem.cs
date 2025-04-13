using CalamityYharonChange.Content.Projs.Bosses.Yharon;
using CalamityYharonChange.Core.RenderHelper;

namespace CalamityYharonChange.Content.NPCs.Dusts
{
    public class DrawYharonFireSystem : DrawRender
    {
        public static List<int> fires = new();
        private static DrawYharonFireSystem instance;

        public static DrawYharonFireSystem Instance 
        {
            get
            {
                instance ??= new();
                return instance;  
            }
            private set => instance = value; 
        }
        public override void Draw(GraphicsDevice gd, SpriteBatch sb, RenderTarget2D renderTarget1, RenderTarget2D renderTarget2)
        {
            #region 保存原图片
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();
            #endregion
            #region 绘制火焰图片到自定义render上
            gd.SetRenderTarget(renderTarget1);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            foreach(var dustId in fires)
            {
                YharonFireDust.DrawSelf(Main.dust[dustId]);
            }
            fires.Clear();
            sb.End();
            #endregion
            #region 调用shader画火焰
            gd.SetRenderTarget(renderTarget2);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            Effect effect = AssetPreservation.YharonFireEffect.Value;
            effect.Parameters["tex"].SetValue(AssetPreservation.Perlin.Value);
            effect.Parameters["uChange"].SetValue(new Vector2(Main.GlobalTimeWrappedHourly * 0.5f));
            effect.Parameters["uColor"].SetValue(Color.OrangeRed.ToVector4() * 4);
            effect.CurrentTechnique.Passes[0].Apply();
            sb.Draw(renderTarget1, Vector2.Zero, null, Color.White);
            sb.End();
            #endregion
            #region 合并到原图片
            gd.SetRenderTarget(Main.screenTarget);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.Draw(renderTarget2, Vector2.Zero, Color.White with { A = 0 });
            sb.End();
            #endregion
        }
    }
}
