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

namespace Ecp.True.Host.Functions.Delta.Bootstrap
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Processors.Delta;
    using Ecp.True.Processors.Delta.Builders;
    using Ecp.True.Processors.Delta.Calculation;
    using Ecp.True.Processors.Delta.Consolidation;
    using Ecp.True.Processors.Delta.Delta;
    using Ecp.True.Processors.Delta.Executors;
    using Ecp.True.Processors.Delta.Integration;
    using Ecp.True.Processors.Delta.Interfaces;
    using Ecp.True.Processors.Delta.Services;
    using Ecp.True.Processors.Deltas;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.OwnershipRules;
    using Ecp.True.Proxies.OwnershipRules.Retry;
    using Microsoft.Extensions.DependencyInjection;
    using OfficialDeltaExecutors = Ecp.True.Processors.Delta.OfficialDeltaExecutors;
    using OwnershipRulesInterfaces = Ecp.True.Proxies.OwnershipRules.Interfaces;
    using OwnershipRulesServices = Ecp.True.Proxies.OwnershipRules.Services;

    /// <summary>
    /// The transform bootstrap service.
    /// </summary>
    /// <seealso cref="Bootstrapper" />
    [ExcludeFromCodeCoverage]
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
            this.RegisterTransient<OwnershipRulesInterfaces.IDeltaProxy, OwnershipRulesServices.DeltaProxy>();
            this.RegisterTransient<OwnershipRulesInterfaces.IOwnershipRuleClient, OwnershipRuleClient>();
            this.RegisterTransient<IAnalysisServiceClient, AnalysisServiceClient>();

            this.RegisterTransient<OwnershipRulesInterfaces.IHttpClientProxy, OwnershipRulesServices.HttpClientProxy>();
            this.RegisterSingleton<IRetryPolicyFactory, RetryPolicyFactory>();
            this.RegisterSingleton<IRetryHandler, OwnershipRuleRetryHandler>();

            this.RegisterTransient<IExecutor, ValidationExecutor>();
            this.RegisterTransient<IExecutor, InfoBuildExecutor>();
            this.RegisterTransient<IExecutor, RequestInvocationExecutor>();
            this.RegisterTransient<IExecutor, CompleteExecutor>();
            this.RegisterTransient<IExecutor, ErrorExecutor>();
            this.RegisterTransient<IExecutor, ProcessResultExecutor>();
            this.RegisterTransient<IExecutor, BuildResultExecutor>();
            this.RegisterTransient<IExecutor, OfficialDeltaExecutors.InfoBuildExecutor>();
            this.RegisterTransient<IExecutor, OfficialDeltaExecutors.RequestInvocationExecutor>();
            this.RegisterTransient<IExecutor, OfficialDeltaExecutors.RegisterMovementsExecutor>();
            this.RegisterTransient<IExecutor, OfficialDeltaExecutors.ErrorExecutor>();
            this.RegisterTransient<IExecutor, OfficialDeltaExecutors.BuildResultExecutor>();
            this.RegisterTransient<IExecutor, OfficialDeltaExecutors.CalculateExecutor>();
            this.RegisterTransient<IExecutionManager, ExecutionManager>();
            this.RegisterTransient<IExecutionManager, OfficialDeltaExecutors.OfficialDeltaExecutionManager>();
            this.RegisterTransient<IExecutionManagerFactory, ExecutionManagerFactory>();
            this.RegisterScoped<IExecutionChainBuilder, ExecutionChainBuilder>();

            this.RegisterScoped<IRegistrationStrategyFactory, RegistrationStrategyFactory>();
            this.RegisterScoped<IDeltaStrategyFactory, DeltaStrategyFactory>();
            this.RegisterTransient<IFailureHandler, DeltaFailureHandler>();
            this.RegisterTransient<IFailureHandlerFactory, FailureHandlerFactory>();

            this.RegisterTransient<IOfficialDeltaBuilder, ConsolidatedInventoryDeltaBuilder>();
            this.RegisterTransient<IOfficialDeltaBuilder, ConsolidatedMovementDeltaBuilder>();
            this.RegisterTransient<IOfficialDeltaBuilder, InventoryOfficialDeltaBuilder>();
            this.RegisterTransient<IOfficialDeltaBuilder, MovementOfficialDeltaBuilder>();
            this.RegisterTransient<IOfficialDeltaBuilder, OfficialDeltaInventoryResultMovementBuilder>();
            this.RegisterTransient<IOfficialDeltaBuilder, OfficialDeltaResponseMovementBuilder>();
            this.RegisterTransient<ICompositeOfficialDeltaBuilder, CompositeOfficialDeltaBuilder>();

            this.RegisterScoped<IConsolidationStrategyFactory, ConsolidationStrategyFactory>();
            this.RegisterTransient<IDeltaBalanceCalculator, DeltaBalanceCalculator>();

            this.RegisterTransient<IDeltaProcessor, DeltaProcessor>();
            this.RegisterTransient<IFinalizerFactory, FinalizerFactory>();
            this.RegisterTransient<IFinalizer, DeltaFinalizer>();
            this.RegisterTransient<IFinalizer, OfficialDeltaFinalizer>();

            this.RegisterTransient<IFailureHandler, OfficialDeltaFailureHandler>();
            this.RegisterTransient<IConsolidationProcessor, ConsolidationProcessor>();
            this.RegisterTransient<IOfficialDeltaProcessor, OfficialDeltaProcessor>();

            this.RegisterTransient<IFileRegistrationTransactionService, FileRegistrationTransactionService>();
            this.RegisterTransient<ISaveIntegrationDeltaFile, SaveIntegrationDeltaFile>();
            this.RegisterSingleton<IMovementAggregationService, MovementAggregationService>();
            this.RegisterSingleton<IOfficialDeltaResponseConvertService, OfficialDeltaResponseConvertService>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
    }
}
