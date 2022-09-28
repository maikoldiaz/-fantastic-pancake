// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnalysisServiceClient.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;

    /// <summary>
    /// The Analysis Service Client Interface.
    /// </summary>
    public interface IAnalysisServiceClient
    {
        /// <summary>
        /// Initializes the specified analysis settings.
        /// </summary>
        /// <param name="analysisSettings">The analysis settings.</param>
        void Initialize(AnalysisSettings analysisSettings);

        /// <summary>
        /// Calls the refresh asynchronous for balance.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <returns>a task.</returns>
        Task RefreshCalculationAsync(int ticketId);

        /// <summary>
        /// Calls the refresh asynchronous for ownership.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <returns>a task.</returns>
        Task RefreshOwnershipAsync(int ticketId);

        /// <summary>
        /// Calls the refresh asynchronous for official delta.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <returns>a task.</returns>
        Task RefreshOfficialDeltaAsync(int ticketId);

        /// <summary>
        /// Calls the refresh asynchronous for audit reports.
        /// </summary>
        /// <returns>a task.</returns>
        Task RefreshAuditReportsAsync();

        /// <summary>
        /// Refreshes the sap mapping details asynchronously.
        /// </summary>
        /// <returns>a task.</returns>
        Task RefreshSapMappingDetailsAsync();
    }
}
