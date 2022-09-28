// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionBootstrapper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Reporting.Bootstrap
{
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Processors.Reporting.Interfaces;
    using Ecp.True.Processors.Reporting.Services;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The transform bootstrap service.
    /// </summary>
    /// <seealso cref="Bootstrapper" />
    public class FunctionBootstrapper : Bootstrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBootstrapper"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public FunctionBootstrapper(IServiceCollection services)
            : base(services)
        {
        }

        /// <inheritdoc/>
        protected override void RegisterServices()
        {
            this.RegisterScoped<ISqlDataContext, SqlDataContext>();
            this.RegisterTransient<IRepositoryFactory, RepositoryFactory>();
            this.RegisterScoped<IAzureClientFactory, AzureClientFactory>();
            this.RegisterSingleton<IBlobStorageClient, BlobStorageClient>();
            this.RegisterTransient<IAnalysisServiceClient, AnalysisServiceClient>();

            this.RegisterTransient<IReportGeneratorFactory, ReportGeneratorFactory>();
            this.RegisterTransient<IReportGenerator, OfficialInitialBalanceReportGenerator>();
            this.RegisterTransient<IReportGenerator, CutoffReportGenerator>();
            this.RegisterTransient<IReportGenerator, OperativeOwnershipReportGenerator>();
            this.RegisterTransient<IReportGenerator, MovementSendToSapReportGenerator>();
            this.RegisterTransient<IReportGenerator, UserRolesAndPermissionsReportGenerator>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
    }
}
