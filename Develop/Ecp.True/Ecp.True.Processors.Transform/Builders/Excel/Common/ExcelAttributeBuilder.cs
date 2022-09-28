// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelAttributeBuilder.cs" company="Microsoft">
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
    /// The attribute builder.
    /// </summary>
    public class ExcelAttributeBuilder : ExcelBuilder, IExcelAttributeBuilder
    {
        /// <summary>
        /// The table name.
        /// </summary>
        private const string MovementTableName = "CALIDADMOV";

        /// <summary>
        /// The table name.
        /// </summary>
        private const string InventoryTableName = "CALIDADINV";

        /// <inheritdoc/>
        public override Task<JObject> BuildAsync(ExcelInput element)
        {
            ArgumentValidators.ThrowIfNull(element, nameof(element));
            var args = (Tuple<DataRow, string, bool>)element.Arguments;
            var dr = args.Item1;

            var attributes = Validate(args.Item3, dr);
            if (attributes != null)
            {
                return Task.FromResult(attributes);
            }

            return BuildAttributesAsync(dr, element.Input, args.Item3);
        }

        private static JObject Validate(bool isMovement, DataRow dr)
        {
            if (isMovement && !dr.Table.Columns.Contains(Constants.MovementIdKey))
            {
                return new JObject
                {
                    { "Attributes", new JArray() },
                };
            }

            if (!isMovement &&
                (!dr.Table.Columns.Contains(Constants.InventoryIdKey) ||
                !dr.Table.Columns.Contains(Constants.NodeIdKey) ||
                !dr.Table.Columns.Contains(Constants.ProductKey)))
            {
                return new JObject
                {
                    { "Attributes", new JArray() },
                };
            }

            return default(JObject);
        }

        private static Task<JObject> BuildAttributesAsync(DataRow parentRow, DataSet dataset, bool isMovement)
        {
            var tableName = isMovement ? MovementTableName : InventoryTableName;
            if (!dataset.Tables.Contains(tableName))
            {
                return Task.FromResult(new JObject
                {
                    { "Attributes", new JArray() },
                });
            }

            var rows = dataset.Tables[tableName].AsEnumerable();
            var attributes = new JArray();

            var collection = isMovement ? rows.Where(r => r[Constants.MovementIdKey].ToString() == parentRow[Constants.MovementIdKey].ToString()) :
                                               rows.Where(r => IdGenerator.GenerateInventoryProductUniqueId(r).EqualsIgnoreCase(IdGenerator.GenerateInventoryProductUniqueId(parentRow)));
            collection.ForEach(r =>
            {
                if (!isMovement && r[Constants.InventoryDateKey] != null && !r[Constants.InventoryDateKey].IsDate())
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Constants.IncorrectDatatypeFailureMessage, r[Constants.InventoryDateKey]));
                }

                var attribute = new JObject
                {
                    new JProperty("AttributeId", GetValue(r, "IdAtributo")),
                    new JProperty("AttributeType", GetValue(r, "TipoAtributo")),
                    GetValue(r, "ValorAtributo", "AttributeValue", JTokenType.Float),
                    new JProperty("ValueAttributeUnit", GetValue(r, "UnidadValorAtributo")),
                    new JProperty("AttributeDescription", GetValue(r, "DescripcionAtributo")),
                };
                attributes.Add(attribute);
            });

            return Task.FromResult(new JObject
            {
                { "Attributes", attributes },
            });
        }
    }
}
