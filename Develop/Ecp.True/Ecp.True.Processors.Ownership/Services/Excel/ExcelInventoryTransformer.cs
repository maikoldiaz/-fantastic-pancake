// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelInventoryTransformer.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services.Excel
{
    using System;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Ownership.Builders.Excel.Interfaces;
    using Ecp.True.Processors.Ownership.Services.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The inventory transformer.
    /// </summary>
    public class ExcelInventoryTransformer : IExcelInventoryTransformer
    {
        /// <summary>
        /// The table name.
        /// </summary>
        private const string TableName = "INVENTARIOS";

        /// <summary>
        /// The inventory builder.
        /// </summary>
        private readonly IExcelInventoryBuilder inventoryBuilder;

        /// <summary>
        /// The error inventory builder.
        /// </summary>
        private readonly IExcelErrorInventoryBuilder errorInventoryBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelInventoryTransformer"/> class.
        /// </summary>
        /// <param name="inventoryBuilder">The inventory builder.</param>
        /// <param name="errorInventoryBuilder">The error inventory builder.</param>
        public ExcelInventoryTransformer(IExcelInventoryBuilder inventoryBuilder, IExcelErrorInventoryBuilder errorInventoryBuilder)
        {
            this.inventoryBuilder = inventoryBuilder;
            this.errorInventoryBuilder = errorInventoryBuilder;
        }

        /// <inheritdoc/>
        public async Task<JArray> TransformInventoryAsync(ExcelInput message, OwnershipExcelType excelType)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            if (!message.Input.Tables.Contains(TableName))
            {
                message.Message.AddError(message.Message.MessageId, "Inventory Sheet Missing");
                return new JArray();
            }

            var result = await this.TryBuildInventoryAsync(message, excelType).ConfigureAwait(false);
            return result;
        }

        private async Task<JArray> TryBuildInventoryAsync(ExcelInput message, OwnershipExcelType excelType)
        {
            try
            {
                JArray inventories = null;
                if (excelType == OwnershipExcelType.RESULTEXCEL)
                {
                    inventories = await this.inventoryBuilder.BuildAsync(message).ConfigureAwait(false);
                }

                if (excelType == OwnershipExcelType.ERROREXCEL)
                {
                    inventories = await this.errorInventoryBuilder.BuildAsync(message).ConfigureAwait(false);
                }

                return inventories;
            }
            catch (Exception ex)
            {
                message.Message.AddError(message.Message.MessageId, ex.Message);
                throw;
            }
        }
    }
}
