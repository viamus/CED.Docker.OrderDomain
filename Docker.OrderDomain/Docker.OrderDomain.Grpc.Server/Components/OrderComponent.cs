using Docker.OrderDomain.Grpc.Context;
using Docker.OrderDomain.Grpc.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Docker.OrderDomain.Grpc.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace Docker.OrderDomain.Grpc.Components
{
    public class OrderComponent
    {
        public SendOrderReply SaveOrder(SendOrderRequest request)
        {
            SendOrderReply response = new SendOrderReply();

            try
            {
                var mapper = OrderDomainMapper.Instance.Mapper.CreateMapper();

                var order = mapper.Map<Order>(request);

                order.Ref = Guid.NewGuid().ToString().ToUpper();
                order.Created = order.Updated = DateTime.Now;
                var products = new List<Context.OrderProduct>();

                request.Products.ToList().ForEach(product =>
                {
                    var productValue = Decimal.Parse(product.Value);
                    order.TotalValue += productValue;
                    products.Add(new Context.OrderProduct { OrderRef = order.Ref, Ref = Guid.NewGuid().ToString().ToUpper(), ProductName = product.ProductName, Value = productValue });
                });

                var orderDal = new OrderDomainDAL(Program.Services);
                
                orderDal.SaveOrder(order, products); 

                response.Message = $"Message {order.Ref} has been processed!";
                response.Status = SendOrderStatus.Created;

            }catch(Exception ex)
            {
                response.Message = $"Error {ex.Message}!";
                response.Status = SendOrderStatus.InternalServerError;
            }

            return response;
        }
    }
}
