// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainExtensionsTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Blockchain.Tests
{
    using System;
    using System.Globalization;
    using Ecp.True.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Blockchain;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The tests for block chain extensions.
    /// </summary>
    [TestClass]
    public class BlockchainExtensionsTests
    {
        /// <summary>
        /// Gets the metadata should serialize movement.
        /// </summary>
        [TestMethod]
        public void GetMetadata_ShouldSerializeMovement()
        {
            var movement = new Movement
            {
                BackupMovementId = "Test",
                ScenarioId = Entities.Enumeration.ScenarioType.OFFICER,
            };

            var result = movement.GetMetadata();

            Assert.AreEqual("Test,,,,,,False,,,2,,,,,,,,,,0", result);
        }

        /// <summary>
        /// Gets the metadata should serialize inventory.
        /// </summary>
        [TestMethod]
        public void GetMetadata_ShouldSerializeInventory()
        {
            var inventoryProduct = new InventoryProduct
            {
                BatchId = "Test",
                SegmentId = 5,
                ScenarioId = Entities.Enumeration.ScenarioType.OFFICER,
                InventoryId = "Id",
            };

            var result = inventoryProduct.GetMetadata();

            Assert.AreEqual("Test,,,,2,5,,,,,0,,Id,", result);
        }

        /// <summary>
        /// Converts to blockchainnumber_shouldconvertdecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToBlockChainNumber_ShouldConvertDecimal_WhenInvoked()
        {
            decimal test = 12.35M;
            Assert.AreEqual(1235, test.ToBlockChainNumber());
        }

        /// <summary>
        /// Converts to blockchainnumber_shouldconvertdecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToBlockChainNumber_ShouldConvertNullableDecimal_WhenInvoked()
        {
            decimal? test = 12.35M;
            Assert.AreEqual(1235, test.ToBlockChainNumber());
        }

        /// <summary>
        /// Converts to blockchainnumber_shouldconvertdecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToBlockChainNumberString_ShouldConvertDecimal_WhenInvoked()
        {
            decimal test = 12.35M;
            Assert.AreEqual("1235", test.ToBlockChainNumberString());
        }

        /// <summary>
        /// Converts to blockchainnumberstring_shouldconvertnullabledecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToBlockChainNumberString_ShouldConvertNullableDecimal_WhenInvoked()
        {
            decimal? test = 12.35M;
            Assert.AreEqual("1235", test.ToBlockChainNumberString());
        }

        /// <summary>
        /// Converts to decimal_shouldconverttodecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDecimal_ShouldConvertEmptyStringToDecimal_WhenInvoked()
        {
            Assert.AreEqual(0.00M, string.Empty.ToDecimal());
        }

        /// <summary>
        /// Converts to decimal_shouldconverttodecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDecimal_ShouldConvertToDecimal_WhenInvoked()
        {
            Assert.AreEqual(12.35M, "1235".ToDecimal());
        }

        /// <summary>
        /// Converts to decimal_shouldconverttodecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDecimal_ShouldConvertDefaultIntToDecimal_WhenInvoked()
        {
            Assert.AreEqual(0.00M, default(int).ToDecimal());
        }

        /// <summary>
        /// Converts to decimal_shouldconverttodecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDecimal_ShouldConvertIntToDecimal_WhenInvoked()
        {
            Assert.AreEqual(12.35M, 1235.ToDecimal());
        }

        /// <summary>
        /// Converts to decimal_shouldconverttodecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDecimal_ShouldConvertDefaultLongToDecimal_WhenInvoked()
        {
            Assert.AreEqual(0.00M, default(long).ToDecimal());
        }

        /// <summary>
        /// Converts to decimal_shouldconverttodecimal_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDecimal_ShouldConvertLongToDecimal_WhenInvoked()
        {
            Assert.AreEqual(12.35M, 1235L.ToDecimal());
        }

        /// <summary>
        /// Converts to datetimefromepoch_shouldconvertdefaultintticks_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDateTimeFromEpoch_ShouldConvertDefaultIntTicks_WhenInvoked()
        {
            Assert.IsNull(default(int).ToDateTimeFromEpoch());
        }

        /// <summary>
        /// Converts to datetimefromepoch_shouldconvertintticks_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDateTimeFromEpoch_ShouldConvertIntTicks_WhenInvoked()
        {
            var epoch = 100000;
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var result = dt.AddSeconds(epoch).ToTrue();

            Assert.AreEqual(result, epoch.ToDateTimeFromEpoch());
        }

        /// <summary>
        /// Converts to datetimefromepoch_shouldconvertdefaultintticks_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDateTimeFromEpoch_ShouldConvertDefaultLongTicks_WhenInvoked()
        {
            Assert.IsNull(default(ulong).ToDateTimeFromEpoch());
        }

        /// <summary>
        /// Converts to datetimefromepoch_shouldconvertintticks_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToDateTimeFromEpoch_ShouldConvertLongTicks_WhenInvoked()
        {
            ulong epoch = 100000;
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var result = dt.AddSeconds(epoch).ToTrue();

            Assert.AreEqual(result, epoch.ToDateTimeFromEpoch());
        }

        /// <summary>
        /// Converts to truedatestring_shouldconvertdefaultlongticks_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToTrueDateString_ShouldConvertDefaultLongTicks_WhenInvoked()
        {
            Assert.AreEqual(string.Empty, default(long).ToTrueDateString());
        }

        /// <summary>
        /// Converts to truedatestring_shouldconvertdefaultlongticks_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToTrueDateString_ShouldConvertLongTicks_WhenInvoked()
        {
            var ticks = DateTime.Now.Ticks;
            var result = new DateTime(ticks).ToString("dd-MMM-yy", CultureInfo.InvariantCulture);
            Assert.AreEqual(result, ticks.ToTrueDateString());
        }

        /// <summary>
        /// Converts to eventtype_shouldreturneventtypestring_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToEventType_ShouldReturnEventTypeString_WhenInvoked()
        {
            Assert.AreEqual(EventType.Insert.ToString("G"), default(int).ToEventType());
            Assert.AreEqual(EventType.Update.ToString("G"), 1.ToEventType());
            Assert.AreEqual(EventType.Delete.ToString("G"), 2.ToEventType());
            Assert.AreEqual(string.Empty, 3.ToEventType());
        }

        /// <summary>
        /// Converts to nodestatetype_shouldreturnnodestatetypestring_wheninvoked.
        /// </summary>
        [TestMethod]
        public void ToNodeStateType_ShouldReturnNodeStateTypeString_WhenInvoked()
        {
            Assert.AreEqual(NodeState.CreatedNode.ToString("G"), 1.ToNodeState());
            Assert.AreEqual(NodeState.UpdatedNode.ToString("G"), 2.ToNodeState());
            Assert.AreEqual(NodeState.OperativeBalanceCalculated.ToString("G"), 3.ToNodeState());
            Assert.AreEqual(NodeState.OperativeBalanceCalculatedWithOwnership.ToString("G"), 4.ToNodeState());
            Assert.AreEqual(string.Empty, 5.ToNodeState());
        }
    }
}
