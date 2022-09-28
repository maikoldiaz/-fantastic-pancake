// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventDecoderV1Tests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Blockchain.Tests.Decoders
{
    using System.Threading.Tasks;
    using Ecp.True.Processors.Blockchain.Decoders;
    using Ecp.True.Processors.Blockchain.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The event decoder version 1 tests.
    /// </summary>
    [TestClass]
    public class EventDecoderV1Tests
    {
        /// <summary>
        /// The decoder.
        /// </summary>
        private EventDecoderV1 decoder;

        /// <summary>
        /// The event mock.
        /// </summary>
        private Mock<IBlockchainEventV1> eventMock;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.decoder = new EventDecoderV1();
            this.eventMock = new Mock<IBlockchainEventV1>();
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeNode_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(8);
            this.eventMock.Setup(e => e.Id).Returns("10");
            this.eventMock.Setup(e => e.Data).Returns("Node1");

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.AreEqual(2, eventInfo.Count);
            Assert.AreEqual("10", eventInfo["NodeId"]);
            Assert.AreEqual("Node1", eventInfo["Name"]);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeNodeConnection_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(9);
            this.eventMock.Setup(e => e.Id).Returns("10");

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.AreEqual(1, eventInfo.Count);
            Assert.AreEqual("10", eventInfo["NodeConnectionId"]);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeUnbalance_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(7);
            this.eventMock.Setup(e => e.Id).Returns("10");
            this.eventMock.Setup(e => e.Data).Returns("1,2,3,4,5,6,7,8,9");

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.AreEqual(10, eventInfo.Count);
            Assert.AreEqual("10", eventInfo["UnbalanceId"]);
            Assert.AreEqual(0.01M, eventInfo["InitialInventory"]);
            Assert.AreEqual(0.02M, eventInfo["FinalInventory"]);
            Assert.AreEqual(0.03M, eventInfo["Inputs"]);
            Assert.AreEqual(0.04M, eventInfo["Outputs"]);
            Assert.AreEqual(0.05M, eventInfo["IdentifiedLosses"]);
            Assert.AreEqual(0.06M, eventInfo["Interface"]);
            Assert.AreEqual(0.07M, eventInfo["Tolerance"]);
            Assert.AreEqual(0.08M, eventInfo["UnidentifiedLosses"]);
            Assert.AreEqual(0.09M, eventInfo["UnbalanceAmount"]);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeMovementOwnership_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(5);
            this.eventMock.Setup(e => e.Id).Returns("10");
            this.eventMock.Setup(e => e.Data).Returns("MovOwner1-MovTrx1");

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.AreEqual(3, eventInfo.Count);
            Assert.AreEqual("10", eventInfo["OwnershipId"]);
            Assert.AreEqual("MovOwner1", eventInfo["OwnerId"]);
            Assert.AreEqual("MovTrx1", eventInfo["TransactionId"]);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeInventoryOwnership_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(6);
            this.eventMock.Setup(e => e.Id).Returns("10");
            this.eventMock.Setup(e => e.Data).Returns("InvOwner1-InvTrx1");

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.AreEqual(3, eventInfo.Count);
            Assert.AreEqual("10", eventInfo["OwnershipId"]);
            Assert.AreEqual("InvOwner1", eventInfo["OwnerId"]);
            Assert.AreEqual("InvTrx1", eventInfo["TransactionId"]);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeMovementOwner_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(2);
            this.eventMock.Setup(e => e.Id).Returns("10");
            this.eventMock.Setup(e => e.Data).Returns("MovOwner1");

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.AreEqual(2, eventInfo.Count);
            Assert.AreEqual("10", eventInfo["Id"]);
            Assert.AreEqual("MovOwner1", eventInfo["OwnerId"]);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeInventoryOwner_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(4);
            this.eventMock.Setup(e => e.Id).Returns("10");
            this.eventMock.Setup(e => e.Data).Returns("InvOwner1");

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.AreEqual(2, eventInfo.Count);
            Assert.AreEqual("10", eventInfo["Id"]);
            Assert.AreEqual("InvOwner1", eventInfo["OwnerId"]);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeMovement_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(1);
            this.eventMock.Setup(e => e.Id).Returns("10");
            this.eventMock.Setup(e => e.Data).Returns("1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18");

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.AreEqual(19, eventInfo.Count);
            Assert.AreEqual("10", eventInfo["TransactionId"]);
            Assert.AreEqual("1", eventInfo["BackupMovementId"]);
            Assert.AreEqual("2", eventInfo["CreatedDate"]);
            Assert.AreEqual("3", eventInfo["EndTime"]);
            Assert.AreEqual("4", eventInfo["StartTime"]);
            Assert.AreEqual("5", eventInfo["EventType"]);
            Assert.AreEqual("6", eventInfo["GlobalMovementId"]);
            Assert.AreEqual("7", eventInfo["IsOfficial"]);
            Assert.AreEqual("8", eventInfo["MovementContractId"]);
            Assert.AreEqual("9", eventInfo["MovementEventId"]);
            Assert.AreEqual("10", eventInfo["ScenarioId"]);
            Assert.AreEqual("11", eventInfo["SegmentId"]);
            Assert.AreEqual("12", eventInfo["SourceSystemId"]);
            Assert.AreEqual("13", eventInfo["UncertaintyPercentage"]);
            Assert.AreEqual("14", eventInfo["Version"]);
            Assert.AreEqual("15", eventInfo["SourceNodeId"]);
            Assert.AreEqual("16", eventInfo["SourceProductId"]);
            Assert.AreEqual("17", eventInfo["DestinationNodeId"]);
            Assert.AreEqual("18", eventInfo["DestinationProductId"]);
        }

        /// <summary>
        /// Decoders the should decode node when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task Decoder_ShouldDecodeInventory_WhenInvokedAsync()
        {
            this.eventMock.Setup(e => e.Type).Returns(3);
            this.eventMock.Setup(e => e.Id).Returns("10");
            this.eventMock.Setup(e => e.Data).Returns("1,2,3,4,5,6,7,8,9,10,11,12");

            var eventInfo = await this.decoder.DecodeAsync(this.eventMock.Object).ConfigureAwait(false);

            Assert.IsNotNull(eventInfo);
            Assert.AreEqual(13, eventInfo.Count);
            Assert.AreEqual("10", eventInfo["TransactionId"]);
            Assert.AreEqual("1", eventInfo["BatchId"]);
            Assert.AreEqual("2", eventInfo["CreatedDate"]);
            Assert.AreEqual("3", eventInfo["EventType"]);
            Assert.AreEqual("4", eventInfo["ProductType"]);
            Assert.AreEqual("5", eventInfo["ScenarioId"]);
            Assert.AreEqual("6", eventInfo["SegmentId"]);
            Assert.AreEqual("7", eventInfo["SourceSystemId"]);
            Assert.AreEqual("8", eventInfo["TankName"]);
            Assert.AreEqual("9", eventInfo["UncertaintyPercentage"]);
            Assert.AreEqual("10", eventInfo["Version"]);
            Assert.AreEqual("11", eventInfo["NodeId"]);
            Assert.AreEqual("12", eventInfo["ProductId"]);
        }
    }
}
