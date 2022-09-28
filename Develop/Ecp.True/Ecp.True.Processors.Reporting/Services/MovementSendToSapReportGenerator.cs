// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementSendToSapReportGenerator.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Constants = Ecp.True.Repositories.Constants;

    /// <summary>
    /// The Operational Data Without Movement Send To Sap report generator.
    /// </summary>
    public class MovementSendToSapReportGenerator : ReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementSendToSapReportGenerator" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="logger">The logger.</param>
        public MovementSendToSapReportGenerator(IUnitOfWorkFactory unitOfWorkFactory, IConfigurationHandler configurationHandler, ITrueLogger<CutoffReportGenerator> logger)
            : base(unitOfWorkFactory, configurationHandler, logger)
        {
        }

        /// <inheritdoc/>
        public override ReportType Type => ReportType.SapBalance;

        /// <inheritdoc/>
        protected override async Task DoGenerateAsync(ReportExecution entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            await this.GenerateSendToSapAsync(entity).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override IDictionary<string, object> GetParameters(ReportExecution entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            return new Dictionary<string, object>
            {
                { "@ElementId", entity.ElementId },
                { "@OwnerId", entity.OwnerId },
                { "@ScenarioId", entity.ScenarioId },
                { "@StartDate", entity.StartDate },
                { "@EndDate", entity.EndDate },
                { "@ExecutionId", entity.ExecutionId },
            };
        }

        private async Task GenerateSendToSapAsync(ReportExecution entity)
        {
            // Run wrapper
            await this.ExecuteAsync(entity, Constants.MovementSendToSapReportRequestProcedureName).ConfigureAwait(false);
        }
    }
}
