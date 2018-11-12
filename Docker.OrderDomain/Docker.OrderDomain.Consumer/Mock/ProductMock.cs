using System;
using System.Collections.Generic;
using System.Text;
using Docker.OrderDomain.Grpc;

namespace Docker.OrderDomain.Consumer.Mock
{
    public class ProductMock
    {
        public string Name { get; set; }
        public decimal Value { get; set; }

        public OrderProduct ConvertToGrpc()
        {
            return new OrderProduct { ProductName = this.Name, Value = this.Value.ToString() };
        }
    }

    public class ProductsMock
    {
        public List<ProductMock> Products { get; set; }
    }
}
