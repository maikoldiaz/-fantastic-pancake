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

namespace Ecp.True.Processors.Ownership.Transform.Services.Excel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Ownership.Input.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The excel transformer.
    /// </summary>
    public class ExcelTransformer : IExcelTransformer
    {
        /// <summary>
        /// The movement transformer.
        /// </summary>
        private readonly string invalidFileStructure = "Invalid file structure, number of sheets is not equal to 2";

        /// <summary>
        /// The movement transformer.
        /// </summary>
        private readonly IExcelMovementTransformer movementTransformer;

        /// <summary>
        /// The inventory transformer.
        /// </summary>
        private readonly IExcelInventoryTransformer inventoryTransformer;

        /// <summary>
        /// The input factory.
        /// </summary>
        private readonly IOwnershipInputFactory ownershipInputFactory;

        /// <summary>Initializes a new instance of the <see cref="ExcelTransformer"/> class.</summary>
        /// <param name="movementTransformer">The movement transformer.</param>
        /// <param name="inventoryTransformer">The inventory transformer.</param>
        /// <param name="ownershipInputFactory">The ownership input factory.</param>
        public ExcelTransformer(
            IExcelMovementTransformer movementTransformer,
            IExcelInventoryTransformer inventoryTransformer,
            IOwnershipInputFactory ownershipInputFactory)
        {
            this.movementTransformer = movementTransformer;
            this.inventoryTransformer = inventoryTransformer;
            this.ownershipInputFactory = ownershipInputFactory;
        }

        /// <inheritdoc/>
        public async Task<JObject> TransformExcelAsync(TrueMessage message, Stream stream, OwnershipExcelType excelType)
        {
            var excelInput = this.ownershipInputFactory.GetExcelInput(message, stream);
            if (excelInput.Input.Tables.Count != 2)
            {
                throw new InvalidDataException(this.invalidFileStructure);
            }

            var entities = new JObject();

            var tasks = new List<Task>();
            tasks.Add(DoTransformAsync(entities, "Movements", excelType, this.movementTransformer.TransformMovementAsync, excelInput));
            tasks.Add(DoTransformAsync(entities, "Inventories", excelType, this.inventoryTransformer.TransformInventoryAsync, excelInput));

            await Task.WhenAll(tasks).ConfigureAwait(false);
            return entities;
        }

        private static async Task DoTransformAsync(
            IDictionary<string, JToken> entities,
            string key,
            OwnershipExcelType excelType,
            Func<ExcelInput, OwnershipExcelType, Task<JArray>> builder,
            ExcelInput message)
        {
            var array = await builder(message, excelType).ConfigureAwait(false);
            entities.Add(key, array);
        }
    }
}
