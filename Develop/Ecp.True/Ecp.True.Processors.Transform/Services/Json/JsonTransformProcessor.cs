// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonTransformProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Services.Json
{
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Processors.Transform.Services.Json.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// THe JSON transform processor.
    /// </summary>
    public class JsonTransformProcessor : TransformProcessor
    {
        /// <summary>
        /// The input factory.
        /// </summary>
        private readonly IInputFactory inputFactory;

        /// <summary>
        /// The json transformer.
        /// </summary>
        private readonly IJsonTransformer jsonTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonTransformProcessor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="inputFactory">The input factory.</param>
        /// <param name="dataService">The data service.</param>
        /// <param name="jsonTransformer">The json transformer.</param>
        public JsonTransformProcessor(
            ITrueLogger<JsonTransformProcessor> logger,
            IInputFactory inputFactory,
            IDataService dataService,
            IJsonTransformer jsonTransformer)
            : base(logger, inputFactory, dataService)
        {
            this.inputFactory = inputFactory;
            this.jsonTransformer = jsonTransformer;
        }

        /// <inheritdoc/>
        public override InputType InputType => InputType.JSON;

        /// <inheritdoc/>
        protected override async Task<JArray> DoTransformAsync(TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            JToken result;
            var data = await this.inputFactory.GetJsonInputAsync(message.InputBlobPath).ConfigureAwait(false);
            result = await this.jsonTransformer.TransformJsonAsync(data, message).ConfigureAwait(false);

            if (message.IsRetry)
            {
                result = new JArray { JObject.FromObject(result) };
            }

            return JArray.Parse(result.ToString());
        }
    }
}