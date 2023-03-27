using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MyDesktopCards.Common;
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

namespace MyDesktopCards.View
{
    /// <summary>
    /// AudioVisualizer.xaml 的交互逻辑
    /// </summary>
    public partial class AudioVisualizer : ControlBase
    {
        public static CardInfo info = new CardInfo(null, "AudioVisualizer", "",typeof(AudioVisualizer));

        public AudioVisualizer(Guid guid):base(guid)
        {
            InitializeComponent();

            this.logger = Logger.CreateLogger<AISchedule>();
            HeightPix = (int)Math.Ceiling(SystemParameters.WorkArea.Height/48);
            WidthPix = (int)Math.Ceiling(SystemParameters.WorkArea.Width / 48);

            this.InitViewModel<AudioVisualizerVM>();
            Info = info;

            Loaded += DigitalClock_Loaded;


        }

        private void DigitalClock_Loaded(object sender, RoutedEventArgs e)
        {
            this.TryLoadCustomeStyle();
        }


        public override void OnEnabled()
        {
            base.OnEnabled();


        }


        public override void OnDisabled()
        {
            base.OnDisabled();


        }

        public override void ShowSetting()
        {

        }
    }
}
