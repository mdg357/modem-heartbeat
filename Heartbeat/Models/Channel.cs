using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heartbeat.Models
{
    class Channel
    {
        public string ChannelID { get; set; }
        public string Frequency { get; set; }
        public string PowerLevel { get; set; }

        public Channel()
        {

        }
    }

    static class ListHelper
    {
        public static List<DownstreamChannel> InitalizeDownstreamChannelList(int count)
        {
            List<DownstreamChannel> list = new List<DownstreamChannel>();
            for (int i = 0; i < count; ++i)
                list.Add(new DownstreamChannel());
            return list;
        }
        public static List<UpstreamChannel> InitalizeUpstreamChannelList(int count)
        {
            List<UpstreamChannel> list = new List<UpstreamChannel>();
            for (int i = 0; i < count; ++i)
                list.Add(new UpstreamChannel());
            return list;
        }
        public static List<SignalStats> InitalizeSignalStatsList(int count)
        {
            List<SignalStats> list = new List<SignalStats>();
            for (int i = 0; i < count; ++i)
                list.Add(new SignalStats());
            return list;
        }

        public static void SetPropertyFromList(List<string> items, ref List<DownstreamChannel> channels, string property)
        {
            for (int index = 0; index < channels.Count; ++index)
            {
                switch (property)
                {
                    case "Channel ID":
                        channels[index].ChannelID = items[index];
                        break;
                    case "Frequency":
                        channels[index].Frequency = items[index];
                        break;
                    case "Signal to Noise Ratio":
                        channels[index].SignalToNoiseRatio = items[index];
                        break;
                    case "Downstream Modulation":
                        channels[index].DownstreamModulation = items[index];
                        break;
                    case "Power Level":
                        channels[index].PowerLevel = items[index];
                        break;
                }
            }
        }

        public static void SetPropertyFromList(List<string> items, ref List<UpstreamChannel> channels, string property)
        {
            for (int index = 0; index < channels.Count; ++index)
            {
                switch (property)
                {
                    case "Channel ID":
                        channels[index].ChannelID = items[index];
                        break;
                    case "Frequency":
                        channels[index].Frequency = items[index];
                        break;
                    case "Ranging Service ID":
                        channels[index].RangingServiceID = items[index];
                        break;
                    case "Symbol Rate":
                        channels[index].SymbolRate = items[index];
                        break;
                    case "Power Level":
                        channels[index].PowerLevel = items[index];
                        break;
                    case "Upstream Modulation":
                        channels[index].UpstreamModulation = items[index];
                        break;
                    case "Ranging Status":
                        channels[index].RangingStatus = items[index];
                        break;
                }
            }
        }

        public static void SetPropertyFromList(List<string> items, ref List<SignalStats> channels, string property)
        {
            for (int index = 0; index < channels.Count; ++index)
            {
                switch (property)
                {
                    case "Channel ID":
                        channels[index].ChannelID = items[index];
                        break;
                    case "Total Unerrored Codewords":
                        channels[index].TotalUnerroredCodewords = items[index];
                        break;
                    case "Total Correctable Codewords":
                        channels[index].TotalCorrectableCodewords = items[index];
                        break;
                    case "Total Uncorrectable Codewords":
                        channels[index].TotalUncorrectableCodewords = items[index];
                        break;
                }
            }
        }
    }
}
