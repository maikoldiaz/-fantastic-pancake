// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialInitialBalanceReportGenerator.cs" company="Microsoft">
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
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Constants = Ecp.True.Repositories.Constants;

    /// <summary>
    /// The OfficialInitialBalanceReportGenerator.
    /// </summary>
    public class OfficialInitialBalanceReportGenerator : ReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialInitialBalanceReportGenerator" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="logger">The logger.</param>
        public OfficialInitialBalanceReportGenerator(IUnitOfWorkFactory unitOfWorkFactory, IConfigurationHandler configurationHandler, ITrueLogger<OfficialInitialBalanceReportGenerator> logger)
            : base(unitOfWorkFactory, configurationHandler, logger)
        {
        }

        /// <inheritdoc/>
        public override ReportType Type => ReportType.OfficialInitialBalance;

        /// <inheritdoc/>
        protected override async Task DoGenerateAsync(ReportExecution entity)
        {
            await this.ExecuteAsync(entity, Constants.SaveMonthlyOfficialDataWithoutCutOffReport).ConfigureAwait(false);

            var tasks = new List<Task>
            {
                this.ExecuteAsync(entity, Constants.SaveMonthlyOfficialMovementDetailsWithoutCutOff),
                this.ExecuteAsync(entity, Constants.SaveMonthlyOfficialInventoryDetailsWithoutCutOff),
            };

            await Task.WhenAll(tasks).ConfigureAwait(false);
            tasks.Clear();

            tasks.Add(this.ExecuteAsync(entity, Constants.SaveMonthlyOfficialDataWithoutCutoff));
            tasks.Add(this.ExecuteAsync(entity, Constants.SaveMonthlyOfficialInventoryQualityDetailsWithoutCutOff));
            tasks.Add(this.ExecuteAsync(entity, Constants.SaveMonthlyOfficialMovementQualityDetailsWithoutCutOff));

            await Task.WhenAll(tasks).ConfigureAwait(false);
            tasks.Clear();
        }
    }
}
