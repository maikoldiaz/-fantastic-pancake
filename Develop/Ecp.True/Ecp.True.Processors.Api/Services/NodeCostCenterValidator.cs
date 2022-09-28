// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeCostCenterValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Services
{
    using System.Threading.Tasks;
    using Ecp.True.Core.Entities;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Services.Interfaces;

    /// <summary>
    /// Validator for the CostCenterMovements.
    /// </summary>
    public class NodeCostCenterValidator : INodeCostCenterValidator
    {
        /// <summary>
        /// The node cost center repository.
        /// </summary>
        private readonly IRepositoryFactory repoFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCostCenterValidator"/> class.
        /// </summary>
        /// <param name="repoFactory">The repository factory.</param>
        public NodeCostCenterValidator(IRepositoryFactory repoFactory)
        {
            this.repoFactory = repoFactory;
        }

        /// <inheritdoc></inheritdoc>/>
        public async Task<ValidationResult> ValidateForDeletionAsync(NodeCostCenter nodeCostCenter)
        {
            var result = new ValidationResult();
            var repo = this.repoFactory.CreateRepository<LogisticMovement>();

            result.IsSuccess = !await HasMovementsAsync(nodeCostCenter, repo).ConfigureAwait(false);

            return result;
        }

        private static async Task<bool> HasMovementsAsync(NodeCostCenter nodeCostCenter, IRepository<LogisticMovement> repo)
        {
            return await repo.GetCountAsync(r => r.CostCenterId == nodeCostCenter.CostCenterId
                                           && (r.StatusProcessId != StatusType.FAILED || r.StatusProcessId != StatusType.ERROR)).ConfigureAwait(false) > 0;
        }
    }
}
