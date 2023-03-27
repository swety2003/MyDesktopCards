using Microsoft.Extensions.Logging;
using MyDesktopCards.ViewModel;
using MyWidgets.SDK;
using MyWidgets.SDK.Common;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyDesktopCards.View
{
    /// <summary>
    /// HardWareMonitor.xaml 的交互逻辑
    /// </summary>
    public partial class HardWareMonitor : ControlBase
    {
        internal static CardInfo info = new CardInfo(null, "资源监控", "硬件占用监控", typeof(HardWareMonitor));


        public HardWareMonitor(Guid guid):base (guid)
        {

            InitializeComponent();

            logger = Logger.CreateLogger<HardWareMonitor>();
            HeightPix = 1;
            WidthPix = 4;
            Info = info;
            this.InitViewModel<HardwareMonitorVM>();

        }



        public override void OnDisabled()
        {
            base.OnDisabled();
        }


        public override void OnEnabled()
        {
            base.OnEnabled();
            Loaded += HardWareMonitor_Loaded;
        }

        private void HardWareMonitor_Loaded(object sender, RoutedEventArgs e)
        {
            this.TryLoadCustomeStyle();
        }



        public override void ShowSetting()
        {
            throw new NotImplementedException();
        }
    }
}
