﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Spectrogram
{
    class Settings
    {
        public readonly int SampleRate;

        // vertical information
        public readonly int FftSize;
        public readonly double FftLengthSec;
        public readonly double FreqNyquist;
        public readonly double HzPerPixel;
        public readonly double PxPerHz;
        public readonly int FftIndex1;
        public readonly int FftIndex2;
        public readonly double FreqMin;
        public readonly double FreqMax;
        public readonly double FreqSpan;
        public readonly int Height;

        // horizontal information
        public readonly double[] Window;
        public readonly int StepSize;
        public readonly double StepLengthSec;
        public readonly double StepOverlapFrac;
        public readonly double StepOverlapSec;

        public Settings(int sampleRate, int fftSize, int stepSize, double minFreq, double maxFreq)
        {
            if (FftSharp.Transform.IsPowerOfTwo(fftSize) == false)
                throw new ArgumentException("FFT size must be a power of 2");

            // FFT info
            SampleRate = sampleRate;
            FftSize = fftSize;
            StepSize = stepSize;
            FftLengthSec = (double)fftSize / sampleRate;

            // vertical
            minFreq = Math.Max(minFreq, 0);
            maxFreq = Math.Min(maxFreq, fftSize / 2);
            FreqNyquist = sampleRate / 2;
            HzPerPixel = (double)sampleRate / fftSize;
            PxPerHz = (double)fftSize / sampleRate;
            FftIndex1 = (int)(minFreq / HzPerPixel);
            FftIndex2 = (int)(maxFreq / HzPerPixel);
            Height = FftIndex2 - FftIndex1;
            FreqMin = FftIndex1 * HzPerPixel;
            FreqMax = FftIndex2 * HzPerPixel;
            FreqSpan = FreqMax - FreqMin;

            // horizontal
            StepLengthSec = (double)StepSize / sampleRate;
            Window = FftSharp.Window.Hanning(fftSize);
            StepOverlapSec = FftLengthSec - StepLengthSec;
            StepOverlapFrac = StepOverlapSec / FftLengthSec;
        }

        public int PixelY(double freq)
        {
            return (int)(Height - (freq - FreqMin + HzPerPixel) * PxPerHz - 1);
        }
    }
}
