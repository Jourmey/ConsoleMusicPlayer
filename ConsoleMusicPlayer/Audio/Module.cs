using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMusicPlayer.Audio
{
    public class MusicContext : EventArgs
    {
        public string LastName { get; set; }
        public string MusicName { get; set; }
        public string NextName { get; set; }

        public TimeSpan CurrentTime { get; set; }
        public TimeSpan TotalTime { get; set; }
    }


    public class MusicWave : EventArgs
    {
        public byte[] Wave { get; set; }
    }
}
