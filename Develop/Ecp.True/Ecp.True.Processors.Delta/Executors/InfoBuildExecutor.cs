// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoBuildExecutor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Execution;
    using Ecp.True.Processors.Delta.Entities;

    /// <summary>
    /// The InfoBuildExecutor.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Core.Execution.ExecutorBase" />
    public class InfoBuildExecutor : ExecutorBase
    {
        /// <summary>
        /// Gets the repository factory.
        /// </summary>
        /// <value>
        /// The repository factory.
        /// </value>
        private readonly IRepositoryFactory repositoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoBuildExecutor" /> class.
        /// </summary>
        /// <param name="factory">The IRepositoryFactory.</param>
        /// <param name="logger">the logger.</param>
        public InfoBuildExecutor(IRepositoryFactory factory, ITrueLogger<InfoBuildExecutor> logger)
                   : base(logger)
        {
            this.repositoryFactory = factory;
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// Gets the type of the process.
        /// </summary>
        /// <value>
        /// The type of the process.
        /// </value>
        public override ProcessType ProcessType => ProcessType.Delta;

        /// <summary>
        /// The ExecuteAsync.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The Task.</returns>
        public override async Task ExecuteAsync(object input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));

            var deltaData = (DeltaData)input;

            await this.BuildAsync(deltaData).ConfigureAwait(false);

            this.ShouldContinue = true;

            await this.ExecuteNextAsync(deltaData).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>
        /// The repository.
        /// </returns>
        private IRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IEntity
        {
            return this.repositoryFactory.CreateRepository<TEntity>();
        }

        /// <summary>
        /// Builds the asynchronous.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        private async Task BuildAsync(DeltaData deltaData)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));

            var originalParams = new Dictionary<string, object>
            {
                { "@SegmentId", deltaData.Ticket.CategoryElementId },
                { "@StartDate", deltaData.Ticket.StartDate },
                { "@EndDate", deltaData.Ticket.EndDate },
                { "@IsOriginal", true },
            };

            var updatedParams = new Dictionary<string, object>
            {
                { "@SegmentId", deltaData.Ticket.CategoryElementId },
                { "@StartDate", deltaData.Ticket.StartDate },
                { "@EndDate", deltaData.Ticket.EndDate },
                { "@IsOriginal", false },
            };

            await Task.WhenAll(
               this.GetDataFromRepositoryAsync<Ecp.True.Entities.Query.OriginalMovement>(
               (originalMov) => deltaData.OriginalMovements = originalMov,
               Repositories.Constants.OriginalOrUpdatedMovementsProcedureName,
               originalParams),
               this.GetDataFromRepositoryAsync<Ecp.True.Entities.Query.UpdatedMovement>(
               (updatedMov) => deltaData.UpdatedMovements = updatedMov,
               Repositories.Constants.OriginalOrUpdatedMovementsProcedureName,
               updatedParams),
               this.GetDataFromRepositoryAsync<Ecp.True.Entities.Query.OriginalInventory>(
               (originalInv) => deltaData.OriginalInventories = originalInv,
               Repositories.Constants.OriginalOrUpdatedInventoriesProcedureName,
               originalParams),
               this.GetDataFromRepositoryAsync<Ecp.True.Entities.Query.UpdatedInventory>(
               (updatedInv) => deltaData.UpdatedInventories = updatedInv,
               Repositories.Constants.OriginalOrUpdatedInventoriesProcedureName,
               updatedParams)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the data from repository asynchronous.
        /// </summary>
        /// <typeparam name="T">The T type.</typeparam>
        /// <param name="setter">The setter.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The tasks.</returns>
        private async Task GetDataFromRepositoryAsync<T>(Action<IEnumerable<T>> setter, string storedProcedureName, IDictionary<string, object> parameters)
                where T : class, IEntity
        {
            setter(await this.CreateRepository<T>()
                            .ExecuteQueryAsync(storedProcedureName, parameters).ConfigureAwait(false));
        }
    }
}
