// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationController.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Filter;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The Register file controller.
    /// </summary>
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Transport Balance")]
    public class FileRegistrationController : ODataController
    {
        /// <summary>
        /// The register file processor.
        /// </summary>
        private readonly IFileRegistrationProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileRegistrationController"/> class.
        /// </summary>
        /// <param name="registerFileProcessor">The register file processor.</param>
        public FileRegistrationController(IFileRegistrationProcessor registerFileProcessor)
        {
            this.processor = registerFileProcessor;
        }

        /// <summary>
        /// Queries the register files asynchronous.
        /// </summary>
        /// <returns>The register files response.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("fileregistrations")]
        [ODataRoute("fileregistrations")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public Task<IQueryable<FileRegistrationInfo>> QueryAllAsync()
        {
            return this.processor.QueryViewAsync<FileRegistrationInfo>();
        }

        /// <summary>
        /// Queries the integration management async.
        /// </summary>
        /// <returns>The file registration response.</returns>
        [HttpGet]
        [EnableQuery]
        [Route("integrationmanagements")]
        [ODataRoute("integrationmanagements")]
        [TrueAuthorize(Role.Administrator)]
        public Task<IQueryable<FileRegistration>> QueryAllIntegrationManagementAsync()
        {
            return this.processor.QueryAllAsync<FileRegistration>(null);
        }

        /// <summary>
        /// Register files the specified register file.
        /// </summary>
        /// <param name="fileRegistration">The register file.</param>
        /// <returns>The task.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/fileregistration")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> RegisterfilesAsync([FromBody]FileRegistration fileRegistration)
        {
            await this.processor.RegisterAsync(fileRegistration).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.RegisterFilesUploadedSuccessfully);
        }

        /// <summary>
        /// Retry the specified Pending Transactions asynchronous.
        /// </summary>
        /// <param name="retryIds">The file registration transaction identifiers.</param>
        /// <returns>The Task.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/retrypendingtransactions")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        [ValidateFileRegistrationTransactionFilter]
        public async Task<IActionResult> RetryPendingTransactionAsync([FromBody]int[] retryIds)
        {
            await this.processor.RetryAsync(retryIds).ConfigureAwait(false);
            return new EntityResult(Entities.Constants.RetryPendingTransaction);
        }

        /// <summary>
        /// Gets the register files by ids asynchronous.
        /// </summary>
        /// <param name="fileUploadIds">The file upload ids.</param>
        /// <returns>
        /// The register files by ids.
        /// </returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/fileregistration/status")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances)]
        public async Task<IActionResult> GetFileRegistrationStatusAsync([FromBody] IEnumerable<Guid> fileUploadIds)
        {
            var result = await this.processor.GetFileRegistrationStatusAsync(fileUploadIds).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Get Write SasToken for file.
        /// </summary>
        /// <param name="blobFileName">The blob file name.</param>
        /// <param name="systemType">Specifies the file type.</param>
        /// <returns>The SAS Token.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/fileregistrations/{blobFileName}/{systemType}/uploadaccessinfo/")]
        [TrueAuthorize(Role.Administrator, Role.ProfessionalSegmentBalances, Role.Programmer)]
        public async Task<IActionResult> GetFileRegistrationAccessInfoAsync(string blobFileName, SystemType systemType)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(blobFileName, nameof(blobFileName));
            var result = await this.processor.GetFileRegistrationAccessInfoAsync(blobFileName, systemType).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Get Read SasToken for container.
        /// </summary>
        /// <returns>The SAS Token.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/fileregistration/readaccessinfo")]
        [TrueAuthorize]
        public async Task<IActionResult> GetFileRegistrationAccessInfoAsync()
        {
            var result = await this.processor.GetFileRegistrationAccessInfoAsync().ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Get Read SasToken for any container.
        /// </summary>
        /// <param name="containerName"> The name of container, example: true, delta, ownership.</param>
        /// <returns>The SAS Token.</returns>
        [HttpGet]
        [Route("api/v{version:apiVersion}/fileregistration/readaccessinfo/{containerName}")]
        [TrueAuthorize]
        public async Task<IActionResult> GetFileRegistrationAccessInfoAsync(string containerName)
        {
            var result = await this.processor.GetFileRegistrationAccessInfoByContainerAsync(containerName).ConfigureAwait(false);
            return new EntityResult(result);
        }
    }
}