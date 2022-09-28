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

namespace Ecp.True.ConfigurationManager.Console
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Ecp.True.ConfigurationManager.Console.Repositories;
    using Ecp.True.ConfigurationManager.Console.Repositories.Interface;
    using Ecp.True.ConfigurationManager.Console.Settings.Interface;
    using Ecp.True.ConfigurationManager.Services;
    using Ecp.True.ConfigurationManager.Services.Interface;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The program.
    /// </summary>
    [CLSCompliant(false)]
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            ArgumentValidators.ThrowIfNull(args, nameof(args));
            var arguments = BuildArguments(args);

            var container = BuildContainer();
            using (var serviceProvider = new AutofacServiceProvider(container))
            {
                var settingManager = container.Resolve<ISettingsManager>();
                if (arguments.Length > 1)
                {
                    settingManager.Initialize(arguments.Skip(3).ToArray());
                }

                await ConfigureAsync(serviceProvider, arguments[0], arguments[1], arguments[2]).ConfigureAwait(false);
            }
        }

        private static IContainer BuildContainer()
        {
            var serviceCollection = RegisterServices();

            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);

            return builder.Build();
        }

        private static ServiceCollection RegisterServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ITableRepository, TableRepository>();
            serviceCollection.AddSingleton<IConnection, Connection>();
            serviceCollection.AddSingleton<ISettingsManager, SettingsManager>();

            var types = typeof(ISettings).Assembly.GetTypes();
            var allSettingTypes = types.Where(t =>
                                            !t.IsAbstract &&
                                            t.IsClass &&
                                            t.IsSubclassOf(typeof(Settings.SettingsBase)));
            allSettingTypes.ToList().ForEach(t =>
            {
                serviceCollection.AddSingleton(typeof(ISettings), t);
            });

            return serviceCollection;
        }

        private static async Task ConfigureAsync(IServiceProvider serviceProvider, string connectionString, string ignorables, string forceUpdates)
        {
            var connection = serviceProvider.GetService<IConnection>();
            connection.Initialize(connectionString);

            var tableRepository = serviceProvider.GetService<ITableRepository>();
            await tableRepository.InitializeAsync().ConfigureAwait(false);
            Console.WriteLine(connectionString);
            await tableRepository.UpsertConfigSettingsAsync(ignorables, forceUpdates).ConfigureAwait(false);
        }

        private static string[] BuildArguments(string[] args)
        {
            var arguments = new List<string>();
            if (Debugger.IsAttached)
            {
                arguments.Add(ConfigurationManager.AppSettings["StorageConnectionString"]);
                arguments.Add(string.Empty);
                arguments.Add("{}");
            }
            else
            {
                arguments.AddRange(args);
            }

            return arguments.ToArray();
        }
    }
}
