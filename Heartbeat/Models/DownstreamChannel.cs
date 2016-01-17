using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heartbeat.Models
{
    class DownstreamChannel: Channel
    {
        public string SignalToNoiseRatio { get; set; }
        public string DownstreamModulation { get; set; }

        public DownstreamChannel() :base()
        {
        }

        public override string ToString()
        {
            return String.Format("\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\"",
                ChannelID, Frequency, SignalToNoiseRatio, 
                DownstreamModulation, PowerLevel);
        }
    }
}
