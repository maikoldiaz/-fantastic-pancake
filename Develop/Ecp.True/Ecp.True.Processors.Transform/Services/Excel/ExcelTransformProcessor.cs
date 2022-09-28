// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelTransformProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services.Excel
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Ecp.True.Processors.Transform.Services.Excel.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The excel tranform processor.
    /// </summary>
    public class ExcelTransformProcessor : TransformProcessor
    {
        /// <summary>
        /// The input factory.
        /// </summary>
        private readonly IInputFactory inputFactory;

        /// <summary>
        /// The excel transformer.
        /// </summary>
        private readonly IExcelTransformer excelTransformer;

        /// <summary>
        /// The data service.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelTransformProcessor" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="inputFactory">The input factory.</param>
        /// <param name="excelTransformer">The excel transformer.</param>
        /// <param name="dataService">The data service.</param>
        public ExcelTransformProcessor(
            ITrueLogger<ExcelTransformProcessor> logger,
            IInputFactory inputFactory,
            IExcelTransformer excelTransformer,
            IDataService dataService)
            : base(logger, inputFactory, dataService)
        {
            this.inputFactory = inputFactory;
            this.excelTransformer = excelTransformer;
            this.dataService = dataService;
        }

        /// <inheritdoc/>
        public override InputType InputType => InputType.EXCEL;

        /// <inheritdoc/>
        public override Task CompleteAsync(JArray homologatedArray, TrueMessage trueMessage)
        {
            var inventory = homologatedArray.OfType<JObject>().SingleOrDefault(t => t.ContainsKey("Inventory"))?.Value<JArray>("Inventory");
            var movements = homologatedArray.OfType<JObject>().SingleOrDefault(t => t.ContainsKey("Movements"))?.Value<JArray>("Movements");
            var events = homologatedArray.OfType<JObject>().SingleOrDefault(t => t.ContainsKey("Events"))?.Value<JArray>("Events");
            var contracts = homologatedArray.OfType<JObject>().SingleOrDefault(t => t.ContainsKey("Contracts"))?.Value<JArray>("Contracts");
            return this.dataService.SaveExcelAsync(inventory, movements, events, contracts, trueMessage);
        }

        /// <inheritdoc/>
        protected override async Task<JArray> DoTransformAsync(TrueMessage message)
        {
            // 1. Fetch Excel Blob
            var element = await this.inputFactory.GetExcelInputAsync(message).ConfigureAwait(false);

            // 2. Canonical Transformation
            return await this.excelTransformer.TransformExcelAsync(element).ConfigureAwait(false);
        }
    }
}
