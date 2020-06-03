using ConsoleMusicPlayer.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMusicPlayer
{
    public class MusicService
    {
        private IWavePlayer _playbackDevice;
        private AudioReader _inputStream;

        public event EventHandler<MusicContext> MusicContextEventHandler;
        public event EventHandler<MusicWave> MusicWaveEventHandler;


        public MusicService()
        {
            string file = @"C:\Users\Administrator\Desktop\庆庆 - 蝙蝠.mp3";
            this._playbackDevice = new WaveOut { DesiredLatency = 200 };

            this._inputStream = new AudioReader(file);
            this._inputStream.MusicWaveEventHandler += (s, m) =>
            {
                if (this.MusicWaveEventHandler != null)
                {
                    MusicWaveEventHandler.Invoke(s, m);
                }

            };
            this._playbackDevice.Init(this._inputStream);

        }


        public void Play()
        {
            if (this._playbackDevice != null && this._inputStream != null && this._playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                this._playbackDevice.Play();
            }

            if (MusicContextEventHandler != null)
            {
                MusicContextEventHandler.Invoke(this, new MusicContext
                {
                    LastName = this._inputStream.AudioFileReader.FileName,
                    MusicName = this._inputStream.AudioFileReader.FileName,
                    NextName = this._inputStream.AudioFileReader.FileName,
                    TotalTime = this._inputStream.AudioFileReader.TotalTime,
                    CurrentTime = this._inputStream.AudioFileReader.CurrentTime
                });
            }
        }

        public void Pause()
        {
            if (this._playbackDevice != null)
            {
                this._playbackDevice.Pause();
            }
        }

    }


   



}
