using CalamityYharonChange.Core.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.Partcles
{
    public class YharonBoomDust : BasicPartcle
    {
        public override Asset<Texture2D> Texture => AssetPreservation.Extra[2];
        public YharonBoomDust()
        {
            TimeLeftMax = 180;
            color = Color.Gray with { A = 0 } * 0.2f;
            extraUpdate = 2;
        }
        public override void PostUpdate()
        {
            base.PostUpdate();
            if (scale.X < 0)
                ShouldRemove = true;
            scale -= new Vector2(0.01f);
            velocity *= 0.94f;
        }
        public override void Draw()
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End(); 
            BlendState blend = new();
            blend.ColorBlendFunction = BlendFunction.ReverseSubtract;
            blend.ColorSourceBlend = Blend.One;
            blend.ColorDestinationBlend = Blend.InverseSourceAlpha;
            blend.AlphaDestinationBlend = Blend.One;
            blend.AlphaSourceBlend = Blend.Zero;

            sb.Begin(SpriteSortMode.Immediate, blend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Texture2D texture = Texture.Value;
            sb.Draw(texture, position - Main.screenPosition, null, color, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None,
               Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
