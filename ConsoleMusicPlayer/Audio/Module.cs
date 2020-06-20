using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMusicPlayer.Audio
{
    public class SongContext : EventArgs
    {
        public string MusicName { get; set; }
        public string NextName { get; set; }

    }


    public class MusicWave : EventArgs
    {
        public byte[] Wave { get; set; }
        public TimeSpan CurrentTime { get; set; }
        public TimeSpan TotalTime { get; set; }

        public bool IsPlaying { get; set; }
    }

    public static class UserConfig
    {
        public static string SongDic = @".\CloudMusic";
    }


    public class EqualizerBand
    {
        public float Frequency { get; set; }
        public float Gain { get; set; }
        public float Bandwidth { get; set; }
    }
}
