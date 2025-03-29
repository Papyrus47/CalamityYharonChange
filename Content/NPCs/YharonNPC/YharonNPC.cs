﻿using CalamityMod;
using CalamityMod.NPCs;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using CalamityYharonChange.Content.NPCs.YharonNPC.Modes;
using CalamityYharonChange.Content.Projs;
using CalamityYharonChange.Content.Skys;
using CalamityYharonChange.Content.Systems;
using CalamityYharonChange.Core.SkillsNPC;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Effects;
using static CalamityMod.NPCStats;

namespace CalamityYharonChange.Content.NPCs.YharonNPC
{
    [AutoloadBossHead]
    public class YharonNPC : BasicSkillNPC
    {
        public Player TargetPlayer
        {
            get
            {
                if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                    NPC.TargetClosest();
                return Main.player[NPC.target];
            }
        }
        public static float normalDR = 0.22f;
        public static float EnragedDR = 0.9f;

        public static Asset<Texture2D> GlowTextureGreen;

        public static Asset<Texture2D> GlowTextureOrange;

        public static Asset<Texture2D> GlowTexturePurple;
        public static int Phase1Music;
        public static int FlareBomb;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.npcFrameCount[NPC.type] = 7;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers();
            nPCBestiaryDrawModifiers.Scale = 0.3f;
            nPCBestiaryDrawModifiers.PortraitScale = 0.4f;
            nPCBestiaryDrawModifiers.PortraitPositionYOverride = -16f;
            nPCBestiaryDrawModifiers.SpriteDirection = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers value = nPCBestiaryDrawModifiers;
            value.Position.X += 26f;
            value.Position.Y -= 14f;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            if (!Main.dedServ)
            {
                GlowTextureGreen = ModContent.Request<Texture2D>(Texture + "GlowGreen");
                GlowTextureOrange = ModContent.Request<Texture2D>(Texture + "GlowOrange");
                GlowTexturePurple = ModContent.Request<Texture2D>(Texture + "GlowPurple");
            }
            CalamityMod.CalamityMod.bossKillTimes.Add(Type, 14700);
            Phase1Music = MusicLoader.GetMusicSlot("CalamityYharonChange/Assets/Sounds/Music/YharonPhase1");
            FlareBomb = ModContent.ProjectileType<FlareBomb>();
        }
        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 50f;
            NPC.damage = Main.masterMode ? 900 : CalamityWorld.death ? 512 : CalamityWorld.revenge ? 480 : Main.expertMode ? 448 : 300;
            NPC.width = 200;
            NPC.height = 200;
            NPC.defense = 90;
            NPC.LifeMaxNERB(1300000, 1560000, 740000);
            double HPBoost = (double)CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.lifeMax /= 10;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.value = Item.buyPrice(10);
            NPC.boss = true;
            NPC.DR_NERD(normalDR);
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.DeathSound = Yharon.DeathSound;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
            Music = Phase1Music;
        }
        public override bool CheckActive() => false;
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[1]
            {
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.Yharon")
            });
        }
        public override void AI()
        {
            CalamityGlobalNPC calamityGlobalNPC = NPC.Calamity();
            calamityGlobalNPC.DR = normalDR;
            calamityGlobalNPC.CurrentlyIncreasingDefenseOrDR = true;
            base.AI();
            SkyManager.Instance.Activate(nameof(YharonSky));
            YharonChangeSystem.YharonBoss = -1;
            if (YharonChangeSystem.YharonBoss != -1)
            {
                NPC.active = false;
                return;
            }
            YharonChangeSystem.YharonBoss = NPC.whoAmI; // 记录Boss的whoAmI
        }
        public override void Init()
        {
            SaveModes = new();
            SaveModesID = new();
            SaveSkills = new();
            SaveSkillsID = new();
            OldSkills = new();

            YharonChangeSystem.YharonFixedPos = TargetPlayer.position; // 固定战斗场地位置
            Projectile.NewProjectile(NPC.GetSource_FromAI(), TargetPlayer.position, Vector2.Zero, ModContent.ProjectileType<YharonLimitWing>(), 0, 0, TargetPlayer.whoAmI, 1);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), TargetPlayer.position, Vector2.Zero, ModContent.ProjectileType<YharonLimitWing>(), 0, 0, TargetPlayer.whoAmI, -1);
            #region 状态
            YharonPhase1 yharonPhase1 = new YharonPhase1(NPC);

            SkillNPC.Register(yharonPhase1);
            CurrentMode = yharonPhase1;
            #endregion
            #region 技能
            #endregion
        }
        /// <summary>
        /// 技能强制跳出后执行
        /// </summary>
        public override void OnSkillTimeOut()
        {

        }
    }
}
