using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Docker.OrderDomain.Consumer
{
    class Program
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            LoadLog4Net();

            Logger.Info("Consumer has been started");
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
