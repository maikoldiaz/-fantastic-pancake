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

namespace Ecp.True.Host.Functions.Ownership.Bootstrap
{
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.DataAccess.Sql;
    using Ecp.True.ExceptionHandling;
    using Ecp.True.ExceptionHandling.Core;
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Processors.Approval;
    using Ecp.True.Processors.Approval.Interfaces;
    using Ecp.True.Processors.Conciliation;
    using Ecp.True.Processors.Conciliation.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Core.HandleFailure;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Processors.Ownership;
    using Ecp.True.Processors.Ownership.Executors;
    using Ecp.True.Processors.Ownership.Input;
    using Ecp.True.Processors.Ownership.Input.Interfaces;
    using Ecp.True.Processors.Ownership.Integration;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Processors.Ownership.Retry;
    using Ecp.True.Processors.Ownership.Services;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Ecp.True.Proxies.OwnershipRules;
    using Ecp.True.Proxies.OwnershipRules.Retry;
    using Ecp.True.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using OwnershipRulesInterfaces = Ecp.True.Proxies.OwnershipRules.Interfaces;
    using OwnershipRulesServices = Ecp.True.Proxies.OwnershipRules.Services;

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
            this.RegisterTransient<OwnershipRulesInterfaces.IOwnershipRuleProxy, OwnershipRulesServices.OwnershipRuleProxy>();
            this.RegisterTransient<IOwnershipRuleProcessor, OwnershipRuleProcessor>();
            this.RegisterTransient<OwnershipRulesInterfaces.IHttpClientProxy, OwnershipRulesServices.HttpClientProxy>();
            this.RegisterTransient<OwnershipRulesInterfaces.IOwnershipRuleClient, OwnershipRuleClient>();
            this.RegisterSingleton<IRetryPolicyFactory, RetryPolicyFactory>();
            this.RegisterSingleton<IOwnershipBalanceRetryHandler, OwnershipBalanceRetryHandler>();
            this.RegisterSingleton<IRetryHandler, OwnershipRuleRetryHandler>();
            this.RegisterTransient<IConciliationProcessor, ConciliationProcessor>();
            this.RegisterTransient<IOwnershipCalculationService, OwnershipCalculationService>();
            this.RegisterTransient<IAnalyticalOwnershipCalculationService, AnalyticalOwnershipCalculationService>();
            this.RegisterTransient<IDataGeneratorService, DataGeneratorService>();
            this.RegisterScoped<ISqlDataContext, SqlDataContext>();
            this.RegisterTransient<IOwnershipProcessor, OwnershipProcessor>();
            this.RegisterTransient<ILogisticsProcessor, LogisticsProcessor>();
            this.RegisterTransient<ILogisticsService, LogisticsService>();
            this.RegisterTransient<IExcelService, ExcelService>();
            this.RegisterTransient<ICalculateOwnership, CalculateOwnership>();
            this.RegisterTransient<ISegmentOwnershipCalculationService, SegmentOwnershipCalculationService>();
            this.RegisterTransient<ISystemOwnershipCalculationService, SystemOwnershipCalculationService>();
            this.RegisterTransient<IOwnershipService, OwnershipService>();
            this.RegisterTransient<IOwnershipResultService, OwnershipResultService>();
            this.RegisterTransient<IOwnershipInputFactory, OwnershipInputFactory>();
            this.RegisterTransient<IOwnershipValidator, OwnershipValidator>();
            this.RegisterTransient<IRepositoryFactory, RepositoryFactory>();
            this.RegisterTransient<IAnalyticsClient, AnalyticsClient>();
            this.RegisterScoped<IAzureClientFactory, AzureClientFactory>();
            this.RegisterSingleton<IBlobStorageClient, BlobStorageClient>();
            this.RegisterTransient<IAnalysisServiceClient, AnalysisServiceClient>();
            this.RegisterTransient<IInventoryOwnershipService, InventoryOwnershipService>();
            this.RegisterTransient<IMovementOwnershipService, MovementOwnershipService>();
            this.RegisterTransient<IExecutor, TransferPointExecutor>();
            this.RegisterTransient<IExecutor, InfoBuildExecutor>();
            this.RegisterTransient<IExecutor, RequestInvocationExecutor>();
            this.RegisterTransient<IExecutor, ErrorValidationExecutor>();
            this.RegisterTransient<IExecutor, ErrorExecutor>();
            this.RegisterTransient<IExecutor, ResultValidationExecutor>();
            this.RegisterTransient<IExecutor, BuildExecutor>();
            this.RegisterTransient<IExecutor, BuildMovementExecutor>();
            this.RegisterTransient<IExecutor, RegisterExecutor>();
            this.RegisterTransient<IExecutor, CalculateExecutor>();
            this.RegisterTransient<IExecutor, CompleteExecutor>();
            this.RegisterTransient<IExecutionManager, ExecutionManager>();
            this.RegisterTransient<IExecutionManagerFactory, ExecutionManagerFactory>();
            this.RegisterScoped<IExecutionChainBuilder, ExecutionChainBuilder>();
            this.RegisterScoped<IRegistrationStrategyFactory, RegistrationStrategyFactory>();
            this.RegisterTransient<IExpressionEvaluator, ConstantExpressionEvaluator>();
            this.RegisterSingleton<ITrueExpressionEvaluator, TrueExpressionEvaluator>();

            this.RegisterTransient<IFailureHandler, ConciliationFailureHandler>();
            this.RegisterTransient<IFailureHandler, OwnershipFailureHandler>();
            this.RegisterTransient<IFailureHandler, OfficialLogisticsFailureHandler>();
            this.RegisterTransient<IFailureHandlerFactory, FailureHandlerFactory>();
            this.RegisterTransient<IApprovalProcessor, ApprovalProcessor>();
            this.RegisterTransient<IFinalizerFactory, FinalizerFactory>();
            this.RegisterTransient<IFinalizer, OwnershipFinalizer>();
            this.RegisterTransient<IFinalizer, ConciliationFinalizer>();

            this.RegisterTransient<IFileRegistrationTransactionService, FileRegistrationTransactionService>();
            this.RegisterTransient<ISaveIntegrationOwnershipFile, SaveIntegrationOwnershipFile>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
    }
}
