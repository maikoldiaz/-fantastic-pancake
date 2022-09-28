// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileRegistrationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The file registration processor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Api.Interfaces.IProcessor" />
    public interface IFileRegistrationProcessor : IProcessor
    {
        /// <summary>
        /// Registers the file information.
        /// </summary>
        /// <param name="fileRegistration">The file registration.</param>
        /// <returns>
        /// Return the task.
        /// </returns>
        Task RegisterAsync(FileRegistration fileRegistration);

        /// <summary>
        /// Gets the register files.
        /// </summary>
        /// <param name="fileUploadIds">The file upload ids.</param>
        /// <returns>
        /// Returns the registered files.
        /// </returns>
        Task<IEnumerable<FileRegistration>> GetFileRegistrationStatusAsync(IEnumerable<Guid> fileUploadIds);

        /// <summary>
        /// Get Write SasToken for file.
        /// </summary>
        /// <param name="blobFileName">The blob File name.</param>
        /// <param name="systemType">Specifies the file type.</param>
        /// <returns>Provide Write SAS token for User.</returns>
        Task<FileAccessInfo> GetFileRegistrationAccessInfoAsync(string blobFileName, SystemType systemType);

        /// <summary>
        /// Get Read SAS Token For Container.
        /// </summary>
        /// <returns>Provide Read SAS token for User.</returns>
        Task<FileAccessInfo> GetFileRegistrationAccessInfoAsync();

        /// <summary>
        /// Gets the file registration access information by container asynchronous.
        /// </summary>
        /// <param name="containerName"> The name of container, example: true, delta, ownership.</param>
        /// <returns>Provide Read SAS token for User.</returns>
        Task<FileAccessInfo> GetFileRegistrationAccessInfoByContainerAsync(string containerName);

        /// <summary>
        /// Updates the file registration transaction asynchronous.
        /// </summary>
        /// <param name="fileRegistrationTransactionIds">The file registration transaction ids.</param>
        /// <returns>Returns a task.</returns>
        Task RetryAsync(int[] fileRegistrationTransactionIds);
    }
}
