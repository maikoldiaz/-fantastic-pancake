// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnalyticalService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Calculation.Services.Interfaces
{
    using Ecp.True.Processors.Ownership.Calculation.Response;

    /// <summary>
    /// The analytical service.
    /// </summary>
    public interface IAnalyticalService
    {
        /// <summary>
        /// Gets the ownership for transfer points.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Ownership percentage.</returns>
        AnalyticalServiceResponseData GetOwnershipPercentage(AnalyticalServiceResponseData data);
    }
}
