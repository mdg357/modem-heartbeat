using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Timers;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Heartbeat.Models;

namespace Heartbeat
{
    class Program
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(Program));
        public static int DOWNSTREAM_CHANNEL_COUNT = 8;
        public static int UPSTREAM_CHANNEL_COUNT = 4;
        public static int SIGNAL_STATS_COUNT = 8;

        static void Main(string[] args)
        {
            int requestInterval = 60 * 1000; // 1 minute
            string startupMessage = string.Format("Application started at {0} {1}",
                DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());

            Console.WriteLine(startupMessage);
            _log.InfoFormat(startupMessage);

            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(RequestModemStatusPage);
            aTimer.Interval = requestInterval;
            aTimer.Enabled = true;

            Console.ReadKey(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        static void RequestModemStatusPage(object source, ElapsedEventArgs e)
        {
            string pageUrl = "http://192.168.100.1/cmSignalData.htm"; // Home
            int requestTimeout = 30 * 1000; // 30 seconds
            string statusMessageFormat = "[{0} {1}] - {2}";
            string responseBody = string.Empty;
            string statusDescription = string.Empty;
            HttpStatusCode statusCode = HttpStatusCode.OK;

            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(pageUrl);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = requestTimeout;

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    statusCode = ((HttpWebResponse)response).StatusCode;
                    statusDescription = ((HttpWebResponse)response).StatusDescription;
                    Stream dataStream = response.GetResponseStream();

                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            catch(Exception)
            {
                statusCode = HttpStatusCode.NotFound;
                statusDescription = "Not Found";
            }

            // If we get anything other than a 200, log an error
            if (statusCode != HttpStatusCode.OK)
            {
                string status = string.Format(statusMessageFormat,
                    DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(),
                    String.Format("Unable to reach page '{0}'. Status: '{1} - {2}'",
                        pageUrl, statusCode, statusDescription));
                WriteStatus(status, true);
            }
            else
            {
                string status = string.Format(statusMessageFormat,
                    DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(),
                    "Success");
                WriteStatus(status, false);
            }

            string cleanedResponse = CleanResponse(responseBody);
            ParseResponse(cleanedResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBody"></param>
        static void ParseResponse(string responseBody)
        {
            List<DownstreamChannel> downstreamChannels = ListHelper.InitalizeDownstreamChannelList(DOWNSTREAM_CHANNEL_COUNT);
            List<UpstreamChannel> upstreamChannels = ListHelper.InitalizeUpstreamChannelList(UPSTREAM_CHANNEL_COUNT);
            List<SignalStats> signalStats = ListHelper.InitalizeSignalStatsList(SIGNAL_STATS_COUNT);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);
            HtmlNodeCollection downstreamNodes = null;
            HtmlNodeCollection upstreamNodes = null;
            HtmlNodeCollection signalStatsNodes = null;

            if (htmlDoc.DocumentNode != null)
            {
                downstreamNodes = htmlDoc.DocumentNode.SelectNodes("/html[1]/body[1]/center[1]/table[1] //td");
                upstreamNodes = htmlDoc.DocumentNode.SelectNodes("/html[1]/body[1]/center[2]/table[1] //td");
                signalStatsNodes = htmlDoc.DocumentNode.SelectNodes("/html[1]/body[1]/center[3]/table[1] //td");
            }

            if(downstreamNodes != null)
                ProcessDownstreamNodes(downstreamNodes, ref downstreamChannels);

            if (upstreamNodes != null)
                ProcessUpstreamNodes(upstreamNodes, ref upstreamChannels);

            if (signalStatsNodes != null)
                ProcessSignalStatsNodes(signalStatsNodes, ref signalStats);

            WriteToFiles(downstreamChannels, upstreamChannels, signalStats);
        }

        static void ProcessDownstreamNodes(HtmlNodeCollection nodes, ref List<DownstreamChannel> channels)
        {
            List<string> propertyNames = new List<string>()
            {
                "Channel ID", "Frequency", "Signal to Noise Ratio",
                "Downstream Modulation", "Power Level"
            };

            foreach (var node in nodes)
            {
                foreach (var name in propertyNames)
                {
                    if (node.InnerText.Contains(name))
                    {
                        List<string> columnValues = GetCellsFromOffset(nodes,
                            nodes.GetNodeIndex(node), DOWNSTREAM_CHANNEL_COUNT);
                        ListHelper.SetPropertyFromList(columnValues, ref channels, name);
                        break;
                    }

                }
            }
        }

        static void ProcessUpstreamNodes(HtmlNodeCollection nodes, ref List<UpstreamChannel> channels)
        {
            List<string> propertyNames = new List<string>()
            {
                "Channel ID", "Frequency", "Ranging Service ID", "Symbol Rate",
                "Power Level", "Upstream Modulation", "Ranging Status"
            };

            foreach (var node in nodes)
            {
                foreach (var name in propertyNames)
                {
                    if (node.InnerText.Contains(name))
                    {
                        List<string> columnValues = GetCellsFromOffset(nodes,
                            nodes.GetNodeIndex(node), UPSTREAM_CHANNEL_COUNT);
                        ListHelper.SetPropertyFromList(columnValues, ref channels, name);
                        break;
                    }

                }
            }
        }

        static void ProcessSignalStatsNodes(HtmlNodeCollection nodes, ref List<SignalStats> channels)
        {
            List<string> propertyNames = new List<string>()
            {
                "Channel ID", "Total Unerrored Codewords",
                "Total Correctable Codewords", "Total Uncorrectable Codewords"
            };

            foreach (var node in nodes)
            {
                foreach (var name in propertyNames)
                {
                    if (node.InnerText.Contains(name))
                    {
                        List<string> columnValues = GetCellsFromOffset(nodes,
                            nodes.GetNodeIndex(node), SIGNAL_STATS_COUNT);
                        ListHelper.SetPropertyFromList(columnValues, ref channels, name);
                        break;
                    }

                }
            }
        }

        static List<string> GetCellsFromOffset(HtmlNodeCollection nodes, int index, int offsetMax)
        {
            List<string> items = new List<string>();

            for (int offset = 1; offset <= offsetMax; ++offset)
                items.Add(nodes[index + offset].InnerText);

            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBody"></param>
        static string CleanResponse(string responseBody)
        {
            // Strip out newlines/tabs and multiple spaces
            string cleaned = Regex.Replace(responseBody, @"\t|\n|\r|&nbsp;", "");
            cleaned = Regex.Replace(cleaned, @"[ ]{2,}", " ");

            return cleaned;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="isError"></param>
        static void WriteStatus(String status, bool isError)
        {
            if(isError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(status);
                _log.Error(status);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(status);
                _log.Info(status);
            }
            Console.ResetColor();
        }

        static void WriteToFiles(List<DownstreamChannel> down, List<UpstreamChannel> up, List<SignalStats> signalStats)
        {
            string downFile = @"C:\Users\User\Downloads\Downstream.csv";
            string upFile = @"C:\Users\User\Downloads\Upstream.csv";
            string signalStatsFile = @"C:\Users\User\Downloads\SignalStats.csv";
            bool writeDownHeader = false;
            bool writeUpHeader = false;
            bool writeSignalStatsHeader = false;

            if (!File.Exists(downFile))
                writeDownHeader = true;

            if (!File.Exists(upFile))
                writeUpHeader = true;

            if (!File.Exists(signalStatsFile))
                writeSignalStatsHeader = true;

            using (StreamWriter sw = new StreamWriter(downFile, true))
            {
                if (writeDownHeader)
                    sw.WriteLine("Channel ID,Frequency,Signal to Noise Ratio,Downstream Modulation,Power Level");

                foreach (var entry in down)
                    sw.WriteLine(entry.ToString());
            }

            using (StreamWriter sw = new StreamWriter(upFile, true))
            {
                if (writeUpHeader)
                    sw.WriteLine("Channel ID,Frequency,Ranging Service ID,Symbol Rate,Power Level,Upstream Modulation,Ranging Status");

                foreach (var entry in up)
                    sw.WriteLine(entry.ToString());
            }

            using (StreamWriter sw = new StreamWriter(signalStatsFile, true))
            {
                if (writeSignalStatsHeader)
                    sw.WriteLine("Channel ID,Total Unerrored Codewords,Total Correctable Codewords,Total Uncorrectable Codewords");

                foreach (var entry in signalStats)
                    sw.WriteLine(entry.ToString());
            }
        }
    }
}
