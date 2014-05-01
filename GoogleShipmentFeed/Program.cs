using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Chronos;
using Chronos.Configuration;
using Data;
using FeedLibrary;
using Models;

namespace GoogleShipmentFeed
{
    static class Program
    {
        static void Main()
        {
            var now = DateTime.Now;
            var lastRunTimestamp = ConfigUtils.GetAppSetting("lastRun", DateTime.Now.AddDays(-30).ToUnixTimestamp());

            var dataRepo = new Repo();

            MakeShipmentFeed(dataRepo, lastRunTimestamp.FromUnixTimestamp(), now);

            MakeCancellationFeed(dataRepo, lastRunTimestamp.FromUnixTimestamp(), now);

            UpdateLastRun(now);

        }

        private static void MakeCancellationFeed(Repo dataRepo, DateTime lastRunDate, DateTime startProgramTime)
        {
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
        }

        static void MakeShipmentFeed(Repo repo, DateTime lastRunDate, DateTime startProgramDate)
        {
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


        }

        static void UpdateLastRun(DateTime date)
        {
            ConfigUtils.UpdateExeAppSetting("lastRun", date.ToUnixTimestamp());
        }
    }

}
