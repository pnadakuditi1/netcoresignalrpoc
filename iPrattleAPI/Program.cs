using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iPrattle.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace iPrattleAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            Log.Information("[Main] Debug logging.");
            Log.Warning("[Main] Warning logging.");

            hostBuilder.Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
