using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityYharonChange.Content.Buffs;
using CalamityYharonChange.Content.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange
{
    public class CalamtiyYharonPlayer : ModPlayer
    {
        /// <summary>
        /// 炼狱龙炎
        /// </summary>
        public bool hellDragonFire;
        /// <summary>
        /// 撕裂buff
        /// </summary>
        public bool flyWind;
        public int PlayerFly;
        public override void ResetEffects()
        {
            hellDragonFire = false;
            flyWind = false;
        }
        public override void UpdateDead()
        {
            hellDragonFire = false;
            flyWind = false;
        }
        public override void PreUpdate()
        {
            if (YharonChangeSystem.YharonBoss != -1)
            {
                if (Math.Abs(Player.Center.X - YharonChangeSystem.YharonFixedPos.X) > 960 || Math.Abs(Player.Center.Y - YharonChangeSystem.YharonFixedPos.Y) > 4000)
                {
                    Player.AddBuff(ModContent.BuffType<HellDragonfire>(), 20);
                }
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (hellDragonFire)
            {
                damageSource = PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral(CalamityUtils.GetText("Status.Death.Dragonfire" + Main.rand.Next(1, 5)).Format(base.Player.name)));
            }
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (hellDragonFire && drawInfo.shadow == 0f)
            {
                Dragonfire.DrawEffects(drawInfo);
            }
        }
        public override void UpdateBadLifeRegen()
        {
            ApplyDoTDebuff(hellDragonFire, 200);
            ApplyDoTDebuff(flyWind, 100);
        }
        public override void PreUpdateMovement()
        {
            base.PreUpdateMovement();
            if(PlayerFly > 0)
            {
                PlayerFly--;
                Player.velocity.Y = -90f;
            }
            if (YharonChangeSystem.YharonBoss != -1)
            {
                if (Math.Abs(Player.Center.X - YharonChangeSystem.YharonFixedPos.X) > 960 || Math.Abs(Player.Center.Y - YharonChangeSystem.YharonFixedPos.Y) > 4000)
                {
                    Player.velocity += (YharonChangeSystem.YharonFixedPos - Player.Center).SafeNormalize(default);
                }
            }
        }
        void ApplyDoTDebuff(bool hasDebuff, int negativeLifeRegenToApply, bool immuneCondition = false)
        {
            if (!(!hasDebuff || immuneCondition))
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }

                Player.lifeRegenTime = 0f;
                Player.lifeRegen -= negativeLifeRegenToApply;
            }
        }
    }
}
