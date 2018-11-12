using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Docker.OrderDomain.Grpc;
using Newtonsoft.Json;
using Docker.OrderDomain.Consumer.Mock;
using System.Collections.Concurrent;
using System.Linq;
using Grpc.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.OrderDomain.Consumer
{
    class Program
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(Program));

        const string Address = "localhost:50051";

        public static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();
        }

        public static async Task Run()
        {
            LoadLog4Net();

            Logger.Info("Consumer has been started");

            Logger.Info("Generating mock data");

            var data = LoadMockData();

            Logger.Info($"Connecting to grpc server at {Address}");

            var channel = new Channel(Address, ChannelCredentials.Insecure);

            var orderService = new OrderService.OrderServiceClient(channel);

            var stream = orderService.SendOrder();

            var requestTask = SendGrpcOrders(data, stream);

            var responseTask = ReceiveGrpcResponse(stream);

            Task.WaitAll(requestTask, responseTask);
        }

        public static async Task SendGrpcOrders(ICollection<SendOrderRequest> data, AsyncDuplexStreamingCall<SendOrderRequest, SendOrderReply> stream)
        {
            Logger.Info($"Sending orders to grpc server at {Address}");

            foreach (var order in data)
            {
                try
                {
                    await stream.RequestStream.WriteAsync(order);
                    Logger.Info($"Order sent {Guid.NewGuid().ToString()}");
                }
                catch (RpcException ex)
                {
                    Logger.Error($"Grpc request had a error {ex.Message}");
                }
            }

            await stream.RequestStream.CompleteAsync();
        }

        public static async Task ReceiveGrpcResponse(AsyncDuplexStreamingCall<SendOrderRequest, SendOrderReply> stream)
        {
            while (await stream.ResponseStream.MoveNext(CancellationToken.None))
            {
                try
                {
                    var response = stream.ResponseStream.Current;

                    Logger.Info($"Order response {response}");
                }
                catch (RpcException ex)
                {
                    Logger.Error($"Grpc response had a error {ex.Message}");
                }
            }
        }

        private static ICollection<SendOrderRequest> LoadMockData()
        {
            var mockFile = File.ReadAllText("products-mock.json");

            var products = JsonConvert.DeserializeObject<List<ProductsMock>>(mockFile);

            ConcurrentBag<SendOrderRequest> requestBag = new ConcurrentBag<SendOrderRequest>();

            products.AsParallel().ForAll((product) =>
            {
                var request = new SendOrderRequest();

                request.Products.AddRange(product.Products.Select(p => p.ConvertToGrpc()).ToList());

                requestBag.Add(request);
            });

            return requestBag.ToList();
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
