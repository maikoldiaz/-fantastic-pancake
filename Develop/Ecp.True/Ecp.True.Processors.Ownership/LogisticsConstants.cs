// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsConstants.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership
{
    /// <summary>
    /// Constants.
    /// </summary>
    public static class LogisticsConstants
    {
        /// <summary>
        /// The material to material.
        /// </summary>
        public const string MaterialToMaterial = "226";

        /// <summary>
        /// The move ce to ce.
        /// </summary>
        public const string MoveCEToCE = "222";

        /// <summary>
        /// The warehouse to warehouse.
        /// </summary>
        public const string WarehouseToWarehouse = "223";

        /// <summary>
        /// The cancellation material to material.
        /// </summary>
        public const string CancellationMaterialToMaterial = "224";

        /// <summary>
        /// The cancellation move ce to ce.
        /// </summary>
        public const string CancellationMoveCEToCE = "225";

        /// <summary>
        /// The cancellation warehouse to warehouse.
        /// </summary>
        public const string CancellationWarehouseToWarehouse = "221";

        /// <summary>
        /// The tautology error.
        /// </summary>
        public const string TautologyError = "La combinación de producto, centro logístico y almacén es inválida y no está considerada para envío a {0}.";

        /// <summary>
        /// The homologation error.
        /// </summary>
        public const string HomologationError = "No se encontró una homologación entre TRUE y {0} para el tipo de movimiento {1}";

        /// <summary>
        /// The no data error.
        /// </summary>
        public const string NoDataError = "No hay información logística {0} para los criterios dados.";

        /// <summary>
        /// The report name.
        /// </summary>
        public const string ReportName = "ReporteLogisticoOficial";

        /// <summary>
        /// The system type.
        /// </summary>
        public const string SystemTypeError = "El tipo de sistema es invalido";

        /// <summary>
        /// The CostCenterError.
        /// </summary>
        public const string CostCenterError = "El nodo origen {0}, nodo destino {1} y tipo de movimiento {2} no tiene configurado un centro de costos";

        /// <summary>
        /// The GmCodeError.
        /// </summary>
        public const string GmCodeError = "El tipo de movimiento {0} no tiene configurado el GM_Code";

        /// <summary>
        /// The NodeErrorSendToSap.
        /// </summary>
        public const string NodeErrorSendToSap = "El nodo aprobado {0} no tiene configurado el envío a SAP";

        /// <summary>
        /// The Oficial.
        /// </summary>
        public const string Oficial = "oficial";

        /// <summary>
        /// The Operative.
        /// </summary>
        public const string Operative = "operativo";
    }
}
