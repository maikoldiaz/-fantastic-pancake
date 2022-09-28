// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelContractBuilder.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Transform.Builders.Excel.Common;
    using Ecp.True.Processors.Transform.Builders.Excel.Interfaces;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The movement builder.
    /// </summary>
    public class ExcelContractBuilder : ExcelBuilder, IExcelContractBuilder
    {
        /// <inheritdoc/>
        public override Task<JObject> BuildAsync(ExcelInput element)
        {
            ArgumentValidators.ThrowIfNull(element, nameof(element));
            var args = (Tuple<DataRow, string, bool>)element.Arguments;
            var dr = args.Item1;
            var result = CreateRoot(dr);
            return Task.FromResult(result);
        }

        private static JObject CreateRoot(DataRow row)
        {
            ArgumentValidators.ThrowIfNull(row, nameof(row));

            return new JObject
                {
                    new JProperty("SessionId", IdGenerator.GenerateContractId(row)),
                    new JProperty("SystemName", SystemType.EXCEL.ToString()),
                    new JProperty("DocumentNumber", GetValue(row, "NumeroDocumento")),
                    new JProperty("Position", GetValue(row, "Posicion")),
                    new JProperty("MovementTypeId", GetValue(row, "Tipo")),
                    new JProperty("SourceNodeId", GetValue(row, "NodoOrigen")),
                    new JProperty("DestinationNodeId", GetValue(row, "NodoDestino")),
                    new JProperty("ProductId", GetValue(row, "Producto")),
                    GetValue(row, "FechaInicio", "StartDate", JTokenType.Date),
                    GetValue(row, "FechaFin", "EndDate", JTokenType.Date),
                    new JProperty("Owner1Id", GetValue(row, "Propietario1", true, Entities.Constants.Owner1Requiredvalidation)),
                    new JProperty("Owner2Id", GetValue(row, "Propietario2", true, Entities.Constants.Owner2Requiredvalidation)),
                    GetValue(row, "Valor", "Volume", JTokenType.Float, true),
                    new JProperty("MeasurementUnit", GetValue(row, "Unidad")),
                    new JProperty("PurchaseOrderType", GetValue(row, "TipoOrden")),
                    new JProperty("Status", GetValue(row, "Estado")),
                    GetValue(row, "VolumenPresupuestado", "EstimatedVolume", JTokenType.Float, false, true),
                    GetValue(row, "Porcentajetolerancia", "Tolerance", JTokenType.Float, false, true),
                    new JProperty("Frequency", GetValue(row, "Frecuencia")),
                    new JProperty("SourceSystem", SystemType.EXCEL.ToString()),
                };
        }
    }
}
