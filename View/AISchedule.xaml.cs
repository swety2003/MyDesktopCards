using Microsoft.Extensions.Logging;
using MyDesktopCards.Model;
using MyDesktopCards.SettingView;
using MyDesktopCards.ViewModel;
using MyWidgets.SDK;
using MyWidgets.SDK.Common;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyDesktopCards.View
{
    /// <summary>
    /// AISchedule.xaml 的交互逻辑
    /// </summary>
    public partial class AISchedule : ControlBase
    {

        internal static CardInfo info = new CardInfo(null, "小爱课程表", "是一个课程表捏", typeof(AISchedule));

        public AISchedule(Guid guid):base(guid)
        {
            InitializeComponent();

            this.logger = Logger.CreateLogger<AISchedule>();
            HeightPix = 6;
            WidthPix = 6;
            Info = info;

            this.InitViewModel<AIScheduleVM>();
        }




        public override void OnDisabled()
        {
            base.OnDisabled();
        }

        public override void OnEnabled()
        {
            ConfigBase.Load<AIScheduleConfig>(this.GetPluginConfigFilePath());

            base.OnEnabled();

            Loaded += AISchedule_Loaded;

        }

        private void AISchedule_Loaded(object sender, RoutedEventArgs e)
        {
            this.TryLoadCustomeStyle();
        }

        public override void ShowSetting()
        {
            new AIScheduleSetting(this).Show();
        }


        public UIElement GetUIElement()
        {
            return this;
        }
    }
}
