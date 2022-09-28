// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialDeltaBuilderBase.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Entities.OfficialDelta;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The official builder base.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Delta.Interfaces.IOfficialDeltaBuilder" />
    public abstract class OfficialDeltaBuilderBase<TEntity> : IOfficialDeltaBuilder
        where TEntity : class, IOfficialResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialDeltaBuilderBase{TEntity}" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected OfficialDeltaBuilderBase(ITrueLogger logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ITrueLogger Logger { get; }

        /// <summary>
        /// Builds the movement.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public abstract Task<IEnumerable<Movement>> BuildMovementsAsync(OfficialDeltaData deltaData);

        /// <summary>
        /// Builds the movement.
        /// </summary>
        /// <param name="deltaData">The delta data.</param>
        /// <returns>
        /// The task.
        /// </returns>
        public abstract Task<IEnumerable<DeltaNodeError>> BuildErrorsAsync(OfficialDeltaData deltaData);

        /// <summary>
        /// The ownership collection.
        /// </summary>
        /// <param name="result">result.</param>
        /// <param name="shouldCheckSign">the should check sign.</param>
        /// <returns>ownerships.</returns>
        protected IEnumerable<Owner> CalculateOwners(TEntity result, bool shouldCheckSign)
        {
            ArgumentValidators.ThrowIfNull(result, nameof(result));
            var owners = new List<Owner>
            {
                new Owner
                {
                    OwnerId = Convert.ToInt32(result.OwnerId, CultureInfo.InvariantCulture),
                    OwnershipValue = shouldCheckSign ? GetSignBasedOfficialDelta(result) : result.OfficialDelta,
                    OwnershipValueUnit = "Volumen",
                },
            };

            return owners;
        }

        /// <summary>
        /// Creates the movement.
        /// </summary>
        /// <param name="ticketId">The ticket identifier.</param>
        /// <returns>
        /// The movement.
        /// </returns>
        protected Movement CreateMovement(int ticketId)
        {
            return new Movement
            {
                MessageTypeId = (int)MessageType.Movement,
                SystemTypeId = (int)SystemType.TRUE,
                SourceSystemId = (int)SourceSystem.FICO,
                EventType = EventType.Insert.ToString(),
                MovementId = Convert.ToString(DateTime.UtcNow.ToTrue().Ticks, CultureInfo.InvariantCulture),
                MovementTypeId = 44,
                IsSystemGenerated = true,
                OfficialDeltaTicketId = ticketId,
                ScenarioId = ScenarioType.OFFICER,
                Observations = string.Empty,
                Classification = string.Empty,
                PendingApproval = true,
            };
        }

        private static decimal GetSignBasedOfficialDelta(TEntity x)
        {
            return x.Sign ? x.OfficialDelta : -1 * x.OfficialDelta;
        }
    }
}
