// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionStruct.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Entities
{
    using Nethereum.ABI.FunctionEncoding.Attributes;

    /// <summary>
    /// The Node Connection Struct DTO.
    /// </summary>
    [FunctionOutput]
    public class NodeConnectionStruct
    {
        /// <summary>
        /// Gets or sets the node connection identifier.
        /// </summary>
        /// <value>
        /// The node connection identifier.
        /// </value>
        [Parameter("string", "NodeConnectionId", 1)]
        public string NodeConnectionId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Parameter("bool", "IsActive", 2)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the source node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        [Parameter("int64", "SourceNodeId", 3)]
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination node identifier.
        /// </summary>
        /// <value>
        /// The destination node identifier.
        /// </value>
        [Parameter("int64", "DestinationNodeId", 4)]
        public int DestinationNodeId { get; set; }
    }
}