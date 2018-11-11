using Grpc.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using static Docker.OrderDomain.Grpc.OrderService;

namespace Docker.OrderDomain.Grpc
{
    internal class OrderImplementation : OrderServiceBase
    {
        public override Task<SendOrderReply> SendOrder(SendOrderRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        const int Port = 50051;

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            Server server = new Server
            {
                Services = { OrderService.BindService(new OrderImplementation()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("OrderDomain server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
