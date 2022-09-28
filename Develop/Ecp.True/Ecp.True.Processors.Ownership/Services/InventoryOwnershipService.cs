// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryOwnershipService.cs" company="Microsoft">
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
    using OwnershipResultInventory = Ecp.True.Proxies.OwnershipRules.Response.OwnershipResultInventory;

    /// <summary>
    /// The InventoryOwnershipService.
    /// </summary>
    public class InventoryOwnershipService : IInventoryOwnershipService
    {
        /// <summary>
        /// Gets the inventory ownerships asynchronous.
        /// </summary>
        /// <param name="inventoryList">The inventory list.</param>
        /// <returns>The collection of InventoryOwnership.</returns>
        public IEnumerable<Ownership> GetInventoryOwnerships(IEnumerable<OwnershipResultInventory> inventoryList)
        {
            ArgumentValidators.ThrowIfNull(inventoryList, nameof(inventoryList));
            var inventoryOwnership = new List<Ownership>();

            foreach (var item in inventoryList)
            {
                inventoryOwnership.Add(new Ownership
                {
                    TicketId = item.Ticket,
                    InventoryProductId = item.InventoryId,
                    OwnerId = item.OwnerId,
                    OwnershipPercentage = item.OwnershipPercentage,
                    OwnershipVolume = item.OwnershipVolume,
                    AppliedRule = item.AppliedRule,
                    RuleVersion = item.RuleVersion.ToString(CultureInfo.InvariantCulture),
                    ExecutionDate = item.ExecutionDate,
                    MessageTypeId = MessageType.InventoryOwnership,
                    BlockchainOwnershipId = Guid.NewGuid(),
                });
            }

            return inventoryOwnership;
        }
    }
}
