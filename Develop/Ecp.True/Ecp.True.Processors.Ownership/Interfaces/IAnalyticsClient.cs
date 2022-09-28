// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnalyticsClient.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Processors.Ownership.Calculation.Request;
    using Ecp.True.Processors.Ownership.Calculation.Response;

    /// <summary>
    /// The analytics client.
    /// </summary>
    public interface IAnalyticsClient
    {
        /// <summary>
        /// Gets the ownership analytics asynchronous.
        /// </summary>
        /// <param name="request">The analytical service request data.</param>
        /// <returns>
        /// The analytical service response data.
        /// </returns>
        Task<IEnumerable<AnalyticalServiceResponseData>> GetOwnershipAnalyticsAsync(AnalyticalServiceRequestData request);
    }
}
