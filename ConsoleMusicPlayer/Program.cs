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

            string file = @"C:\Users\Administrator\Desktop\庆庆 - 蝙蝠.mp3";
            var inputStream = new AudioFileReader(file);

            var aggregator = new SampleAggregator(inputStream);
            aggregator.NotificationCount = inputStream.WaveFormat.SampleRate / 100;
            aggregator.PerformFFT = true;

            AudioPlayback audioPlayback = new AudioPlayback();
            audioPlayback.OpenFile(inputStream, aggregator);
            audioPlayback.Play();


            var a = new AudioAnalyzer(aggregator);

            a.AnalyzedAudioEventHandler += A_AnalyzedAudioEventHandler;

            var s = Console.ReadKey();

        }

        private static void A_AnalyzedAudioEventHandler(object sender, AnalyzedAudioEventArgs e)
        {

            if (e?.AnalyzedAudio.Samples != null)
            {
                var max = e.AnalyzedAudio.Samples.Sum((w) => w.MaxSample);
                var min = e.AnalyzedAudio.Samples.Sum((w) => w.MinSample);

                StringBuilder a = new StringBuilder();


                for (int i = 0; i < 0 - (int)(min * 10); i++)
                {
                    a.Append("+");
                }

                for (int i = 0; i < (int)(max * 10); i++)
                {
                    a.Append("-");
                }


                Console.WriteLine(a.ToString());
            }
        }
    }
}
