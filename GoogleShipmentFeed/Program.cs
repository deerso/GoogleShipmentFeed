using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedLibrary;
using Models;

namespace GoogleShipmentFeed
{
    class Program
    {
        static void Main(string[] args)
        {


            var dataRepo = new Data.Repo();
            var feedData = dataRepo.GetShipmentFeed(DateTime.Now.BeginningOfDayYesterday());
            var feed = FeedMaker.GenerateFeed<Feed>(FeedType.TSV, new FeedOptions { QualifyOptions = QualifyData.None}, feedData );
            Console.Write(feed);
            Console.Read();
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
