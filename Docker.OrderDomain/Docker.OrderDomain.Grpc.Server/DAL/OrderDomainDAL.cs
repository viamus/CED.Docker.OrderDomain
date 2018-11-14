using Docker.OrderDomain.Grpc.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Docker.OrderDomain.Grpc.DAL
{
    public class OrderDomainDAL : IDisposable
    {
        private IServiceProvider _Services;

        public OrderDomainDAL(IServiceProvider serivces)
        {
            _Services = serivces;
        }

        public void Dispose()
        {
            this.Dispose();
        }

        public void SaveOrder(Context.Order order, List<Context.OrderProduct> products)
        {
            var context = _Services.GetService(typeof(OrderDomainContext)) as OrderDomainContext;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Order.Add(order);

                    context.SaveChanges();

                    foreach (var product in products)
                    {
                        context.OrderProduct.Add(product);
                    }

                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
