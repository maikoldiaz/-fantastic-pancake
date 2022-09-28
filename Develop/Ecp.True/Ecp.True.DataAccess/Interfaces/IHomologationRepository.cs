// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHomologationRepository.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// Homologation Repository Interface.
    /// </summary>
    public interface IHomologationRepository : IRepository<Homologation>
    {
        /// <summary>
        /// Get Homologation By Id And GroupId.
        /// </summary>
        /// <param name="homologationId">homologation Id.</param>
        /// <param name="groupId">group Id.</param>
        /// <returns>Homologation hierarchy.</returns>
        Task<HomologationGroup> GetHomologationByIdAndGroupIdAsync(int homologationId, int groupId);

        /// <summary>
        /// Gets the homologation group asynchronous.
        /// </summary>
        /// <param name="homologationGroupId">The homologation group identifier.</param>
        /// <returns>The group.</returns>
        Task<IEnumerable<HomologationDataMapping>> GetHomologationGroupMappingsAsync(int homologationGroupId);
    }
}
