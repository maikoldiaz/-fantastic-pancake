// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelErrorInventoryBuilder.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Builders.Excel
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Ownership.Builders.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The inventory builder.
    /// </summary>
    public class ExcelErrorInventoryBuilder : IExcelErrorInventoryBuilder
    {
        /// <summary>
        /// The execution date.
        /// </summary>
        private const string ExecutionDate = "FechaEjecucion";

        /// <inheritdoc/>
        public Task<JArray> BuildAsync(ExcelInput element)
        {
            ArgumentValidators.ThrowIfNull(element, nameof(element));
            var dataset = element.Input;
            var table = dataset.Tables["Inventarios"];

            var rows = table.AsEnumerable();
            var inventories = new JArray();
            var collection = rows;

            collection.ForEach(r =>
            {
                if (!ValidateColumnNames(r))
                {
                    throw new ArgumentException("One or more incorrect column name");
                }

                if (!r[ExecutionDate].IsDate())
                {
                    throw new ArgumentException("One or more column has incorrect datatype");
                }

                if (r.ItemArray.Any(i => string.IsNullOrWhiteSpace(i.ToString())))
                {
                    throw new ArgumentException("Entry in one or more rows is empty");
                }

                if (r.ItemArray.Length != 5)
                {
                    throw new ArgumentException("Incorrect column count");
                }

                var inventory = new JObject
                {
                    new JProperty("Ticket", Convert.ToInt32(r["Tiquete"], CultureInfo.InvariantCulture)),
                    new JProperty("InventoryId", Convert.ToInt32(r["IdInventario"], CultureInfo.InvariantCulture)),
                    new JProperty("NodeId", Convert.ToInt32(r["IdNodo"], CultureInfo.InvariantCulture)),
                    new JProperty("ErrorDescription", r["DescripcionError"].ToString()),
                    new JProperty("ExecutionDate", r[ExecutionDate]),
                };
                inventories.Add(inventory);
            });

            return Task.FromResult(inventories);
        }

        private static bool ValidateColumnNames(DataRow r)
        {
            List<string> columnNames = new List<string> { "Tiquete", "IdInventario", "IdNodo", "DescripcionError", ExecutionDate };

            foreach (DataColumn c in r.Table.Columns)
            {
                if (!columnNames.Contains(c.ColumnName))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
