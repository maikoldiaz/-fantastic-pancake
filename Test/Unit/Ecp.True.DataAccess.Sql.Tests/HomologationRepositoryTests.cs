// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationRepositoryTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.DataAccess.Sql.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.DataAccess.Sql.Interfaces;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Repositories.Specialized;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The Homologation Repository Tests.
    /// </summary>
    [TestClass]
    public class HomologationRepositoryTests
    {
        /// <summary>
        /// The business context.
        /// </summary>
        private Mock<IBusinessContext> businessContext;

        /// <summary>
        /// The data context.
        /// </summary>
        private SqlDataContext dataContext;

        /// <summary>
        /// The movement repository.
        /// </summary>
        private HomologationRepository homologationRepository;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<Homologation> dataAccessHomologation;

        /// <summary>
        /// The data access.
        /// </summary>
        private SqlDataAccess<HomologationGroup> dataAccessHomologationGroup;

        /// <summary>
        /// The mock audit service.
        /// </summary>
        private Mock<IAuditService> mockAuditService;

        /// <summary>
        /// The SQL token provider.
        /// </summary>
        private Mock<ISqlTokenProvider> sqlTokenProvider;

        /// <summary>
        /// Initialize Method for the test class.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockAuditService = new Mock<IAuditService>();
            this.businessContext = new Mock<IBusinessContext>();
            this.sqlTokenProvider = new Mock<ISqlTokenProvider>();
            var options = new DbContextOptionsBuilder<SqlDataContext>()
                                    .UseInMemoryDatabase(databaseName: $"InMemoryDatabase_{Guid.NewGuid()}")
                                    .Options;
            this.mockAuditService.Setup(m => m.GetAuditLogs(It.IsAny<ChangeTracker>())).Returns(new List<AuditLog>());
            this.dataContext = new SqlDataContext(options, this.mockAuditService.Object, this.businessContext.Object, this.sqlTokenProvider.Object);

            this.dataAccessHomologation = new SqlDataAccess<Homologation>(this.dataContext);
            this.dataAccessHomologationGroup = new SqlDataAccess<HomologationGroup>(this.dataContext);

            this.homologationRepository = new HomologationRepository(this.dataAccessHomologation);
        }

        /// <summary>
        /// Test Method which adds the data[Homogolation & Homogolation Group] and check if the data is returned back.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task GetHomologationByIdAndGroupIdAsync_ShouldReturnTrueAsync()
        {
            // Arrange
            var homologation = this.GetNewHomologation();
            var homologationGroup = this.GetNewHomologationGroup(homologation.HomologationId);

            // Act
            this.dataAccessHomologation.Insert(homologation);
            this.dataAccessHomologationGroup.Insert(homologationGroup);
            var rows = await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(3, rows);

            var result = await this.homologationRepository.GetHomologationByIdAndGroupIdAsync(homologation.HomologationId, homologationGroup.GroupTypeId.Value).ConfigureAwait(false);
            Assert.IsNotNull(result, "Method must return HomologationGroup by HomogolationId and GroupId");

            await this.CleanupEntitiesAsync(homologation, homologationGroup).ConfigureAwait(false);
        }

        /// <summary>
        /// Test Method which adds the data[Homogolation & Homogolation Group] and check if null is returned with different Ids.
        /// </summary>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task GetHomologationByIdAndGroupIdAsync_ShouldReturnFalseAsync()
        {
            // Arrange
            var homologation = this.GetNewHomologation();
            var homologationGroup = this.GetNewHomologationGroup(homologation.HomologationId);

            // Act
            this.dataAccessHomologation.Insert(homologation);
            this.dataAccessHomologationGroup.Insert(homologationGroup);
            var rows = await this.dataContext.SaveChangesAsync().ConfigureAwait(false);

            // Assert
            Assert.AreEqual(3, rows);

            var result = await this.homologationRepository.GetHomologationByIdAndGroupIdAsync(2, 20).ConfigureAwait(false);
            Assert.IsNull(result, "Method must return null with different HomogolationId and GroupId");

            await this.CleanupEntitiesAsync(homologation, homologationGroup).ConfigureAwait(false);
        }

        /// <summary>
        /// Test method to get homologation group mappings.
        /// </summary>
        /// <param name="homologationGroupId">homologation Group Id.</param>
        /// <returns>Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetHomologationGroupMappingsAsync_ShouldRaiseExceptionAsync()
        {
            // Arrange
            var homologation = this.GetNewHomologation();
            var homologationGroup = this.GetNewHomologationGroup(homologation.HomologationId);

            // Act
            this.dataAccessHomologation.Insert(homologation);
            this.dataAccessHomologationGroup.Insert(homologationGroup);
            var rows = await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            Assert.IsNotNull(rows);
            await this.homologationRepository.GetHomologationGroupMappingsAsync(10).ConfigureAwait(false);
        }

        /// <summary>
        /// Test method to get homologation group mappings.
        /// </summary>
        /// <param name="homologationGroupId">homologation Group Id.</param>
        /// <returns>Task.</returns>
        [TestMethod]
        public async Task GetHomologationGroupMappingsAsync_ShouldReturnMappingsAsync()
        {
            // Arrange
            var homologation = this.GetNewHomologation();
            var homologationGroup = this.GetNewHomologationGroup(homologation.HomologationId);

            // Act
            this.dataAccessHomologation.Insert(homologation);
            this.dataAccessHomologationGroup.Insert(homologationGroup);
            await this.dataContext.SaveChangesAsync().ConfigureAwait(false);
            var result = await this.homologationRepository.GetHomologationGroupMappingsAsync(20).ConfigureAwait(false);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Fake Homogolation Object.
        /// </summary>
        /// <returns>Homogolation Object.</returns>
        private Homologation GetNewHomologation()
        {
            return new Homologation
            {
                HomologationId = 1,
                SourceSystemId = (int)SystemType.TRUE,
                DestinationSystemId = (int)SystemType.SINOPER,
            };
        }

        /// <summary>
        /// Fake Homogolation Group Object.
        /// </summary>
        /// <returns>Homogolation Group Object.</returns>
        private HomologationGroup GetNewHomologationGroup(int homologationId)
        {
            return new HomologationGroup
            {
                GroupTypeId = 13,
                HomologationId = homologationId,
                HomologationGroupId = 20,
                Group = new Category()
                {
                    CategoryId = 13,
                    Name = "Nodos",
                },
            };
        }

        /// <summary>
        /// clean up  entities.
        /// </summary>
        /// <param name="entities">entities.</param>
        /// <returns>Task.</returns>
        private Task CleanupEntitiesAsync(params object[] entities)
        {
            this.dataContext.RemoveRange(entities);
            return this.dataContext.SaveChangesAsync();
        }
    }
}
