// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OriginalMovement.cs" company="Microsoft">
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
    /// The OriginalMovement.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Query.QueryEntity" />
    public class OriginalMovement : QueryEntity
    {
        /// <summary>
        /// Gets or sets the movement identifier.
        /// </summary>
        /// <value>
        /// The movement identifier.
        /// </value>mo
        public string MovementId { get; set; }

        /// <summary>
        /// Gets or sets the movement transaction identifier.
        /// </summary>
        /// <value>
        /// The movement transaction identifier.
        /// </value>
        public int MovementTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        public decimal NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the type of the cancellation.
        /// </summary>
        /// <value>
        /// The type of the cancellation.
        /// </value>
        public string CancellationType { get; set; }
    }
}
