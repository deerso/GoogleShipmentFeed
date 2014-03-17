using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Data
{
    public class Repo
    {
        private readonly ShipmentFeedDataContext _context;
        public Repo()
        {
            _context = new ShipmentFeedDataContext();
        }

        public IEnumerable<Feed> GetShipmentFeed(DateTime fromDate, DateTime toDate)
        {

            var query = _context.Trackings.Join(
                _context.Orders,
                x => x.OrderNum,
                x => x.OrderNumber,
                (x, y) => new {Tracking = x, Order = y}
                )
                .Where(x => x.Order.OrderSource == "WS")
                .Where(x => x.Tracking.DateAdded > fromDate && x.Tracking.DateAdded <= toDate);

            return query.ToList().Select(x => new Feed
            {
                CarrierName = "UPS",
                CarrierTrackingNumber = x.Tracking.TrackingID,
                OrderId = x.Tracking.OrderNum.ToString(),
                ShipmentDate = x.Tracking.DateAdded.GetValueOrDefault().ToString("yyyy-MM-dd")
            });
        }
    }
}
