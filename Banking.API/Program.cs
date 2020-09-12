using Banking.API.Infra.Persistence.Migrations.MySQL;
using Banking.Net.Transactions.Messages.Commands;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using Environment = System.Environment;

namespace Banking.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Banking.API";
            var serviceProvider = CreateServices();
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
            CreateHostBuilder(args).Build().Run();
        }

        private static IServiceProvider CreateServices()
        {
            string stringConnection = Environment.GetEnvironmentVariable("MYSQL_BANKING_CORE_NSB");
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .WithGlobalCommandTimeout(new TimeSpan(1, 0, 0))
                    .AddMySql5()
                    .WithGlobalConnectionString(stringConnection)
                    .ScanIn(typeof(CreateInitialschema).Assembly)
                    .For.All()
                )
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseMicrosoftLogFactoryLogging()
                .UseNServiceBus(hostBuilderContext =>
                {
                    var endPointName = "Banking.API";
                    string rabbitmqUrl = Environment.GetEnvironmentVariable("RABBITMQ_BANKING_NSB_CORE");
                    var endpointConfiguration = new EndpointConfiguration(endPointName);
                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                    transport.ConnectionString(rabbitmqUrl);
                    transport.UseConventionalRoutingTopology();
                    var routing = transport.Routing();
                    routing.RouteToEndpoint(typeof(StartTransfer).Assembly, "Banking.Transactions");
                    endpointConfiguration.SendOnly();
                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}