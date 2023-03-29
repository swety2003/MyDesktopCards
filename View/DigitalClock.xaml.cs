using Microsoft.Extensions.Logging;
using MyDesktopCards.ViewModel;
using MyWidgets.SDK;
using MyWidgets.SDK.Common;
using MyWidgets.SDK.Core.Card;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyDesktopCards.View
{
    /// <summary>
    /// DigitalClock.xaml 的交互逻辑
    /// </summary>
    public partial class DigitalClock : ControlBase
    {

        public static CardInfo info = new CardInfo(null, "数字时钟", "简单的时钟", typeof(DigitalClock));


        public DigitalClock(Guid guid):base(guid)
        {
            InitializeComponent();

            Info = info;
            HeightPix = 4;
            WidthPix = 9;

            this.InitViewModel<DigitalClockVM>();

            logger = Logger.CreateLogger<DigitalClock>();

            Loaded += DigitalClock_Loaded;
        }



        public override void OnDisabled()
        {
            base.OnDisabled();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override void ShowSetting()
        {
            throw new NotImplementedException();
        }

        private void DigitalClock_Loaded(object sender, RoutedEventArgs e)
        {
            this.TryLoadCustomeStyle();
        }

    }
}
