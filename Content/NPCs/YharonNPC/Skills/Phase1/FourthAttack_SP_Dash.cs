using CalamityMod;
using CalamityMod.NPCs.Yharon;
using CalamityYharonChange.Content.NPCs.Dusts;
using CalamityYharonChange.Content.NPCs.YharonNPC.Skills.General;
using CalamityYharonChange.Content.Projs;
using CalamityYharonChange.Content.Projs.Bosses.Yharon;
using CalamityYharonChange.Content.UIs;
using CalamityYharonChange.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills.Phase1
{
    /// <summary>
    /// 冲刺改版例子
    /// </summary>
    public class FourthAttack_SP_Dash : Dash
    {
        /// <summary>
        /// 冲刺方向
        /// </summary>
        public Vector2 DashDir;
        /// <summary>
        /// 是否使用相反的技能
        /// </summary>
        public bool[] UseInvertSkill = new bool[3];
        public FourthAttack_SP_Dash(NPC npc) : base(npc, 10, 23)
        {
        }
        public override void AI()
        {
            if (NPC.alpha > 0)
            {
                NPC.velocity *= 0;
                NPC.alpha -= 255 / 30;
                return;
            }
            else
                NPC.alpha = 0;
            switch (DashState)
            {
                case DashMode.Wait:
                    Vector2 vel = (Target.Center - NPC.Center).SafeNormalize(default);
                    string Skill1 = UseInvertSkill[0] ? TheUtility.RegisterTextBySkill("FrontZhua").Value : TheUtility.RegisterTextBySkill("BackZhua").Value;
                    string Skill2 = UseInvertSkill[1] ? TheUtility.RegisterTextBySkill("Moon").Value : TheUtility.RegisterTextBySkill("Iron").Value;
                    string Skill3 = UseInvertSkill[2] ? TheUtility.RegisterTextBySkill("FireWing").Value : TheUtility.RegisterTextBySkill("FireTrail").Value;
                    SkillTimeUI.Active = true;
                    SkillTimeUI.SkillName = Skill1 + Skill2 + Skill3;
                    SkillTimeUI.SkillTimeMax = 240;
                    SkillTimeUI.SkillTime = (int)NPC.ai[2];
                    if (NPC.ai[2]++ < 20)
                    {
                        NPC.spriteDirection = NPC.direction = (vel.X > 0).ToDirectionInt();
                        DashDir = vel;
                        NPC.velocity += vel;
                        NPC.rotation = NPC.velocity.ToRotation();
                        NPC.velocity *= 0.2f;
                        NPC.ai[3] = Math.Max((Target.Center - NPC.Center).Length(),500) + 200;
                        if (NPC.spriteDirection == -1)
                            NPC.rotation += MathHelper.Pi;
                    }
                    else
                    {
                        NPC.velocity *= 0.9f;
                        if (NPC.ai[2] > 240)
                        {
                            NPC.ai[1] = 1;
                        }
                    }
                    if (NPC.ai[1] >= 1)
                    {
                        NPC.ai[2] = 0;
                        NPC.ai[1] = 0;
                        DashState = DashMode.Dash;
                        NPC.velocity = DashDir * (NPC.ai[3] / DashTime);
                        int yharonWind = YharonNPC.YharonWind;
                        Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), Target.Bottom, Vector2.Zero, yharonWind, 0, 0f, Target.whoAmI);
                        if (NPC.ai[3] / DashTime > 30)
                        {
                            SoundEngine.PlaySound(Yharon.RoarSound with { Volume = 2f }, NPC.Center);
                        }
                        else
                        {
                            SoundEngine.PlaySound(Yharon.ShortRoarSound with { Volume = 2f }, NPC.Center);
                        }
                    }
                    break;
                case DashMode.Dash:
                    SkillTimeUI.Active = false;
                    base.AI();
                    #region 烈翼
                    if ((int)NPC.ai[1] /* % 2*/ == 0)
                    {
                        if (UseInvertSkill[2])
                        {
                            /*
                            for (int i = 1; i <= 5; i++)
                            {
                                var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + NPC.velocity.RotatedBy(MathHelper.PiOver2).SafeNormalize(default) * (160 * i + 160f), Vector2.Zero, YharonNPC.YharonFire, NPC.GetProjectileDamage(YharonNPC.YharonFire), 0f, Target.whoAmI);
                                proj.scale = 1;
                                proj.timeLeft = 16;
                                proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + NPC.velocity.RotatedBy(-MathHelper.PiOver2).SafeNormalize(default) * (160 * i + 160f), Vector2.Zero, YharonNPC.YharonFire, NPC.GetProjectileDamage(YharonNPC.YharonFire), 0f, Target.whoAmI);
                                proj.scale = 1;
                                proj.timeLeft = 16;
                            }*/
                            Vector2 v = Vector2.Normalize(NPC.velocity);
                            Vector2 vert = Helper.VerticalVec(v);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vert * 600, v, ModContent.ProjectileType<FireBurst_Rectangle>(), NPC.GetProjectileDamage(ModContent.ProjectileType<FireBurst_Rectangle>()), 1, Main.myPlayer, 0, 8000, 800);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - vert * 600, v, ModContent.ProjectileType<FireBurst_Rectangle>(), NPC.GetProjectileDamage(ModContent.ProjectileType<FireBurst_Rectangle>()), 1, Main.myPlayer, 0, 8000, 800);

                        }
                        else
                        {
                            Vector2 v = Vector2.Normalize(NPC.velocity);
                            Vector2 vert = Helper.VerticalVec(v);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<FireBurst_Rectangle>(), NPC.GetProjectileDamage(ModContent.ProjectileType<FireBurst_Rectangle>()), 1, Main.myPlayer, 0, 8000, 800);

                            /*
                            var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, YharonNPC.YharonFire, NPC.GetProjectileDamage(YharonNPC.YharonFire), 0f, Target.whoAmI);
                            proj.scale = 2;
                            proj.timeLeft = 16;*/
                        }
                    }
                    #endregion
                    break;
                case DashMode.End:
                    NPC.velocity *= 0.6f;
                    #region 前后爪
                    if ((int)NPC.ai[1] == 90)
                    {
                        if (UseInvertSkill[0])
                        {
                            /*
                            for (int i = -10; i <= 10; i++)
                            {
                                var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, DashDir.RotatedBy(MathHelper.ToRadians(120) / 20f * i) * 10, YharonNPC.YharonFire, NPC.GetProjectileDamage(YharonNPC.YharonFire), 0f, Target.whoAmI);
                                proj.extraUpdates = 10;
                                proj.localAI[1] = Math.Abs(i / 10f);
                            }*/
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, DashDir, ModContent.ProjectileType<FireBurst>(), NPC.GetProjectileDamage(ModContent.ProjectileType<FireBurst>()), 0f, Main.myPlayer);
                            NPC.velocity = -DashDir * 80;
                        }
                        else
                        {
                            /*
                            for (int i = -10; i <= 10; i++)
                            {
                                var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, -DashDir.RotatedBy(MathHelper.ToRadians(120) / 20f * i) * 10, YharonNPC.YharonFire, NPC.GetProjectileDamage(YharonNPC.YharonFire), 0f, Target.whoAmI);
                                proj.extraUpdates = 10;
                                proj.localAI[1] = Math.Abs(i / 10f);
                            }*/
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, -DashDir, ModContent.ProjectileType<FireBurst>(), NPC.GetProjectileDamage(ModContent.ProjectileType<FireBurst>()), 0f, Main.myPlayer);

                            NPC.velocity = DashDir * 80;
                        }
                    }
                    else if (!UseInvertSkill[0] && NPC.ai[1] > 80 && NPC.ai[1] < 90)
                    {
                        
                        NPC.velocity = (NPC.velocity * 10 - DashDir * 10) / 11f;
                        NPC.rotation = NPC.velocity.ToRotation();
                        NPC.velocity *= 0.5f;
                        NPC.spriteDirection = NPC.direction = (DashDir.X < 0).ToDirectionInt();
                        if (NPC.spriteDirection == -1)
                            NPC.rotation += MathHelper.Pi;
                    }
                    #endregion
                    #region 月华/钢铁
                    if ((int)NPC.ai[1] == 90)
                    {
                        if (UseInvertSkill[1]) // 月华
                        {
                            //var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, YharonNPC.YharonMoonLighting, NPC.GetProjectileDamage(YharonNPC.YharonMoonLighting), 0f, Target.whoAmI, 400f,4000);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MoonHalo>(), NPC.GetProjectileDamage(ModContent.ProjectileType<MoonHalo>()), 0f, Main.myPlayer);
                        }
                        else // 钢铁
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<FireBurstRing2>(), NPC.GetProjectileDamage(ModContent.ProjectileType<FireBurstRing2>()), 0f, Main.myPlayer, 0, 0f);
                            //var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, YharonNPC.YharonFireBoom, NPC.GetProjectileDamage(YharonNPC.YharonFireBoom), 0f, Target.whoAmI, 5f);
                        }
                    }
                    #endregion
                    if (NPC.ai[1]++ > 240)
                    {
                        NPC.ai[0]++;
                    }
                    else if (NPC.ai[1] > 120)
                    {
                        /*
                        vel = (Target.Center - NPC.Center).SafeNormalize(default);
                        NPC.spriteDirection = NPC.direction = (vel.X > 0).ToDirectionInt();
                        NPC.velocity += vel;
                        NPC.rotation = NPC.velocity.ToRotation();
                        NPC.velocity *= 0.2f;
                        if (NPC.spriteDirection == -1)
                            NPC.rotation += MathHelper.Pi;*/
                    }
                    break;
                default:
                    break;
            }
        }
        public override void OnSkillActive(NPCSkills activeSkill)
        {
            base.OnSkillActive(activeSkill);
            UseInvertSkill[0] = Main.rand.NextBool();
            UseInvertSkill[1] = Main.rand.NextBool();
            UseInvertSkill[2] = Main.rand.NextBool();
            NPC.alpha = 255;
            NPC.Center = Target.Center + Main.rand.NextVector2Unit() * 300;
        }
        public override bool SwitchCondition(NPCSkills changeToSkill) => NPC.ai[0] > (int)DashMode.End;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;
        public override void FindFrame(int frameHeight)
        {
            switch ((DashMode)NPC.ai[0])
            {
                case DashMode.Wait:
                    if (NPC.frameCounter++ > 4)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y += frameHeight;
                        if (NPC.frame.Y >= frameHeight * (Main.npcFrameCount[NPC.type] - 2))
                        {
                            NPC.frame.Y = 0;
                        }
                    }
                    break;
                case DashMode.Dash:
                    NPC.frame.Y = frameHeight * (Main.npcFrameCount[NPC.type] - 2);
                    NPC.frameCounter = 0;
                    break;
                case DashMode.End:
                    if (NPC.frameCounter++ > 4)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y += frameHeight;
                        if (NPC.frame.Y >= frameHeight * (Main.npcFrameCount[NPC.type] - 2))
                        {
                            NPC.frame.Y = 0;
                        }
                    }
                    if (NPC.ai[1] > 82 && NPC.ai[1] <= 90)
                        NPC.frame.Y = frameHeight * (Main.npcFrameCount[NPC.type] - 2);
                    else if (NPC.ai[1] > 90 && NPC.ai[1] <= 98)
                        NPC.frame.Y = frameHeight * (Main.npcFrameCount[NPC.type] - 1);
                    break;
            }
        }
    }
}
