// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnulationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Admin
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Api.Interfaces;
    using Ecp.True.Processors.Core;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The category processor.
    /// </summary>
    public class AnnulationProcessor : ProcessorBase, IAnnulationProcessor
    {
        /// <summary>
        /// The unit of work factory.
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnulationProcessor" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        public AnnulationProcessor(IRepositoryFactory factory, IUnitOfWorkFactory unitOfWorkFactory)
            : base(factory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Creates the Annulation asynchronous.
        /// </summary>
        /// <param name="annulation">The reversal.</param>
        /// <returns>State of create the reversal operation.</returns>
        public async Task CreateAnnulationRelationshipAsync(Annulation annulation)
        {
            ArgumentValidators.ThrowIfNull(annulation, nameof(annulation));

            var existsAnnulation = await this.ExistsAnnulationRelationshipAsync(annulation).ConfigureAwait(false);
            if (existsAnnulation != null)
            {
                throw new InvalidDataException(EntityConstants.AnnulationExists);
            }

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<Annulation>();
            repository.Insert(annulation);
            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the annulation relationship asynchronous.
        /// </summary>
        /// <param name="annulation">The annulation.</param>
        /// <exception cref="KeyNotFoundException">Annulation not found.</exception>
        /// <returns>Return the status of update annulation operation.</returns>
        public async Task UpdateAnnulationRelationshipAsync(Annulation annulation)
        {
            ArgumentValidators.ThrowIfNull(annulation, nameof(annulation));

            var existsAnnulation = await this.ExistsAnnulationRelationshipAsync(annulation).ConfigureAwait(false);
            if (existsAnnulation != null)
            {
                throw new InvalidDataException(EntityConstants.AnnulationExists);
            }

            using var unitOfWork = this.unitOfWorkFactory.GetUnitOfWork();
            var repository = unitOfWork.CreateRepository<Annulation>();
            var existing = await repository.GetByIdAsync(annulation.AnnulationId).ConfigureAwait(false);
            if (existing == null)
            {
                throw new KeyNotFoundException(EntityConstants.AnnulationDoesNotExist);
            }

            existing.CopyFrom(annulation);
            repository.Update(existing);

            await unitOfWork.SaveAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the Annulation relationship.
        /// </summary>
        /// <param name="annulation">The reversal.</param>
        /// <returns>Validation results.</returns>
        public async Task<AnnulationInfo> ExistsAnnulationRelationshipAsync(Annulation annulation)
        {
            ArgumentValidators.ThrowIfNull(annulation, nameof(annulation));

            var repository = this.CreateRepository<Annulation>();
            AnnulationInfo reversalMovementExists = null;
            var annulations = await repository.GetAllAsync(
                r => annulation.AnnulationId != r.AnnulationId &&
                (annulation.SourceMovementTypeId == r.SourceMovementTypeId || annulation.AnnulationMovementTypeId == r.AnnulationMovementTypeId),
                "SourceCategoryElement",
                "AnnulationCategoryElement").ConfigureAwait(false);

            foreach (var item in annulations)
            {
                if (item.SourceMovementTypeId == annulation.SourceMovementTypeId)
                {
                    return new AnnulationInfo(item.SourceCategoryElement.Name, item.AnnulationCategoryElement.Name, "source");
                }

                if (item.AnnulationMovementTypeId == annulation.AnnulationMovementTypeId)
                {
                    reversalMovementExists ??= new AnnulationInfo(item.SourceCategoryElement.Name, item.AnnulationCategoryElement.Name, "annulation");
                }
            }

            return reversalMovementExists ?? null;
        }
    }
}
