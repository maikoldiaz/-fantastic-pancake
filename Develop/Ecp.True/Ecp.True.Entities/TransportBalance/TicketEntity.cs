// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketEntity.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.Processors.Api.Tests")]

namespace Ecp.True.Entities.TransportBalance
{
    using System;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// Ticket Entity.
    /// </summary>
    public class TicketEntity : Entity
    {
        /// <summary>
        /// Gets or sets the ticket identifier.
        /// </summary>
        /// <value>
        /// The ticket identifier.
        /// </value>
        public int TicketId { get; set; }

        /// <summary>
        /// Gets or sets the ticket type identifier.
        /// </summary>
        /// <value>
        /// The ticket type identifier.
        /// </value>
        public TicketType TicketTypeId { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public string Segment { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the ticket start date.
        /// </summary>
        /// <value>
        /// The ticket start date.
        /// </value>
        public DateTime TicketStartDate { get; set; }

        /// <summary>
        /// Gets or sets the ticket final date.
        /// </summary>
        /// <value>
        /// The ticket final date.
        /// </value>
        public DateTime TicketFinalDate { get; set; }

        /// <summary>
        /// Gets or sets the cutoff execution date.
        /// </summary>
        /// <value>
        /// The cutoff execution date.
        /// </value>
        public DateTime CutoffExecutionDate { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the name of the owner.
        /// </summary>
        /// <value>
        /// The name of the owner.
        /// </value>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the BLOB path.
        /// </summary>
        /// <value>
        /// The BLOB path.
        /// </value>
        public string BlobPath { get; set; }

        /// <summary>
        /// Gets or sets the node name.
        /// </summary>
        /// <value>
        /// The node name.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the scenario name.
        /// </summary>
        /// <value>
        /// The scenario name.
        /// </value>
        public string ScenarioName { get; set; }
    }
}
