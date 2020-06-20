using ConsoleMusicPlayer;
using ConsoleMusicPlayer.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace ConsoleMusicPlayer
{
    class Program
    {
        private static MusicService _musicService;

        static void Main(string[] args)
        {
            _musicService = new MusicService();
            _musicService.MusicWaveEventHandler += MusicService_MusicWaveEventHandler;
            _musicService.SongContextEventHandler += MusicService_MusicContextEventHandler;

            _musicService.Init();
            _musicService.Play();

            Timer timer = new Timer(updateUIWave, null, 500, 100);


            var s = Console.ReadKey();
            while (true)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.P:
                        if (_musicService.IsPlaying())
                        {
                            _musicService.Pause();
                        }
                        else
                        {
                            _musicService.Play();
                        }
                        break;
                    case ConsoleKey.N:
                        _musicService.Next();
                        break;
                    default:
                        break;

                }
            }

        }

        private static void updateUIWave(object state)
        {

            MusicService_MusicWaveEventHandler(null, _musicService.GetMusicWave());
        }

        private const int MaxHeight = 12;
        private const int MaxWidth = 32;
        private static int[] _topPosition = new int[MaxWidth];
        private static bool[,] _topP = new bool[MaxWidth, MaxHeight];


        private static void MusicService_MusicWaveEventHandler(object sender, MusicWave e)
        {
            if (e == null || e.Wave == null)
            {
                return;
            }


            for (int i = 0; i < Math.Min(MaxWidth, e.Wave.Length); i++)
            {
                var w = e.Wave[i] * MaxHeight / 256;

                for (int j = 0; j < MaxHeight; j++)
                {
                    if (j <= (MaxHeight - w))
                    {
                        _topP[i, j] = false;
                    }
                    else
                    {
                        _topP[i, j] = true;
                    }

                }

                _topPosition[i] = Math.Max(_topPosition[i] - 1, w);

            }

            //// 降落滑块
            //for (int i = 0; i < MaxWidth; i++)
            //{
            //    var j = MaxHeight - _topPosition[i];
            //    if (j >= 0 && j < MaxHeight)
            //    {
            //        _topP[i, j] = true;
            //    }
            //}


            for (int j = 0; j < MaxHeight; j++)
            {
                List<char> buffer2 = new List<char>(MaxHeight);
                for (int i = 0; i < MaxWidth; i++)
                {
                    if (_topP[i, j] == true)
                    {
                        buffer2.Add('█');
                    }
                    else
                    {
                        buffer2.Add(' ');
                        buffer2.Add(' ');
                    }
                }
                Console.SetCursorPosition(0, j + 5);
                Console.WriteLine(buffer2.ToArray(), 0, buffer2.Count);
            }

            Console.SetCursorPosition(5, 20);
            Console.WriteLine(getSongProcess(e.IsPlaying, e.CurrentTime, e.TotalTime));

        }

        private static string getSongProcess(bool isPlaying, TimeSpan currentTime, TimeSpan totalTime)
        {
            var thu = (int)(currentTime.TotalSeconds / totalTime.TotalSeconds * 20) - 1;

            StringBuilder s = new StringBuilder("--------------------");


            if (thu >= 0 && thu < s.Length)
            {
                s[thu] = '◆';
            }


            string p = isPlaying ? "▲" : "■";

            return $"{p}  {currentTime.ToString(@"mm\:ss")} {s} {totalTime.ToString(@"mm\:ss")}";

        }

        private static void MusicService_MusicContextEventHandler(object sender, SongContext e)
        {
            if (e == null)
            {
                return;
            }

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("---------------------");
            Console.WriteLine($"Next |{ e.NextName }");
            Console.WriteLine($"Now  |{ e.MusicName}");
            Console.WriteLine("---------------------");

        }

    }
}
