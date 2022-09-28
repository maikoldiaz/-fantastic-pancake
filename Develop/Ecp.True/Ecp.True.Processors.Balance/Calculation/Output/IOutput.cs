// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOutput.cs" company="Microsoft">
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
    /// The output.
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        int NodeId { get; }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        string ProductId { get; }

        /// <summary>
        /// Gets the initial inventory.
        /// </summary>
        /// <value>
        /// The initial inventory.
        /// </value>
        decimal InitialInventory { get; }

        /// <summary>
        /// Gets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        decimal Inputs { get; }

        /// <summary>
        /// Gets the outputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        decimal Outputs { get; }

        /// <summary>
        /// Gets the final inventory.
        /// </summary>
        /// <value>
        /// The final inventory.
        /// </value>
        decimal FinalInventory { get; }

        /// <summary>
        /// Gets the identified losses.
        /// </summary>
        /// <value>
        /// The identified losses.
        /// </value>
        decimal IdentifiedLosses { get; }

        /// <summary>
        /// Gets the unbalance.
        /// </summary>
        /// <value>
        /// The unbalance.
        /// </value>
        decimal Unbalance { get; }
    }
}
