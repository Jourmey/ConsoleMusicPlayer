using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMusicPlayer.Audio
{
    //public class AudioReader : ISampleProvider
    //{

    //    public event EventHandler<MusicWave> MusicWaveEventHandler;
    //    private readonly BiQuadFilter[,] filters;

    //    private AudioFileReader _source;
    //    public AudioFileReader AudioFileReader => this._source;
    //    private readonly EqualizerBand[] bands;


    //    public AudioReader()
    //    {

    //bands = new EqualizerBand[]
    //            {
    //                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 100, Gain = 0},
    //                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 200, Gain = 0},
    //                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 400, Gain = 0},
    //                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 800, Gain = 0},
    //                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 1200, Gain = 0},
    //                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 2400, Gain = 0},
    //                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 4800, Gain = 0},
    //                    new EqualizerBand {Bandwidth = 0.8f, Frequency = 9600, Gain = 0},
    //            };
    //        //channels = _source.WaveFormat.Channels;
    //        int channels = 2;

    //        filters = new BiQuadFilter[channels, bands.Length];

    //    }

    //    public void ChangeSong(string file)
    //    {
    //        _source = new AudioFileReader(file);


    //    }


    //    public WaveFormat WaveFormat => _source.WaveFormat;

    //    public int Read(float[] buffer, int offset, int count)
    //    {
    //        int number = this._source.Read(buffer, offset, count);


    //        if (MusicWaveEventHandler != null)
    //        {
    //            MusicWave musicWave = getMusicWave();
    //            musicWave.TotalTime = _source.TotalTime;
    //            musicWave.CurrentTime = _source.CurrentTime;

    //            MusicWaveEventHandler.Invoke(this, musicWave);
    //        }

    //        return number;
    //    }

    //    private MusicWave getMusicWave()
    //    {
    //        Random rd = new Random();
    //        var data = new byte[0x20];
    //        rd.NextBytes(data);

    //        return new MusicWave() { Wave = data };
    //    }
    //}




    public class AudioReader : ISampleProvider
    {

        public event EventHandler<MusicWave> MusicWaveEventHandler;


        private ISampleProvider sourceProvider;
        private EqualizerBand[] bands;
        private BiQuadFilter[,] filters;
        private int channels;
        private int bandCount;
        private bool updated;

        public AudioReader()
        {


        }

        public void ChangeSong(string file)
        {
            sourceProvider = new AudioFileReader(file);
            var bands = new EqualizerBand[]
              {
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 100, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 200, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 400, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 800, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 1200, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 2400, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 4800, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 9600, Gain = 0},
              };

            this.bands = bands;
            channels = sourceProvider.WaveFormat.Channels;
            bandCount = bands.Length;
            filters = new BiQuadFilter[channels, bands.Length];
            CreateFilters();

        }

        private void CreateFilters()
        {
            for (int bandIndex = 0; bandIndex < bandCount; bandIndex++)
            {
                var band = bands[bandIndex];
                for (int n = 0; n < channels; n++)
                {
                    if (filters[n, bandIndex] == null)
                        filters[n, bandIndex] = BiQuadFilter.PeakingEQ(sourceProvider.WaveFormat.SampleRate, band.Frequency, band.Bandwidth, band.Gain);
                    else
                        filters[n, bandIndex].SetPeakingEq(sourceProvider.WaveFormat.SampleRate, band.Frequency, band.Bandwidth, band.Gain);
                }
            }
        }

        public WaveFormat WaveFormat { get { return sourceProvider.WaveFormat; } }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = sourceProvider.Read(buffer, offset, count);

            CreateFilters();

            for (int n = 0; n < samplesRead; n++)
            {
                int ch = n % channels;

                for (int band = 0; band < bandCount; band++)
                {
                    buffer[offset + n] = filters[ch, band].Transform(buffer[offset + n]);
                }
            }
            return samplesRead;
        }
    }
}