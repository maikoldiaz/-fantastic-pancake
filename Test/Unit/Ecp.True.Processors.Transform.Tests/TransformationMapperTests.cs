// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationMapperTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Transform.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The transformation mapper tests.
    /// </summary>
    [TestClass]
    public class TransformationMapperTests
    {
        /// <summary>
        /// The repository factory mock.
        /// </summary>
        private Mock<IRepositoryFactory> repositoryFactoryMock;

        /// <summary>
        /// The transformation version repository mock.
        /// </summary>
        private Mock<IRepository<Entities.Admin.Version>> versionRepositoryMock;

        /// <summary>
        /// The transformation repository mock.
        /// </summary>
        private Mock<IRepository<Transformation>> transformationRepositoryMock;

        /// <summary>
        /// The homologation mapper.
        /// </summary>
        private TransformationMapper transformationMapper;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<TransformationMapper>> mockLogger;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<TransformationMapper>>();
            this.repositoryFactoryMock = new Mock<IRepositoryFactory>();
            this.versionRepositoryMock = new Mock<IRepository<Entities.Admin.Version>>();
            this.transformationRepositoryMock = new Mock<IRepository<Transformation>>();

            var transformations = new List<Transformation>()
            {
                new Transformation
                {
                    TransformationId = 1,
                    MessageTypeId = 1,
                    OriginSourceNodeId = 2,
                    OriginDestinationNodeId = 5,
                    OriginSourceProductId = "10000002318",
                    OriginDestinationProductId = "10000002318",
                    OriginMeasurementId = 31,
                    DestinationSourceNodeId = 3,
                    DestinationDestinationNodeId = 222,
                    DestinationSourceProductId = "10000002372",
                    DestinationDestinationProductId = "10000002372",
                    DestinationMeasurementId = 31,
                },
                new Transformation
                {
                    TransformationId = 2,
                    MessageTypeId = 4,
                    OriginSourceNodeId = 32,
                    OriginDestinationNodeId = 55,
                    OriginSourceProductId = "10000002320",
                    OriginDestinationProductId = "10000002320",
                    OriginMeasurementId = 31,
                    DestinationSourceNodeId = 3,
                    DestinationDestinationNodeId = 22,
                    DestinationSourceProductId = "10000002382",
                    DestinationDestinationProductId = "10000002382",
                    DestinationMeasurementId = 31,
                },
            };

            this.repositoryFactoryMock.Setup(x => x.CreateRepository<Entities.Admin.Version>()).Returns(this.versionRepositoryMock.Object);
            this.repositoryFactoryMock.Setup(x => x.CreateRepository<Transformation>()).Returns(this.transformationRepositoryMock.Object);
            this.versionRepositoryMock.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Entities.Admin.Version, bool>>>())).ReturnsAsync(new Entities.Admin.Version() { Number = 8,Type = "Transformation", VersionId = 2 });
            this.transformationRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Transformation, bool>>>())).ReturnsAsync(transformations);

            this.transformationMapper = new TransformationMapper(this.repositoryFactoryMock.Object, this.mockLogger.Object);
        }

        /// <summary>
        /// Initializes the asynchronous should return when no version exists.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task InitializeAsync_ShouldReturn_WhenNoVersionExistsAsync()
        {
            await this.transformationMapper.InitializeAsync(15).ConfigureAwait(false);

            this.versionRepositoryMock.Verify(c => c.SingleOrDefaultAsync(It.IsAny<Expression<Func<Entities.Admin.Version, bool>>>()), Times.Once);
            this.transformationRepositoryMock.Verify(c => c.GetAllAsync(null), Times.Never);
        }

        /// <summary>
        /// Transform should transform movement when invoked with j token asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task Transform_ShouldTransformMovement_WhenInvokedWithJTokenAsync()
        {
            await this.transformationMapper.InitializeAsync(15).ConfigureAwait(false);

            var homologatedMovement = GetHomologatedMovement();
            this.transformationMapper.Transform(homologatedMovement);

            Assert.IsNotNull(homologatedMovement);
            var movementSource = homologatedMovement.Value<JToken>("MovementSource");
            var movementDestination = homologatedMovement.Value<JToken>("MovementDestination");

            Assert.AreEqual(1876, movementSource["SourceNodeId"]);
            Assert.AreEqual("10000002318", movementSource["SourceProductId"]);
            Assert.AreEqual(1877, movementDestination["DestinationNodeId"]);
            Assert.AreEqual("10000002049", movementDestination["DestinationProductId"]);
        }

        /// <summary>
        /// Transform should transform and update movement when invoked with JToken asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task Transform_ShouldTransformAndUpdateMovement_WhenInvokedWithJTokenAsync()
        {
            await this.transformationMapper.InitializeAsync(15).ConfigureAwait(false);

            var homologatedMovement = GetHomologatedMovement();
            var movementSource = homologatedMovement.Value<JToken>("MovementSource");
            var movementDestination = homologatedMovement.Value<JToken>("MovementDestination");
            movementSource["SourceNodeId"] = 2;
            movementSource["SourceProductId"] = "10000002318";
            movementDestination["DestinationNodeId"] = 5;
            movementDestination["DestinationProductId"] = "10000002318";
            homologatedMovement["MovementSource"] = movementSource;
            homologatedMovement["MovementDestination"] = movementDestination;

            this.transformationMapper.Transform(homologatedMovement);

            Assert.IsNotNull(homologatedMovement);
            var movementSourceUpdated = homologatedMovement.Value<JToken>("MovementSource");
            var movementDestinationUpdated = homologatedMovement.Value<JToken>("MovementDestination");

            Assert.AreEqual(3, movementSourceUpdated["SourceNodeId"]);
            Assert.AreEqual("10000002372", movementSourceUpdated["SourceProductId"]);
            Assert.AreEqual(222, movementDestinationUpdated["DestinationNodeId"]);
            Assert.AreEqual("10000002372", movementDestinationUpdated["DestinationProductId"]);
        }

        /// <summary>
        /// Transform should transform Inventory when invoked with j token asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task Transform_ShouldTransformInventory_WhenInvokedWithJTokenAsync()
        {
            await this.transformationMapper.InitializeAsync(15).ConfigureAwait(false);

            var homologatedInventory = GetHomologatedInventory();
            this.transformationMapper.Transform(homologatedInventory);

            Assert.IsNotNull(homologatedInventory);

            Assert.AreEqual("10000002049", homologatedInventory["ProductId"]);
            Assert.AreEqual(31, homologatedInventory["MeasurementUnit"]);
            Assert.AreEqual(1836, homologatedInventory["NodeId"]);
        }

        /// <summary>
        /// Transform should transform and update Inventory when invoked with JToken asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task Transform_ShouldTransformAndUpdateInventory_WhenInvokedWithJTokenAsync()
        {
            await this.transformationMapper.InitializeAsync(15).ConfigureAwait(false);

            var homologatedInventory = GetHomologatedInventory();
            homologatedInventory["NodeId"] = 32;
            homologatedInventory["ProductId"] = "10000002320";
            homologatedInventory["MeasurementUnit"] = 31;

            this.transformationMapper.Transform(homologatedInventory);

            Assert.IsNotNull(homologatedInventory);
            Assert.AreEqual("10000002382", homologatedInventory["ProductId"]);
            Assert.AreEqual(31, homologatedInventory["MeasurementUnit"]);
            Assert.AreEqual(3, homologatedInventory["NodeId"]);
        }

        private static JToken GetHomologatedMovement()
        {
            string movementString = "{\"SourceSystem\": \"SINOPER\",\"SystemName\": \"SINOPER\"," +
                " \"EventType\": \"INSERT\", \"MovementId\": 751326, \"MovementTypeId\": 9273," +
                " \"OperationalDate\": \"2020-04-13T00:00:00\", \"GrossStandardVolume\": 0," +
                " \"NetStandardVolume\": 118020.43,\"MeasurementUnit\": 31, \"Scenario\": \"Operativo\", " +
                " \"Observations\": null,\"Classification\": \"Movimiento\", \"SegmentId\": 10," +
                " \"MovementDestination\": {\"DestinationNodeId\": 1877,\"DestinationStorageLocationId\": null," +
                "   \"DestinationProductId\": \"10000002049\",\"DestinationProductTypeId\": 9276}," +
                " \"MovementSource\": {\"SourceNodeId\": 1876,\"SourceStorageLocationId\": null," +
                " \"SourceProductId\": \"10000002318\",  \"SourceProductTypeId\": 9275  }, " +
                "\"Period\": {  \"StartTime\": \"2020-04-13T00:00:00\",\"EndTime\": \"2020-04-15T00:00:00\" }," +
                " \"Owners\": [ {\"OwnerId\": 9277,   \"OwnershipValue\": 118020.43, " +
                " \"OwnershipValueUnit\": \"Bbl\"} ],\"Attributes\": [ { " +
                "\"AttributeId\": \"Producto Destino\", \"AttributeValue\": \"CRUDOS IMPORTADOS\", " +
                "\"ValueAttributeUnit\": \"ADM\",  \"AttributeDescription\": \"Producto Destino\"  " +
                "}, { \"AttributeId\": \"Neto-Volumen L\u00EDquido\"," +
                "\"AttributeValue\": \"118020.43\", \"ValueAttributeUnit\": \"Bbl\", " +
                "\"AttributeDescription\": null  }  ],  \"MessageId\": 751326,  \"Type\": \"Movement\", " +
                " \"BlobPath\": \"sinoper\\/json\\/movement\\/qu1rievbstaylkquuu0gif6ucq0gl5h==\\/751326\\/751326_1862_1\", " +
                " \"IsHomologated\": true}";

            return JToken.Parse(movementString);
        }

        private static JToken GetHomologatedInventory()
        {
            string inventoryString = "{\"ProductId\": \"10000002049\",\"ProductType\": 9090," +
                "\"BatchId\": \"pwa5bsfbxf1w5cvzbuk7j8ksf\",\"ProductVolume\": 177506," +
                "\"MeasurementUnit\": 31,\"Owners\": [{ \"OwnerId\": 9093," +
                "\"OwnershipValue\": 177506, \"OwnershipValueUnit\": \"Bbl\" } ]," +
                "\"Attributes\": [{\"AttributeId\": \"Neto-Volumen L\u00EDquido\"," +
                "\"AttributeValue\": 177506, \"ValueAttributeUnit\": \"Bbl\", \"AttributeDescription\": null" +
                "}],\"SourceSystem\": \"SINOPER\", \"SystemName\": \"SINOPER\"," +
                "\"DestinationSystem\": \"TRUE\", \"EventType\": \"INSERT\", \"InventoryId\": 20200414, " +
                "\"InventoryDate\": \"2020-04-14T00:00:00\", \"Observations\": \"\", \"Scenario\": \"Operativo\"," +
                "\"NodeId\": 1836, \"TankName\": null, \"SegmentId\": 10, " +
                "\"MessageId\": \"MjAyMDA0MTRfQVlBQ1VDSE9fQ1JVRE9TIElNUE9SVEFET1NfMDQvMTQvMjAyMCAwMDowMDowMF9wd2E1YnNmYnhmMXc1Y3Z6YnVrN2o4a3NmX05B\"," +
                "\"Type\": \"Inventory\"," +
                "  \"BlobPath\": \"sinoper\\/json\\/inventory\\/qu1rievbstaylkquuu0gif6ucq0gl4ds\\/MjAyMDA0MTRfQVlBQ1VDSE9fQ1JVRE9TIElNUE9SVEFET1NfMDQvMTQvMjAyMCAwMDowMDowMF9wd2E1YnNmYnhmMXc1Y3Z6YnVrN2o4a3NmX05B\\/MjAyMDA0MTRfQVlBQ1VDSE9fQ1JVRE9TIElNUE9SVEFET1NfMDQvMTQvMjAyMCAwMDowMDowMF9wd2E1YnNmYnhmMXc1Y3Z6YnVrN2o4a3NmX05B_1861_1\"," +
                " \"IsHomologated\": true,\r\n  \"InventoryProductUniqueId\": \"MjAyMDA0MTRfMTgzNl8xMDAwMDAwMjA0OV8wNC8xNC8yMDIwIDAwOjAwOjAwX3B3YTVic2ZieGYxdzVjdnpidWs3ajhrc2ZfTkE=\"\r\n}";

            return JToken.Parse(inventoryString);
        }
    }
}
