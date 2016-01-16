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
    }
}
