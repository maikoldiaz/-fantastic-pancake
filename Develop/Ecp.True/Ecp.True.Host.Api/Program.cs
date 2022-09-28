// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// The program.
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
