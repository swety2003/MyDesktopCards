using MyDesktopCards.ViewModel;
using MyWidgets.SDK;
using MyWidgets.SDK.Common;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyDesktopCards.View
{
    /// <summary>
    /// Clock.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleClock : ControlBase
    {

        public static CardInfo info => new CardInfo(null, "简单时钟", "模仿安卓13的时钟部件样式", typeof(SimpleClock));

        public SimpleClock(Guid g):base(g)
        {
            InitializeComponent();
            HeightPix = 4;
            WidthPix = 4;
            Info = info;
            InitViewModel<SimpleClockVM>();

        }



        public override void OnDisabled()
        {
            base.OnDisabled();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            Loaded += DigitalClock_Loaded;
        }

        private void DigitalClock_Loaded(object sender, RoutedEventArgs e)
        {
            this.TryLoadCustomeStyle();
        }


        public override void ShowSetting()
        {
            throw new NotImplementedException();
        }
    }
}
