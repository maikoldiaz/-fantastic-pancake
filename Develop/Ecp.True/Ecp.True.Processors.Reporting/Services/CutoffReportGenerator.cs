// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CutoffReportGenerator.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Constants = Ecp.True.Repositories.Constants;

    /// <summary>
    /// The Operational Data Without Cutoff report generator.
    /// </summary>
    public class CutoffReportGenerator : ReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CutoffReportGenerator" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="logger">The logger.</param>
        public CutoffReportGenerator(IUnitOfWorkFactory unitOfWorkFactory, IConfigurationHandler configurationHandler, ITrueLogger<CutoffReportGenerator> logger)
            : base(unitOfWorkFactory, configurationHandler, logger)
        {
        }

        /// <inheritdoc/>
        public override ReportType Type => ReportType.BeforeCutOff;

        /// <inheritdoc/>
        protected override async Task DoGenerateAsync(ReportExecution entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            var type = entity.CategoryId == 2 ? Constants.SegmentCategoryNameEn : Constants.SystemCategoryNameEn;
            await this.GenerateCutOffAsync(entity, type).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override IDictionary<string, object> GetParameters(ReportExecution entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));
            return new Dictionary<string, object>
            {
                { "@CategoryId", entity.CategoryId },
                { "@ElementId", entity.ElementId },
                { "@NodeId", entity.NodeId.GetValueOrDefault() },
                { "@StartDate", entity.StartDate },
                { "@EndDate", entity.EndDate },
                { "@ExecutionId", entity.ExecutionId.ToString(CultureInfo.InvariantCulture) },
            };
        }

        private async Task GenerateCutOffAsync(ReportExecution entity, string type)
        {
            // Run wrapper
            await this.ExecuteAsync(entity, Constants.SaveOperationalDataWithoutCutOffForReportProcedure).ConfigureAwait(false);

            var tasks = new List<Task>();

            // Run movement calculation
            await this.ExecuteAsync(entity, $"{Constants.SaveOperationalMovementWithoutCutOffProcedure}{type}").ConfigureAwait(false);

            // Run movement owner & quality in parallel
            tasks.Add(this.ExecuteAsync(entity, $"{Constants.SaveOperationalMovementOwnerWithoutCutOffProcedure}{type}"));
            tasks.Add(this.ExecuteAsync(entity, $"{Constants.SaveOperationalMovementQualityWithoutCutOffProcedure}{type}"));

            await Task.WhenAll(tasks).ConfigureAwait(false);
            tasks.Clear();

            // Run inventory calculation
            await this.ExecuteAsync(entity, $"{Constants.SaveInventoryDetailsWithoutCutOffProcedure}{type}").ConfigureAwait(false);

            // Run inventory owner, quality and operation data in parallel
            // Since operational data depends on both movement and inventory calculation, this can be triggered now
            tasks.Add(this.ExecuteAsync(entity, $"{Constants.SaveInventoryQualityDetailsWithoutCutOffProcedure}{type}"));
            tasks.Add(this.ExecuteAsync(entity, $"{Constants.SaveInventoryOwnerDetailsWithoutCutOffProcedure}{type}"));
            tasks.Add(this.ExecuteAsync(entity, $"{Constants.SaveOperationalDataWithoutCutOffProcedure}{type}"));

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
