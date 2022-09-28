// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportExecution.cs" company="Microsoft">
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
    using System;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The report execution entity.
    /// </summary>
    public class ReportExecution : Entity
    {
        /// <summary>
        /// Gets or sets the execution identifier.
        /// </summary>
        /// <value>
        /// The execution identifier.
        /// </value>
        public int ExecutionId { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public int? NodeId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        /// <value>
        /// The element identifier.
        /// </value>
        public int? ElementId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required(ErrorMessage = Constants.ReportNameRequired)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>
        /// The hash.
        /// </value>
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Required(ErrorMessage = Constants.EventStartDateIsMandatory)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Required(ErrorMessage = Constants.EventEndDateIsMandatory)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the report type identifier.
        /// </summary>
        /// <value>
        /// The report type identifier.
        /// </value>
        public ReportType ReportTypeId { get; set; }

        /// <summary>
        /// Gets or sets the status type identifier.
        /// </summary>
        /// <value>
        /// The status type identifier.
        /// </value>
        public StatusType StatusTypeId { get; set; }

        /// <summary>
        /// Gets or sets the scenario identifier.
        /// </summary>
        /// <value>
        /// The scenario identifier.
        /// </value>
        public ScenarioType ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public virtual Node Node { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public virtual CategoryElement Element { get; set; }
    }
}
