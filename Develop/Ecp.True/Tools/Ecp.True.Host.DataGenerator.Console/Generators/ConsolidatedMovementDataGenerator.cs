// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsolidatedMovementDataGenerator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The Consolidated Movement Data Generator.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Generators.DataGeneratorBase" />
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Interfaces.IDataGenerator" />
    public class ConsolidatedMovementDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The consolidated movement repository.
        /// </summary>
        private readonly IRepository<ConsolidatedMovement> consolidatedMovementRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedMovementDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ConsolidatedMovementDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.consolidatedMovementRepository = unitOfWork.CreateRepository<ConsolidatedMovement>();
        }

        /// <summary>
        /// Generates the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The identity.
        /// </returns>
        public Task<int> GenerateAsync(IDictionary<string, object> parameters)
        {
            ArgumentValidators.ThrowIfNull(parameters, nameof(parameters));

            var consolidatedMovement = GetConsolidatedMovement(parameters);
            this.consolidatedMovementRepository.Insert(consolidatedMovement);
            return Task.FromResult(consolidatedMovement.ConsolidatedMovementId);
        }

        private static ConsolidatedMovement GetConsolidatedMovement(IDictionary<string, object> parameters)
        {
            var consolidatedMovement = new ConsolidatedMovement
            {
                SourceNodeId = GetInt(parameters, "SourceNodeId", null),
                SourceProductId = GetString(parameters, "SourceProductId", null),
                DestinationNodeId = GetInt(parameters, "DestinationNodeId", null),
                DestinationProductId = GetString(parameters, "DestinationProductId", null),
                MovementTypeId = GetString(parameters, "MovementTypeId", "156"),
                StartDate = GetDate(parameters, "StartDate", DateTime.UtcNow.ToTrue()),
                EndDate = GetDate(parameters, "EndDate", DateTime.UtcNow.ToTrue()),
                NetStandardVolume = GetDecimal(parameters, "NetStandardVolume", 100000.00M),
                GrossStandardVolume = GetDecimal(parameters, "GrossStandardVolume", 120000.00M),
                MeasurementUnit = "31",
                TicketId = GetInt(parameters, "TicketId", 1),
                SegmentId = GetInt(parameters, "SegmentId", 1),
                SourceSystemId = GetInt(parameters, "SourceSystemId", (int)SystemType.TRUE),
                ExecutionDate = GetDate(parameters, "ExecutionDate", DateTime.UtcNow.ToTrue()),
                IsActive = true,
            };
            GetConsolidatedOwners(parameters).ForEach(x => consolidatedMovement.ConsolidatedOwners.Add(x));
            return consolidatedMovement;
        }

        private static IEnumerable<ConsolidatedOwner> GetConsolidatedOwners(IDictionary<string, object> parameters)
        {
            var volume = parameters.TryGetValue("ProductVolume", out object productVolume) ? Convert.ToDecimal(productVolume, CultureInfo.InvariantCulture) : 10000.00M;
            return new List<ConsolidatedOwner>
            {
                new ConsolidatedOwner
            {
                OwnerId = 30,
                OwnershipVolume = volume * 0.60M,
                OwnershipPercentage = 0.60M,
            },
                new ConsolidatedOwner
            {
                OwnerId = 124,
                OwnershipVolume = volume * 0.40M,
                OwnershipPercentage = 0.40M,
            },
            };
        }
    }
}
