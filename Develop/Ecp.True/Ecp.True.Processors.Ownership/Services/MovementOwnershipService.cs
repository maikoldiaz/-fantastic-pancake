// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementOwnershipService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Processors.Ownership.Interfaces;
    using Ecp.True.Proxies.OwnershipRules.Response;

    /// <summary>
    /// The MovementOwnershipService.
    /// </summary>
    public class MovementOwnershipService : IMovementOwnershipService
    {
        /// <summary>
        /// Gets the movement ownerships.
        /// </summary>
        /// <param name="movementList">The movement list.</param>
        /// <returns>The collection of MovementOwnership.</returns>
        public IEnumerable<Ownership> GetMovementOwnerships(IEnumerable<OwnershipResultMovement> movementList)
        {
            ArgumentValidators.ThrowIfNull(movementList, nameof(movementList));
            var movementOwnerships = new List<Ownership>();

            foreach (var item in movementList)
            {
                movementOwnerships.Add(new Ownership
                {
                    TicketId = item.Ticket,
                    MovementTransactionId = item.MovementId,
                    OwnerId = item.OwnerId,
                    OwnershipPercentage = item.OwnershipPercentage,
                    OwnershipVolume = item.OwnershipVolume,
                    AppliedRule = item.AppliedRule,
                    RuleVersion = item.RuleVersion.ToString(CultureInfo.InvariantCulture),
                    ExecutionDate = item.ExecutionDate,
                    MessageTypeId = MessageType.MovementOwnership,
                    BlockchainOwnershipId = Guid.NewGuid(),
                });
            }

            return movementOwnerships;
        }
    }
}
