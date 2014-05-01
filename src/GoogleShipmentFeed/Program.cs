using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Chronos;
using Chronos.Configuration;
using Data;
using Deerso.Logging;
using FeedLibrary;
using Models;
using ServiceStack.Logging;

namespace GoogleShipmentFeed
{
    static class Program
    {
        private static ILog _log;
        static void Main()
        {
            Init();
            _log.Debug("App started");

            try
            {
                var now = DateTime.Now;
                var lastRunTimestamp = ConfigUtils.GetAppSetting("lastRun", DateTime.Now.AddDays(-30).ToUnixTimestamp());

                var dataRepo = new Repo();

                MakeShipmentFeed(dataRepo, lastRunTimestamp.FromUnixTimestamp(), now);

                MakeCancellationFeed(dataRepo, lastRunTimestamp.FromUnixTimestamp(), now);

                UpdateLastRun(now);
            }
            catch (Exception e)
            {
               _log.Fatal("Error in Catch all", e); 
                throw;
            }
            _log.Debug("App finished");
        }

        private static void Init()
        {
            LogManager.LogFactory = new DeersoLogFactory(debugEnabled:false, notifyNames: new [] {"@KyleGobel"});
            _log = LogManager.GetLogger("");
        }

        private static void MakeCancellationFeed(Repo dataRepo, DateTime lastRunDate, DateTime startProgramTime)
        {
            _log.Debug("Making cancellation feed");
            var feedData = dataRepo.GetCancelFeed(lastRunDate, startProgramTime);

            var feed = FeedMaker.GenerateFeed(FeedType.TSV, new FeedOptions {QualifyOptions = QualifyData.None},
                feedData);

            var feedPath = ConfigUtils.GetAppSetting("cancelFeedFile");
            var appendDate = ConfigUtils.GetAppSetting("appendDate", false);

            if (appendDate)
            {
                feedPath = Path.GetFileNameWithoutExtension(feedPath) + DateTime.Now.ToString("yyyy-MM-dd") +
                    Path.GetExtension(feedPath);
            }

            File.WriteAllText(feedPath, feed);
            _log.DebugFormat("Finished making cancel feed with {0} items",feedData.Count());
        }

        static void MakeShipmentFeed(Repo repo, DateTime lastRunDate, DateTime startProgramDate)
        {
            _log.Debug("Making shipment feed");
            var feedData = repo.GetShipmentFeed(lastRunDate, startProgramDate);

            var feed = FeedMaker.GenerateFeed(FeedType.TSV, new FeedOptions {QualifyOptions = QualifyData.None},
                feedData);
            var feedPath = ConfigUtils.GetAppSetting("shipmentFeedFile");
            var appendDate = ConfigUtils.GetAppSetting("appendDate", false);

            if (appendDate)
            {
                feedPath = Path.GetFileNameWithoutExtension(feedPath) + DateTime.Now.ToString("yyyy-MM-dd") +
                    Path.GetExtension(feedPath);
            }
            File.WriteAllText(feedPath, feed);
            _log.DebugFormat("Completed shipment feed, added {0} item(s)",feedData.Count());
        }

        static void UpdateLastRun(DateTime date)
        {
            ConfigUtils.UpdateExeAppSetting("lastRun", date.ToUnixTimestamp());
        }
    }

}
