using System;
using System.Collections.Generic;

namespace Docker.OrderDomain.Grpc.Context
{
    public partial class OrderProduct
    {
        public string Ref { get; set; }
        public string OrderRef { get; set; }
        public string ProductName { get; set; }
        public decimal Value { get; set; }

        public Order OrderRefNavigation { get; set; }
    }
}
