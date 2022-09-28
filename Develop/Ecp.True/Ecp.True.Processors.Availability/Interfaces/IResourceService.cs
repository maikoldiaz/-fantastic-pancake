// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResourceService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Availability.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Processors.Availability.Response;

    /// <summary>
    /// IResourceService.
    /// </summary>
    public interface IResourceService
    {
        /// <summary>
        /// Gets the resources asynchronous.
        /// </summary>
        /// <param name="availabilitySettings">The availability settings.</param>
        /// <returns>The collections.</returns>
        Task<IEnumerable<ResourceDetails>> GetResourcesAsync(AvailabilitySettings availabilitySettings);

        /// <summary>
        /// Gets the availability asynchronous.
        /// </summary>
        /// <param name="availabilitySettings">The availabilitySettings.</param>
        /// <returns>The collections.</returns>
        Task<IEnumerable<ResourceAvailability>> GetAvailabilityAsync(AvailabilitySettings availabilitySettings);
    }
}
