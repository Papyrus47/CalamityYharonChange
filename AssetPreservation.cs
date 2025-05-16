namespace CalamityYharonChange
{
    /// <summary>
    /// 资产保存类
    /// </summary>
    public static class AssetPreservation
    {
        public static string SavePath = "CalamityYharonChange/Assets/";
        public static string SavePath_Images = "CalamityYharonChange/Assets/Images/";
        public static string SavePath_Effect = "CalamityYharonChange/Assets/Effects/";
        public static Asset<Texture2D> Perlin;
        public static Dictionary<int, Asset<Texture2D>> Extra;
        public static Asset<Effect> YharonFireEffect;
        public static void Load()
        {
            Perlin = ModContent.Request<Texture2D>(SavePath_Images + "Perlin");
            YharonFireEffect = ModContent.Request<Effect>(SavePath_Effect + "YharonFireEffect");
            AddExtra();
        }
        private static void AddExtra()
        {
            Extra = new();
            int i = 0;
            while (true)
            {
                if (ModContent.HasAsset(SavePath_Images + "Extra/Extra_" + i))
                    Extra.Add(i, ModContent.Request<Texture2D>(SavePath_Images + "Extra/Extra_" + i));
                else
                    break;
                i++;
            }
        }
        public static void UnLood()
        {
            Perlin = null;

            Extra?.Clear();
            Extra = null;
        }
    }
}
