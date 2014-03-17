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
        [FeedData(HeaderName = "Order ID")]
        public string OrderId { get; set; }

        [FeedData(HeaderName = "Shipment Date")]
        public string ShipmentDate { get; set; }

        [FeedData(HeaderName = "Carrier Tracking Number")]
        public string CarrierTrackingNumber { get; set; }

        [FeedData(HeaderName = "Carrier Name")]
        public string CarrierName { get; set; }
    }
}
