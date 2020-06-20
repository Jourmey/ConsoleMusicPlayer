using AudioRecorder.Model;
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
        public const int SPECTRUM_BIN_SIZE = 32;


        private IWavePlayer _playbackDevice;
        private SongManager _songManager;
        private MediaFoundationReader _mediaFoundationReader;
        private SampleAggregator _sampleAggregator;
        //private SpectrumAnalyzer _spectrumAnalyser = new SpectrumAnalyzer();
        private SpectrumAnalyser _analyser = new SpectrumAnalyser();


        private MusicWave _wave = new MusicWave();



        public event EventHandler<SongContext> SongContextEventHandler;
        public event EventHandler<MusicWave> MusicWaveEventHandler;



        public MusicService()
        {
            this._songManager = new SongManager();
            this._playbackDevice = new WaveOut { DesiredLatency = 200 };
            this._wave.Wave = new byte[SPECTRUM_BIN_SIZE];
        }

        internal void Init()
        {
            this._mediaFoundationReader = new MediaFoundationReader(this._songManager.GetNextSong());
            this._sampleAggregator = new SampleAggregator(this._mediaFoundationReader.ToSampleProvider())
            {
                NotificationCount = this._mediaFoundationReader.WaveFormat.SampleRate / 1024,
                PerformFFT = true,
            };


            this._sampleAggregator.PerformFFT = true;

            this._sampleAggregator.FftCalculated += _sampleAggregator_FftCalculated;
            this._sampleAggregator.MaximumCalculated += _sampleAggregator_MaximumCalculated;
            //this._inputStream = new AudioReader();

            this._playbackDevice.PlaybackStopped += _playbackDevice_PlaybackStopped;
            this._playbackDevice.Init(this._sampleAggregator);
        }

        public MusicWave GetMusicWave()
        {
            return this._wave;
        }


        private void _sampleAggregator_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            //_spectrumAnalyser.AddAmplitude(e.MaxSample, e.MinSample);
        }

        private void _sampleAggregator_FftCalculated(object sender, FftEventArgs e)
        {
            //    _spectrumAnalyser.CalculateFFT(e.Result);

            //    for (int i = 0; i < _spectrumAnalyser.SpecturmValue.Length; i++)
            //    {
            //        this._wave.Wave[i] = (byte)(_spectrumAnalyser.SpecturmValue[i] / _spectrumAnalyser.MaxSpectrumValue * 256);
            //    }
            var point = this._analyser.GetPoint(e.Result);

            int step = point.Count / SPECTRUM_BIN_SIZE;
            for (int i = 0; i < SPECTRUM_BIN_SIZE; i++)
            {
                this._wave.Wave[i] = 0;
            }

            for (int i = 0; i < point.Count; i++)
            {
                this._wave.Wave[i / step] += (byte)(point[i].Y / step * 256);
            }

            this._wave.CurrentTime = this._mediaFoundationReader.CurrentTime;
            this._wave.TotalTime = this._mediaFoundationReader.TotalTime;

            //for (int i = 0; i < point.Count; i++)
            //{
            //    this._wave.Wave[i] = (byte)(point[i].Y * 256);
            //}
        }

        private void _inputStream_MusicWaveEventHandler(object sender, MusicWave e)
        {
            if (this.MusicWaveEventHandler != null)
            {
                e.IsPlaying = IsPlaying();
                MusicWaveEventHandler.Invoke(this, e);
            }
        }

        private void _playbackDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            changeSong();
        }

        public void Play()
        {
            if (this._playbackDevice != null && this._sampleAggregator != null && this._playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                this._playbackDevice.Play();
            }

            if (SongContextEventHandler != null)
            {
                SongContext songContext = this._songManager.GetSongContext();
                SongContextEventHandler.Invoke(this, songContext);
            }
        }

        public void Pause()
        {
            if (this._playbackDevice != null)
            {
                this._playbackDevice.Pause();
            }
        }

        public void Next()
        {
            changeSong();
        }


        public bool IsPlaying()
        {
            if (this._playbackDevice == null)
            {
                return false;
            }

            return this._playbackDevice.PlaybackState == PlaybackState.Playing;
        }


        private void changeSong()
        {

            this._mediaFoundationReader = new MediaFoundationReader(this._songManager.GetNextSong());
            this._sampleAggregator.SetSource(this._mediaFoundationReader.ToSampleProvider());

            if (SongContextEventHandler != null)
            {
                SongContext songContext = this._songManager.GetSongContext();
                SongContextEventHandler.Invoke(this, songContext);
            }
        }


    }






}
