// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperativeOwnershipReportGenerator.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Logging;
    using Constants = Ecp.True.Repositories.Constants;

    /// <summary>
    /// The Operational Data Without Ownership report generator.
    /// </summary>
    public class OperativeOwnershipReportGenerator : ReportGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperativeOwnershipReportGenerator" /> class.
        /// </summary>
        /// <param name="unitOfWorkFactory">The unit of work factory.</param>
        /// <param name="configurationHandler">The configuration handler.</param>
        /// <param name="logger">The logger.</param>
        public OperativeOwnershipReportGenerator(IUnitOfWorkFactory unitOfWorkFactory, IConfigurationHandler configurationHandler, ITrueLogger<OperativeOwnershipReportGenerator> logger)
            : base(unitOfWorkFactory, configurationHandler, logger)
        {
        }

        /// <inheritdoc/>
        public override ReportType Type => ReportType.OperativeBalance;

        /// <inheritdoc/>
        protected override async Task DoGenerateAsync(ReportExecution entity)
        {
            ArgumentValidators.ThrowIfNull(entity, nameof(entity));

            await this.ExecuteAsync(entity, Constants.SaveNonSonSegmentInventoryDetails).ConfigureAwait(false);
            await this.ExecuteAsync(entity, Constants.SaveNonSonSegmentInventoryQualityDetails).ConfigureAwait(false);
            await this.ExecuteAsync(entity, Constants.SaveNonSonSegmentMovementDetails).ConfigureAwait(false);
            await this.ExecuteAsync(entity, Constants.SaveNonSonSegmentMovementQualityDetails).ConfigureAwait(false);
            await this.ExecuteAsync(entity, Constants.SaveNonSonSegment).ConfigureAwait(false);
        }
    }
}
