// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDeltaProxy.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Interfaces
{
    using System.Threading.Tasks;

    using Ecp.True.Entities.Configuration;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The delta proxy.
    /// </summary>
    public interface IDeltaProxy
    {
        /// <summary>
        /// Initializes the specified ownership rule settings.
        /// </summary>
        /// <param name="ownershipRuleSettings">The ownership rule settings.</param>
        void Initialize(OwnershipRuleSettings ownershipRuleSettings);

        /// <summary>
        /// Processes the delta asynchronous.
        /// </summary>
        /// <param name="deltaRequest">The delta request.</param>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>The DeltaResponse.</returns>
        Task<DeltaResponse> ProcessDeltaAsync(DeltaRequest deltaRequest, int ticketId);

        /// <summary>
        /// Processes the delta asynchronous.
        /// </summary>
        /// <param name="deltaRequest">The delta request.</param>
        /// <returns>
        /// The Official Delta Response.
        /// </returns>
        Task<OfficialDeltaResponse> ProcessOfficialDeltaAsync(OfficialDeltaRequest deltaRequest);
    }
}
