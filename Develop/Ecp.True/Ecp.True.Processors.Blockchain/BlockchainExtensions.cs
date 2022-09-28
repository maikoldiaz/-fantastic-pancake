// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainExtensions.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Blockchain
{
    using System;
    using System.Globalization;
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The Extensions for Blockchain.
    /// </summary>
    public static class BlockchainExtensions
    {
        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <param name="inventoryProduct">The inventory product.</param>
        /// <returns>The Inventory Metadata.</returns>
        public static string GetMetadata(this InventoryProduct inventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));

            return string.Join(
                ',',
                inventoryProduct.BatchId,
                inventoryProduct.CreatedDate?.ToTrueString(),
                inventoryProduct.EventType,
                inventoryProduct.ProductType,
                (int)inventoryProduct.ScenarioId,
                inventoryProduct.SegmentId,
                inventoryProduct.SourceSystemId,
                inventoryProduct.TankName,
                inventoryProduct.UncertaintyPercentage,
                inventoryProduct.Version,
                inventoryProduct.NodeId,
                inventoryProduct.ProductId,
                inventoryProduct.InventoryId,
                inventoryProduct.InventoryProductUniqueId);
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <param name="movement">The movement.</param>
        /// <returns>The Movement Metadata.</returns>
        public static string GetMetadata(this Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));

            return string.Join(
                ',',
                movement.BackupMovementId,
                movement.CreatedDate?.ToTrueString(),
                movement.Period?.EndTime?.ToTrueString(),
                movement.Period?.StartTime?.ToTrueString(),
                movement.EventType,
                movement.GlobalMovementId,
                movement.IsOfficial,
                movement.MovementContractId,
                movement.MovementEventId,
                (int)movement.ScenarioId,
                movement.SegmentId,
                movement.SourceSystemId,
                movement.UncertaintyPercentage,
                movement.Version,
                movement.MovementSource?.SourceNodeId,
                movement.MovementSource?.SourceProductId,
                movement.MovementDestination?.DestinationNodeId,
                movement.MovementDestination?.DestinationProductId,
                movement.MovementId,
                movement.MovementTypeId);
        }

        /// <summary>Converts to blockchainnumber.</summary>
        /// <param name="input">The input.</param>
        /// <returns>Return the integer for Blockchain properties.</returns>
        public static long ToBlockChainNumber(this decimal input)
        {
            return Convert.ToInt64(input.ToTrueDecimal() * 100, CultureInfo.InvariantCulture);
        }

        /// <summary>Converts to blockchainnumber.</summary>
        /// <param name="input">The input.</param>
        /// <returns>Return the integer for Blockchain properties.</returns>
        public static long ToBlockChainNumber(this decimal? input)
        {
            return input.GetValueOrDefault().ToBlockChainNumber();
        }

        /// <summary>
        /// Converts to blockchain number string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The string block chain number.</returns>
        public static string ToBlockChainNumberString(this decimal input)
        {
            return Convert.ToString(input.ToBlockChainNumber(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts to blockchain number string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The string block chain number.</returns>
        public static string ToBlockChainNumberString(this decimal? input)
        {
            return Convert.ToString(input.ToBlockChainNumber(), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="blockchainNumber">The blockchain number.</param>
        /// <returns>The decimal representation of blockchain number.</returns>
        public static decimal ToDecimal(this string blockchainNumber)
        {
            if (string.IsNullOrWhiteSpace(blockchainNumber))
            {
                return 0.00M;
            }

            return Convert.ToDecimal(blockchainNumber, CultureInfo.InvariantCulture) / 100;
        }

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="blockchainNumber">The blockchain number.</param>
        /// <returns>The decimal.</returns>
        public static decimal ToDecimal(this int blockchainNumber)
        {
            if (blockchainNumber == default)
            {
                return 0.00M;
            }

            return Convert.ToDecimal(blockchainNumber, CultureInfo.InvariantCulture) / 100;
        }

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="blockchainNumber">The blockchain number.</param>
        /// <returns>The decimal.</returns>
        public static decimal ToDecimal(this long blockchainNumber)
        {
            if (blockchainNumber == default)
            {
                return 0.00M;
            }

            return Convert.ToDecimal(blockchainNumber, CultureInfo.InvariantCulture) / 100;
        }

        /// <summary>
        /// Converts to datetime from UNIX epoch.
        /// </summary>
        /// <param name="epoch">The epoch.</param>
        /// <returns>The date time.</returns>
        public static DateTime? ToDateTimeFromEpoch(this int epoch)
        {
            if (epoch == 0)
            {
                return null;
            }

            // Unix timestamp is seconds past epoch
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(epoch).ToTrue();
        }

        /// <summary>
        /// Converts to datetime from UNIX epoch.
        /// </summary>
        /// <param name="epoch">The epoch.</param>
        /// <returns>The date time.</returns>
        public static DateTime? ToDateTimeFromEpoch(this ulong epoch)
        {
            if (epoch == 0)
            {
                return null;
            }

            // Unix timestamp is seconds past epoch
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(epoch).ToTrue();
        }

        /// <summary>
        /// Converts to truedatestring.
        /// </summary>
        /// <param name="ticks">The ticks.</param>
        /// <returns>The string date.</returns>
        public static string ToTrueDateString(this long ticks)
        {
            if (ticks == 0)
            {
                return string.Empty;
            }

            return new DateTime(ticks).ToString("dd-MMM-yy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts to eventtype.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <returns>The event type.</returns>
        public static string ToEventType(this int actionType)
        {
            if (actionType == default)
            {
                return EventType.Insert.ToString("G");
            }

            if (actionType == 1)
            {
                return EventType.Update.ToString("G");
            }

            if (actionType == 2)
            {
                return EventType.Delete.ToString("G");
            }

            return string.Empty;
        }

        /// <summary>
        /// Converts to nodestate.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>The node state.</returns>
        public static string ToNodeState(this int state)
        {
            if (state == 1)
            {
                return NodeState.CreatedNode.ToString("G");
            }

            if (state == 2)
            {
                return NodeState.UpdatedNode.ToString("G");
            }

            if (state == 3)
            {
                return NodeState.OperativeBalanceCalculated.ToString("G");
            }

            if (state == 4)
            {
                return NodeState.OperativeBalanceCalculatedWithOwnership.ToString("G");
            }

            return string.Empty;
        }
    }
}
