// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaStrategyBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Delta
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    ///  the delta strategy base class.
    /// </summary>
    public abstract class DeltaStrategyBase : IDeltaStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaStrategyBase" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected DeltaStrategyBase(ITrueLogger logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ITrueLogger Logger { get; set; }

        /// <summary>
        /// Build Movement.
        /// </summary>
        /// <param name="deltaData">The deltaData.</param>
        /// <returns> The task.</returns>
        public abstract IEnumerable<Movement> Build(DeltaData deltaData);

        /// <summary>
        /// The ownership collection.
        /// </summary>
        /// <param name="ownerships">ownerships.</param>
        /// <param name="delta">the delta.</param>
        /// <returns>Ownerships.</returns>
        protected IEnumerable<Owner> CalculateOwners(IEnumerable<Ownership> ownerships, decimal delta)
        {
            var owners = ownerships
                          .Where(x => !x.IsDeleted)
                          .Select(x => new Owner
                          {
                              OwnerId = x.OwnerId,
                              OwnershipValue = ((delta * x.OwnershipPercentage) / 100).ToTrueDecimal(),
                              OwnershipValueUnit = "Volumen",
                          })?.ToList();

            // Rounding off
            if (owners != null && owners.Count > 0)
            {
                var lastOwner = owners.Last();
                lastOwner.OwnershipValue = delta - owners.Sum(x => x.OwnershipValue) + lastOwner.OwnershipValue;
            }

            return owners;
        }

        /// <summary>
        /// Creates the movement.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <param name="delta">The delta.</param>
        /// <returns>
        /// The movement.
        /// </returns>
        protected Movement CreateMovement(DeltaData deltaData, decimal delta)
        {
            ArgumentValidators.ThrowIfNull(deltaData, nameof(deltaData));
            return new Movement
            {
                MessageTypeId = (int)MessageType.Movement,
                SystemTypeId = (int)SystemType.TRUE,
                SourceSystemId = (int)SourceSystem.FICO,
                EventType = EventType.Insert.ToString(),
                MovementId = Convert.ToString(DateTime.UtcNow.ToTrue().Ticks, CultureInfo.InvariantCulture),
                MovementTypeId = 44,
                IsSystemGenerated = true,
                DeltaTicketId = deltaData.Ticket.TicketId,
                OperationalDate = deltaData.NextCutOffDate.Date,
                ScenarioId = ScenarioType.OPERATIONAL,
                Observations = string.Empty,
                NetStandardVolume = delta,
                Classification = string.Empty,
                SegmentId = deltaData.Ticket.CategoryElementId,
            };
        }
    }
}
