// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;
    using Ecp.True.Host.DataGenerator.Console.Strategies;
    using Ecp.True.Repositories;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The program class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The task.</returns>
        public static async Task Main(string[] args)
        {
            var sqlConnectionSettings = new SqlConnectionSettings
            {
                ConnectionString = string.Empty,
                MaxRetryCount = 3,
                RetryIntervalInSecs = 10,
                TenantId = string.Empty,
            };

            await MainAsync(sqlConnectionSettings, args).ConfigureAwait(false);
        }

        /// <summary>
        /// Mains the asynchronous.
        /// </summary>
        /// <param name="sqlConnectionSettings">The SQL connection settings.</param>
        /// <param name="args">The arguments.</param>
        private static async Task MainAsync(SqlConnectionSettings sqlConnectionSettings, string[] args)
        {
            var container = BuildContainer();
            using (var serviceProvider = new AutofacServiceProvider(container))
            {
                Configure(serviceProvider, sqlConnectionSettings);
                var dataGenerationManager = (IDataGenerationManager)serviceProvider.GetService(typeof(IDataGenerationManager));

                DisplayReadMe();

                var menu = new ActionMenu()
                    .Add("Generate Delta Data", async () => await dataGenerationManager.GenerateDeltaDataAsync(args).ConfigureAwait(false))
                    .Add("Generate Consolidation Delta Data", async () => await dataGenerationManager.GenerateConsolidationDeltaDataAsync(args).ConfigureAwait(false))
                    .Add("Generate Official Delta Data", async () => await dataGenerationManager.GenerateOfficialDeltaDataAsync(args).ConfigureAwait(false))
                    .Add("Generate Official Logistics Data", async () => await dataGenerationManager.GenerateOfficialLogisticsDataAsync(args).ConfigureAwait(false))
                    .Add("Generate CutOff Data", async () => await dataGenerationManager.GenerateCutOffDataAsync(args).ConfigureAwait(false));
                await menu.DisplayAsync(true).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Displays the read me.
        /// </summary>
        private static void DisplayReadMe()
        {
            Console.WriteLine($" --------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"  This tool generates complex data with relationships to test the functionalities.");
            Console.WriteLine($"  Once you run the cases/scenarios tool will generate unique TestId which you can use to query the database.");
            Console.WriteLine($"  For Example: SELECT * FROM [ADMIN].[CATEGORYELEMENT] WHERE NAME LIKE '%<TestId>%'");
            Console.WriteLine($" --------------------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// Builds the container.
        /// </summary>
        /// <returns>The IContainer.</returns>
        private static IContainer BuildContainer()
        {
            var serviceCollection = RegisterServices();

            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);

            return builder.Build();
        }

        /// <summary>
        /// Registers the services.
        /// </summary>
        /// <returns>The ServiceCollection.</returns>
        private static ServiceCollection RegisterServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ISqlDataContext, SqlDataContext>();
            serviceCollection.AddScoped<ISqlTokenProvider, SqlTokenProvider>();
            serviceCollection.AddSingleton<IBusinessContext, BusinessContext>();
            serviceCollection.AddSingleton<IConnectionFactory, ConnectionFactory>();
            serviceCollection.AddTransient(typeof(ISqlDataAccess<>), typeof(SqlDataAccess<>));
            serviceCollection.AddTransient(typeof(IDataAccess<>), typeof(SqlDataAccess<>));
            serviceCollection.AddTransient<IRepositoryFactory, RepositoryFactory>();
            serviceCollection.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
            serviceCollection.AddTransient<IAuditService, AuditService>();

            serviceCollection.AddTransient<IDataGeneratorStrategyFactory, DataGeneratorStrategyFactory>();
            serviceCollection.AddTransient<IDataGenerationManager, DataGenerationManager>();

            return serviceCollection;
        }

        /// <summary>
        /// Configures the asynchronous.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="sqlConnectionSettings">The SQL connection settings.</param>
        private static void Configure(IServiceProvider serviceProvider, SqlConnectionSettings sqlConnectionSettings)
        {
            var connectionFactory = (IConnectionFactory)serviceProvider.GetService(typeof(IConnectionFactory));
            connectionFactory.SetupSqlConfig(sqlConnectionSettings);

            var businessContext = (IBusinessContext)serviceProvider.GetService(typeof(IBusinessContext));
            businessContext.Populate("Data Generator", "Data Generator", new List<string> { "Data Generator Application" });
        }
    }
}
