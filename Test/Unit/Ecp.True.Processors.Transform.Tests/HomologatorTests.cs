// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Homologate;
    using Ecp.True.Processors.Transform.Homologate.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The homologator test class.
    /// </summary>
    [TestClass]
    public class HomologatorTests : HomologatorTestBase
    {
        /// <summary>
        /// The homologator.
        /// </summary>
        private Homologator homologator;

        /// <summary>
        /// The homologation mapper.
        /// </summary>
        private Mock<IHomologationMapper> homologationMapper;

        /// <summary>
        /// The transformation mapper.
        /// </summary>
        private Mock<ITransformationMapper> transformationMapper;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<Homologator>> mockLogger;

        /// <summary>
        /// The transaction generator mock.
        /// </summary>
        private IFileRegistrationTransactionGenerator transactionGenerator;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockLogger = new Mock<ITrueLogger<Homologator>>();
            this.homologationMapper = new Mock<IHomologationMapper>();
            this.transactionGenerator = new FileRegistrationTransactionGenerator();
            this.transformationMapper = new Mock<ITransformationMapper>();
            this.homologator = new Homologator(this.homologationMapper.Object, this.transformationMapper.Object, this.mockLogger.Object, this.transactionGenerator);
        }

        /// <summary>
        /// Homologates the object asynchronous should homologate object when invoked asynchronous.
        /// </summary>
        [TestMethod]
        public void HomologateObjectAsync_Should_HomologateObjectWhenInvoked()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            var objectName = "Classification";
            var originalValue = "Movimiento";
            var updateValue = "Movimiento_Updated";
            this.homologationMapper.Setup(x => x.Homologate(messge, objectName, originalValue)).Returns(updateValue);
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObject();
            var result = this.homologator.HomologateObject(messge, jobject);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, objectName, originalValue), Times.AtLeast(1));
        }

        /// <summary>
        /// Homologates the object asynchronous should not homologate object when invoked asynchronous for Source system SAP and ShouldHomologate = false for Retry.
        /// </summary>
        [TestMethod]
        public void HomologateObjectAsync_ShouldNot_HomologateObjectWhenInvoked()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SAP, TargetSystem = SystemType.TRUE, IsRetry = true, ShouldHomologate = false };
            var objectName = "Classification";
            var originalValue = "Movimiento";
            var updateValue = "Movimiento_Updated";
            this.homologationMapper.Setup(x => x.Homologate(messge, objectName, originalValue)).Returns(updateValue);
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObject();
            var result = this.homologator.HomologateObject(messge, jobject);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, objectName, originalValue), Times.Never);
        }

        /// <summary>
        /// Homologates the asynchronous should homologate when invoked.
        /// </summary>
        /// <returns>
        /// The Tasks.
        /// </returns>
        [TestMethod]
        public async Task HomologateAsync_Should_HomologateWhenInvokedAsync()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            var objectName = "Classification";
            var originalValue = "Movimiento";
            var updateValue = "Movimiento_Updated";
            var typetName = "Type";
            var originalTypeValue = "Movement";
            var updateTypeValue = "Movement";
            this.homologationMapper.Setup(x => x.Homologate(messge, objectName, originalValue)).Returns(updateValue);
            this.homologationMapper.Setup(x => x.Homologate(messge, typetName, originalTypeValue)).Returns(updateTypeValue);

            var jobject = this.GetJObject();
            var array = new JArray();
            array.Add(jobject);
            var result = await this.homologator.HomologateAsync(messge, array, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, objectName, originalValue), Times.AtLeast(1));
        }

        /// <summary>
        /// Homologates the asynchronous verify string type homologation asynchronous.
        /// </summary>
        /// <returns>The Tasks.</returns>
        [TestMethod]
        public async Task HomologateAsync_VerifyStringTypeHomologationAsync()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            this.homologationMapper.Setup(x => x.Homologate(messge, "SourceSystem", "SINOPER")).Returns("SINOPER");
            this.homologationMapper.Setup(x => x.Homologate(messge, "Classification", "Movimiento")).Returns("Movimiento");
            this.homologationMapper.Setup(x => x.Homologate(messge, "ValueAttributeUnit", "Bls")).Returns("Bls");
            this.homologationMapper.Setup(x => x.Homologate(messge, "ValueAttributeUnit", "SALGAR:17:ECOPETROL")).Returns("SALGAR:17:ECOPETROL");
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObjectWithValidDataTypes();
            var array = new JArray();
            array.Add(jobject);
            var result = await this.homologator.HomologateAsync(messge, array, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, "SourceSystem", "SINOPER"), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "Classification", "Movimiento"), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "ValueAttributeUnit", "Bls"), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "DestinationNodeId", "SALGAR:17:ECOPETROL"), Times.Once);
        }

        /// <summary>
        /// Homologates the asynchronous verify integer type homologation asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologateAsync_VerifyIntegerTypeHomologationAsync()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            this.homologationMapper.Setup(x => x.Homologate(messge, "MovementId", It.IsAny<long>())).Returns(600);
            this.homologationMapper.Setup(x => x.Homologate(messge, "AttributeId", It.IsAny<long>())).Returns(11);
            this.homologationMapper.Setup(x => x.Homologate(messge, "AttributeId", It.IsAny<long>())).Returns(12);
            this.homologationMapper.Setup(x => x.Homologate(messge, "DestinationStorageLocationId", It.IsAny<long>())).Returns(11);
            this.homologationMapper.Setup(x => x.Homologate(messge, "DestinationProductId", It.IsAny<long>())).Returns(100);
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObjectWithValidDataTypes();
            var array = new JArray();
            array.Add(jobject);
            var result = await this.homologator.HomologateAsync(messge, array, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, "MovementId", It.IsAny<long>()), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "AttributeId", It.IsAny<long>()), Times.Exactly(2));
            this.homologationMapper.Verify(x => x.Homologate(messge, "DestinationStorageLocationId", It.IsAny<long>()), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "DestinationProductId", It.IsAny<long>()), Times.Once);
        }

        /// <summary>
        /// Homologates the asynchronous verify boolean type homologation asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologateAsync_VerifyBooleanTypeHomologationAsync()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            this.homologationMapper.Setup(x => x.Homologate(messge, "HasMovement", It.IsAny<bool>())).Returns(true);
            this.homologationMapper.Setup(x => x.Homologate(messge, "HasAttribute", true)).Returns(true);
            this.homologationMapper.Setup(x => x.Homologate(messge, "HasAttribute", false)).Returns(true);
            this.homologationMapper.Setup(x => x.Homologate(messge, "HasDestination", It.IsAny<bool>())).Returns(true);
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObjectWithValidDataTypes();
            var array = new JArray();
            array.Add(jobject);
            var result = await this.homologator.HomologateAsync(messge, array, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, "HasMovement", It.IsAny<bool>()), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "HasAttribute", true), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "HasAttribute", false), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "HasDestination", It.IsAny<bool>()), Times.Once);
        }

        /// <summary>
        /// Homologates the asynchronous verify date time type homologation asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologateAsync_VerifyDateTimeTypeHomologationAsync()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            this.homologationMapper.Setup(x => x.Homologate(messge, "OperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(messge, "AttributeOperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(messge, "AttributeOperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(messge, "DestinationOperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObjectWithValidDataTypes();
            var array = new JArray();
            array.Add(jobject);
            var result = await this.homologator.HomologateAsync(messge, array, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, "OperationalDate", It.IsAny<DateTime>()), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "AttributeOperationalDate", It.IsAny<DateTime>()), Times.Exactly(2));
            this.homologationMapper.Verify(x => x.Homologate(messge, "DestinationOperationalDate", It.IsAny<DateTime>()), Times.Once);
        }

        /// <summary>
        /// Homologates the asynchronous verify date time type homologation asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologateAsync_ShouldUpdateTransactions_IfExceptionIsThrownByMapperAsync()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            this.homologationMapper.Setup(x => x.Homologate(messge, "OperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(messge, "AttributeOperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(messge, "AttributeOperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(messge, "DestinationOperationalDate", It.IsAny<DateTime>())).Throws(new KeyNotFoundException());
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObjectWithValidDataTypes();
            var array = new JArray();
            array.Add(jobject);
            var result = await this.homologator.HomologateAsync(messge, array, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, "OperationalDate", It.IsAny<DateTime>()), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "AttributeOperationalDate", It.IsAny<DateTime>()), Times.Exactly(2));
            this.homologationMapper.Verify(x => x.Homologate(messge, "DestinationOperationalDate", It.IsAny<DateTime>()), Times.Once);
        }

        /// <summary>
        /// Homologates the object should update transactions if exception is thrown by mapper.
        /// </summary>
        [TestMethod]
        public void HomologateObject_ShouldUpdateTransactions_IfExceptionIsThrownByMapper()
        {
            var message = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            message.FileRegistration = new Entities.Admin.FileRegistration();
            var transactions = new List<FileRegistrationTransaction>();
            transactions.Add(new FileRegistrationTransaction() { BlobPath = "test/sinoper/1" });
            message.FileRegistration.AddRecords(transactions);
            message.InputBlobPath = "test/sinoper/1";
            this.homologationMapper.Setup(x => x.Homologate(message, "OperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));

            this.homologationMapper.Setup(x => x.Homologate(message, "OperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(message, "AttributeOperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(message, "AttributeOperationalDate", It.IsAny<DateTime>())).Returns(default(DateTime));
            this.homologationMapper.Setup(x => x.Homologate(message, "DestinationOperationalDate", It.IsAny<DateTime>())).Throws(new KeyNotFoundException());
            this.homologationMapper.Setup(x => x.Homologate(message, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObjectWithValidDataTypes();
            jobject.Add("BlobPath", "test/sinoper/1");
            var result = this.homologator.HomologateObject(message, jobject);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(message, "OperationalDate", It.IsAny<DateTime>()), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(message, "AttributeOperationalDate", It.IsAny<DateTime>()), Times.Exactly(2));
            this.homologationMapper.Verify(x => x.Homologate(message, "DestinationOperationalDate", It.IsAny<DateTime>()), Times.Once);
            Assert.AreEqual(1, message.PendingTransactions.Count);
            Assert.AreEqual(1, message.PendingTransactions.ToArray()[0].Errors.Count);
        }

        /// <summary>
        /// Homologates the asynchronous verify float type homologation asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologateAsync_VerifyDecimalTypeHomologationAsync()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            this.homologationMapper.Setup(x => x.Homologate(messge, "NetStandardVolume", It.IsAny<decimal>())).Returns(default(decimal));
            this.homologationMapper.Setup(x => x.Homologate(messge, "AttributeValue", 200.20M)).Returns(default(decimal));
            this.homologationMapper.Setup(x => x.Homologate(messge, "AttributeValue", 201.20M)).Returns(default(decimal));
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObjectWithValidDataTypes();
            var array = new JArray();
            array.Add(jobject);
            var result = await this.homologator.HomologateAsync(messge, array, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, "NetStandardVolume", It.IsAny<decimal>()), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "AttributeValue", 200.20M), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "AttributeValue", 201.20M), Times.Once);
        }

        /// <summary>
        /// Homologates the asynchronous verify nullable type homologation asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologateAsync_VerifyNullableTypeHomologationAsync()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            this.homologationMapper.Setup(x => x.Homologate(messge, "Observations", null)).Returns(default(object));
            this.homologationMapper.Setup(x => x.Homologate(messge, "AttributeDescription", null)).Returns(default(object));
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Movement")).Returns("Movement");

            var jobject = this.GetJObjectWithValidDataTypes();

            var array = new JArray();
            array.Add(jobject);
            var result = await this.homologator.HomologateAsync(messge, array, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, "Observations", null), Times.Once);
            this.homologationMapper.Verify(x => x.Homologate(messge, "AttributeDescription", null), Times.Once);
        }

        /// <summary>
        /// Homologates the asynchronous verify returns null if type not handled asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task HomologateAsync_VerifyReturnsNullIfTypeNotHandledAsync()
        {
            var messge = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            this.homologationMapper.Setup(x => x.Homologate(messge, "ByteValue", It.IsAny<TimeSpan>())).Returns(new TimeSpan(11));
            this.homologationMapper.Setup(x => x.Homologate(messge, "Type", "Inventory")).Returns("Inventory");

            var array = new JArray();
            var jobject = new JObject();
            array.Add(jobject);
            jobject.Add("ByteValue", new TimeSpan(11));
            jobject.Add("MessageId", "123");
            jobject.Add("Type", "Inventory");
            jobject.Add("InventoryProductUniqueId", "23456");
            var result = await this.homologator.HomologateAsync(messge, array, true).ConfigureAwait(false);

            Assert.IsNotNull(result);
            this.homologationMapper.Verify(x => x.Homologate(messge, "ByteValue", null), Times.Once);
        }
    }
}
