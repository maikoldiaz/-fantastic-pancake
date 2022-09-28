// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISapTrackingProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Sap.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface ISapTrackingProcessor.
    /// </summary>
    public interface ISapTrackingProcessor
    {
        /// <summary>
        /// Creates the BLOB asynchronous.
        /// </summary>
        /// <param name="processId">processId.</param>
        /// <param name="payload">payload.</param>
        /// <returns>string.</returns>
        Task<string> CreateBlobAsync(string processId, object payload);

        /// <summary>
        /// Inserts the sap tracking status.
        /// </summary>
        /// <param name="fileRegistrationId">The file registration identifier.</param>
        /// <param name="statusType">if set to <c>true</c> [status type].</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="blobPath">The BLOB path.</param>
        /// <returns>The Task.</returns>
        Task InsertSapTrackingAsync(int fileRegistrationId, bool statusType, string errorMessage, string blobPath);
    }
}
