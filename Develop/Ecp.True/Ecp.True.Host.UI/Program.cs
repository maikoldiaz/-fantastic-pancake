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

namespace Ecp.True.Host.UI
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureKeyVault;

    /// <summary>
    /// The program file.
    /// </summary>
    [CLSCompliant(false)]
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// Builds the web host.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The web host.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder<Startup>(args)
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        var builtConfig = config.Build();

                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(
                                azureServiceTokenProvider.KeyVaultTokenCallback));

                        config.AddAzureKeyVault(
                            $"https://{builtConfig["Settings:VaultName"]}.vault.azure.net/",
                            keyVaultClient,
                            new DefaultKeyVaultSecretManager());
                    })
                    .ConfigureKestrel((context, options) =>
                    {
                        // Set properties and call methods on options
                        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
                        options.AddServerHeader = false;
                    })
                    .ConfigureServices(s => s.AddAutofac())
                    .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                    .CaptureStartupErrors(true);
        }

        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}
