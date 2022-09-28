// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportExecutionEntity.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The report execution entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class ReportExecutionEntity : Entity
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
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public string NodeName { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string Segment { get; set; }

        /// <summary>
        /// Gets or sets the element identifier.
        /// </summary>
        /// <value>
        /// The element identifier.
        /// </value>
        public int? ElementId { get; set; }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        public string System { get; set; }

        /// <summary>
        /// Gets or sets the report name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the report type identifier.
        /// </summary>
        /// <value>
        /// The report type identifier.
        /// </value>
        public string ReportType { get; set; }

        /// <summary>
        /// Gets or sets the status type identifier.
        /// </summary>
        /// <value>
        /// The status type identifier.
        /// </value>
        public string StatusType { get; set; }

        /// <summary>
        /// Gets or sets the scenario identifier.
        /// </summary>
        /// <value>
        /// The scenario identifier.
        /// </value>
        public string ScenarioType { get; set; }
    }
}
