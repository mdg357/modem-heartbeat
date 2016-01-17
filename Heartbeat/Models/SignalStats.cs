using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heartbeat.Models
{
    class SignalStats
    {
        public string ChannelID { get; set; }
        public string TotalUnerroredCodewords { get; set; }
        public string TotalCorrectableCodewords { get; set; }
        public string TotalUncorrectableCodewords { get; set; }

        public SignalStats()
        {

        }

        public override string ToString()
        {
            return String.Format("\"{0}\", \"{1}\", \"{2}\", \"{3}\"",
                ChannelID, TotalUnerroredCodewords, TotalCorrectableCodewords,
                TotalUncorrectableCodewords);
        }
    }
}
