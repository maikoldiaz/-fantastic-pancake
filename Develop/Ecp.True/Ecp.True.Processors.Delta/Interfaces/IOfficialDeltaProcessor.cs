// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOfficialDeltaProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;

    /// <summary>
    /// The official delta processor interface.
    /// </summary>
    public interface IOfficialDeltaProcessor
    {
        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="officialDeltaData">The data.</param>
        /// <returns>The object.</returns>
        Task<OfficialDeltaData> BuildOfficialDataAsync(OfficialDeltaData officialDeltaData);

        /// <summary>
        /// exclude data asynchronous.
        /// </summary>
        /// <param name="officialDeltaData">The data.</param>
        /// <returns>The object.</returns>
        Task<OfficialDeltaData> ExcludeDataAsync(OfficialDeltaData officialDeltaData);

        /// <summary>
        /// register asynchronous.
        /// </summary>
        /// <param name="officialDeltaData">The data.</param>
        /// <returns>The object.</returns>
        Task<OfficialDeltaData> RegisterAsync(OfficialDeltaData officialDeltaData);

        /// <summary>
        /// Validates the ticket asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The isValid and ticket entity.</returns>
        Task<(bool isValid, Ticket ticket, string errorMessage)> ValidateTicketAsync(int ticketId);

        /// <summary>
        /// Processes the asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="chainType">The chain type.</param>
        /// <returns>The object.</returns>
        Task<OfficialDeltaData> ProcessAsync(object data, ChainType chainType);

        /// <summary>
        /// Builds official delta data.
        /// </summary>
        /// <param name="officialDeltaData">The data.</param>
        /// <returns>The task.</returns>
        Task BuildOfficialDeltaDataAsync(OfficialDeltaData officialDeltaData);

        /// <summary>
        /// Finalizes the process asynchronous.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The task.</returns>
        Task FinalizeProcessAsync(int ticketId);
    }
}
