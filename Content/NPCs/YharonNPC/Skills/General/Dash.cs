using CalamityMod.NPCs.Yharon;
using CalamityYharonChange.Core.SkillsNPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills.General
{
    /// <summary>
    /// 冲刺AI
    /// </summary>
    public class Dash : BasicYharonSkills
    {
        /// <summary>
        /// 冲刺持续时间,单位帧
        /// </summary>
        public int DashTime = 30;
        /// <summary>
        /// 冲刺速度
        /// </summary>
        public int DashSpeed = 10;
        public enum DashState : int
        {
            /// <summary>
            /// 等待
            /// </summary>
            Wait,
            /// <summary>
            /// 冲刺
            /// </summary>
            Dash,
            /// <summary>
            /// 结束
            /// </summary>
            End
        }
        public Action<Dash> OnDashAI;
        public Dash(NPC npc, int dashTime, int dashSpeed) : base(npc)
        {
            DashTime = dashTime;
            if (DashTime < 60)
                DashTime = 60;
            DashSpeed = dashSpeed;
        }
        public override void AI()
        {
            switch ((DashState)NPC.ai[0])
            {
                case DashState.Wait:
                    Vector2 vel = (Target.Center - NPC.Center).SafeNormalize(default);
                    NPC.spriteDirection = NPC.direction = (vel.X > 0).ToDirectionInt();
                    NPC.velocity += vel;
                    NPC.rotation = NPC.velocity.ToRotation();
                    NPC.velocity *= 0.9f;
                    if (NPC.spriteDirection == -1)
                        NPC.rotation += MathHelper.Pi;
                    if (NPC.ai[1] > 1)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[0] = (int)DashState.Dash;
                        NPC.velocity = vel * DashSpeed;
                        if(DashTime <= 30)
                        {
                            SoundEngine.PlaySound(Yharon.RoarSound, NPC.Center);
                        }
                        else
                        {
                            SoundEngine.PlaySound(Yharon.ShortRoarSound, NPC.Center);
                        }
                    }
                    break;
                case DashState.Dash:
                    OnDashAI?.Invoke(this);
                    NPC.ai[1]++;
                    if (NPC.ai[1] > DashTime)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[0] = (int)DashState.End;
                    }
                    break;
            }
        }
        public override bool SwitchCondition(NPCSkills changeToSkill) => (DashState)NPC.ai[0] == DashState.End;
        public override bool ActivationCondition(NPCSkills activeSkill) => true;
        public override void FindFrame(int frameHeight)
        {
            switch ((DashState)NPC.ai[0])
            {
                case DashState.Wait:
                    if(NPC.frameCounter++ > 4)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y += frameHeight;
                        if(NPC.frame.Y >= frameHeight * (Main.npcFrameCount[NPC.type] - 2))
                        {
                            NPC.ai[1]++;
                            NPC.frame.Y = 0;
                        }
                    }
                    break;
                case DashState.Dash:
                    NPC.frame.Y = 0;
                    break;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            DrawAfterimage(spriteBatch, screenPos);
            base.PreDraw(spriteBatch, screenPos, drawColor);
            return false;
        }
    }
}
