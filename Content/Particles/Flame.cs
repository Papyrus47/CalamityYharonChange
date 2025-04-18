﻿using CalamityYharonChange.Core.Particles;
using CalamityYharonChange.Graphics;

namespace CalamityYharonChange.Content.Particles
{
    // From MEACMod
    public class Flame : BasicParticle
    {
        public float gravity = -0f;
        public new GradientColor color = Color.White;
        public Vector2 scaleVec = Vector2.One;
        public Color drawColor;

        public override void PostDraw(SpriteBatch spriteBatch)
        {
            velocity.Y += gravity;
            float p = lifetime / maxTime;
            drawColor = color.GetColor(p);
            drawColor.A = (byte)(255 * p);
        }

        public static Flame AddFlame(Vector2 pos, Vector2 vel, GradientColor color, int time = 40, float scale = 30)
        {
            Flame f = new Flame();
            f.position = pos;
            f.velocity = vel;
            f.color = color;
            f.maxTime = time;
            f.scaleVec = new Vector2(scale);
            f.rotation = Main.rand.NextFloat(6.28f);
            ParticlesSystem.particle.Add(f);
            return f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            return false;
        }

        //合批渲染
        public static void DrawFlames()
        {
            bool flag = false;
            foreach (BasicParticle p in ParticlesSystem.particle)
            {
                if (p is Flame f)
                {
                    flag = true;
                    break;
                }
            }

            if (flag)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                List<CustomVertexInfo> vertices = new List<CustomVertexInfo>();
                foreach (BasicParticle p in ParticlesSystem.particle)
                {
                    if (p is Flame f)
                    {
                        Vector2 sc = f.scaleVec;

                        Vector2 center = f.position;
                        Vector2 to11 = Vector2.One.RotatedBy(f.rotation);
                        Vector2 to10 = to11.RotatedBy(-1.5708f);
                        Vector2 to00 = -to11;
                        Vector2 to01 = -to10;

                        vertices.Add(new CustomVertexInfo(center + to00 * sc, new Vector3(0, 0, 0), f.drawColor));
                        vertices.Add(new CustomVertexInfo(center + to10 * sc, new Vector3(1, 0, 0), f.drawColor));
                        vertices.Add(new CustomVertexInfo(center + to01 * sc, new Vector3(0, 1, 0), f.drawColor));
                        vertices.Add(new CustomVertexInfo(center + to10 * sc, new Vector3(1, 0, 0), f.drawColor));
                        vertices.Add(new CustomVertexInfo(center + to01 * sc, new Vector3(0, 1, 0), f.drawColor));
                        vertices.Add(new CustomVertexInfo(center + to11 * sc, new Vector3(1, 1, 0), f.drawColor));
                    }
                }
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Flame").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("CalamityYharonChange/Assets/Images/Perlin").Value;
                Effect shader = ModContent.Request<Effect>("CalamityYharonChange/Assets/Effects/AlphaDissolve", AssetRequestMode.ImmediateLoad).Value;
                Helper.SetWCSMatrix(shader);
                shader.CurrentTechnique.Passes[0].Apply();
                if (vertices.Count > 3)
                {
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count / 3);
                }
                Main.spriteBatch.End();
                //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
