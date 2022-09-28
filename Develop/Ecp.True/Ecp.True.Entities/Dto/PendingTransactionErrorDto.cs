// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PendingTransactionErrorDto.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The pending transaction error.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Registration.PendingTransactionError" />
    public class PendingTransactionErrorDto : PendingTransactionError
    {
        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public string DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets the Unit Name.
        /// </summary>
        /// <value>
        /// The Unit Name.
        /// </value>
        public string Units { get; set; }

        /// <summary>
        /// Gets or sets the name of the system.
        /// </summary>
        /// <value>
        /// The name of the system.
        /// </value>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the system type identifier.
        /// </summary>
        /// <value>
        /// The system type identifier.
        /// </value>
        public int SystemTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the system type.
        /// </summary>
        /// <value>
        /// The name of the system type.
        /// </value>
        public string SystemTypeName { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        public string ActionType { get; set; }
    }
}
