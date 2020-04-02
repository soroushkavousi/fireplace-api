using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using GamingCommunityApi.Api.Tools.NLog;
using GamingCommunityApi.Api.Tools;

namespace GamingCommunityApi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = Utils.SetupLogger();
            if (logger == null)
                return;

            StartApi(logger, args);
        }

        public static void StartApi(Logger logger, string[] args)
        {
            try
            {
                logger.Trace("Starting api...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception!");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                logger.Trace("Flushing logger...");
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(
                        "secrets.json", optional: false, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel((context, serverOptions) =>
                    {
                        serverOptions.Configure(context.Configuration.GetSection("Kestrel"))
                            .Endpoint("Https", listenOptions =>
                            {
                                listenOptions.HttpsOptions.SslProtocols = SslProtocols.Tls12;
                            });
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
    }
}
