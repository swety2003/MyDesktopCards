using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Utils;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyDesktopCards.Common
{
    public class AudioSpectrum : IDisposable
    {

        private float[] ringBuffer = new float[2048];
        private Complex[] fftBuffer = new Complex[2048];

        private int startPointer = 0;

        public float[] FFTOutput = new float[1024];

        public float[] captureBuffer;

        public float DBRange = -60;

        private LowLatencyLoopbackCapture capturer;
        private BufferedWaveProvider waveProvider;
        private ISampleProvider waveToSampleProvider;
        public AudioSpectrum(MMDevice device)
        {
            capturer = new LowLatencyLoopbackCapture(device, 50);
            waveProvider = new BufferedWaveProvider(capturer.WaveFormat);
            capturer.DataAvailable += Capturer_DataAvailable;
            waveProvider.BufferDuration = TimeSpan.FromMilliseconds(50);
            waveProvider.DiscardOnBufferOverflow = true;
            waveProvider.ReadFully = false;
            captureBuffer = new float[2048];
            waveToSampleProvider = new WaveToSampleProvider(waveProvider).ToMono();
        }

        private void Capturer_DataAvailable(object? sender, WaveInEventArgs e)
        {
            waveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private Thread? readThread;
        DateTime beginDate = DateTime.Now;
        private void ReadBufferThread()
        {
            try
            {
                while (true)
                {
                    int bufferReaded = waveToSampleProvider.Read(captureBuffer, 0, captureBuffer.Length);
                    // 读入输入缓冲区
                    for (int i = 0; i < bufferReaded; i++)
                    {
                        ringBuffer[startPointer] = captureBuffer[i];
                        startPointer++;
                        startPointer = startPointer % ringBuffer.Length;
                    }

                    // 转换为复数形式
                    for (int i = 0; i < fftBuffer.Length; i++)
                    {
                        int i2 = (startPointer + i) % fftBuffer.Length;
                        // 加窗防止频谱泄露
                        fftBuffer[i].X = (float)FastFourierTransform.HammingWindow(i, 2048) * ringBuffer[i2];
                        fftBuffer[i].Y = 0;
                    }

                    //进行变换！
                    FastFourierTransform.FFT(true, 11, fftBuffer);
                    double elapsed = ElapsedDouble();
                    for (int i = 0; i < FFTOutput.Length; i++)
                    {
                        double d = (float)Decibels.LinearToDecibels(Math.Sqrt(Math.Pow(fftBuffer[i].X, 2) + Math.Pow(fftBuffer[i].Y, 2)));
                        if (d < DBRange) { d = DBRange; }
                        d = (d - DBRange) / (-DBRange);

                        FFTOutput[i] -= (float)(elapsed / 0.500f);

                        FFTOutput[i] = FFTOutput[i] > (float)d ? FFTOutput[i] : (float)d;
                    }

                    Thread.Sleep(1);
                }
            }
            catch (ThreadInterruptedException ex)
            {
                return;
            }
        }

        private double ElapsedDouble()
        {
            double d = (DateTime.Now - beginDate).TotalSeconds;
            beginDate = DateTime.Now;
            return d;
        }

        public void Start()
        {
            capturer.StartRecording();
            readThread = new Thread(ReadBufferThread);
            readThread.Start();
        }

        public void Stop()
        {
            capturer.StopRecording();
            readThread?.Interrupt();
            readThread = null;
        }

        public void Dispose()
        {
            Stop();
            capturer.Dispose();
        }


        private class LowLatencyLoopbackCapture : WasapiCapture
        {

            public override WaveFormat WaveFormat
            {
                get
                {
                    return base.WaveFormat;
                }
                set
                {
                    throw new InvalidOperationException("WaveFormat cannot be set for WASAPI Loopback Capture");
                }
            }

            public LowLatencyLoopbackCapture(MMDevice captureDevice, int desiredLancey) : base(captureDevice, true, desiredLancey)
            {

            }

            protected override AudioClientStreamFlags GetAudioClientStreamFlags()
            {
                return AudioClientStreamFlags.Loopback;
            }
        }

    }
}
