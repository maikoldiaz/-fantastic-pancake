// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationProcessorTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Processors.Api.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The homologation processor tests.
    /// </summary>
    [TestClass]
    public class HomologationProcessorTests : ProcessorTestBase
    {
        /// <summary>
        /// The processor.
        /// </summary>
        private HomologationProcessor processor;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;

        /// <summary>
        /// The mock business context.
        /// </summary>
        private Mock<IUnitOfWorkFactory> mockUnitOfWorkFactory;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockFactory = new Mock<IRepositoryFactory>();
            this.mockUnitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            this.processor = new HomologationProcessor(this.mockFactory.Object, this.mockUnitOfWorkFactory.Object);
        }

        /// <summary>
        /// Gets the category by identifier asynchronous shoul get category by identifier from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetHomologationByIdAsync_ShoulGetHomologationByIdFromRepository_WhenInvokedAsync()
        {
            var homologationId = 1;
            var homologation = new Homologation();
            var repoMock = new Mock<IHomologationRepository>();

            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>(), "HomologationGroups", "HomologationGroups.HomologationObjects", "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologation);
            this.mockFactory.SetupGet(m => m.HomologationRepository).Returns(repoMock.Object);

            var result = await this.processor.GetHomologationByIdAsync(homologationId).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(result, homologation);

            this.mockFactory.VerifyGet(m => m.HomologationRepository, Times.Once);
            repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>(), "HomologationGroups", "HomologationGroups.HomologationObjects", "HomologationGroups.HomologationDataMapping"), Times.Once);
        }

        /// <summary>
        /// Gets the category by identifier asynchronous shoul get category by identifier from repository when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetHomologationByIdAndGroupIdAsync_ShoulGetHomologationFromRepository_WhenInvokedAsync()
        {
            var group = new HomologationGroup();
            var repoMock = new Mock<IHomologationRepository>();

            repoMock.Setup(r => r.GetHomologationByIdAndGroupIdAsync(1, 1)).ReturnsAsync(group);
            this.mockFactory.SetupGet(m => m.HomologationRepository).Returns(repoMock.Object);

            var result = await this.processor.GetHomologationByIdAndGroupIdAsync(1, 1).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(result, group);

            this.mockFactory.VerifyGet(m => m.HomologationRepository, Times.Once);
            repoMock.Verify(r => r.GetHomologationByIdAndGroupIdAsync(1, 1), Times.Once);
        }

        /// <summary>
        /// Should return Homologation Object types when invoked async.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetHomologationObjectTypesAsync_ShouldReturnObjectTypes_WhenInvokedAsync()
        {
            var repoMock = new Mock<IRepository<HomologationObjectType>>();
            var homologationObjectTypes = new List<HomologationObjectType>();
            repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(homologationObjectTypes);
            this.mockFactory.Setup(m => m.CreateRepository<HomologationObjectType>()).Returns(repoMock.Object);
            var result = await this.processor.GetHomologationObjectTypesAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(result, homologationObjectTypes);
            repoMock.Verify(r => r.GetAllAsync(null), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<HomologationObjectType>(), Times.Once);
        }

        /// <summary>
        /// Should Check homologation exists.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CheckHomologationGroupExistsAsync_ShouldCheck_GroupExistsAsync()
        {
            var repoMock = new Mock<IRepository<HomologationGroup>>();
            var homologationGroup = new HomologationGroup();
            repoMock.Setup(r => r.SingleOrDefaultAsync(
                 It.IsAny<Expression<Func<HomologationGroup, bool>>>())).ReturnsAsync(homologationGroup);
            this.mockFactory.Setup(m => m.CreateRepository<HomologationGroup>()).Returns(repoMock.Object);
            var result = await this.processor.CheckHomologationGroupExistsAsync(1, 2, 3).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(result, homologationGroup);
            repoMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<HomologationGroup, bool>>>()), Times.Once);
            this.mockFactory.Verify(m => m.CreateRepository<HomologationGroup>(), Times.Once);
        }

        /// <summary>
        /// Should Delete homologation group and homologation when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Should_DeleteHomologation_WhenInvokedAsync()
        {
            var filePath = @"HomologationJson/homologation.json";
            var fileText = File.ReadAllText(filePath);
            var homologation = Newtonsoft.Json.JsonConvert.DeserializeObject<Homologation>(fileText);

            var repoMock = new Mock<IRepository<Homologation>>();
            var token = new CancellationToken(false);
            repoMock.Setup(r => r.SingleOrDefaultAsync(
                    It.IsAny<Expression<Func<Homologation, bool>>>(),
                    "HomologationGroups",
                    "HomologationGroups.HomologationObjects",
                    "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologation);
            repoMock.Setup(r => r.Delete(It.IsAny<Homologation>()));
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Homologation>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            var group = new DeleteHomologationGroup()
            {
                HomologationId = homologation.HomologationId,
                HomologationGroupId = homologation.HomologationGroups.FirstOrDefault().HomologationGroupId,
                RowVersion = "AAAAAAAAru4=".FromBase64(),
            };

            await this.processor.DeleteHomologationGroupAsync(group).ConfigureAwait(false);

            repoMock.Verify(
                r => r.SingleOrDefaultAsync(
                    It.IsAny<Expression<Func<Homologation, bool>>>(),
                    "HomologationGroups",
                    "HomologationGroups.HomologationObjects",
                    "HomologationGroups.HomologationDataMapping"), Times.Once);
            repoMock.Verify(r => r.Delete(It.IsAny<Homologation>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Homologation>(), Times.Once);
        }

        /// <summary>
        /// Should Delete homologation group and homologation when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task Should_DeleteHomologationGroup_WhenInvokedAsync()
        {
            var filePath = @"HomologationJson/homologationWithMultipleGroups.json";
            var fileText = File.ReadAllText(filePath);
            var homologation = Newtonsoft.Json.JsonConvert.DeserializeObject<Homologation>(fileText);
            homologation.HomologationGroups.First().RowVersion = Extensions.FromBase64("AAAAAAAAru4=");
            homologation.HomologationGroups.Last().RowVersion = Extensions.FromBase64("AAAAAAAAru8=");

            var repoMockHomologation = new Mock<IRepository<Homologation>>();
            var repoMockHomologtionGroup = new Mock<IRepository<HomologationGroup>>();
            var token = new CancellationToken(false);
            repoMockHomologation.Setup(r => r.SingleOrDefaultAsync(
                    It.IsAny<Expression<Func<Homologation, bool>>>(),
                    "HomologationGroups",
                    "HomologationGroups.HomologationObjects",
                    "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologation);
            repoMockHomologation.Setup(r => r.Delete(It.IsAny<Homologation>()));
            repoMockHomologtionGroup.Setup(s => s.SingleOrDefaultAsync(
                    It.IsAny<Expression<Func<HomologationGroup, bool>>>(),
                    "HomologationObjects",
                    "HomologationDataMapping")).ReturnsAsync(homologation.HomologationGroups.First());

            repoMockHomologtionGroup.Setup(s => s.Delete(It.IsAny<HomologationGroup>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Homologation>()).Returns(repoMockHomologation.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<HomologationGroup>()).Returns(repoMockHomologtionGroup.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            var group = new DeleteHomologationGroup()
            {
                HomologationId = homologation.HomologationId,
                HomologationGroupId = homologation.HomologationGroups.FirstOrDefault().HomologationGroupId,
                RowVersion = "AAAAAAAAru4=".FromBase64(),
            };

            await this.processor.DeleteHomologationGroupAsync(group).ConfigureAwait(false);

            repoMockHomologation.Verify(
                r => r.SingleOrDefaultAsync(
                    It.IsAny<Expression<Func<Homologation, bool>>>(),
                    "HomologationGroups",
                    "HomologationGroups.HomologationObjects",
                    "HomologationGroups.HomologationDataMapping"), Times.Once);
            repoMockHomologation.Verify(r => r.Delete(It.IsAny<Homologation>()), Times.Never);

            repoMockHomologtionGroup.Verify(
                r => r.SingleOrDefaultAsync(
                    It.IsAny<Expression<Func<HomologationGroup, bool>>>(),
                    "HomologationObjects",
                    "HomologationDataMapping"), Times.Once);
            repoMockHomologtionGroup.Verify(r => r.Delete(It.IsAny<HomologationGroup>()), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Homologation>(), Times.Once);
        }

        /// <summary>
        /// Should Delete homologation group and homologation when invoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Should_RaiseDeleteHomologationException_WhenInvokedAsync()
        {
            var filePath = @"HomologationJson/homologation.json";
            var fileText = File.ReadAllText(filePath);
            var homologation = Newtonsoft.Json.JsonConvert.DeserializeObject<Homologation>(fileText);

            var repoMock = new Mock<IRepository<Homologation>>();
            var token = new CancellationToken(false);
            repoMock.Setup(r => r.SingleOrDefaultAsync(
                    It.IsAny<Expression<Func<Homologation, bool>>>(),
                    "HomologationGroups",
                    "HomologationGroups.HomologationObjects",
                    "HomologationGroups.HomologationDataMapping")).ReturnsAsync(homologation);
            repoMock.Setup(r => r.Delete(It.IsAny<Homologation>()));
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Homologation>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            var group = new DeleteHomologationGroup()
            {
                HomologationId = homologation.HomologationId,
                HomologationGroupId = homologation.HomologationGroups.FirstOrDefault().HomologationGroupId + 1,
                RowVersion = "AAAAAAAAru4=".FromBase64(),
            };
            await this.processor.DeleteHomologationGroupAsync(group).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the homologation asynchronous should create homologation when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveHomologationAsync_ShouldCreateHomologation_WhenInvokedAsync()
        {
            var homologation = new Homologation();
            HomologationGroup homologationGroup = new HomologationGroup();
            homologationGroup.GroupTypeId = 13;
            HomologationObject homologationObject = new HomologationObject();
            HomologationDataMapping homologationDataMapping = new HomologationDataMapping();
            homologationGroup.HomologationDataMapping.Add(homologationDataMapping);
            homologationGroup.HomologationObjects.Add(homologationObject);
            homologation.HomologationGroups.Add(homologationGroup);
            var token = new CancellationToken(false);
            var repoMock = new Mock<IRepository<Homologation>>();
            repoMock.Setup(r => r.Insert(It.IsAny<Homologation>()));

            var homologationVersion = new Entities.Admin.Version();
            homologationVersion.VersionId = 1;
            homologationVersion.Number = 405;

            var repoVersionMock = new Mock<IRepository<Entities.Admin.Version>>();
            repoVersionMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(homologationVersion);
            repoVersionMock.Setup(r => r.Update(It.IsAny<Entities.Admin.Version>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Homologation>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>()).Returns(repoVersionMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await this.processor.SaveHomologationAsync(homologation).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Homologation>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Insert(It.IsAny<Homologation>()), Times.Once);
        }

        /// <summary>
        /// Saves the homologation asynchronous should update homologation group when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveHomologationAsync_ShouldUpdateHomologationGroup_WhenInvokedAsync()
        {
            var homologation = new Homologation();
            HomologationGroup homologationGroup = new HomologationGroup();
            homologationGroup.GroupTypeId = 13;
            HomologationObject homologationObject = new HomologationObject();
            HomologationDataMapping homologationDataMapping = new HomologationDataMapping();
            homologationGroup.HomologationDataMapping.Add(homologationDataMapping);
            homologationGroup.HomologationObjects.Add(homologationObject);
            homologation.HomologationGroups.Add(homologationGroup);
            var token = new CancellationToken(false);
            var repoMock = new Mock<IRepository<Homologation>>();
            repoMock.Setup(r => r.Insert(It.IsAny<Homologation>()));
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>(), "HomologationGroups")).ReturnsAsync(homologation);

            var homologationVersion = new Entities.Admin.Version
            {
                VersionId = 1,
                Number = 405,
            };

            var repoVersionMock = new Mock<IRepository<Entities.Admin.Version>>();
            repoVersionMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(homologationVersion);
            repoVersionMock.Setup(r => r.Update(It.IsAny<Entities.Admin.Version>()));

            var repoHomologationGroup = new Mock<IRepository<HomologationGroup>>();
            repoHomologationGroup.Setup(r => r.FirstOrDefaultAsync(
                        It.IsAny<Expression<Func<HomologationGroup, bool>>>(),
                        "HomologationObjects",
                        "HomologationDataMapping")).ReturnsAsync(homologationGroup);
            repoHomologationGroup.Setup(r => r.Update(It.IsAny<HomologationGroup>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Homologation>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>()).Returns(repoVersionMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<HomologationGroup>()).Returns(repoHomologationGroup.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await this.processor.SaveHomologationAsync(homologation).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Homologation>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<HomologationGroup>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Insert(It.IsAny<Homologation>()), Times.Never);
            repoHomologationGroup.Verify(r => r.Update(It.IsAny<HomologationGroup>()), Times.Once);
        }

        /// <summary>
        /// Saves the homologation asynchronous should create homologation group when invoked asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task SaveHomologationAsync_ShouldCreateHomologationGroup_WhenInvokedAsync()
        {
            var homologation = new Homologation();
            HomologationGroup homologationGroup = new HomologationGroup();
            homologationGroup.GroupTypeId = 13;
            HomologationObject homologationObject = new HomologationObject();
            HomologationDataMapping homologationDataMapping = new HomologationDataMapping();
            homologationGroup.HomologationDataMapping.Add(homologationDataMapping);
            homologationGroup.HomologationObjects.Add(homologationObject);
            homologation.HomologationGroups.Add(homologationGroup);
            homologation.HomologationId = 10;
            var token = new CancellationToken(false);
            var repoMock = new Mock<IRepository<Homologation>>();
            repoMock.Setup(r => r.Insert(It.IsAny<Homologation>()));
            repoMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Homologation, bool>>>(), "HomologationGroups")).ReturnsAsync(homologation);

            var homologationVersion = new Entities.Admin.Version
            {
                VersionId = 1,
                Number = 405,
            };

            HomologationGroup existingHomologationGroup = null;

            var repoVersionMock = new Mock<IRepository<Entities.Admin.Version>>();
            repoVersionMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(homologationVersion);
            repoVersionMock.Setup(r => r.Update(It.IsAny<Entities.Admin.Version>()));

            var repoHomologationGroup = new Mock<IRepository<HomologationGroup>>();
            repoHomologationGroup.Setup(r => r.FirstOrDefaultAsync(
                        It.IsAny<Expression<Func<HomologationGroup, bool>>>(),
                        "HomologationObjects",
                        "HomologationDataMapping")).ReturnsAsync(existingHomologationGroup);
            repoHomologationGroup.Setup(r => r.Insert(It.IsAny<HomologationGroup>()));

            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Homologation>()).Returns(repoMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>()).Returns(repoVersionMock.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().CreateRepository<HomologationGroup>()).Returns(repoHomologationGroup.Object);
            this.mockUnitOfWorkFactory.Setup(m => m.GetUnitOfWork().SaveAsync(token));

            await this.processor.SaveHomologationAsync(homologation).ConfigureAwait(false);

            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Homologation>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<Entities.Admin.Version>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().CreateRepository<HomologationGroup>(), Times.Once);
            this.mockUnitOfWorkFactory.Verify(m => m.GetUnitOfWork().SaveAsync(token), Times.Once);
            repoMock.Verify(r => r.Insert(It.IsAny<Homologation>()), Times.Never);
            repoHomologationGroup.Verify(r => r.Insert(It.IsAny<HomologationGroup>()), Times.Once);
        }

        /// <summary>
        /// Should return Homologation Group Mappings when invoked async.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetHomologationGroupMappingAsync_ShouldReturnGroupMappings_WhenInvokedAsync()
        {
            var repoMock = new Mock<IHomologationRepository>();
            var homologationDataMappings = new List<HomologationDataMapping>();
            repoMock.Setup(r => r.GetHomologationGroupMappingsAsync(It.IsAny<int>())).ReturnsAsync(homologationDataMappings);
            this.mockFactory.SetupGet(m => m.HomologationRepository).Returns(repoMock.Object);
            var result = await this.processor.GetHomologationGroupMappingsAsync(1).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(result, homologationDataMappings);
            repoMock.Verify(r => r.GetHomologationGroupMappingsAsync(1), Times.Once);
            this.mockFactory.VerifyGet(m => m.HomologationRepository, Times.Once);
        }
    }
}
