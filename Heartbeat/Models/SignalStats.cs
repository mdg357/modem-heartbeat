using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heartbeat.Models
{
    class SignalStats
    {
        public DateTime Timestamp { get; set; }
        public string ChannelID { get; set; }
        public string TotalUnerroredCodewords { get; set; }
        public string TotalCorrectableCodewords { get; set; }
        public string TotalUncorrectableCodewords { get; set; }

        public SignalStats(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
        
        public override string ToString()
        {
            return String.Format("{0} {1}, {2}, {3}, {4}, {5}",
                Timestamp.ToShortDateString(), Timestamp.ToShortTimeString(),
                ChannelID, TotalUnerroredCodewords, TotalCorrectableCodewords,
                TotalUncorrectableCodewords);
        }
    }

    static class SignalStatsHelper
    {
        public static List<SignalStats> Initialize(DateTime timestamp)
        {
            List<SignalStats> list = new List<SignalStats>();
            for (int i = 0; i < Properties.Settings.Default.Count_Signal_Stats; ++i)
                list.Add(new SignalStats(timestamp));
            return list;
        }

        public static Dictionary<string, string> PropertyMap()
        {
            return new Dictionary<string, string>()
            {
                { "ChannelID", "Channel ID" },
                { "TotalUnerroredCodewords", "Total Unerrored Codewords" },
                { "TotalCorrectableCodewords", "Total Correctable Codewords" },
                { "TotalUncorrectableCodewords", "Total Uncorrectable Codewords" }
            };
        }

        public static Dictionary<string, int> PropertyOffset()
        {
            return new Dictionary<string, int>()
            {
                { "ChannelID", 0 },
                { "TotalUnerroredCodewords", 0 },
                { "TotalCorrectableCodewords", 0 },
                { "TotalUncorrectableCodewords", 0 }
            };
        }
    }
}
