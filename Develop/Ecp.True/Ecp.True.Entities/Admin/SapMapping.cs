// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapMapping.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The Sap Node.
    /// </summary>
    public class SapMapping : Entity
    {
        /// <summary>
        /// Gets or sets the sap node identifier.
        /// </summary>
        /// <value>
        /// The sap node identifier.
        /// </value>
        public int SapMappingId { get; set; }

        /// <summary>
        /// Gets or sets the official system.
        /// </summary>
        /// <value>
        /// The official system.
        /// </value>
        public int OfficialSystem { get; set; }

        /// <summary>
        /// Gets or sets the source system identifier.
        /// </summary>
        /// <value>
        /// The source system identifier.
        /// </value>
        public int SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the source movement type identifier.
        /// </summary>
        /// <value>
        /// The source movement type identifier.
        /// </value>
        public int SourceMovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the source product identifier.
        /// </summary>
        /// <value>
        /// The source product identifier.
        /// </value>
        public string SourceProductId { get; set; }

        /// <summary>
        /// Gets or sets the source system source node identifier.
        /// </summary>
        /// <value>
        /// The source system source node identifier.
        /// </value>
        public int SourceSystemSourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the source system source node identifier.
        /// </summary>
        /// <value>
        /// The source system source node identifier.
        /// </value>
        public int SourceSystemDestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination system identifier.
        /// </summary>
        /// <value>
        /// The destination system identifier.
        /// </value>
        public int DestinationSystemId { get; set; }

        /// <summary>
        /// Gets or sets the destination movement type identifier.
        /// </summary>
        /// <value>
        /// The destination movement type identifier.
        /// </value>
        public int DestinationMovementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the destination product identifier.
        /// </summary>
        /// <value>
        /// The destination product identifier.
        /// </value>
        public string DestinationProductId { get; set; }

        /// <summary>
        /// Gets or sets the destination system source node identifier.
        /// </summary>
        /// <value>
        /// The destination system source node identifier.
        /// </value>
        public int DestinationSystemSourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets the destination system source node identifier.
        /// </summary>
        /// <value>
        /// The destination system source node identifier.
        /// </value>
        public int DestinationSystemDestinationNodeId { get; set; }
    }
}
