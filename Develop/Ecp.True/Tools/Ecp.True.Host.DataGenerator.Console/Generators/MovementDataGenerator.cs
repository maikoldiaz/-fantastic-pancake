// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementDataGenerator.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The movement data generator.
    /// </summary>
    public class MovementDataGenerator : DataGeneratorBase, IDataGenerator
    {
        /// <summary>
        /// The movement repository.
        /// </summary>
        private readonly IRepository<Movement> movementRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementDataGenerator"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public MovementDataGenerator(IUnitOfWork unitOfWork)
        {
            ArgumentValidators.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            this.movementRepository = unitOfWork.CreateRepository<Movement>();
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
            var movement = GetMovement(parameters);
            if (parameters.TryGetValue("HasDelta", out object hasDelta) && Convert.ToBoolean(hasDelta, CultureInfo.InvariantCulture))
            {
                var sourceNodeId = parameters["SourceNodeId"];
                var sourceProductId = parameters["SourceProductId"];
                parameters["SourceNodeId"] = parameters["DestinationNodeId"];
                parameters["SourceProductId"] = parameters["DestinationProductId"];
                parameters["DestinationNodeId"] = sourceNodeId;
                parameters["DestinationProductId"] = sourceProductId;
                parameters["MovementTypeId"] = parameters["CancellationMovementTypeId"];
                parameters["NetStandardVolume"] = parameters["Delta"];

                parameters["NoSource"] = true;
                var sourceDeltaMovement = GetMovement(parameters);
                sourceDeltaMovement.OriginalMovement = movement;

                parameters["NoSource"] = false;
                parameters["NoDestination"] = true;
                var destinationDeltaMovement = GetMovement(parameters);
                destinationDeltaMovement.OriginalMovement = movement;

                this.movementRepository.Insert(sourceDeltaMovement);
                this.movementRepository.Insert(destinationDeltaMovement);
            }
            else
            {
                this.movementRepository.Insert(movement);
            }

            return Task.FromResult(movement.MovementTransactionId);
        }

        private static Movement GetMovement(IDictionary<string, object> parameters)
        {
            var movement = new Movement
            {
                MessageTypeId = GetInt(parameters, "MessageTypeId", (int)MessageType.Movement),
                SystemTypeId = GetInt(parameters, "SystemTypeId", (int)SystemType.TRUE),
                SourceSystemId = GetInt(parameters, "SourceSystemId", (int)SourceSystem.TRUE),
                EventType = GetString(parameters, "EventType", EventType.Insert.ToString()),
                MovementTypeId = GetInt(parameters, "MovementTypeId", 156),
                MovementId = GetString(parameters, "MovementId", Convert.ToString(DateTime.UtcNow.ToTrue().Ticks, CultureInfo.InvariantCulture)),
                TicketId = GetInt(parameters, "TicketId", null),
                OperationalDate = GetDate(parameters, "OperationalDate", DateTime.UtcNow.Date.ToTrue()),
                MeasurementUnit = 31,
                ScenarioId = parameters.TryGetValue("ScenarioId", out object scenarioId) ? (ScenarioType)scenarioId : ScenarioType.OPERATIONAL,
                OfficialDeltaTicketId = GetInt(parameters, "OfficialDeltaTicketId", null),
                OfficialDeltaMessageTypeId = GetOfficialMessageTypeId(parameters),
                Observations = string.Empty,
                NetStandardVolume = GetDecimal(parameters, "NetStandardVolume", 10000.00M),
                GrossStandardVolume = 10000.00M,
                Classification = string.Empty,
                OwnershipTicketId = GetInt(parameters, "OwnershipTicketId", null),
                SegmentId = GetInt(parameters, "SegmentId", 10),
                SystemId = GetInt(parameters, "SystemId", null),
                IsSystemGenerated = GetBoolean(parameters, "IsSystemGenerated", null),
                MovementDestination = parameters.TryGetValue("NoDestination", out object noDestination) && Convert.ToBoolean(noDestination, CultureInfo.InvariantCulture) ? null
            : GetMovementDestination(parameters),
                MovementSource = parameters.TryGetValue("NoSource", out object noSource) && Convert.ToBoolean(noSource, CultureInfo.InvariantCulture) ? null
            : GetMovementSource(parameters),
                Ownerships = GetOwnerships(parameters).ToList(),
                Period = !parameters.TryGetValue("StartDate", out object movPeriodStartDate) ? null : GetMovementPeriod(parameters),
                BlockchainStatus = StatusType.PROCESSED,
            };

            movement.Owners.AddRange(GetOwners(parameters));
            return movement;
        }

        private static MovementPeriod GetMovementPeriod(IDictionary<string, object> parameters)
        {
            return new MovementPeriod
            {
                StartTime = GetDate(parameters, "StartDate", DateTime.UtcNow),
                EndTime = GetDate(parameters, "EndDate", DateTime.UtcNow),
            };
        }

        private static OfficialDeltaMessageType? GetOfficialMessageTypeId(IDictionary<string, object> parameters)
        {
            if (parameters.TryGetValue("OfficialDeltaMessageTypeId", out object officialDeltaMessageTypeId) && parameters["OfficialDeltaMessageTypeId"] != null)
            {
                return (OfficialDeltaMessageType)parameters["OfficialDeltaMessageTypeId"];
            }

            return null;
        }

        private static MovementDestination GetMovementDestination(IDictionary<string, object> parameters)
        {
            return new MovementDestination
            {
                DestinationNodeId = GetInt(parameters, "DestinationNodeId", null),
                DestinationStorageLocationId = GetInt(parameters, "DestinationStorageLocationId", null),
                DestinationProductId = GetString(parameters, "DestinationProductId", "10000002318"),
                DestinationProductTypeId = GetInt(parameters, "DestinationProductTypeId", 87194),
            };
        }

        private static MovementSource GetMovementSource(IDictionary<string, object> parameters)
        {
            return new MovementSource
            {
                SourceNodeId = GetInt(parameters, "SourceNodeId", null),
                SourceStorageLocationId = GetInt(parameters, "SourceStorageLocationId", null),
                SourceProductId = GetString(parameters, "SourceProductId", "10000002318"),
                SourceProductTypeId = GetInt(parameters, "SourceProductTypeId", 87194),
            };
        }

        private static IEnumerable<Ownership> GetOwnerships(IDictionary<string, object> parameters)
        {
            var volume = GetDecimal(parameters, "NetStandardVolume", 10000.00M);
            var ticketId = GetInt(parameters, "OwnershipTicketId", null);
            var notRequiresOwnership = parameters.TryGetValue("NotRequiresOwnership", out object notRequireOwnership) && Convert.ToBoolean(notRequireOwnership, CultureInfo.InvariantCulture);
            if (notRequiresOwnership)
            {
                return new List<Ownership>();
            }

            return new List<Ownership>
            {
                new Ownership
            {
                OwnerId = 30,
                OwnershipPercentage = 60.00M,
                OwnershipVolume = volume * 0.6M,
                AppliedRule = "1",
                RuleVersion = "1",
                TicketId = ticketId.GetValueOrDefault(1),
                ExecutionDate = DateTime.UtcNow.ToTrue(),
                MessageTypeId = MessageType.MovementOwnership,
                BlockchainStatus = StatusType.PROCESSING,
            },
                new Ownership
            {
                OwnerId = 124,
                OwnershipPercentage = 40.00M,
                OwnershipVolume = volume * 0.4M,
                AppliedRule = "1",
                RuleVersion = "1",
                TicketId = ticketId.GetValueOrDefault(1),
                ExecutionDate = DateTime.UtcNow.ToTrue(),
                MessageTypeId = MessageType.MovementOwnership,
                BlockchainStatus = StatusType.PROCESSING,
            },
            };
        }

        private static IEnumerable<Owner> GetOwners(IDictionary<string, object> parameters)
        {
            var requiresOwner = parameters.TryGetValue("RequiresOwner", out object requireOwner) && Convert.ToBoolean(requireOwner, CultureInfo.InvariantCulture);
            if (!requiresOwner)
            {
                return new List<Owner>();
            }

            return new List<Owner>
            {
                new Owner
            {
                OwnerId = 30,
                OwnershipValue = 60,
                OwnershipValueUnit = "%",
                BlockchainStatus = StatusType.PROCESSING,
            },
                new Owner
            {
                OwnerId = 124,
                OwnershipValue = 40,
                OwnershipValueUnit = "%",
                BlockchainStatus = StatusType.PROCESSING,
            },
            };
        }
    }
}
