using System;

namespace Heartbeat.Models
{
    class Channel
    {
        protected string frequency;
        protected int frequencyVal;
        protected string powerLevel;
        protected int powerLevelVal;

        public DateTime Timestamp { get; set; }
        public string ChannelID { get; set; }
        public string Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                int.TryParse(value.Trim().Replace(" Hz", ""), out frequencyVal);
                frequency = value;
            }
        }
        public string PowerLevel
        {
            get
            {
                return powerLevel;
            }
            set
            {
                int.TryParse(value.Trim().Replace(" dBmV", ""), out powerLevelVal);
                powerLevel = value;
            }
        }

        public Channel(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }
}
