// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelInventoryBuilder.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Ownership.Builders.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The inventory builder.
    /// </summary>
    public class ExcelInventoryBuilder : IExcelInventoryBuilder
    {
        /// <summary>
        /// The percentage.
        /// </summary>
        private const string Percentage = "PorcentajePropiedad";

        /// <summary>
        /// The volume.
        /// </summary>
        private const string Volume = "VolumenPropiedad";

        /// <summary>
        /// The execution date.
        /// </summary>
        private const string ExecutionDate = "FechaEjecucion";

        /// <inheritdoc/>
        public Task<JArray> BuildAsync(ExcelInput element)
        {
            ArgumentValidators.ThrowIfNull(element, nameof(element));
            var dataset = element.Input;
            var rows = dataset.Tables["Inventarios"].AsEnumerable();
            var inventories = new JArray();
            var collection = rows;
            collection.ForEach(r =>
            {
                if (!ValidateColumnNames(r))
                {
                    throw new ArgumentException("One or more incorrect column name or value");
                }

                if (!ValidateColumnDatatypes(r))
                {
                    throw new ArgumentException("One or more column has incorrect datatype");
                }

                var inventory = new JObject
                {
                    new JProperty("Ticket", Convert.ToInt32(r["Tiquete"], CultureInfo.InvariantCulture)),
                    new JProperty("InventoryId", Convert.ToInt32(r["IdInventario"], CultureInfo.InvariantCulture)),
                    new JProperty("OwnerId", Convert.ToInt32(r["IdPropietario"], CultureInfo.InvariantCulture)),
                    new JProperty("Owner", r["Propietario"].ToString()),
                    new JProperty("OwnershipPercentage", r[Percentage]),
                    new JProperty("OwnershipVolume", r[Volume]),
                    new JProperty("AppliedRule", r["ReglaAplicada"].ToString()),
                    new JProperty("RuleVersion", r["VersionRegla"].ToString()),
                    new JProperty("ExecutionDate", r[ExecutionDate]),
                };
                inventories.Add(inventory);
            });

            return Task.FromResult(inventories);
        }

        private static bool ValidateColumnDatatypes(DataRow r)
        {
            return r[Percentage].IsNumber() &&
                r[Volume].IsNumber() &&
                r[ExecutionDate].IsDate();
        }

        private static bool ValidateColumnNames(DataRow r)
        {
            List<string> columnNames = new List<string>
            {
                "Tiquete",
                "IdInventario",
                "IdPropietario",
                "Propietario",
                Percentage,
                Volume,
                "ReglaAplicada",
                "VersionRegla",
                ExecutionDate,
            };

            foreach (var columnName in columnNames)
            {
                if (!r.Table.Columns.Contains(columnName))
                {
                    return false;
                }
                else
                {
                    var column = r.Table.Columns[columnName];
                    if (string.IsNullOrWhiteSpace(r[column].ToString()))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
