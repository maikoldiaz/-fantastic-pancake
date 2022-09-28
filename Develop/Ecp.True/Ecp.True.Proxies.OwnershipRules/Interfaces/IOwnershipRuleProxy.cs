// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOwnershipRuleProxy.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.OwnershipRules.Interfaces
{
    using System.Threading.Tasks;

    using Ecp.True.Entities.Configuration;
    using Ecp.True.Proxies.OwnershipRules.Request;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The Ownership rule proxy Interface.
    /// </summary>
    public interface IOwnershipRuleProxy
    {
        /// <summary>
        /// Initializes the specified ownership rule settings.
        /// </summary>
        /// <param name="ownershipRuleSettings">The ownership rule settings.</param>
        void Initialize(OwnershipRuleSettings ownershipRuleSettings);

        /// <summary>
        /// Gets the active rules asynchronous.
        /// </summary>
        /// <returns>Returns Ownership Rules as response.</returns>
        Task<OwnershipRuleResponse> GetActiveRulesAsync();

        /// <summary>
        /// Gets the active rules asynchronous.
        /// </summary>
        /// <returns>Returns Ownership Rules as response.</returns>
        Task<OwnershipRuleResponse> GetInactiveRulesAsync();

        /// <summary>
        /// Processes the ownership asynchronous.
        /// </summary>
        /// <param name="ownershipRuleRequest">The ownership rule request.</param>
        /// <param name="ticketId">The ticketId.</param>
        /// <returns>Returns Ownership Rules as response.</returns>
        Task<OwnershipRuleResponse> ProcessOwnershipAsync(OwnershipRuleRequest ownershipRuleRequest, int ticketId);
    }
}
