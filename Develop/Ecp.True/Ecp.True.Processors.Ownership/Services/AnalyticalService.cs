// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticalService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Calculation.Services
{
    using Ecp.True.Processors.Ownership.Calculation.Response;
    using Ecp.True.Processors.Ownership.Calculation.Services.Interfaces;

    /// <summary>
    /// The analytic service client.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Ownership.Calculation.Services.Interfaces.IAnalyticalService" />
    public class AnalyticalService : IAnalyticalService
    {
        /// <summary>
        /// Gets the ownership for transfer points.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Ownership percentage.</returns>
        public AnalyticalServiceResponseData GetOwnershipPercentage(AnalyticalServiceResponseData data)
        {
            return new AnalyticalServiceResponseData();
        }
    }
}
