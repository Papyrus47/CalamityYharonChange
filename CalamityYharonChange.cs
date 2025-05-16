global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using ReLogic.Content;
global using System;
global using System.Collections.Generic;
global using System.Reflection;
global using Terraria;
global using Terraria.Localization;
global using CalamityYharonChange.Core.SwingHelpers;
global using Terraria.ModLoader;
global using System.IO;
global using Terraria.DataStructures;
global using Terraria.GameContent;
global using Terraria.Audio;
global using Terraria.Graphics.CameraModifiers;
global using Terraria.ID;
using CalamityYharonChange.Core.RenderHelper;
using Terraria.Graphics.Effects;
using CalamityYharonChange.Content.Skys;

namespace CalamityYharonChange
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class CalamityYharonChange : Mod
	{
        public static RenderTarget2D render1;
        public static RenderTarget2D render2;
        public static RenderHelperSystem renderHelperSystem = new();
        public override void Load()
        {
            On_FilterManager.EndCapture += On_FilterManager_EndCapture;
            On_Main.LoadWorlds += On_Main_LoadWorlds;
            Main.OnResolutionChanged += Main_OnResolutionChanged;
            SkyManager.Instance[nameof(YharonSky)] = new YharonSky();
            //MusicLoader.AddMusic(this, "Assets/Sounds/Music/YharonPhase1");
            AssetPreservation.Load();
        }
        public override void Unload()
        {
            AssetPreservation.UnLood();
        }
        private static void Main_OnResolutionChanged(Vector2 obj)
        {
            Main.QueueMainThreadAction(() =>
            {
                GraphicsDevice g = Main.instance.GraphicsDevice;
                render1 = new RenderTarget2D(Main.graphics.GraphicsDevice, g.PresentationParameters.BackBufferWidth,
                    g.PresentationParameters.BackBufferHeight, false, g.PresentationParameters.BackBufferFormat, 0);
                render2 = new RenderTarget2D(Main.graphics.GraphicsDevice, g.PresentationParameters.BackBufferWidth,
                    g.PresentationParameters.BackBufferHeight, false, g.PresentationParameters.BackBufferFormat, 0);
            });
        }

        private static void On_Main_LoadWorlds(On_Main.orig_LoadWorlds orig)
        {
            Main.QueueMainThreadAction(() =>
            {
                GraphicsDevice g = Main.instance.GraphicsDevice;
                render1 = new RenderTarget2D(Main.graphics.GraphicsDevice, g.PresentationParameters.BackBufferWidth,
                    g.PresentationParameters.BackBufferHeight, false, g.PresentationParameters.BackBufferFormat, 0);
                render2 = new RenderTarget2D(Main.graphics.GraphicsDevice, g.PresentationParameters.BackBufferWidth,
                    g.PresentationParameters.BackBufferHeight, false, g.PresentationParameters.BackBufferFormat, 0);
            });
            orig.Invoke();
        }
        public static void On_FilterManager_EndCapture(On_FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            renderHelperSystem?.Draw(render1, render2);
            orig.Invoke(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
    }
}
