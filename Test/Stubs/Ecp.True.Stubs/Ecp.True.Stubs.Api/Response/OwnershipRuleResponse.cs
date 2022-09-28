// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRuleResponse.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Stubs.Api.Response
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// The Volume Output class.
    /// </summary>
    [DisplayName("VolOutput")]
    public class OwnershipRuleResponse
    {
        /// <summary>
        /// Gets or sets the list of Movement Results.
        /// </summary>
        /// <value>
        /// Return the list of Movement Results.
        /// </value>
        [JsonProperty("AuditedSteps")]
        public IEnumerable<AuditedStep> AuditedSteps { get; set; }

        /// <summary>
        /// Gets or sets the list of calculated variables.
        /// </summary>
        /// <value>
        /// Return the list of calculated variables.
        /// </value>
        [JsonProperty("CalculatedVars")]
        public IEnumerable<CalculatedVariable> CalculatedVars { get; set; }

        /// <summary>
        /// Gets or sets the list of ownership rule connections.
        /// </summary>
        /// <value>
        /// Return the list of ownership rule connections.
        /// </value>
        [JsonProperty("EstrategiaConexiones")]
        public IEnumerable<OwnershipRule> OwnershipRuleConnections { get; set; }

        /// <summary>
        /// Gets or sets the list of node ownership rule.
        /// </summary>
        /// <value>
        /// Return the list of node ownership rule.
        /// </value>
        [JsonProperty("EstrategiaPropiedadNodo")]
        public IEnumerable<OwnershipRule> NodeOwnershipRules { get; set; }

        /// <summary>
        /// Gets or sets the list of Movement Results.
        /// </summary>
        /// <value>
        /// Return the list of Movement Results.
        /// </value>
        [JsonProperty("ResultadoMovimientos")]
        public IEnumerable<OwnershipResultMovement> MovementResults { get; set; }

        /// <summary>
        /// Gets or sets the list of Inventory Results.
        /// </summary>
        /// <value>
        /// Return the list of Inventory Results.
        /// </value>
        [JsonProperty("ResultadoInventarios")]
        public IEnumerable<OwnershipResultInventory> InventoryResults { get; set; }

        /// <summary>
        /// Gets or sets the list of Movement Errors.
        /// </summary>
        /// <value>
        /// Return the list of Movement Errors.
        /// </value>
        [JsonProperty("MovimientosErrores")]
        public IEnumerable<OwnershipErrorMovement> MovementErrors { get; set; }

        /// <summary>
        /// Gets or sets the list of Inventory Results.
        /// </summary>
        /// <value>
        /// Return the list of Inventory Results.
        /// </value>
        [JsonProperty("InventariosErrores")]
        public IEnumerable<OwnershipErrorInventory> InventoryErrors { get; set; }

        /// <summary>
        /// Gets or sets the list of node product ownership rule.
        /// </summary>
        /// <value>
        /// Return the list of node product ownership rule.
        /// </value>
        [JsonProperty("EstrategiaPropiedadNodoProducto")]
        public IEnumerable<OwnershipRule> NodeProductOwnershipRules { get; set; }

        /// <summary>
        /// Gets or sets the list of New Movements.
        /// </summary>
        /// <value>
        /// Return the list of New Movement.
        /// </value>
        [JsonProperty("MovimientosNuevos")]
        public IEnumerable<NewMovement> NewMovements { get; set; }

        /// <summary>
        /// Gets or sets the list of Commercial Movements Result.
        /// </summary>
        /// <value>
        /// Return the list of Commercial Movements Result.
        /// </value>
        [JsonProperty("resultadoMovimientosComerciales")]
        public IEnumerable<CommercialMovementResult> CommercialMovementsResults { get; set; }
    }
}
