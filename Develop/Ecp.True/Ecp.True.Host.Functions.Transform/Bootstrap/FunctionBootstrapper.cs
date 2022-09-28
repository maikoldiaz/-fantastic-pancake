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

namespace Ecp.True.Host.Functions.Transform.Bootstrap
{
    using System.Diagnostics.CodeAnalysis;

    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Host.Functions.Core.Interfaces;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Interfaces;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Processors.Registration;
    using Ecp.True.Processors.Registration.Interfaces;
    using Ecp.True.Processors.Registration.Validation;
    using Ecp.True.Processors.Transform.Homologate;
    using Ecp.True.Processors.Transform.Homologate.Interfaces;
    using Ecp.True.Processors.Transform.Input;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Ecp.True.Processors.Transform.Services;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Processors.Transform.Services.Json;
    using Ecp.True.Processors.Transform.Services.Json.Interfaces;
    using Ecp.True.Proxies.Azure;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The transform bootstrap service.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.Setup.Bootstrapper" />
    [ExcludeFromCodeCoverage]
    public class FunctionBootstrapper : Bootstrapper
    {
        /// <summary>
        /// The excel tranformation.
        /// </summary>
        private readonly ExcelTransformBootstrapper excel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionBootstrapper"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public FunctionBootstrapper(IServiceCollection services)
            : base(services)
        {
            this.excel = new ExcelTransformBootstrapper(services);
        }

        /// <inheritdoc/>
        protected override void RegisterServices()
        {
            this.RegisterTransient<IInputFactory, InputFactory>();
            this.RegisterTransient<ITransformProcessor, JsonTransformProcessor>();
            this.RegisterTransient<IJsonTransformer, JsonTransformer>();
            this.RegisterTransient<IDataService, DataService>();
            this.RegisterTransient<IBlobGenerator, BlobGenerator>();
            this.RegisterTransient<IFileRegistrationTransactionService, FileRegistrationTransactionService>();
            this.RegisterTransient<IFileRegistrationTransactionGenerator, FileRegistrationTransactionGenerator>();

            this.RegisterScoped<IHomologationMapper, HomologationMapper>();
            this.RegisterScoped<ITransformationMapper, TransformationMapper>();
            this.RegisterTransient<IHomologator, Homologator>();
            this.RegisterScoped<IAzureClientFactory, AzureClientFactory>();
            this.RegisterSingleton<IBlobStorageClient, BlobStorageClient>();
            this.RegisterTransient<IBlobOperations, BlobOperations>();
            this.RegisterTransient<IAnalysisServiceClient, AnalysisServiceClient>();
            this.RegisterTransient<IMovementRegistrationService, MovementRegistrationService>();

            // Registration Strategy
            this.RegisterTransient<IRegistrationProcessor, RegistrationProcessor>();
            this.RegisterTransient<IRegistrationStrategyFactory, RegistrationStrategyFactory>();

            // Validator
            this.RegisterTransient(typeof(ICompositeValidator<>), typeof(CompositeValidator<>));

            this.RegisterTransient(typeof(IValidator<>), typeof(DataAnnotationValidator<>));
            this.RegisterTransient(typeof(IValidator<>), typeof(NodeValidator<>));
            this.RegisterTransient(typeof(IValidator<>), typeof(ProductValidator<>));
            this.RegisterTransient(typeof(IValidator<>), typeof(OwnershipValidator<>));
            this.RegisterTransient(typeof(IValidator<>), typeof(VolumeValidator<>));
            this.RegisterTransient(typeof(IValidator<>), typeof(DateValidator<>));
            this.RegisterTransient(typeof(IValidator<>), typeof(ClassificationValidator<>));
            this.RegisterTransient(typeof(IValidator<>), typeof(SegmentValidator<>));
            this.RegisterTransient(typeof(IValidator<>), typeof(ElementValidator<>));

            this.RegisterScoped<ICompositeValidatorFactory, CompositeValidatorFactory>();
            this.RegisterTransient<IInventoryValidator, InventoryValidator>();
            this.RegisterTransient<IMovementValidator, MovementValidator>();
            this.RegisterTransient<IEventValidator, EventValidator>();
            this.RegisterTransient<IContractValidator, ContractValidator>();

            this.excel.Bootstrap();
        }
    }
}
