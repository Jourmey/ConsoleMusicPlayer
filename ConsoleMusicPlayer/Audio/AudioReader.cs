using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMusicPlayer.Audio
{
    public class AudioReader : ISampleProvider
    {

        public event EventHandler<MusicWave> MusicWaveEventHandler;

        private AudioFileReader _source;
        public AudioFileReader AudioFileReader => this._source;


        public AudioReader(string file)
        {
            _source = new AudioFileReader(file);
        }

        public WaveFormat WaveFormat => _source.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int number = this._source.Read(buffer, offset, count);


            if (MusicWaveEventHandler != null)
            {
                MusicWave musicWave = getMusicWave();
                MusicWaveEventHandler.Invoke(this, musicWave);
            }

            return number;
        }

        private MusicWave getMusicWave()
        {
            Random rd = new Random();
            var data = new byte[0x20];
            rd.NextBytes(data);

            return new MusicWave() { Wave = data };
        }
    }

}
