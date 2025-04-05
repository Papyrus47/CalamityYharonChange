namespace CalamityYharonChange.Core
{
    /// <summary>
    /// 音乐卡点
    /// </summary>
    public struct MusicSupport
    {
        public float Unit { get; }

        private int startOffset;
        public MusicSupport(float bpm, int startOffset)
        {
            this.startOffset = startOffset;
            Unit = 4 * 3600 / bpm;
        }
        public MusicSupport(float bpm, float sectionOffset)
        {
            Unit = 4 * 3600 / bpm;
            startOffset = (int)(sectionOffset * Unit);
        }
        /// <summary>
        /// 有几个小节的起始偏移
        /// </summary>
        /// <param name="sectionOffset"></param>
        public void SetOffset(float sectionOffset)
        {
            startOffset = (int)(sectionOffset * Unit);
        }
        public float Timer => realTimer;
        public float AdjTimer => realTimer - startOffset;
        private float realTimer;
        public void Update() => realTimer++;
        public void SetBar(float bar) => realTimer = bar * Unit + startOffset;
        public bool OnBeat(float n, int index = -1)
        {
            float rt = AdjTimer; // 获取音乐播放时间
            while (rt < 0)
            {
                rt += Unit;
            }
            bool on = Math.Abs(rt % (Unit / n)) < 1f;
            if (!on)
                return false;
            if (index < 0)
                return true;
            index--;
            float t = Math.Abs(rt % Unit);
            float per = Unit / n;
            int id = (int)(t / per);
            return id == index;
            //return Math.Abs((AdjTimer - startOffset) % (Unit / n)) < 1f;
        }
        public bool OnBeat(float n, out int index)
        {
            float rt = AdjTimer;
            while (rt < 0)
            {
                rt += Unit;
            }
            bool on = Math.Abs(rt % (Unit / n)) < 1f;
            float t = Math.Abs(rt % Unit);
            float per = Unit / n;
            index = (int)(t / per);
            return on;
        }
        public int Bar
        {
            get
            {
                int sign = Math.Sign(AdjTimer);
                int bar = (int)(AdjTimer / Unit);
                if (sign == -1)
                    bar--;
                return bar;
            }
        }
    }
}