// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeStorage.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// -

namespace Ecp.True.Entities.Query
{
    /// <summary>
    /// The GenericLogisticsMovement.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    public class NodeStorage : QueryEntity
    {
        /// <summary>
        /// Gets or sets the LogisticCenter Id identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public string LogisticCenterId { get; set; }

        /// <summary>
        /// Gets or sets the SapStorage.
        /// </summary>
        /// <value>
        /// The SapStorage.
        /// </value>
        public string SapStorage { get; set; }

        /// <summary>
        /// Gets or sets the Product  identifier.
        /// </summary>
        /// <value>
        /// The source Product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the Node identifier.
        /// </summary>
        /// <value>
        /// The source node identifier.
        /// </value>
        public int NodeId { get; set; }
    }
}
