// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup(typeof(Ecp.True.Host.Functions.Deadletter.Bootstrap.Startup))]

namespace Ecp.True.Host.Functions.Deadletter.Bootstrap
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Host.Functions.Core.Setup;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The function startup class.
    /// </summary>
    /// <seealso cref="Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup" />
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc/>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ArgumentValidators.ThrowIfNull(builder, nameof(builder));

            // 1. Binding redirects configuration
            BindingRedirectHelper.ConfigureBindingRedirects();

            // 2. Register dependencies
            builder.Services.AddHttpClient(Constants.DefaultHttpClient);
            var bootstrap = new FunctionBootstrapper(builder.Services);
            bootstrap.RegisterDependencies();

            // 3. Register http client factory
            builder.Services.AddHttpClient();

            builder.Services.EnableSqlInstrumentation();
        }
    }
}
