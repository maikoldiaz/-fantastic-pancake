// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Reporting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Reporting.Interfaces;

    /// <summary>
    /// The report generator base.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Balance.Interfaces.IReportGenerator" />
    public abstract class ReportGenerator : IReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportGenerator" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="logger">The logger.</param>
        protected ReportGenerator(IUnitOfWorkFactory unitOfWorkFactory, IConfigurationHandler configurationHandler, ITrueLogger logger)
        {
            ArgumentValidators.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            this.UnitOfWork = unitOfWorkFactory.GetUnitOfWork();
            this.ConfigurationHandler = configurationHandler;
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the report type.
        /// </summary>
        /// <value>
        /// The report type.
        /// </value>
        public abstract ReportType Type { get; }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        protected IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Gets the configuration handler.
        /// </summary>
        /// <value>
        /// The configuration handler.
        /// </value>
        protected IConfigurationHandler ConfigurationHandler { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ITrueLogger Logger { get; private set; }

        /// <summary>
        /// Generates the report.
        /// </summary>
        /// <param name="executionId">The execution identifier.</param>
        /// <returns>
        /// The Task.
        /// </returns>
        public async Task GenerateAsync(int executionId)
        {
            var entity = await this.GetEntityAsync(executionId).ConfigureAwait(false);

            if (entity == null)
            {
                this.Logger.LogInformation($"Report {executionId} is already completed.");
                return;
            }

            try
            {
                await this.DoGenerateAsync(entity).ConfigureAwait(false);
                entity.StatusTypeId = StatusType.PROCESSED;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, ex.Message);
                entity.StatusTypeId = StatusType.FAILED;
            }

            await this.UpdateEntityAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// Purges the report history.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        public async Task PurgeReportHistoryAsync()
        {
            var settings = await this.ConfigurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings).ConfigureAwait(false);
            var timeWindow = settings.ReportsCleanupDurationInHours.GetValueOrDefault(12);
            var tasks = new List<Task>
            {
                this.PurgeAsync(Repositories.Constants.PurgeOperationalDateWithoutCutoffProcedureName, timeWindow),
                this.PurgeAsync(Repositories.Constants.PurgeMonthlyOfficialDataWithoutCutOffProcedureName, timeWindow),
                this.PurgeAsync(Repositories.Constants.PurgeNonSonSegmentDataWithoutCutOffProcedureName, timeWindow),
                this.PurgeAsync(Repositories.Constants.PurgeInventoryMovementIndexProcedureName),
            };

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        /// <summary>
        /// Does the generate asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The task.</returns>
        protected abstract Task DoGenerateAsync(ReportExecution entity);

        /// <summary>
        /// Gets the execution status asynchronous.
        /// </summary>
        /// <param name="executionId">The data queue message.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        protected virtual Task<ReportExecution> GetEntityAsync(int executionId)
        {
            return this.UnitOfWork.CreateRepository<ReportExecution>().FirstOrDefaultAsync(x => x.ExecutionId == executionId && x.StatusTypeId == StatusType.PROCESSING);
        }

        /// <summary>
        /// Updates the entity asynchronous.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        protected virtual Task UpdateEntityAsync(ReportExecution execution)
        {
            ArgumentValidators.ThrowIfNull(execution, nameof(execution));

            this.UnitOfWork.CreateRepository<ReportExecution>().Update(execution);
            return this.UnitOfWork.SaveAsync(CancellationToken.None);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The parameters.</returns>
        protected virtual IDictionary<string, object> GetParameters(ReportExecution entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            return new Dictionary<string, object>
            {
                { "@ElementId", entity.ElementId },
                { "@NodeId", entity.NodeId.GetValueOrDefault() },
                { "@StartDate", entity.StartDate },
                { "@EndDate", entity.EndDate },
                { "@ExecutionId", entity.ExecutionId.ToString(CultureInfo.InvariantCulture) },
            };
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns>The task.</returns>
        protected Task ExecuteAsync(ReportExecution entity, string procedureName)
        {
            return this.UnitOfWork.CreateRepository<ReportExecution>().ExecuteAsync(procedureName, this.GetParameters(entity));
        }

        /// <summary>
        /// Purges the asynchronous.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="timeWindow">The time window.</param>
        /// <returns>The task.</returns>
        protected async Task PurgeAsync(string procedureName, int timeWindow)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Hour", timeWindow },
            };

            try
            {
                var repo = this.UnitOfWork.CreateRepository<ReportExecution>();
                await repo.ExecuteAsync(procedureName, parameters).ConfigureAwait(false);
            }
            catch (SqlException ex)
            {
                this.Logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Purges the asynchronous.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns>The task.</returns>
        protected async Task PurgeAsync(string procedureName)
        {
            try
            {
                var repo = this.UnitOfWork.CreateRepository<InventoryMovementIndex>();
                await repo.ExecuteAsync(procedureName, new Dictionary<string, object>()).ConfigureAwait(false);
            }
            catch (SqlException ex)
            {
                this.Logger.LogError(ex, ex.Message);
            }
        }
    }
}
