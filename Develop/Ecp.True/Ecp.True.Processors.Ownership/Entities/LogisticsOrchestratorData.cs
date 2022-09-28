// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogisticsOrchestratorData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Entities
{
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.TransportBalance;

    /// <summary>
    /// Logistics Orchestrator Data.
    /// </summary>
    public class LogisticsOrchestratorData : OrchestratorMetaData
    {
        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public Ticket Ticket { get; set; }

        /// <summary>
        /// Gets or sets the SystemType.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        public int? SystemType { get; set; }
    }
}
