// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutOffBatch.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The OperationalCutOffBatch.
    /// </summary>
    public class OperationalCutOffBatch
    {
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the segment identifier.
        /// </summary>
        /// <value>
        /// The segment identifier.
        /// </value>
        public string SegmentId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the transfer points.
        /// </summary>
        /// <value>
        /// The transfer points.
        /// </value>
        public IEnumerable<TransferPoints> TransferPoints { get; set; }

        /// <summary>
        /// Gets or sets the unbalances.
        /// </summary>
        /// <value>
        /// The unbalances.
        /// </value>
        public IEnumerable<UnbalanceComment> Unbalances { get; set; }

        /// <summary>
        /// Gets or sets the pending transaction errors list.
        /// </summary>
        /// <value>
        /// The pending transaction errors list.
        /// </value>
        public IEnumerable<PendingTransactionError> Errors { get; set; }
    }
}
