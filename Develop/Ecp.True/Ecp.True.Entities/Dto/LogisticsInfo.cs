// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsInfo.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// The logistic information DTO.
    /// </summary>
    public class LogisticsInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogisticsInfo"/> class.
        /// </summary>
        public LogisticsInfo()
        {
            this.LogisticInventoryDetail = new List<LogisticsInventoryDetail>();
            this.LogisticMovementDetail = new List<OperativeLogisticsMovement>();
        }

        /// <summary>
        /// Gets or sets the logistic inventory detail.
        /// </summary>
        /// <value>
        /// The logistic inventory detail.
        /// </value>
        public IEnumerable<LogisticsInventoryDetail> LogisticInventoryDetail { get; set; }

        /// <summary>
        /// Gets or sets the logistic movement detail.
        /// </summary>
        /// <value>
        /// The logistic movement detail.
        /// </value>
        public IEnumerable<OperativeLogisticsMovement> LogisticMovementDetail { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public Ticket Ticket { get; set; }
    }
}
