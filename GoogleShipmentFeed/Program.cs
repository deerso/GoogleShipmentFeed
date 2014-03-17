using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using Chronos;
using Chronos.Configuration;
using FeedLibrary;
using Models;

namespace GoogleShipmentFeed
{
    static class Program
    {
        static void Main()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var dataRepo = new Data.Repo();
            var feedData = dataRepo.GetShipmentFeed(yesterday.StartOfDay(), yesterday.EndOfDay());
            var feed = FeedMaker.GenerateFeed<Feed>(FeedType.TSV, new FeedOptions { QualifyOptions = QualifyData.None}, feedData );
            Console.Write(feed);

            var feedPath = ConfigUtils.GetAppSetting("feedFile");
            var appendDate = ConfigUtils.GetAppSetting("appendDate", false);

            if (appendDate)
            {
                feedPath = Path.GetFileNameWithoutExtension(feedPath) + DateTime.Now.ToString("yyyy-MM-dd") +
                    Path.GetExtension(feedPath);
            }
            File.WriteAllText(feedPath, feed);
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime BeginningOfDayYesterday(this DateTime This)
        {
            var yesterday = This - TimeSpan.FromDays(1);
            return new DateTime(yesterday.Year, yesterday.Month, yesterday.Day);           
        }
    }
}
