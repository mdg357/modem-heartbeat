using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heartbeat.Models
{
    class UpstreamChannel: Channel
    {
        private string symbolRate;
        private double symbolRateVal;

        public string RangingServiceID { get; set; }
        public string SymbolRate
        {
            get
            {
                return symbolRate;
            }
            set
            {
                double.TryParse(value.Trim().Replace(" Msym/sec", ""), out symbolRateVal);
                symbolRate = value;
            }
        }
        public string UpstreamModulation { get; set; }
        public string RangingStatus { get; set; }

        public UpstreamChannel(DateTime timestamp) :base(timestamp)
        {
        }

        // TODO: remove UOM from output
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}, {3}, {4}, {5:0.000}, {6}, {7}",
                Timestamp.ToShortDateString(), Timestamp.ToShortTimeString(),
                ChannelID, frequencyVal, RangingServiceID, symbolRateVal, powerLevelVal,
                UpstreamModulation, RangingStatus);
        }
    }

    static class UpstreamHelper
    {
        public static List<UpstreamChannel> Initialize(DateTime timestamp)
        {
            List<UpstreamChannel> list = new List<UpstreamChannel>();
            for (int i = 0; i < Properties.Settings.Default.Channel_Count_Upstream; ++i)
                list.Add(new UpstreamChannel(timestamp));
            return list;
        }

        public static Dictionary<string, string> PropertyMap()
        {
            return new Dictionary<string, string>()
                {
                    { "ChannelID", "Channel ID" },
                    { "Frequency", "Frequency" },
                    { "RangingServiceID", "Ranging Service ID" },
                    { "SymbolRate", "Symbol Rate" },
                    { "PowerLevel", "Power Level" },
                    { "UpstreamModulation", "Upstream Modulation" },
                    { "RangingStatus", "Ranging Status" }
                };
        }

        public static Dictionary<string, int> PropertyOffset()
        {
            return new Dictionary<string, int>()
                {
                    { "ChannelID", 0 },
                    { "Frequency", 0 },
                    { "RangingServiceID", 0 },
                    { "SymbolRate", 0 },
                    { "PowerLevel", 0 },
                    { "UpstreamModulation", 0 },
                    { "RangingStatus", 0 }
                };
        }
    }
}
