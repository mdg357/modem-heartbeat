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
            int requestInterval = 10000; // 60 * 1000; // 1 minute
            string startupMessage = string.Format("Application started at {0} {1}",
                DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());

            Console.WriteLine(startupMessage);
            _log.InfoFormat(startupMessage);

            /*Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(RequestModemStatusPage);
            aTimer.Interval = requestInterval;
            aTimer.Enabled = true;

            Console.ReadKey(true);*/

            RequestModemStatusPage(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        static void RequestModemStatusPage(object source, ElapsedEventArgs e)
        {
            string pageUrl = "http://192.168.100.1/RgSignal.asp"; // Starbucks
            //string pageUrl = "http://192.168.100.1/cmSignal.htm"; // Home
            int requestTimeout = 30 * 1000; // 30 seconds
            string statusMessageFormat = "[{0} {1}] - {2}";
            string responseBody = string.Empty;
            string statusDescription = string.Empty;
            HttpStatusCode statusCode = HttpStatusCode.OK;

            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(pageUrl);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = requestTimeout;

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
            List<DownstreamChannel> downstreamChannels = new List<DownstreamChannel>();
            List<UpstreamChannel> upstreamChannels = new List<UpstreamChannel>();
            List<SignalStats> signalStats = new List<SignalStats>();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);
            HtmlNodeCollection downstreamNodes = null;
            HtmlNodeCollection upstreamNodes = null;
            HtmlNodeCollection signalStatsNodes = null;

            if (htmlDoc.DocumentNode != null)
            {
                downstreamNodes = htmlDoc.DocumentNode.SelectNodes("/html[1]/body[1]/table[1]/tr[3]/td[2]/table[1] //td");
                upstreamNodes = htmlDoc.DocumentNode.SelectNodes("/html[1]/body[1]/table[1]/tr[3]/td[2]/table[2] //td");
                signalStatsNodes = htmlDoc.DocumentNode.SelectNodes("/html[1]/body[1]/table[1]/tr[3]/td[2]/table[3] //td");
            }

            if(downstreamNodes != null)
            {
                foreach(var node in downstreamNodes)
                {
                    if(node.InnerText.Contains("Channel ID"))
                    {

                    }
                    else if(node.InnerText.Contains("Frequency"))
                    {

                    }
                    else if (node.InnerText.Contains("Signal to Noise Ratio"))
                    {

                    }
                    else if (node.InnerText.Contains("Downstream Modulation"))
                    {

                    }
                    else if (node.InnerText.Contains("Power Level"))
                    {

                    }
                }
            }

            if (upstreamNodes != null)
            {
            }

            if (signalStatsNodes != null)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBody"></param>
        static string CleanResponse(string responseBody)
        {
            // Strip out newlines/tabs and multiple spaces
            string cleaned = Regex.Replace(responseBody, @"\t|\n|\r", "");
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
    }
}
