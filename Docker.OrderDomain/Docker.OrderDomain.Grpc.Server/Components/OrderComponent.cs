using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.OrderDomain.Grpc.Components
{
    public class OrderComponent
    {
        public SendOrderReply SaveOrder(SendOrderRequest request)
        {
            return new SendOrderReply { Message = "Funfou", Status = SendOrderStatus.Created };
        }
    }
}
