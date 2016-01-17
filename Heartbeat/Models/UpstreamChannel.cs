using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heartbeat.Models
{
    class UpstreamChannel: Channel
    {
        public string RangingServiceID { get; set; }
        public string SymbolRate { get; set; }
        public string UpstreamModulation { get; set; }
        public string RangingStatus { get; set; }

        public UpstreamChannel() :base()
        {

        }

        public override string ToString()
        {
            return String.Format("\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\"",
                ChannelID, Frequency, RangingServiceID, SymbolRate, PowerLevel,
                UpstreamModulation, RangingStatus);
        }
    }
}
