using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Content.NPCs.YharonNPC.Skills.General
{
    public class OnDead : BasicYharonSkills
    {
        public OnDead(NPC npc) : base(npc)
        {
        }
        public override void AI()
        {
            Vector2 vel = (Target.Center - NPC.Center).SafeNormalize(default);
            NPC.spriteDirection = NPC.direction = (vel.X > 0).ToDirectionInt();
            vel.X = 0;
            vel.Y = NPC.ai[0];
            NPC.ai[0] += NPC.ai[0] + 0.1f;
            NPC.velocity = vel;
            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2 * NPC.spriteDirection;
            NPC.velocity *= 0.9f;
            if (NPC.spriteDirection == -1)
                NPC.rotation += MathHelper.Pi;
            if (NPC.Distance(Target.Center) > 1000)
                NPC.active = false;
        }
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ > 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= frameHeight * (Main.npcFrameCount[NPC.type] - 2))
                {
                    NPC.ai[1]++;
                    NPC.frame.Y = 0;
                }
            }
        }
    }
}
