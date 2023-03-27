using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MyDesktopCards.Common;
using MyDesktopCards.View;
using MyDesktopCards.ViewModel;
using MyWidgets.SDK;
using MyWidgets.SDK.Common;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace MyDesktopCards.ViewModel
{
    internal partial class AudioVisualizerVM : ViewModelBase
    {

        public AudioVisualizerVM(ControlBase control) : base(control)
        {
            this.OnActiveChanged += AudioVisualizerVM_OnActiveChanged;
        }

        DispatcherTimer timer1 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(40) };

        [ObservableProperty]
        GeometryGroup circleData;

        [ObservableProperty]
        GeometryGroup stripData;
        private void AudioVisualizerVM_OnActiveChanged(object? sender, bool e)
        {
            if (e)
            {

                MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
                var device = enumerator.GetDefaultAudioEndpoint(NAudio.CoreAudioApi.DataFlow.Render, NAudio.CoreAudioApi.Role.Multimedia);
                audioSpectrum = new AudioSpectrum(device);
                audioSpectrum.Start();

                timer1.Tick += Timer1_Tick;
                timer1.Start();
            }
            else
            {

                this.audioSpectrum.Dispose();

                timer1.Stop();
            }
        }


        private AudioSpectrum audioSpectrum;

        private double rotation = 0d;

        private void Timer1_Tick(object? sender, EventArgs e)
        {
            rotation += .1;

            double bassScale = 1;


            var spectrumData = audioSpectrum.FFTOutput.Take(128).ToArray();
            var drawingPanel = this.GetView<AudioVisualizer>();


            DrawCircleGradientStrips(
                spectrumData,
                spectrumData.Length, drawingPanel.ActualWidth / 2, drawingPanel.ActualHeight / 2,
                Math.Min(drawingPanel.ActualWidth, drawingPanel.ActualHeight) / 5 + bassScale,
                1,
                rotation,
                drawingPanel.ActualWidth / 10);

            DrawGradientStrips(
                spectrumData,
                spectrumData.Length,
                drawingPanel.ActualWidth,
                0,
                drawingPanel.ActualHeight,
                2,
                -drawingPanel.ActualHeight/3);

        }

        private void DrawCircleGradientStrips(float[] spectrumData, int stripCount, double xOffset, double yOffset, double radius, double spacing, double rotation, double scale)
        {
            double rotationAngle = Math.PI / 180 * rotation;
            double blockWidth = Math.PI * 2 / stripCount;           // angle
            double stripWidth = blockWidth - MathF.PI / 180 * spacing;                // angle
            Point[] points = new Point[stripCount];

            for (int i = 0; i < stripCount; i++)
            {
                double x = blockWidth * i + rotationAngle;      // angle
                double y = spectrumData[i * spectrumData.Length / stripCount] * scale;   // height


                if (y == 0)
                {
                    y = 1;
                }

                points[i] = new Point((float)x, (float)y);
            }

            double maxHeight = points.Max(v => v.Y);
            double outerRadius = radius + maxHeight;

            Point[] endPoints = new Point[4];
            GeometryGroup geo = new GeometryGroup();

            for (int i = 0; i < stripCount; i++)
            {
                Point p = points[i];
                double sinStart = Math.Sin(p.X);
                double sinEnd = Math.Sin(p.X + stripWidth);
                double cosStart = Math.Cos(p.X);
                double cosEnd = Math.Cos(p.X + stripWidth);

                Point
                    p1 = new Point((float)(cosStart * radius + xOffset), (float)(sinStart * radius + yOffset)),
                    p2 = new Point((float)(cosEnd * radius + xOffset), (float)(sinEnd * radius + yOffset)),
                    p3 = new Point((float)(cosEnd * (radius + p.Y) + xOffset), (float)(sinEnd * (radius + p.Y) + yOffset)),
                    p4 = new Point((float)(cosStart * (radius + p.Y) + xOffset), (float)(sinStart * (radius + p.Y) + yOffset));

                endPoints[0] = p1;
                endPoints[1] = p2;
                endPoints[2] = p3;
                endPoints[3] = p4;


                PathFigure fig = new PathFigure();

                fig.StartPoint = endPoints[0];
                fig.Segments.Add(new PolyLineSegment(endPoints, false));
                //fig.IsClosed = true;

                geo.Children.Add(new PathGeometry() { Figures = { fig } });

            }

            //g.Fill = Brushes.Green;


            CircleData = geo;

            //g.Fill = circle_color.Background;
        }

        private void DrawGradientStrips(float[] spectrumData, int stripCount, double drawingWidth, double xOffset, double yOffset, double spacing, double scale)
        {
            float stripWidth = (float)((drawingWidth - spacing * stripCount) / stripCount);
            Point[] points = new Point[stripCount];

            for (int i = 0; i < stripCount; i++)
            {
                double x = stripWidth * i + spacing * i + xOffset;
                double y = spectrumData[i * spectrumData.Length / stripCount] * scale;   // height
                points[i] = new Point((float)x, (float)y);
            }

            float upP = (float)points.Min(v => v.Y < 0 ? yOffset + v.Y : yOffset);
            float downP = (float)points.Max(v => v.Y < 0 ? yOffset : yOffset + v.Y);

            if (downP < yOffset)
                downP = (float)yOffset;

            if (Math.Abs(upP - downP) < 1)
                return;


            GeometryGroup geo = new GeometryGroup();

            Point[] endPoints = new Point[4];
            for (int i = 0; i < stripCount; i++)
            {
                Point p = points[i];
                double y = yOffset;
                double height = p.Y;

                if (height < 0)
                {
                    y += height;
                    height = -height;
                }


                endPoints[0] = new Point(p.X + xOffset, p.Y + yOffset);
                endPoints[1] = new Point(p.X + xOffset, p.Y + height + yOffset);
                endPoints[2] = new Point(p.X + stripWidth + xOffset, p.Y + height + yOffset);
                endPoints[3] = new Point(p.X + stripWidth + xOffset, p.Y + yOffset);

                PathFigure fig = new PathFigure();

                fig.StartPoint = endPoints[0];
                fig.Segments.Add(new PolyLineSegment(endPoints, false));
                //fig.IsClosed = true;

                geo.Children.Add(new PathGeometry() { Figures = { fig } });

            }

            StripData = geo;

            //g.Fill = (Brush)FindResource("TertiaryBrush");
            //g.Fill = strips_color.Background;

        }



    }
}
