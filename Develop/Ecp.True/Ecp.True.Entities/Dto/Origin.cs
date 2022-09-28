// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Origin.cs" company="Microsoft">
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
    /// <summary>
    /// The Origin.
    /// </summary>
    public class Origin
    {
        /// <summary>Gets or sets the source node.</summary>
        /// <value>The origin source node.</value>
        public int SourceNodeId { get; set; }

        /// <summary>Gets or sets the destination node.</summary>
        /// <value>The destination node.</value>
        public int? DestinationNodeId { get; set; }

        /// <summary>Gets or sets the source product.</summary>
        /// <value>The source product.</value>
        public string SourceProductId { get; set; }

        /// <summary>Gets or sets the destination product.</summary>
        /// <value>The destination product.</value>
        public string DestinationProductId { get; set; }

        /// <summary>Gets or sets the measurement.</summary>
        /// <value>The measurement.</value>
        public int MeasurementUnitId { get; set; }
    }
}