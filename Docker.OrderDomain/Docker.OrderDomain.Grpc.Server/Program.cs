using Docker.OrderDomain.Grpc.Components;
using Grpc.Core;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using static Docker.OrderDomain.Grpc.OrderService;

namespace Docker.OrderDomain.Grpc
{
    internal class OrderImplementation : OrderServiceBase
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(OrderImplementation));

        public override Task<SendOrderReply> SendOrder(SendOrderRequest request, ServerCallContext context)
        {
            Logger.Info("Received request: " + request);

            OrderComponent component = new OrderComponent();
            var result = component.SaveOrder(request);

            Logger.Info("Request response: " + request);

            return Task.FromResult<SendOrderReply>(result);
        }
    }

    class Program
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(Program));

        const int Port = 50051;

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            LoadLog4Net();

            var server = LoadGrpcServer();

            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }

        private static Server LoadGrpcServer()
        {
            Server server = new Server
            {
                Services = { BindService(new OrderImplementation()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };

            server.Start();

            Logger.Info("Server listening on port " + Port);

            return server;
        }

        private static void LoadLog4Net()
        {
            XmlDocument log4netConfig = new XmlDocument();

            log4netConfig.Load(File.OpenRead("log4net.config"));

            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}
