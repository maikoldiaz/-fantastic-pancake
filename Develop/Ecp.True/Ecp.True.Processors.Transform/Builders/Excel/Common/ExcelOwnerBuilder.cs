// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelOwnerBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Builders.Excel.Movement
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Common;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The owner builder.
    /// </summary>
    public class ExcelOwnerBuilder : ExcelBuilder, IExcelOwnerBuilder
    {
        /// <summary>
        /// The table name.
        /// </summary>
        private const string MovementTableName = "PROPIETARIOSMOV";

        /// <summary>
        /// The table name.
        /// </summary>
        private const string InventoryTableName = "PROPIETARIOSINV";

        /// <inheritdoc/>
        public override Task<JObject> BuildAsync(ExcelInput element)
        {
            ArgumentValidators.ThrowIfNull(element, nameof(element));

            var args = (Tuple<DataRow, string, bool>)element.Arguments;
            var dr = args.Item1;

            var owners = Validate(args.Item3, dr);
            if (owners != null)
            {
                return Task.FromResult(owners);
            }

            return BuildOwnersAsync(dr, element.Input, args.Item3);
        }

        private static JObject Validate(bool isMovement, DataRow dr)
        {
            if (isMovement && !dr.Table.Columns.Contains(Constants.MovementIdKey))
            {
                return new JObject
                {
                    { "Owners", new JArray() },
                };
            }

            if (!isMovement &&
                (!dr.Table.Columns.Contains(Constants.InventoryIdKey) ||
                !dr.Table.Columns.Contains(Constants.NodeIdKey) ||
                !dr.Table.Columns.Contains(Constants.ProductKey)))
            {
                return new JObject
                {
                    { "Owners", new JArray() },
                };
            }

            return default(JObject);
        }

        private static Task<JObject> BuildOwnersAsync(DataRow parentRow, DataSet dataset, bool isMovement)
        {
            var tableName = isMovement ? MovementTableName : InventoryTableName;
            if (!dataset.Tables.Contains(tableName))
            {
                return Task.FromResult(new JObject
                {
                    { "Owners", new JArray() },
                });
            }

            var rows = dataset.Tables[tableName].AsEnumerable();
            var owners = new JArray();

            var collection = isMovement ? rows.Where(r => r[Constants.MovementIdKey].ToString() == parentRow[Constants.MovementIdKey].ToString()) :
                                               rows.Where(r => IdGenerator.GenerateInventoryProductUniqueId(r).EqualsIgnoreCase(IdGenerator.GenerateInventoryProductUniqueId(parentRow)));
            collection.ForEach(r =>
            {
                if (!isMovement && r[Constants.InventoryDateKey] != null && !r[Constants.InventoryDateKey].IsDate())
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Constants.IncorrectDatatypeFailureMessage, r[Constants.InventoryDateKey]));
                }

                var owner = new JObject
                {
                    new JProperty("OwnerId", GetValue(r, "IdPropietario")),
                    GetValue(r, "ValorPropiedad", "OwnershipValue", JTokenType.Float),
                    new JProperty("OwnershipValueUnit", GetValue(r, "UnidadValorPropiedad")),
                };
                owners.Add(owner);
            });

            return Task.FromResult(new JObject
            {
                { "Owners", owners },
            });
        }
    }
}
