// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExcelService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The excel service.
    /// </summary>
    public interface IExcelService
    {
        /// <summary>
        /// Processes the excel asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="stream">The blob stream.</param>
        /// <param name="excelType">The excel type.</param>
        /// <param name="ticketId">The ticketId from file name.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<(JObject result, ICollection<ErrorInfo> errorInfo)> ProcessExcelAsync(TrueMessage message, Stream stream, OwnershipExcelType excelType, int ticketId);

        /// <summary>
        /// Exports the and upload logistics excel asynchronous.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="pathName">Name of the path.</param>
        /// <param name="ticketNumber">The ticket number.</param>
        /// <param name="segmentName">Name of the segment.</param>
        /// <param name="ownerName">Name of the owner.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// Returns completed task.
        /// </returns>
        Task ExportAndUploadLogisticsExcelAsync(DataSet dataSet, string containerName, string pathName, string ticketNumber, string segmentName, string ownerName, string fileName);
    }
}
