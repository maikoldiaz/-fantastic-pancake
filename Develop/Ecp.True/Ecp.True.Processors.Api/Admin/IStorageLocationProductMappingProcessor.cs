// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorageLocationProductMappingProcessor.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The storage location product mapping processor.
    /// </summary>
    public interface IStorageLocationProductMappingProcessor
    {
        /// <summary>
        /// Creates a range of new storage location product mappings.
        /// </summary>
        /// <param name="mappings">The storage location mappings.</param>
        /// <returns>The info about the creation process.</returns>
        Task<IEnumerable<StorageLocationProductMappingInfo>> CreateStorageLocationProductMappingAsync(
            IEnumerable<StorageLocationProductMapping> mappings);

        /// <summary>
        /// Updates a storage location product mapping.
        /// </summary>
        /// <param name="mappingId">The storage location mappings.</param>
        /// <returns>The task.</returns>
        Task DeleteStorageLocationProductMappingAsync(string mappingId);
    }
}