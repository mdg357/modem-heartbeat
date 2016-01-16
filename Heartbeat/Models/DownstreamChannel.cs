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
    }
}
