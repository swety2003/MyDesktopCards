using Microsoft.Extensions.Logging;
using MyDesktopCards.View;
using MyWidgets.SDK;
using MyWidgets.SDK.Core.Card;
using MyWidgets.SDK.Core.SideBar;
using System;
using System.Collections.Generic;

namespace MyDesktopCards
{
    public class PluginInfo : IPlugin
    {

        public Version version { get; } = new Version();
        public string url { get; } = "";
        public string author { get; } = "";


        public PluginInfo()
        {
        }

        public static List<CardInfo> infos { get; } = new List<CardInfo>()
        {
            //DevTest.info
            DigitalClock.info,
            HardWareMonitor.info,
            AISchedule.info,
            SimpleClock.info,
            AudioVisualizer.info,
        };



        public string name => "×ÀÃæ¿¨Æ¬";

        public ILoggerFactory LoggerFactory { get; set; }

        public IEnumerable<object> GetAllTypeInfo()
        {
            return infos;
        }
    }
}
