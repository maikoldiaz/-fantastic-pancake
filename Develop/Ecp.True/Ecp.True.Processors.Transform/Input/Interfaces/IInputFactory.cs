// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Input.Interfaces
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Core.Input;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The factory to build the input from blob.
    /// </summary>
    public interface IInputFactory
    {
        /// <summary>
        /// Gets the file registration transaction asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransactionId">The file registration transaction identifier.</param>
        /// <returns>Returns file registration transaction.</returns>
        Task<FileRegistrationTransaction> GetFileRegistrationTransactionAsync(int fileRegistrationTransactionId);

        /// <summary>
        /// Gets the json input asynchronous.
        /// </summary>
        /// <param name="blobPath">The BLOB path.</param>
        /// <returns>Returns JObject.</returns>
        Task<JToken> GetJsonInputAsync(string blobPath);

        /// <summary>
        /// Gets the excel input asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The excel input.</returns>
        Task<ExcelInput> GetExcelInputAsync(TrueMessage message);

        /// <summary>
        /// Getting the file registration details.
        /// </summary>
        /// <param name="uploadId">The upload ID.</param>
        /// <returns>The file registration.</returns>
        Task<FileRegistration> GetFileRegistrationAsync(string uploadId);

        /// <summary>
        /// Saves the sap json array asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<string> SaveSapJsonArrayAsync(object entity, TrueMessage message);

        /// <summary>
        /// Saves the sap json asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<string> SaveSapJsonAsync(object entity, TrueMessage message);

        /// <summary>
        /// Saves the sap logistic json asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<string> SaveSapLogisticJsonAsync(object entity, TrueMessage message);
    }
}
