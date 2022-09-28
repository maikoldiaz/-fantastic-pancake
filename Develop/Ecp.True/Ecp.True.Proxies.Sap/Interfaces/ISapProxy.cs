// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISapProxy.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Ecp.True.Proxies.Sap.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Proxies.Sap.Request;
    using Ecp.True.Proxies.Sap.Response;

    /// <summary>
    /// The sap proxy.
    /// </summary>
    public interface ISapProxy
    {
        /// <summary>
        /// Updates the movement transfer point asynchronous.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <param name="movementTransactionId">The movement transaction identifier.</param>
        /// <returns>The upload status.</returns>
        Task<SapTrackingStatus> UpdateMovementTransferPointAsync(SapMovementRequest movement, int movementTransactionId);

        /// <summary>
        /// Get Mappings asynchronous.
        /// </summary>
        /// <returns>The boolean.</returns>
        Task<IEnumerable<SapMappingResponse>> GetMappingsAsync();

        /// <summary>
        /// Updates the upload status asynchronous.
        /// </summary>
        /// <param name="sapUpload">The sap upload.</param>
        /// <returns>The boolean.</returns>
        Task<bool> UpdateUploadStatusAsync(SapUploadStatus sapUpload);

        /// <summary>
        /// Updates the upload status contract asynchronous.
        /// </summary>
        /// <param name="sapUpload">The sap upload.</param>
        /// <returns>The boolean.</returns>
        Task<bool> UpdateUploadStatusContractAsync(SapUploadStatusContract sapUpload);

        /// <summary>
        /// Send operating balance information with property and official deltas.
        /// </summary>
        /// <param name="sapUpload">The sap upload.</param>
        /// <returns>The boolean.</returns>
        Task<bool> SendLogisticMovementAsync(object sapUpload);
    }
}