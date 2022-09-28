// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHomologationProcessor.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Core.Interfaces;

    /// <summary>
    /// The Homologation Processor Interface.
    /// </summary>
    public interface IHomologationProcessor : IProcessor
    {
        /// <summary>
        /// Saves the node asynchronous.
        /// </summary>
        /// <param name="homologation">The homologation.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        Task SaveHomologationAsync(Homologation homologation);

        /// <summary>
        /// Gets the Homologation hierarchy by identifier.
        /// </summary>
        /// <param name="homologationId">The homologation identifier.</param>
        /// <returns>Return Homologation hierarchy by homologationId.</returns>
        Task<Homologation> GetHomologationByIdAsync(int homologationId);

        /// <summary>
        /// Get Homologation hierarchy By HomologationId And HomologationGroupId.
        /// </summary>
        /// <param name="homologationId">Homologation Id.</param>
        /// <param name="groupTypeId">The Homologation Group Type Id.</param>
        /// <returns>
        /// Homologation Data.
        /// </returns>
        Task<HomologationGroup> GetHomologationByIdAndGroupIdAsync(int homologationId, int groupTypeId);

        /// <summary>
        /// Delete Homologation Group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>
        /// Homologation Data.
        /// </returns>
        Task DeleteHomologationGroupAsync(DeleteHomologationGroup group);

        /// <summary>
        /// Gets the HomologationObjectType asynchronous.
        /// </summary>
        /// <returns>
        /// The HomologationObjectType.
        /// </returns>
        Task<IEnumerable<HomologationObjectType>> GetHomologationObjectTypesAsync();

        /// <summary>
        /// Check Homologation Group Exists.
        /// </summary>
        /// <param name="groupTypeId">The group type Id.</param>
        /// <param name="sourceSystemId">The source system Id.</param>
        /// <param name="destinationSystemId">The destination system Id.</param>
        /// <returns>Check homologation group for source, destination system.</returns>
        Task<HomologationGroup> CheckHomologationGroupExistsAsync(int groupTypeId, int sourceSystemId, int destinationSystemId);

        /// <summary>
        /// Returns homologation group details.
        /// </summary>
        /// <param name="homologationGroupId">The homologation Group Id.</param>
        /// <returns>Check homologation group for source, destination system.</returns>
        Task<IEnumerable<HomologationDataMapping>> GetHomologationGroupMappingsAsync(int homologationGroupId);
    }
}