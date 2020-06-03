using ConsoleMusicPlayer;
using ConsoleMusicPlayer.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            MusicService musicService = new MusicService();
            musicService.MusicWaveEventHandler += MusicService_MusicWaveEventHandler;
            musicService.MusicContextEventHandler += MusicService_MusicContextEventHandler;

            musicService.Play();
            var s = Console.ReadKey();

        }


        private const int MaxHeight = 10;
        private const int MaxWidth = 32;
        private static int[] _topPosition = new int[MaxWidth];
        private static bool[,] _topP = new bool[MaxWidth, MaxHeight];


        private static void MusicService_MusicWaveEventHandler(object sender, MusicWave e)
        {
            if (e == null || e.Wave == null)
            {
                return;
            }


            for (int i = 0; i < MaxWidth; i++)
            {
                var w = e.Wave[i] * MaxHeight / 256;

                for (int j = 0; j < MaxHeight; j++)
                {
                    if (j <= w)
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

            // 降落滑块
            for (int i = 0; i < MaxWidth; i++)
            {
                var j = MaxHeight - _topPosition[i];
                if (j >= 0 && j < MaxHeight)
                {
                    _topP[i, j] = true;
                }
            }


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

        }

        private static void MusicService_MusicContextEventHandler(object sender, MusicContext e)
        {
            if (e == null)
            {
                return;
            }

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("---------------------");
            Console.WriteLine($"Last |{e.LastName}");
            Console.WriteLine($"Next |{ e.NextName }");
            Console.WriteLine($"Now  |{ e.MusicName}");
            Console.WriteLine("---------------------");

        }

    }
}
