using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityYharonChange.Core.NPCAI
{
    public abstract class ExtraAI
    {
        /// <summary>
        /// 引用自身所在的集合，方便移除
        /// </summary>
        public List<ExtraAI> extraAIs;

        public NPC npc;

        public virtual void AI() { }

        public virtual void Remove()
        {
            extraAIs.Remove(this);
        }

        public void ApplyExtraAI(List<ExtraAI> list,NPC npc)
        {
            extraAIs = list;
            this.npc = npc;
            list.Add(this);
        }
        
    }
}
