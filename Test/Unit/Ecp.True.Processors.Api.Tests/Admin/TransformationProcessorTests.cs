// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class TransformationProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private TransformationProcessor processor;

        /// <summary>
        /// The mock repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockRepositoryFactory;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// The mock transformation repository.
        /// </summary>
        private Mock<IRepository<Transformation>> mockTransformationRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.mockRepositoryFactory = new Mock<IRepositoryFactory>();
            this.mockTransformationRepository = new Mock<IRepository<Transformation>>();

            this.processor = new TransformationProcessor(this.mockUnitOfWorkFactory.Object, this.mockRepositoryFactory.Object);
        }

        /// <summary>
        /// Creates the transformation asynchronous creates transformation when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task CreateTransformationAsync_CreatesTransformation_WhenInvokedAsync()
        {
            var origin = new Origin
            {
                SourceNodeId = 300,
                DestinationNodeId = 100,
                SourceProductId = "1",
                DestinationProductId = "1",
                MeasurementUnitId = 31,
            };
            var destination = new Destination
            {
                SourceNodeId = 400,
                DestinationNodeId = 200,
                SourceProductId = "2",
                DestinationProductId = "3",
                MeasurementUnitId = 31,
            };

            var transformationVersion = new Entities.Admin.Version();
            transformationVersion.VersionId = 2;
            transformationVersion.Number = 1;

            var transformationDto = new TransformationDto { TransformationId = 1, MessageTypeId = MessageType.Movement, Origin = origin, Destination = destination };

            this.mockTransformationRepository.Setup(r => r.Insert(It.IsAny<Transformation>()));
            this.mockTransformationRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Transformation, bool>>>())).ReturnsAsync(new List<Transformation>());

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            var repoVersionMock = new Mock<IRepository<Entities.Admin.Version>>();
            repoVersionMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(transformationVersion);
            repoVersionMock.Setup(r => r.Insert(It.IsAny<Entities.Admin.Version>()));
            var token = new CancellationToken(false);

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>()).Returns(repoVersionMock.Object);

            await this.processor.CreateTransformationAsync(transformationDto).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Transformation>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            this.mockTransformationRepository.Verify(r => r.Insert(It.IsAny<Transformation>()), Times.Once);
            this.mockTransformationRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Transformation, bool>>>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>(), Times.Once);
        }

        /// <summary>
        /// Creates the transformation asynchronous create transformation should return invalid data exception when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task CreateTransformationAsync_CreateTransformation_ShouldReturnInvalidDataException_WhenInvokedAsync()
        {
            var origin = new Origin
            {
                SourceNodeId = 300,
                DestinationNodeId = 100,
                SourceProductId = "1",
                DestinationProductId = "1",
                MeasurementUnitId = 31,
            };
            var destination = new Destination
            {
                SourceNodeId = 400,
                DestinationNodeId = 200,
                SourceProductId = "2",
                DestinationProductId = "3",
                MeasurementUnitId = 31,
            };

            var transformationVersion = new Entities.Admin.Version();
            transformationVersion.VersionId = 2;
            transformationVersion.Number = 1;

            var transformationDto = new TransformationDto { TransformationId = 1, MessageTypeId = MessageType.Movement, Origin = origin, Destination = destination };

            this.mockTransformationRepository.Setup(r => r.Insert(It.IsAny<Transformation>()));
            this.mockTransformationRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Transformation, bool>>>())).ReturnsAsync(new List<Transformation> { new Transformation() });

            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            var repoVersionMock = new Mock<IRepository<Entities.Admin.Version>>();
            repoVersionMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(transformationVersion);
            repoVersionMock.Setup(r => r.Insert(It.IsAny<Entities.Admin.Version>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>()).Returns(repoVersionMock.Object);

            await this.processor.CreateTransformationAsync(transformationDto).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the transformation asynchronous updates transformation when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task UpdateTransformationAsync_UpdatesTransformation_WhenInvokedAsync()
        {
            var origin = new Origin
            {
                SourceNodeId = 300,
                DestinationNodeId = 100,
                SourceProductId = "1",
                DestinationProductId = "1",
                MeasurementUnitId = 31,
            };
            var destination = new Destination
            {
                SourceNodeId = 400,
                DestinationNodeId = 200,
                SourceProductId = "2",
                DestinationProductId = "3",
                MeasurementUnitId = 31,
            };

            var transformationVersion = new Entities.Admin.Version();
            transformationVersion.VersionId = 2;
            transformationVersion.Number = 1;

            var transformationDto = new TransformationDto { TransformationId = 1234, MessageTypeId = MessageType.Movement, Origin = origin, Destination = destination };
            this.mockTransformationRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Transformation, bool>>>())).ReturnsAsync(new List<Transformation>());
            this.mockTransformationRepository.Setup(r => r.GetByIdAsync(transformationDto.TransformationId)).ReturnsAsync(new Transformation());
            this.mockTransformationRepository.Setup(r => r.Update(It.IsAny<Transformation>()));
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            var token = new CancellationToken(false);

            var repoVersionMock = new Mock<IRepository<Entities.Admin.Version>>();
            repoVersionMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(transformationVersion);
            repoVersionMock.Setup(r => r.Update(It.IsAny<Entities.Admin.Version>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>()).Returns(repoVersionMock.Object);

            await this.processor.UpdateTransformationAsync(transformationDto).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Transformation>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            this.mockTransformationRepository.Verify(r => r.Update(It.IsAny<Transformation>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>(), Times.Once);
        }

        /// <summary>
        /// Updates the transformation asynchronous update transformation should return invalid data exception when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public async Task UpdateTransformationAsync_UpdateTransformation_ShouldReturnInvalidDataException_WhenInvokedAsync()
        {
            var origin = new Origin
            {
                SourceNodeId = 300,
                DestinationNodeId = 100,
                SourceProductId = "1",
                DestinationProductId = "1",
                MeasurementUnitId = 31,
            };
            var destination = new Destination
            {
                SourceNodeId = 400,
                DestinationNodeId = 200,
                SourceProductId = "2",
                DestinationProductId = "3",
                MeasurementUnitId = 31,
            };

            var transformationVersion = new Entities.Admin.Version();
            transformationVersion.VersionId = 2;
            transformationVersion.Number = 1;

            var transformationDto = new TransformationDto { TransformationId = 1234, MessageTypeId = MessageType.Movement, Origin = origin, Destination = destination };
            this.mockTransformationRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Transformation, bool>>>())).ReturnsAsync(new List<Transformation> { new Transformation() });
            this.mockTransformationRepository.Setup(r => r.GetByIdAsync(transformationDto.TransformationId)).ReturnsAsync(new Transformation());
            this.mockTransformationRepository.Setup(r => r.Update(It.IsAny<Transformation>()));
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);

            var repoVersionMock = new Mock<IRepository<Entities.Admin.Version>>();
            repoVersionMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(transformationVersion);
            repoVersionMock.Setup(r => r.Update(It.IsAny<Entities.Admin.Version>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>()).Returns(repoVersionMock.Object);

            await this.processor.UpdateTransformationAsync(transformationDto).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the transformation asynchronous updates transformation should throw exception when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task UpdateTransformationAsync_UpdatesTransformation__ShouldThrowException_WhenInvokedAsync()
        {
            var transformationDto = new TransformationDto { TransformationId = 1234, MessageTypeId = MessageType.Movement, Origin = new Origin(), Destination = new Destination() };
            this.mockTransformationRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Transformation, bool>>>())).ReturnsAsync(new List<Transformation>());
            this.mockTransformationRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);
            this.mockRepositoryFactory.Setup(a => a.CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);

            await this.processor.UpdateTransformationAsync(transformationDto).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the transformation asynchronous deletes transformation when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task DeleteTransformationAsync_DeletesTransformation_WhenInvokedAsync()
        {
            var token = new CancellationToken(false);
            string rowVersion = "AAAAAAAAcHE = ";

            var transformationVersion = new Entities.Admin.Version();
            transformationVersion.VersionId = 2;
            transformationVersion.Number = 1;

            var repoVersionMock = new Mock<IRepository<Entities.Admin.Version>>();
            repoVersionMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(transformationVersion);
            repoVersionMock.Setup(r => r.Update(It.IsAny<Entities.Admin.Version>()));

            var transformation = new Transformation { TransformationId = 1, DestinationDestinationNodeId = 1, DestinationDestinationProductId = "1", DestinationMeasurementId = 1, DestinationSourceNodeId = 2, DestinationSourceProductId = "2", OriginDestinationNodeId = 3, OriginDestinationProductId = "3", OriginSourceNodeId = 4, OriginSourceProductId = "4", MessageTypeId = 1 };

            var mockTranfomationRepository = new Mock<IRepository<Transformation>>();
            mockTranfomationRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Transformation, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(transformation);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>()).Returns(repoVersionMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Transformation>()).Returns(mockTranfomationRepository.Object);

            var deleteTransformation = new DeleteTransformation()
            {
                TransformationId = 1,
                RowVersion = rowVersion.FromBase64(),
            };

            await this.processor.DeleteTransformationAsync(deleteTransformation).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            mockTranfomationRepository.Verify(r => r.Update(It.IsAny<Transformation>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>(), Times.Once);
        }

        /// <summary>
        /// Deletes the transformation asynchronous deletes transformation should throw exception when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task DeleteTransformationAsync_DeletesTransformation__ShouldThrowException_WhenInvokedAsync()
        {
            string rowVersion = "AAAAAAAAcHE = ";

            var mockTranfomationRepository = new Mock<IRepository<Transformation>>();
            mockTranfomationRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Transformation, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(() => null);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Transformation>()).Returns(mockTranfomationRepository.Object);

            var deleteTransformation = new DeleteTransformation()
            {
                TransformationId = 1,
                RowVersion = rowVersion.FromBase64(),
            };

            await this.processor.DeleteTransformationAsync(deleteTransformation).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the existing transformation asynchronous validates transformation when message type is movement asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateExistingTransformationAsync_ValidatesTransformation_WhenMessageTypeIsMovementAsync()
        {
            var origin = new Origin
            {
                SourceNodeId = 300,
                DestinationNodeId = 100,
                SourceProductId = "1",
                DestinationProductId = "1",
                MeasurementUnitId = 31,
            };
            var destination = new Destination
            {
                SourceNodeId = 400,
                DestinationNodeId = 200,
                SourceProductId = "2",
                DestinationProductId = "3",
                MeasurementUnitId = 31,
            };
            var transformationDto = new TransformationDto { TransformationId = 1, MessageTypeId = MessageType.Movement, Origin = origin, Destination = destination };

            var validationResult = new Mock<IEnumerable<Transformation>>();
            var repoMock = new Mock<IRepository<Transformation>>();
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Transformation, bool>>>())).ReturnsAsync(validationResult.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Transformation>()).Returns(repoMock.Object);

            var result = await this.processor.ExistsTransformationAsync(transformationDto).ConfigureAwait(false);
            Assert.IsNotNull(result);
            this.mockRepositoryFactory.Verify(m => m.CreateRepository<Transformation>(), Times.Once);
        }

        /// <summary>
        /// Validates the existing transformation asynchronous validates transformation when message type is not movement asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateExistingTransformationAsync_ValidatesTransformation_WhenMessageTypeIsNotMovementAsync()
        {
            var origin = new Origin
            {
                SourceNodeId = 300,
                DestinationNodeId = 100,
                SourceProductId = "1",
                DestinationProductId = "1",
                MeasurementUnitId = 31,
            };
            var destination = new Destination
            {
                SourceNodeId = 400,
                DestinationNodeId = 200,
                SourceProductId = "2",
                DestinationProductId = "3",
                MeasurementUnitId = 32,
            };
            var transformationDto = new TransformationDto { TransformationId = 1, MessageTypeId = MessageType.Inventory, Origin = origin, Destination = destination };

            var validationResult = new Mock<IEnumerable<Transformation>>();
            this.mockTransformationRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Transformation, bool>>>())).ReturnsAsync(validationResult.Object);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);

            var result = await this.processor.ExistsTransformationAsync(transformationDto).ConfigureAwait(false);
            Assert.IsNotNull(result);
            this.mockRepositoryFactory.Verify(m => m.CreateRepository<Transformation>(), Times.Once);
        }

        /// <summary>
        /// Gets the transformation information asynchronous get transformation information when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task GetTransformationInfoAsync_GetTransformationInfo_WhenInvokedAsync()
        {
            var transformation = new Transformation { TransformationId = 1, DestinationDestinationNodeId = 1, DestinationDestinationProductId = "1", DestinationMeasurementId = 1, DestinationSourceNodeId = 2, DestinationSourceProductId = "2", OriginDestinationNodeId = 3, OriginDestinationProductId = "3", OriginSourceNodeId = 4, OriginSourceProductId = "4", MessageTypeId = 1 };

            this.mockTransformationRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Transformation, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(transformation);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Transformation>()).Returns(this.mockTransformationRepository.Object);

            var mockNodeConnectionRepository = new Mock<IRepository<NodeConnection>>();
            mockNodeConnectionRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<NodeConnection>());
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<NodeConnection>()).Returns(mockNodeConnectionRepository.Object);

            var mockNodeStorageLocationRepository = new Mock<IRepository<NodeStorageLocation>>();
            mockNodeStorageLocationRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(new List<NodeStorageLocation>());
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<NodeStorageLocation>()).Returns(mockNodeStorageLocationRepository.Object);

            await this.processor.GetTransformationInfoAsync(1).ConfigureAwait(false);

            this.mockTransformationRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Transformation, bool>>>(), It.IsAny<string[]>()), Times.Once);
            mockNodeConnectionRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<NodeConnection, bool>>>(), It.IsAny<string[]>()), Times.AtLeastOnce);
            mockNodeStorageLocationRepository.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<NodeStorageLocation, bool>>>(), It.IsAny<string[]>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Gets the transformation information asynchronous get transformation information should throw exception when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetTransformationInfoAsync_GetTransformationInfo_ShouldThrowException_WhenInvokedAsync()
        {
            var mockTranfomationRepository = new Mock<IRepository<Transformation>>();
            mockTranfomationRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Transformation, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(() => null);
            this.mockRepositoryFactory.Setup(m => m.CreateRepository<Transformation>()).Returns(mockTranfomationRepository.Object);

            await this.processor.GetTransformationInfoAsync(1).ConfigureAwait(false);
        }
    }
}