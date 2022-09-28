// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductMappingController.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Host.Core;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The storageLocationProductMappingController controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [ApiVersion("1")]
    [CLSCompliant(false)]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = "Admin")]
    public class StorageLocationProductMappingController
    {
        private readonly IStorageLocationProductMappingProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageLocationProductMappingController"/> class.
        /// </summary>
        /// <param name="processor">The storage location product mapping processor.</param>
        public StorageLocationProductMappingController(IStorageLocationProductMappingProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Creates a range of new storage location product mappings.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        /// <returns>The entity result.</returns>
        [HttpPost]
        [Route("api/v{version:apiVersion}/StorageLocationProductMappings")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> CreateStorageLocationProductMappingAsync(
            IEnumerable<StorageLocationProductMapping> mappings)
        {
            var result = await this.processor.CreateStorageLocationProductMappingAsync(mappings).ConfigureAwait(false);
            return new EntityResult(result);
        }

        /// <summary>
        /// Deletes a storage location product mapping.
        /// </summary>
        /// <param name="mappingId">The mapping Id.</param>
        /// <returns>The entity result.</returns>
        [HttpDelete]
        [Route("api/v{version:apiVersion}/StorageLocationProductMappings/{mappingId}")]
        [TrueAuthorize(Role.Administrator)]
        public async Task<IActionResult> DeleteStorageLocationProductMappingAsync(string mappingId)
        {
            await this.processor.DeleteStorageLocationProductMappingAsync(mappingId).ConfigureAwait(false);
            return new EntityResult();
        }
    }
}