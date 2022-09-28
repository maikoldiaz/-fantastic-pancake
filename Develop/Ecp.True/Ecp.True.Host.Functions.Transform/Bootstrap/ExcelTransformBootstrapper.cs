// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelTransformBootstrapper.cs" company="Microsoft">
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
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Builders.Excel.Movement;
    using Ecp.True.Processors.Transform.Services.Excel;
    using Ecp.True.Processors.Transform.Services.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The transformation registration entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.Bootstrap.BootstrapperBase" />
    public class ExcelTransformBootstrapper : BootstrapperBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelTransformBootstrapper"/> class.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        public ExcelTransformBootstrapper(IServiceCollection serviceCollection)
            : base(serviceCollection)
        {
        }

        /// <summary>
        /// Bootstraps this instance.
        /// </summary>
        public void Bootstrap()
        {
            this.RegisterTransient<IExcelTransformer, ExcelTransformer>();
            this.RegisterTransient<ITransformProcessor, ExcelTransformProcessor>();
            this.RegisterTransient<IExcelInventoryTransformer, ExcelInventoryTransformer>();
            this.RegisterTransient<IExcelMovementTransformer, ExcelMovementTransformer>();
            this.RegisterTransient<IExcelEventTransformer, ExcelEventTransformer>();
            this.RegisterTransient<IExcelContractTransformer, ExcelContractTransformer>();

            this.RegisterTransient<IExcelAttributeBuilder, ExcelAttributeBuilder>();
            this.RegisterTransient<IExcelOwnerBuilder, ExcelOwnerBuilder>();

            this.RegisterTransient<IExcelInventoryBuilder, ExcelInventoryBuilder>();

            this.RegisterTransient<IExcelMovementBuilder, ExcelMovementBuilder>();

            this.RegisterTransient<IExcelEventBuilder, ExcelEventBuilder>();
            this.RegisterTransient<IExcelContractBuilder, ExcelContractBuilder>();
        }
    }
}
