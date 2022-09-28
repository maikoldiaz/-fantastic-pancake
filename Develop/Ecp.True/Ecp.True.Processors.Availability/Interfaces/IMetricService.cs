// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMetricService.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Processors.Availability.Response;

    /// <summary>
    /// IMetricService.
    /// </summary>
    public interface IMetricService
    {
        /// <summary>
        /// Gets the availability asynchronous.
        /// </summary>
        /// <param name="notAvailableResources">The not available resources.</param>
        /// <param name="availabilitySettings">The availability settings.</param>
        /// <returns>The collections.</returns>
        Task<IEnumerable<ResourceAvailability>> GetAvailabilityAsync(IEnumerable<ResourceDetails> notAvailableResources, AvailabilitySettings availabilitySettings);

        /// <summary>
        /// Reports the availability.
        /// </summary>
        /// <param name="resourceAvailabilities">The resource availabilities.</param>
        /// <param name="moduleAvailabilitySettings">The module availability settings.</param>
        /// <param name="elapsedTimeSpan">The elapsed time span.</param>
        /// <param name="isChaos">The chaos.</param>
        void ReportAvailability(IEnumerable<ResourceAvailability> resourceAvailabilities, IEnumerable<ModuleAvailabilitySettings> moduleAvailabilitySettings, TimeSpan elapsedTimeSpan, bool isChaos);
    }
}
