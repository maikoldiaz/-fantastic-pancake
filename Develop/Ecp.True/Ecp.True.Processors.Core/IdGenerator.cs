// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdGenerator.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Newtonsoft.Json.Linq;
    using Constants = Ecp.True.Core.Constants;

    /// <summary>
    /// The IdGenerator.
    /// </summary>
    public static class IdGenerator
    {
        /// <summary>
        /// The inventory identifier key.
        /// </summary>
        private const string InventoryIdKey = "IdInventario";

        /// <summary>
        /// The node identifier key.
        /// </summary>
        private const string NodeIdKey = "IdNodo";

        /// <summary>
        /// The product key.
        /// </summary>
        private const string ProductKey = "Producto";

        /// <summary>
        /// The batch identifier key.
        /// </summary>
        private const string BatchIdKey = "BatchId";

        /// <summary>
        /// The tank name identifier key.
        /// </summary>
        private const string TankNameKey = "Tanque";

        /// <summary>
        /// The inventory date identifier key.
        /// </summary>
        private const string InventoryDateKey = "FechaInventario";

        /// <summary>
        /// The hash key length.
        /// </summary>
        private const int HashKeyLength = 50;

        /// <summary>
        /// Generates the inventory product unique identifier.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns Inventory Product Unique Identifier.</returns>
        public static string GenerateInventoryProductUniqueId(JToken input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            var inventoryId = input.GetOrDefault(Constants.InventoryId, string.Empty);
            if (string.IsNullOrEmpty(inventoryId))
            {
                throw new MissingFieldException(Entities.Constants.InventoryIdRequired);
            }

            if (input.HasStringValue(Constants.Scenario, Convert.ToString((int)ScenarioType.OFFICER, CultureInfo.InvariantCulture)))
            {
                return string.Concat(inventoryId, Constants.Inventory).GetHash(HashKeyLength);
            }

            var builder = new StringBuilder();
            builder.Append(inventoryId);
            builder.Append(input.GetOrDefault(Constants.NodeId, string.Empty));
            builder.Append(input.GetOrDefault(Constants.ProductId, string.Empty));
            builder.Append(GetDate(Convert.ToString(input[Constants.InventoryDate], CultureInfo.InvariantCulture)));
            builder.Append(input.GetOrDefault(Constants.BatchId, string.Empty));
            builder.Append(input.GetOrDefault(Constants.TankName, string.Empty));

            return builder.ToString().GetHash(HashKeyLength);
        }

        /// <summary>
        /// Generates the inventory product unique identifier.
        /// </summary>
        /// <param name="dataRow">The input.</param>
        /// <returns>Returns Inventory Product Unique Identifier.</returns>
        public static string GenerateInventoryProductUniqueId(DataRow dataRow)
        {
            ArgumentValidators.ThrowIfNull(dataRow, nameof(dataRow));

            var inventoryId = GetValueIfNotNull(dataRow, InventoryIdKey);
            if (string.IsNullOrEmpty(inventoryId))
            {
                throw new MissingFieldException(Entities.Constants.InventoryIdRequired);
            }

            var builder = new StringBuilder();
            builder.Append(inventoryId);
            builder.Append(GetValueIfNotNull(dataRow, NodeIdKey));
            builder.Append(GetValueIfNotNull(dataRow, ProductKey));
            builder.Append(GetDate(GetValueIfNotNull(dataRow, InventoryDateKey)));
            builder.Append(GetValueIfNotNull(dataRow, BatchIdKey));
            builder.Append(GetValueIfNotNull(dataRow, TankNameKey));

            return builder.ToString().GetHash(HashKeyLength);
        }

        /// <summary>
        /// Generates the movement unique identifier.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns Movement Unique Identifier.</returns>
        public static string GenerateMovementId(JToken input)
        {
            var movementId = input.GetOrDefault(Constants.MovementId, string.Empty);
            if (string.IsNullOrEmpty(movementId))
            {
                throw new MissingFieldException(Entities.Constants.MovementIdentifierRequired);
            }

            var result = input.HasStringValue(Constants.Scenario, Convert.ToString((int)ScenarioType.OFFICER, CultureInfo.InvariantCulture)) ?
                string.Concat(movementId, Constants.Movement).GetHash(HashKeyLength) : movementId;

            if (result.Length > 50)
            {
                throw new InvalidDataException(Entities.Constants.MovementIdentifierLengthExceeded);
            }

            return result;
        }

        /// <summary>
        /// Generates the unique identifier.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns>Returns Unique Identifier.</returns>
        public static string GenerateEventId(DataRow row)
        {
            ArgumentValidators.ThrowIfNull(row, nameof(row));
            var sourceNodeId = GetValueIfNotNull(row, "NodoOrigen");
            if (string.IsNullOrEmpty(sourceNodeId))
            {
                throw new MissingFieldException(Entities.Constants.SourceNodeIdRequired);
            }

            var builder = new StringBuilder();
            builder.Append(GetValueIfNotNull(row, "NodoOrigen"));
            builder.Append(GetValueIfNotNull(row, "NodoDestino"));
            builder.Append(GetValueIfNotNull(row, "ProductoOrigen"));
            builder.Append(GetValueIfNotNull(row, "ProductoDestino"));
            builder.Append(GetValueIfNotNull(row, "Unidad"));

            return builder.ToString().GetHash(HashKeyLength);
        }

        /// <summary>
        /// Generates the unique identifier.
        /// </summary>
        /// <param name="messageId">The messageId.</param>
        /// <param name="positionId">The positionId.</param>
        /// <returns>Returns Unique Identifier.</returns>
        public static string GenerateSalePurchaseUniqueId(string messageId, int positionId)
        {
            if (string.IsNullOrEmpty(messageId))
            {
                throw new MissingFieldException(Entities.Constants.SourceNodeIdRequired);
            }

            var builder = new StringBuilder();
            builder.Append(messageId);
            builder.Append(positionId);

            return builder.ToString().GetHash(HashKeyLength);
        }

        /// <summary>
        /// Generates the inventory product unique identifier.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="message">The message.</param>
        /// <returns>Returns Purchase Unique Identifier.</returns>
        public static string GenerateSalePurchaseId(JToken input, TrueMessage message)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            ArgumentValidators.ThrowIfNull(message, nameof(message));

            StringBuilder builder = new StringBuilder();

            builder.Append(input["SessionId"]);

            return builder.ToString().GetHash(HashKeyLength);
        }

        /// <summary>
        /// Generates the unique identifier.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns>Returns Unique Identifier.</returns>
        public static string GenerateContractId(DataRow row)
        {
            ArgumentValidators.ThrowIfNull(row, nameof(row));
            var sourceNodeId = GetValueIfNotNull(row, "NodoOrigen");
            if (string.IsNullOrEmpty(sourceNodeId))
            {
                throw new MissingFieldException(Entities.Constants.SourceNodeIdRequired);
            }

            var builder = new StringBuilder();
            builder.Append(GetValueIfNotNull(row, "Tipo"));
            builder.Append(GetValueIfNotNull(row, "NodoOrigen"));
            builder.Append(GetValueIfNotNull(row, "NodoDestino"));
            builder.Append(GetValueIfNotNull(row, "Producto"));
            builder.Append(GetDate(GetValueIfNotNull(row, "FechaInicio")));
            builder.Append(GetDate(GetValueIfNotNull(row, "FechaFin")));

            return builder.ToString().GetHash(HashKeyLength);
        }

        /// <summary>
        /// Generates the repor identifier.
        /// </summary>
        /// <param name="execution">The report execution.</param>
        /// <returns>The String.</returns>
        public static string GenerateReportHash(ReportExecution execution)
        {
            ArgumentValidators.ThrowIfNull(execution, nameof(execution));

            var builder = new StringBuilder();
            builder.Append(execution.ReportTypeId);
            builder.Append(execution.CategoryId);
            builder.Append(execution.ElementId);
            builder.Append(execution.NodeId);
            builder.Append(execution.StartDate.Date);
            builder.Append(execution.EndDate.Date);
            builder.Append(execution.Name);
            builder.Append(execution.ScenarioId);

            return builder.ToString().GetHash(HashKeyLength);
        }

        private static string GetValueIfNotNull(DataRow dr, string key)
        {
            return dr[key] != null ? dr[key].ToString() : string.Empty;
        }

        private static string GetDate(string dateTime)
        {
            return string.IsNullOrEmpty(dateTime) ?
                string.Empty :
                DateTime.Parse(dateTime, CultureInfo.InvariantCulture).ToString("ddMMyyyy", CultureInfo.InvariantCulture);
        }
    }
}