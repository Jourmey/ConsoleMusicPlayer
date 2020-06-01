using System;
using NAudio.Wave;


namespace ConsoleMusicPlayer.Audio
{

    public class AudioPlayback : IDisposable
    {
        private IWavePlayer playbackDevice;
        private WaveStream fileStream;


        public AudioPlayback(/*IApplicationShell appShell*/)
        {
            this.playbackDevice = new WaveOut { DesiredLatency = 200 };
        }

        private void CloseFile()
        {
            if (fileStream != null)
            {
                fileStream.Dispose();
                fileStream = null;
            }
        }

        public void OpenFile(AudioFileReader fileReader, SampleAggregator aggregator)
        {
            try
            {
                this.fileStream = fileReader;
                playbackDevice.Init(aggregator);

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Problem opening file");
                CloseFile();
            }
        }


        public void Play()
        {
            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                playbackDevice.Play();
            }
        }

        public void Pause()
        {
            if (playbackDevice != null)
            {
                playbackDevice.Pause();
            }
        }

        public void Stop()
        {
            if (playbackDevice != null)
            {
                playbackDevice.Stop();
            }
            if (fileStream != null)
            {
                fileStream.Position = 0;
            }
        }

        public void Dispose()
        {
            Stop();
            CloseFile();
            if (playbackDevice != null)
            {
                playbackDevice.Dispose();
                playbackDevice = null;
            }
        }
    }
}
