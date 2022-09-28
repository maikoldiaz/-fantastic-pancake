// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform
{
    /// <summary>
    /// The Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Gets the net liquid volume.
        /// </summary>
        /// <value>
        /// The net volume liquid.
        /// </value>
        public const string NetLiquidVolume = "Neto-Volumen Líquido";

        /// <summary>
        /// Gets the gross liquid volume.
        /// </summary>
        /// <value>
        /// The gross volume liquid.
        /// </value>
        public const string GrossLiquidVolume = "Volumen Bruto Líquido";

        /// <summary>
        /// The destination product.
        /// </summary>
        public const string DestinationProduct = "Producto Destino";

        /// <summary>
        /// Gets the value not found message.
        /// </summary>
        /// <value>
        /// The value not found message.
        /// </value>
        public const string ValueNotFound = "{0} : no tiene un mapeo de datos para realizar la homologación. Tipo: {1}";

        /// <summary>
        /// The error message when more than 2000 records found in SAP PO entity.
        /// </summary>
        public const string MoreThan2KRecordsFound = "Solo se admiten hasta 2000 registros por llamada";

        /// <summary>
        /// The products.
        /// </summary>
        public const string Products = "Products";

        /// <summary>
        /// The inventory.
        /// </summary>
        public const string Inventory = "Inventory";

        /// <summary>
        /// The inventory product unique identifier.
        /// </summary>
        public const string InventoryProductUniqueId = "InventoryProductUniqueId";

        /// <summary>
        /// Gets the homologation not configured message.
        /// </summary>
        /// <value>
        /// The value not found message.
        /// </value>
        public const string HomologationNotConfigured = "No existe una homologación configurada entre {0} : {1}";

        /// <summary>
        /// Gets the value movement type not found message.
        /// </summary>
        /// <value>
        /// The value movement type not found message.
        /// </value>
        public const string ValueNotFoundMovementType = "El tipo solo permite los valores: venta, compra, traslado o autoconsumo";
    }
}