using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedLibrary.Attributes;

namespace Models
{
    public class Feed
    {
        [FeedData(HeaderName = "order id")]
        public string OrderId { get; set; }

        [FeedData(HeaderName = "ship date")]
        public string ShipmentDate { get; set; }

        [FeedData(HeaderName = "tracking number")]
        public string CarrierTrackingNumber { get; set; }

        [FeedData(HeaderName = "carrier code")]
        public string CarrierName { get; set; }
    }

    public class CancelFeed
    {
        [FeedData(HeaderName = "order id")]
        public string OrderId { get; set; }
        
        [FeedData(HeaderName = "reason")]
        public string ReasonForCancellation { get; set; }
    }
}
