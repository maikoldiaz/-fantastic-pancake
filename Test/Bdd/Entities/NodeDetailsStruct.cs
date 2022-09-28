// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeDetailsStruct.cs" company="Microsoft">
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
    /// The Node Struct DTO.
    /// </summary>
    [FunctionOutput]
    public class NodeDetailsStruct
    {
        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [Parameter("string", "NodeId", 1)]
        public string NodeId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating name of the entity.
        /// </summary>
        /// <value>
        ///    Name of the entity.
        /// </value>
        [Parameter("string", "Name", 2)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the state/status of the node.
        /// </summary>
        /// <value>
        /// The state of the node entity.
        /// </value>
        [Parameter("string", "State", 3)]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the Last Updated Date.
        /// </summary>
        /// <value>
        /// The Last date entity was updated.
        /// </value>
        [Parameter("int64", "LastUpdateDate", 4)]
        public long LastUpdateDate { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Parameter("bool", "IsActive", 5)]
        public bool IsActive { get; set; }
    }
}