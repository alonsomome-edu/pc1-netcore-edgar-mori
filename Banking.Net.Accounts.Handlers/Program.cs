using Banking.Net.Command.Accounts.Infra.Persistence.NHibernate.Mapping;
using Banking.Net.Common.Infra.NSB;
using Banking.Net.Common.Infra.Persistence.NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.Extensions.Hosting;
using NHibernate.Cfg;
using NServiceBus;
using NServiceBus.NHibernate.Outbox;
using NServiceBus.Persistence;
using System;
using System.Threading.Tasks;
using Environment = System.Environment;
using NHEnvironment = NHibernate.Cfg.Environment;

namespace Banking.Accounts.Handlers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Banking.BankAccounts";
            var host = CreateHostBuilder(args).Build();
            await host.StartAsync();
            Console.WriteLine("Press any key to shutdown");
            Console.ReadKey();
            await host.StopAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseMicrosoftLogFactoryLogging()
                .UseNServiceBus(hostBuilderContext =>
                {
                    var endpointConfiguration = ConfigureEndpoint("Banking.BankAccounts");
                    return endpointConfiguration;
                });

        public static EndpointConfiguration ConfigureEndpoint(string endpointName)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.EnableOutbox();
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            string rabbitmqUrl = Environment.GetEnvironmentVariable("RABBITMQ_BANKING_NSB_CORE");
            transport.ConnectionString(rabbitmqUrl);
            transport.UseConventionalRoutingTopology();
            var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
            persistence.UseOutboxRecord<Outbox, OutboxMap>();
            var nHibernateConfig = new Configuration();
            nHibernateConfig.SetProperty(NHEnvironment.ConnectionProvider, typeof(NHibernate.Connection.DriverConnectionProvider).FullName);
            nHibernateConfig.SetProperty(NHEnvironment.ConnectionDriver, typeof(NHibernate.Driver.MySqlDataDriver).FullName);
            nHibernateConfig.SetProperty(NHEnvironment.Dialect, typeof(NHibernate.Dialect.MySQLDialect).FullName);
            string stringConnection = Environment.GetEnvironmentVariable("MYSQL_BANKING_CORE_NSB");
            nHibernateConfig.SetProperty(NHEnvironment.ConnectionString, stringConnection);
            AddFluentMappings(nHibernateConfig, stringConnection);
            persistence.UseConfiguration(nHibernateConfig);
            persistence.DisableSchemaUpdate();
            return endpointConfiguration;
        }

        private static Configuration AddFluentMappings(Configuration nhConfiguration, string stringConnection)
        {
            return Fluently
                .Configure(nhConfiguration)
                .Database(MySQLConfiguration.Standard.ConnectionString(stringConnection))
                .Mappings(cfg =>
                {
                    cfg.FluentMappings.AddFromAssembly(typeof(BankAccountMap).Assembly);
                    cfg.FluentMappings.Conventions.Add(
                        ForeignKey.EndsWith("_id"),
                        ConventionBuilder.Property.When(criteria => criteria.Expect(x => x.Nullable, Is.Not.Set), x => x.Not.Nullable()));
                    cfg.FluentMappings.Conventions.Add<OtherConversions>();
                    cfg.FluentMappings.Conventions.Add<TableNameConvention>();
                })
                .BuildConfiguration();
        }
    }
}