using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heartbeat.Models
{
    class DownstreamChannel: Channel
    {
        private string signalToNoiseRatio;
        private int signalToNoiseRatioVal;

        public string SignalToNoiseRatio
        {
            get
            {
                return signalToNoiseRatio;
            }
            set
            {
                int.TryParse(value.Trim().Replace(" dB", ""), out signalToNoiseRatioVal);
                signalToNoiseRatio = value;
            }
        }
        public string DownstreamModulation { get; set; }

        public DownstreamChannel(DateTime timestamp) :base(timestamp)
        {
        }
        
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}, {3}, {4}, {5}, {6}",
                Timestamp.ToShortDateString(), Timestamp.ToShortTimeString(), 
                ChannelID, frequencyVal, signalToNoiseRatioVal, DownstreamModulation, 
                powerLevelVal);
        }
    }

    static class DownstreamHelper
    {
        public static List<DownstreamChannel> Initialize(DateTime timestamp)
        {
            List<DownstreamChannel> list = new List<DownstreamChannel>();
            for (int i = 0; i < Properties.Settings.Default.Channel_Count_Downstream; ++i)
                list.Add(new DownstreamChannel(timestamp));
            return list;
        }

        public static Dictionary<string, string> PropertyMap()
        {
            return new Dictionary<string, string>()
                {
                    { "ChannelID", "Channel ID" },
                    { "Frequency", "Frequency" },
                    { "SignalToNoiseRatio", "Signal to Noise Ratio" },
                    { "DownstreamModulation", "Downstream Modulation" },
                    { "PowerLevel", "Power Level" }
                };
        }

        public static Dictionary<string, int> PropertyOffset()
        {
            return new Dictionary<string, int>()
                {
                    { "ChannelID", 0 },
                    { "Frequency", 0 },
                    { "SignalToNoiseRatio", 0 },
                    { "DownstreamModulation", 0 },
                    { "PowerLevel", 1 }
                };
        }
    }
}
