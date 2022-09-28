// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialNodeStatusReportRequest.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Node configuration report request data.
    /// </summary>
    public class OfficialNodeStatusReportRequest
    {
        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string ElementName { get; set; }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        public string ElementId { get; set; }

        /// <summary>
        /// Gets or sets the execution identifier.
        /// </summary>
        /// <value>
        /// The execution identifier.
        /// </value>
        public string ExecutionId { get; set; }

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
    }
}
