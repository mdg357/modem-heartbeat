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

        static void Main(string[] args)
        {
            Console.Title = Properties.Resources.ConsoleTitle;

            string startupMessage = string.Format(Properties.Resources.Format_StartupMessage,
                DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            Console.WriteLine(startupMessage);
            _log.InfoFormat(startupMessage);

            if (Properties.Settings.Default.IsDebug == false)
            {
                Timer aTimer = new Timer();
                aTimer.Elapsed += new ElapsedEventHandler(RequestModemStatusPage);
                aTimer.Interval = Properties.Settings.Default.QueryInterval;
                aTimer.Enabled = true;

                Console.ReadKey(true);
            }
            else
            {
                RequestModemStatusPage(null, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        static void RequestModemStatusPage(object source, ElapsedEventArgs e)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string responseBody = string.Empty;

            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(Properties.Resources.URL);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = Properties.Settings.Default.Request_Timeout;

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    statusCode = ((HttpWebResponse)response).StatusCode;
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
            }

            // If we get anything other than a 200, log an error            
            if (statusCode != HttpStatusCode.OK)
            {
                WriteStatus(DateTime.Now, true, statusCode);
            }
            else
            {
                DateTime currentTime = DateTime.Now;
                string cleanedResponse = CleanResponse(responseBody);
                ParseResponse(cleanedResponse, currentTime);
                WriteStatus(currentTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBody"></param>
        static void ParseResponse(string responseBody, DateTime currentTime)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            HtmlNodeCollection downstreamNodes = null;
            HtmlNodeCollection upstreamNodes = null;
            HtmlNodeCollection signalStatsNodes = null;

            htmlDoc.LoadHtml(responseBody);

            if (htmlDoc.DocumentNode != null)
            {
                downstreamNodes = htmlDoc.DocumentNode.SelectNodes(Properties.Resources.NodeSelection_Downstream);
                upstreamNodes = htmlDoc.DocumentNode.SelectNodes(Properties.Resources.NodeSelection_Upstream);
                signalStatsNodes = htmlDoc.DocumentNode.SelectNodes(Properties.Resources.NodeSelection_SignalStats);
            }

            if (downstreamNodes != null)
            {
                List<DownstreamChannel> downstreamChannels = DownstreamHelper.Initialize(currentTime);
                ProcessNodes(downstreamNodes, DownstreamHelper.PropertyMap(), DownstreamHelper.PropertyOffset(), 
                    ref downstreamChannels, Properties.Settings.Default.Channel_Count_Downstream);
                WriteToFiles(downstreamChannels, Properties.Resources.Header_Downstream, 
                    Properties.Resources.File_Downstream);
            }

            if (upstreamNodes != null)
            {
                List<UpstreamChannel> upstreamChannels = UpstreamHelper.Initialize(currentTime);
                
                
                ProcessNodes(upstreamNodes, UpstreamHelper.PropertyMap(), UpstreamHelper.PropertyOffset(), 
                    ref upstreamChannels, Properties.Settings.Default.Channel_Count_Upstream);
                WriteToFiles(upstreamChannels, Properties.Resources.Header_Upstream, 
                    Properties.Resources.File_Upstream);
            }

            if (signalStatsNodes != null)
            {
                List<SignalStats> signalStats = SignalStatsHelper.Initialize(currentTime);
                ProcessNodes(signalStatsNodes, SignalStatsHelper.PropertyMap(), SignalStatsHelper.PropertyOffset(), 
                    ref signalStats, Properties.Settings.Default.Count_Signal_Stats);
                WriteToFiles(signalStats, Properties.Resources.Header_SignalStats, 
                    Properties.Resources.File_SignalStats);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes"></param>
        /// <param name="propertyMap"></param>
        /// <param name="propertyOffset"></param>
        /// <param name="list"></param>
        /// <param name="columnCount"></param>
        static void ProcessNodes<T>(HtmlNodeCollection nodes, Dictionary<string, string> propertyMap, 
            Dictionary<string, int> propertyOffset, ref List<T> list, int columnCount)
        {
            foreach (var node in nodes)
            {
                foreach (KeyValuePair<string, string> name in propertyMap)
                {
                    if (node.InnerText.Trim().StartsWith(name.Value))
                    {
                        int indexWithOffset = nodes.GetNodeIndex(node) + propertyOffset[name.Key];
                        List<string> columnValues = GetCellsFromOffset(nodes, indexWithOffset, columnCount);
                        ListHelper.SetPropertyFromList(columnValues, ref list, name.Key);
                        break;
                    }
                }
            }
        }        

        static List<string> GetCellsFromOffset(HtmlNodeCollection nodes, int index, int offsetMax)
        {
            List<string> items = new List<string>();

            for (int offset = 1; offset <= offsetMax; ++offset)
                items.Add(nodes[index + offset].InnerText.Trim());

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
        static void WriteStatus(DateTime time, bool isError = false, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if(isError)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(Properties.Resources.Format_StatusMessageConsole_Error,
                    time.ToShortDateString(), time.ToShortTimeString(), statusCode);
                _log.ErrorFormat(Properties.Resources.Format_StatusMessageLog_Error, statusCode);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(Properties.Resources.Format_StatusMessageConsole_Success,
                    time.ToShortDateString(), time.ToShortTimeString());
                _log.Info(Properties.Resources.Format_StatusMessageLog_Success);
            }
            Console.ResetColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="header"></param>
        /// <param name="filename"></param>
        static void WriteToFiles<T>(List<T> list, string header, string filename)
        {
            bool writeHeader = false;
            string fileNameAndPath = GetFileNameAndPath(filename);

            if (!File.Exists(fileNameAndPath))
                writeHeader = true;

            using (StreamWriter sw = new StreamWriter(fileNameAndPath, true))
            {
                if (writeHeader)
                    sw.WriteLine(header);

                foreach (var entry in list)
                    sw.WriteLine(entry.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static string GetFileNameAndPath(String filename)
        {
            return String.Format(Properties.Resources.FileNameFormat, 
                filename, DateTime.Today.ToString(Properties.Resources.Format_FileTimestamp));
        }
    }
}
