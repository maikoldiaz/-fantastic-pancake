// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelTransformer.cs" company="Microsoft">
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
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Services.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The excel transformer.
    /// </summary>
    public class ExcelTransformer : IExcelTransformer
    {
        /// <summary>
        /// The movement transformer.
        /// </summary>
        private readonly IExcelMovementTransformer movementTransformer;

        /// <summary>
        /// The inventory transformer.
        /// </summary>
        private readonly IExcelInventoryTransformer inventoryTransformer;

        /// <summary>
        /// The excel event transformer.
        /// </summary>
        private readonly IExcelEventTransformer excelEventTransformer;

        /// <summary>
        /// The excel event transformer.
        /// </summary>
        private readonly IExcelContractTransformer excelContractTransformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelTransformer"/> class.
        /// </summary>
        /// <param name="movementTransformer">The movement transformer.</param>
        /// <param name="inventoryTransformer">The inventory transformer.</param>
        /// <param name="excelEventTransformer">The excel event transformer.</param>
        /// <param name="excelContractTransformer">The excel contract transformer.</param>
        public ExcelTransformer(
            IExcelMovementTransformer movementTransformer,
            IExcelInventoryTransformer inventoryTransformer,
            IExcelEventTransformer excelEventTransformer,
            IExcelContractTransformer excelContractTransformer)
        {
            this.movementTransformer = movementTransformer;
            this.inventoryTransformer = inventoryTransformer;
            this.excelEventTransformer = excelEventTransformer;
            this.excelContractTransformer = excelContractTransformer;
        }

        /// <inheritdoc/>
        public async Task<JArray> TransformExcelAsync(ExcelInput message)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            var entities = new ConcurrentBag<JObject>();
            if (message.Message.SourceSystem != SystemType.EVENTS && message.Message.SourceSystem != SystemType.CONTRACT)
            {
                var tasks = new List<Task>
                {
                    DoTransformAsync(entities, "Movements", this.movementTransformer.TransformMovementAsync, message),
                    DoTransformAsync(entities, "Inventory", this.inventoryTransformer.TransformInventoryAsync, message),
                };

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            else if (message.Message.SourceSystem == SystemType.EVENTS)
            {
                await DoTransformAsync(entities, "Events", this.excelEventTransformer.TransformEventAsync, message).ConfigureAwait(false);
            }
            else
            {
                await DoTransformAsync(entities, "Contracts", this.excelContractTransformer.TransformContractAsync, message).ConfigureAwait(false);
            }

            return new JArray(entities);
        }

        private static async Task DoTransformAsync(ConcurrentBag<JObject> entities, string key, Func<ExcelInput, Task<JArray>> builder, ExcelInput message)
        {
            var array = await builder(message).ConfigureAwait(false);
            entities.Add(new JObject
            {
                { key, array },
            });
        }
    }
}
