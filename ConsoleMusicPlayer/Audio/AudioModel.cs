using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMusicPlayer.Audio
{
    public struct ComplexValue
    {
        public readonly float X;
        public readonly float Y;

        public ComplexValue(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public struct AudioSample
    {
        public readonly float MaxSample;
        public readonly float MinSample;

        public AudioSample(float maxSample, float minSample)
        {
            this.MaxSample = maxSample;
            this.MinSample = minSample;
        }
    }

    public struct AnalyzedAudio
    {
        public readonly ComplexValue[] FFT;
        public readonly ComplexValue[] SmoothFFT;

        public readonly AudioSample[] Samples;

        public AnalyzedAudio(ComplexValue[] fft, ComplexValue[] smoothFft, AudioSample[] samples)
        {
            this.FFT = fft;
            this.SmoothFFT = smoothFft;
            this.Samples = samples;
        }
    }

}
