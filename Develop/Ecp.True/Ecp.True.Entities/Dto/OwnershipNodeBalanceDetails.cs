// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipNodeBalanceDetails.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// Ownership node balance details.
    /// </summary>
    public class OwnershipNodeBalanceDetails
    {
        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public IEnumerable<OwnershipNodeBalanceSummary> Summary { get; set; }

        /// <summary>
        /// Gets or sets the name of the balance professional user.
        /// </summary>
        /// <value>
        /// The name of the balance professional user.
        /// </value>
        public string BalanceProfessionalUserName { get; set; }

        /// <summary>
        /// Gets or sets the email of the balance professional user.
        /// </summary>
        /// <value>
        /// The email of the balance professional user.
        /// </value>
        public string BalanceProfessionalEmail { get; set; }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public string Segment { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the report URL.
        /// </summary>
        /// <value>
        /// The report URL.
        /// </value>
        public string ReportPath { get; set; }
    }
}
