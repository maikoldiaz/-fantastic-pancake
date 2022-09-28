// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelMovementTransformer.cs" company="Microsoft">
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
    /// The movement transformer.
    /// </summary>
    public class ExcelMovementTransformer : IExcelMovementTransformer
    {
        /// <summary>
        /// The table name.
        /// </summary>
        private const string TableName = "MOVIMIENTOS";

        /////// <summary>
        /////// The movement ID.
        /////// </summary>
        ////private readonly string movementId = "MovementId";

        /// <summary>
        /// The movement builder.
        /// </summary>
        private readonly IExcelMovementBuilder movementBuilder;

        /// <summary>
        /// The error movement builder.
        /// </summary>
        private readonly IExcelErrorMovementBuilder errorMovementBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelMovementTransformer"/> class.
        /// </summary>
        /// <param name="movementBuilder">The movement builder.</param>
        /// <param name="errorMovementBuilder">The error movement builder. </param>
        public ExcelMovementTransformer(IExcelMovementBuilder movementBuilder, IExcelErrorMovementBuilder errorMovementBuilder)
        {
            this.movementBuilder = movementBuilder;
            this.errorMovementBuilder = errorMovementBuilder;
        }

        /// <inheritdoc/>
        public async Task<JArray> TransformMovementAsync(ExcelInput message, OwnershipExcelType excelType)
        {
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            if (!message.Input.Tables.Contains(TableName))
            {
                message.Message.AddError(message.Message.MessageId, "Movement Sheet Missing");
                return new JArray();
            }

            var result = await this.TryBuildMovementAsync(message, excelType).ConfigureAwait(false);

            return result;
        }

        private async Task<JArray> TryBuildMovementAsync(ExcelInput message, OwnershipExcelType excelType)
        {
            try
            {
                //// var input = new ExcelInput(Tuple.Create(element, message.Message.ActionType, true), message.Input);
                JArray movements = null;
                if (excelType == OwnershipExcelType.RESULTEXCEL)
                {
                    movements = await this.movementBuilder.BuildAsync(message).ConfigureAwait(false);
                }

                if (excelType == OwnershipExcelType.ERROREXCEL)
                {
                    movements = await this.errorMovementBuilder.BuildAsync(message).ConfigureAwait(false);
                }

                return movements;
            }
            catch (Exception ex)
            {
                message.Message.AddError(message.Message.MessageId, ex.Message);
                throw;
            }
        }
    }
}
