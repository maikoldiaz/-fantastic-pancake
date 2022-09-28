// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutputBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Balance.Calculation.Output
{
    /// <summary>
    /// The output base.
    /// </summary>
    public class OutputBase : IOutput
    {
        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the initial inventory.
        /// </summary>
        /// <value>
        /// The initial inventory.
        /// </value>
        public decimal InitialInventory { get; set; }

        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        public decimal Inputs { get; set; }

        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        public decimal Outputs { get; set; }

        /// <summary>
        /// Gets or sets the final inventory.
        /// </summary>
        /// <value>
        /// The final inventory.
        /// </value>
        public decimal FinalInventory { get; set; }

        /// <summary>
        /// Gets or sets the identified losses.
        /// </summary>
        /// <value>
        /// The identified losses.
        /// </value>
        public decimal IdentifiedLosses { get; set; }

        /// <summary>
        /// Gets or sets the unbalance.
        /// </summary>
        /// <value>
        /// The unbalance.
        /// </value>
        public decimal Unbalance { get; set; }
    }
}
