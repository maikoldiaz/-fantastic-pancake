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

namespace Ecp.True.Host.Functions.Balance.Bootstrap
{
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Processors.Balance;
    using Ecp.True.Processors.Balance.Interfaces;
    using Ecp.True.Processors.Balance.Retry;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The transform bootstrap service.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.Setup.Bootstrapper" />
    public class FunctionBootstrapper : Bootstrapper
    {
        /// <summary>
        /// The balance calculation.
        /// </summary>
        private readonly CalculationBootstrapper calculation;

        /// <summary>
        /// The movement generation.
        /// </summary>
        private readonly MovementBootstrapper movement;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBootstrapper"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public FunctionBootstrapper(IServiceCollection services)
            : base(services)
        {
            this.calculation = new CalculationBootstrapper(services);
            this.movement = new MovementBootstrapper(services);
        }

        /// <inheritdoc/>
        protected override void RegisterServices()
        {
            this.RegisterTransient<IBalanceService, BalanceService>();
            this.RegisterTransient<ISegmentBalanceService, SegmentBalanceService>();
            this.RegisterTransient<ISystemBalanceService, SystemBalanceService>();
            this.RegisterTransient<IBalanceProcessor, BalanceProcessor>();
            this.RegisterSingleton<IRetryPolicyFactory, RetryPolicyFactory>();
            this.RegisterSingleton<ICutOffBalanceRetryHandler, CutOffBalanceRetryHandler>();
            this.RegisterScoped<IAzureClientFactory, AzureClientFactory>();
            this.RegisterSingleton<IBlobStorageClient, BlobStorageClient>();
            this.RegisterTransient<IAnalysisServiceClient, AnalysisServiceClient>();
            this.RegisterTransient<IFailureHandler, CutOffFailureHandler>();
            this.RegisterTransient<IFailureHandlerFactory, FailureHandlerFactory>();
            this.RegisterTransient<IFinalizer, OperationalCutOffFinalizer>();

            this.calculation.Bootstrap();
            this.movement.Bootstrap();
        }
    }
}
