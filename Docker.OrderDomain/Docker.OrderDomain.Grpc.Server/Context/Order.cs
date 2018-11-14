using System;
using System.Collections.Generic;

namespace Docker.OrderDomain.Grpc.Context
{
    public partial class Order
    {
        public Order()
        {
            OrderProduct = new HashSet<OrderProduct>();
        }

        public string Ref { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public ICollection<OrderProduct> OrderProduct { get; set; }
    }
}
