// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationNodesResult.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    /// <summary>
    /// The ConciliationNodesResult DTO.
    /// </summary>
    public class ConciliationNodesResult : QueryEntity
    {
        /// <summary>
        /// Gets or sets a value NodeConnectionId.
        /// </summary>
        /// <value>
        /// The NodeConnectionId.
        /// </value>
        public int NodeConnectionId { get; set; }

        /// <summary>
        /// Gets or sets a value Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value SourceSegment.
        /// </summary>
        /// <value>
        /// The SourceSegment.
        /// </value>
        public int SourceSegmentId { get; set; }

        /// <summary>
        /// Gets or sets a value DestinationSegment.
        /// </summary>
        /// <value>
        /// The DestinationSegment.
        /// </value>
        public int DestinationSegmentId { get; set; }

        /// <summary>
        /// Gets or sets a value SourceNodeId.
        /// </summary>
        /// <value>
        /// The SourceNodeId.
        /// </value>
        public int SourceNodeId { get; set; }

        /// <summary>
        /// Gets or sets a value DestinationNodeId.
        /// </summary>
        /// <value>
        /// The DestinationNodeId.
        /// </value>
        public int DestinationNodeId { get; set; }

        /// <summary>
        /// Gets or sets a value SourceNodeName.
        /// </summary>
        /// <value>
        /// The SourceNodeName.
        /// </value>
        public string SourceNodeName { get; set; }

        /// <summary>
        /// Gets or sets a value DestinationNodeName.
        /// </summary>
        /// <value>
        /// The DestinationNodeName.
        /// </value>
        public string DestinationNodeName { get; set; }

        /// <summary>
        /// Gets or sets a value SourceSegmentName.
        /// </summary>
        /// <value>
        /// The SourceSegmentName.
        /// </value>
        public string SourceSegmentName { get; set; }

        /// <summary>
        /// Gets or sets a value DestinationSegmentName.
        /// </summary>
        /// <value>
        /// The DestinationSegmentName.
        /// </value>
        public string DestinationSegmentName { get; set; }
    }
}
