// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticalServiceRequestData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Calculation.Request
{
    /// <summary>
    /// Analytical movement operational data.
    /// </summary>
    public class AnalyticalServiceRequestData
    {
        /// <summary>
        /// Gets or sets the algorithm identifier.
        /// </summary>
        /// <value>
        /// The algorithm identifier.
        /// </value>
        public string AlgorithmId { get; set; }

        /// <summary>
        /// Gets or sets the movement type name.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>
        public string MovementType { get; set; }

        /// <summary>
        /// Gets or sets the name of the source node.
        /// </summary>
        /// <value>
        /// The name of the source node.
        /// </value>
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the name of the source node type.
        /// </summary>
        /// <value>
        /// The name of the source node type.
        /// </value>
        public string SourceNodeType { get; set; }

        /// <summary>
        /// Gets or sets the name of the destintion node.
        /// </summary>
        /// <value>
        /// The name of the destintion node.
        /// </value>
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the name of the destintion type node.
        /// </summary>
        /// <value>
        /// The name of the destintion type node.
        /// </value>
        public string DestinationNodeType { get; set; }

        /// <summary>
        /// Gets or sets the name of the source product.
        /// </summary>
        /// <value>
        /// The name of the source product.
        /// </value>
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the name of the source product type.
        /// </summary>
        /// <value>
        /// The name of the source product type.
        /// </value>
        public string SourceProductType { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public string EndDate { get; set; }
    }
}
