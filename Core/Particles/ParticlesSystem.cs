using CalamityYharonChange.Content.Particles;
using CalamityYharonChange.Graphics;

namespace CalamityYharonChange.Core.Particles
{
    public class ParticlesSystem : ModSystem
    {
        public static Dictionary<BasicParticle.DrawLayer, List<BasicParticle>> particle = new();
        private static Dictionary<BasicParticle.DrawLayer, List<int>> particlesRemoveCache = new();

        //一种常用的火焰粒子，需要大量使用(增、删)以及合批渲染，所以单开一个集合
        public static HashSet<Flame> flames = new();

        public override void Load()
        {
            On_Main.DrawDust += PostDrawDusts;
            On_Main.DrawProjectiles += PostDrawProjectiles;
            On_Main.DrawTiles += OnMain_PostDrawTiles;
            On_Main.DrawPlayers_BehindNPCs += OnMain_PostDrawPlayers_BehindNPCs;
        }

        private static void DrawParticles(BasicParticle.DrawLayer layer)
        {
            if (particle.TryGetValue(layer, out List<BasicParticle> value))
            {
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                for (int i = 0; i < value.Count; i++) // 遍历每一个粒子
                {
                    BasicParticle particle = value[i];
                    particle.Draw();
                }
                Main.spriteBatch.End();
            }
        }

        private static void OnMain_PostDrawPlayers_BehindNPCs(On_Main.orig_DrawPlayers_BehindNPCs orig, Main self)
        {
            orig.Invoke(self);
            DrawParticles(BasicParticle.DrawLayer.AfterPlayer);
        }

        private static void OnMain_PostDrawTiles(On_Main.orig_DrawTiles orig, Main self, bool solidLayer, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride)
        {
            orig.Invoke(self, solidLayer, forRenderTargets, intoRenderTargets, waterStyleOverride);
            DrawParticles(BasicParticle.DrawLayer.AfterTile);
        }

        private static void PostDrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
        {
            orig.Invoke(self);
            DrawParticles(BasicParticle.DrawLayer.AfterProj);
        }

        private static void PostDrawDusts(On_Main.orig_DrawDust orig, Main self)
        {
            orig.Invoke(self);
            DrawParticles(BasicParticle.DrawLayer.AfterDust);
            Flame.DrawFlames();
        }

        public override void PostUpdateDusts()
        {
            particlesRemoveCache.Clear();
            foreach (var layer in particle.Keys) // 遍历每一层
            {
                for (int i = 0; i < particle[layer].Count; i++) // 遍历每一个粒子
                {
                    BasicParticle partcle = ParticlesSystem.particle[layer][i];
                    int extraUpdate = partcle.extraUpdate;
                    while (extraUpdate >= 0)
                    {
                        partcle.Update();
                        extraUpdate--;
                    }
                    if (partcle.ShouldRemove)
                    {
                        if (particlesRemoveCache.ContainsKey(layer))
                        {
                            particlesRemoveCache[layer].Add(i);
                        }
                        else
                        {
                            particlesRemoveCache.Add(layer, new(){ i });
                        }
                    }
                }
            }
            try
            {
                foreach (var layer in particlesRemoveCache.Keys)
                {
                    foreach (var part in particlesRemoveCache[layer])
                    {
                        particle[layer].RemoveAt(part);
                    }
                }
            }
            catch { }

            Flame.UpdateFlames();
        }
        
        public static void AddParticle(BasicParticle.DrawLayer drawLayer, BasicParticle particle)
        {
            if (ParticlesSystem.particle.TryGetValue(drawLayer, out List<BasicParticle> value))
                value.Add(particle);
            else
                ParticlesSystem.particle.Add(drawLayer, new() { particle });
        }
        
    }
}
