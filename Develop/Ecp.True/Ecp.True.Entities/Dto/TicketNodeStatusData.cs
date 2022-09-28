// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TicketNodeStatusData.cs" company="Microsoft">
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

    /// <summary>
    /// Ticket node status data.
    /// </summary>
    public class TicketNodeStatusData
    {
        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        public string ElementName { get; set; }

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
        /// Gets or sets the execution identifier.
        /// </summary>
        /// <value>
        /// The execution identifier.
        /// </value>
        public string ExecutionId { get; set; }

        /// <summary>
        /// Gets or sets the ans configuration days.
        /// </summary>
        /// <value>
        /// The ans configuration days.
        /// </value>
        public int AnsConfigurationDays { get; set; }
    }
}
