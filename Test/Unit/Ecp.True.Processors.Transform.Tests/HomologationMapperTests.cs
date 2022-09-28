// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationMapperTests.cs" company="Microsoft">
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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Homologate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Version = Ecp.True.Entities.Admin.Version;

    /// <summary>
    /// The homologation mapper tests.
    /// </summary>
    [TestClass]
    public class HomologationMapperTests
    {
        /// <summary>
        /// The repository factory.
        /// </summary>
        private Mock<IRepositoryFactory> repositoryFactory;

        /// <summary>
        /// The homologation mapper.
        /// </summary>
        private HomologationMapper homologationMapper;

        /// <summary>
        /// The homologations.
        /// </summary>
        private IList<Homologation> homologations;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<HomologationMapper>> mockLogger;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<HomologationMapper>>();
            this.repositoryFactory = new Mock<IRepositoryFactory>();
            this.homologationMapper = new HomologationMapper(this.repositoryFactory.Object, this.mockLogger.Object);
            HomologationMapper.Clear();
            this.homologations = this.GetHomologations();
        }

        /// <summary>
        /// Initializes the asynchronous should initilize mappers.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task InitializeAsync_Should_InitilizeMappersAsync()
        {
            Mock<IRepository<Entities.Admin.Version>> homologationRepos;
            Mock<IRepository<HomologationObjectType>> homologationObjectTypeRepos;
            Mock<IHomologationRepository> repoMock;
            this.MockSetups(out homologationRepos, out homologationObjectTypeRepos, out repoMock);

            await this.homologationMapper.InitializeAsync(5).ConfigureAwait(false);

            homologationRepos.Verify(c => c.SingleOrDefaultAsync(It.IsAny<Expression<Func<Version, bool>>>()), Times.Once());
            homologationObjectTypeRepos.Verify(c => c.GetAllAsync(null), Times.Once());
            repoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Homologation, bool>>>(), "HomologationGroups", "HomologationGroups.Group", "HomologationGroups.HomologationObjects", "HomologationGroups.HomologationDataMapping"), Times.Once());
            this.repositoryFactory.VerifyGet(x => x.HomologationRepository, Times.Once);
        }

        /// <summary>
        /// Initializes the asynchronous should return if homologations not exists asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task InitializeAsync_Should_ReturnIfHomologationsVersionNotExistsAsync()
        {
            Mock<IRepository<Entities.Admin.Version>> homologationVersionRepos;
            Mock<IRepository<HomologationObjectType>> homologationObjectTypeRepos;
            Mock<IHomologationRepository> repoMock;
            this.MockSetups(out homologationVersionRepos, out homologationObjectTypeRepos, out repoMock);
            homologationVersionRepos.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Version, bool>>>())).ReturnsAsync(default(Entities.Admin.Version));

            await this.homologationMapper.InitializeAsync(5).ConfigureAwait(false);

            homologationVersionRepos.Verify(c => c.SingleOrDefaultAsync(It.IsAny<Expression<Func<Version, bool>>>()), Times.Once());
            homologationObjectTypeRepos.Verify(c => c.GetAllAsync(null), Times.Never());
            repoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Homologation, bool>>>(), "HomologationGroups", "HomologationGroups.HomologationObjects", "HomologationGroups.HomologationDataMapping"), Times.Never());
            this.repositoryFactory.VerifyGet(x => x.HomologationRepository, Times.Never);
        }

        /// <summary>
        /// Initializes the asynchronous should re build homologation asynchronous.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task InitializeAsync_Should_ReBuildHomologationAsync()
        {
            var homologationVersion = new Entities.Admin.Version { Number = 1 };
            Mock<IRepository<Entities.Admin.Version>> homologationVersionRepos;
            Mock<IRepository<HomologationObjectType>> homologationObjectTypeRepos;
            Mock<IHomologationRepository> repoMock;
            this.MockSetups(out homologationVersionRepos, out homologationObjectTypeRepos, out repoMock);
            homologationVersionRepos.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Version, bool>>>())).ReturnsAsync(homologationVersion);

            await this.homologationMapper.InitializeAsync(1).ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(false);
            homologationVersion.Number = 2;
            await this.homologationMapper.InitializeAsync(1).ConfigureAwait(false);

            homologationVersionRepos.Verify(c => c.SingleOrDefaultAsync(It.IsAny<Expression<Func<Version, bool>>>()), Times.Exactly(2));
            homologationObjectTypeRepos.Verify(c => c.GetAllAsync(null), Times.Exactly(2));
            repoMock.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<Homologation, bool>>>(), "HomologationGroups", "HomologationGroups.Group", "HomologationGroups.HomologationObjects", "HomologationGroups.HomologationDataMapping"), Times.Exactly(2));
            this.repositoryFactory.VerifyGet(x => x.HomologationRepository, Times.Exactly(2));
        }

        /// <summary>
        /// Homologates the should homologate when invoked.
        /// </summary>
        /// <returns>
        /// The Task.
        /// </returns>
        [TestMethod]
        public async Task Homologate_ShouldHomologateWhenInvokedAsync()
        {
            var originalValue = "OriginalValue_1";
            var message = new TrueMessage() { SourceSystem = SystemType.SINOPER, TargetSystem = SystemType.TRUE };
            Mock<IRepository<Entities.Admin.Version>> homologationVersionRepos;
            Mock<IRepository<HomologationObjectType>> homologationObjectTypeRepos;
            Mock<IHomologationRepository> repoMock;
            this.MockSetups(out homologationVersionRepos, out homologationObjectTypeRepos, out repoMock);

            await this.homologationMapper.InitializeAsync(1).ConfigureAwait(false);
            var homologatedValue = this.homologationMapper.Homologate(message, "ProductId", originalValue);

            var expectedValue = this.GetHomologations().First().HomologationGroups.First().HomologationDataMapping.FirstOrDefault(x => x.SourceValue == originalValue);

            Assert.IsNotNull(homologatedValue);
            Assert.AreEqual(homologatedValue, expectedValue.DestinationValue);
        }

        private void MockSetups(out Mock<IRepository<Entities.Admin.Version>> homologationVersionRepos, out Mock<IRepository<HomologationObjectType>> homologationObjectTypeRepos, out Mock<IHomologationRepository> repoMock)
        {
            var homologationObjectType = new List<HomologationObjectType>();
            homologationObjectType.Add(new HomologationObjectType { HomologationObjectTypeId = 1, Name = "ProductId" });
            homologationObjectType.Add(new HomologationObjectType { HomologationObjectTypeId = 2, Name = "ProductName" });

            var homologationVersion = new Entities.Admin.Version { Number = 1 };

            homologationVersionRepos = new Mock<IRepository<Entities.Admin.Version>>();
            homologationVersionRepos.Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Version, bool>>>())).ReturnsAsync(homologationVersion);

            homologationObjectTypeRepos = new Mock<IRepository<HomologationObjectType>>();
            homologationObjectTypeRepos.Setup(x => x.GetAllAsync(null)).ReturnsAsync(homologationObjectType);

            repoMock = new Mock<IHomologationRepository>();
            repoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Homologation, bool>>>(), "HomologationGroups", "HomologationGroups.Group", "HomologationGroups.HomologationObjects", "HomologationGroups.HomologationDataMapping")).ReturnsAsync(this.homologations);
            this.repositoryFactory.SetupGet(m => m.HomologationRepository).Returns(repoMock.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<HomologationObjectType>()).Returns(homologationObjectTypeRepos.Object);
            this.repositoryFactory.Setup(x => x.CreateRepository<Entities.Admin.Version>()).Returns(homologationVersionRepos.Object);
        }

        private IList<Homologation> GetHomologations()
        {
            var homologationsList = new List<Homologation>();
            var homologationGroup = new HomologationGroup { GroupTypeId = 14, Group = new Category { Name = "Product" } };

            homologationGroup.HomologationObjects.Add(new HomologationObject { HomologationObjectTypeId = 1, IsRequiredMapping = true, HomologationGroup = homologationGroup });
            homologationGroup.HomologationObjects.Add(new HomologationObject { HomologationObjectTypeId = 2, IsRequiredMapping = false, HomologationGroup = homologationGroup });
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "OriginalValue_1", DestinationValue = "UpdatedValue_1", HomologationGroup = new HomologationGroup { GroupTypeId = 14, Group = new Category { Name = "Product", Description = "Product" } } });
            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping { SourceValue = "OriginalValue_2", DestinationValue = "UpdatedValue_2", HomologationGroup = new HomologationGroup { GroupTypeId = 14, Group = new Category { Name = "Product", Description = "Product" } } });

            var homologation = new Homologation() { SourceSystemId = (int)SystemType.SINOPER, DestinationSystemId = (int)SystemType.TRUE };
            homologation.HomologationGroups.Add(homologationGroup);

            homologationsList.Add(homologation);
            return homologationsList;
        }
    }
}
