using NAudio.Dsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ConsoleMusicPlayer.Audio
{
    public class SpectrumAnalyser
    {
        public static double XScale = 32;
        private int bins = 1024; // guess a 1024 size FFT, bins is half FFT size
        private List<Point> _points = new List<Point>();


        public SpectrumAnalyser()
        {
        }



        private const int binsPerPoint = 2; // reduce the number of points we plot for a less jagged line?
        private int updateCount;


        public List<Point> GetPoint(Complex[] fftResults)
        {
            // no need to repaint too many frames per second
            if (updateCount++ % 2 == 0)
            {
                return this._points;
            }

            if (fftResults.Length / 2 != bins)
            {
                this.bins = fftResults.Length / 2;
            }

            for (int n = 0; n < fftResults.Length / 2; n += binsPerPoint)
            {
                // averaging out bins
                double yPos = 0;
                for (int b = 0; b < binsPerPoint; b++)
                {
                    yPos += getYPosLog(fftResults[n + b]);
                }
                addResult(n / binsPerPoint, yPos / binsPerPoint);
            }

            return this._points;
        }

        private double getYPosLog(Complex c)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 10 from here http://stackoverflow.com/a/10636698/7532
            double intensityDB = 10 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
            double minDB = -60;
            if (intensityDB < minDB) intensityDB = minDB;
            double percent = intensityDB / minDB;
            // we want 0dB to be at the top (i.e. yPos = 0)
            //double yPos = percent * this.ActualHeight - 20;
            return percent;
        }

        private void addResult(int index, double power)
        {
            Point p = new Point(calculateXPos(index), power);
            if (index >= _points.Count)
            {
                _points.Add(p);
            }
            else
            {
                _points[index] = p;
            }
        }

        private double calculateXPos(int bin)
        {
            if (bin == 0) return 0;
            return bin * XScale;
        }
    }
}
