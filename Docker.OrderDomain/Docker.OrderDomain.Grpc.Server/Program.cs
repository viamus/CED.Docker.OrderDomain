﻿using Docker.OrderDomain.Grpc.Components;
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
using System.Threading;
using static Docker.OrderDomain.Grpc.OrderService;
using Docker.OrderDomain.Grpc.Context;
using Microsoft.EntityFrameworkCore;
using Docker.OrderDomain.Grpc.Mapper;

namespace Docker.OrderDomain.Grpc
{
    internal class OrderImplementation : OrderServiceBase
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(OrderImplementation));

        public override async Task SendOrder(IAsyncStreamReader<SendOrderRequest> requestStream, IServerStreamWriter<SendOrderReply> responseStream, ServerCallContext context)
        {
            string requestGuid = Guid.NewGuid().ToString();

            Logger.Info($"Started a new grpc stream {requestGuid}");

            try
            {
                var component = new OrderComponent();

                while(await requestStream.MoveNext(CancellationToken.None))
                {
                    var requestMessage = requestStream.Current;

                    Logger.Info($"Stream {requestGuid} received new message {requestMessage}");

                    var response = component.SaveOrder(requestMessage);

                    Logger.Info($"Stream {requestGuid} had a response {response}");

                    await responseStream.WriteAsync(response);
                }
            }
            catch(RpcException ex)
            {
                Logger.Error($"Stream {requestGuid} had a processing error {ex.Message}");
            }
            finally
            {
                Logger.Info($"Stream {requestGuid} has been closed");
            }
        }
    }

    class Program
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(Program));

        public static ServiceProvider Services;

        private static AutoResetEvent waitHandle = new AutoResetEvent(false);

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            LoadLog4Net();

            LoadInstanceContext();

            var server = LoadGrpcServer();

            Console.CancelKeyPress += (o, e) =>
            {
                Logger.Info($"Server closing");
                waitHandle.Set();
            };

            waitHandle.WaitOne();

            server.ShutdownAsync().Wait();
        }

        private static Server LoadGrpcServer()
        {
            Server server = new Server
            {
                Services = { BindService(new OrderImplementation()) },
                Ports = { new ServerPort("0.0.0.0", 50051, ServerCredentials.Insecure) }
            };

            server.Start();

            Logger.Info("Server listening on port " + 50051);

            return server;
        }

        private static void LoadLog4Net()
        {
            XmlDocument log4netConfig = new XmlDocument();

            log4netConfig.Load(File.OpenRead("log4net.config"));

            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }

        private static void LoadInstanceContext()
        {
            OrderDomainMapper.Instance.LoadMapperConfig();

            var configuration = new ConfigurationBuilder()
                           .AddEnvironmentVariables()
                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                           .Build();

            var services = new ServiceCollection();

            var connectionString = configuration.GetConnectionString("DefaultConnectionString");

            if (configuration["DbHost"] != null && configuration["DbUser"] != null && configuration["DbPassword"] != null)
            {
                connectionString = string.Format(configuration.GetConnectionString("DockerConnectionString"), configuration["DbHost"], configuration["DbUser"], configuration["DbPassword"]);
            }

            services.AddDbContext<OrderDomainContext>(
                   options => options.UseMySql(connectionString));


            Services = services.BuildServiceProvider();
        }
    }
}
