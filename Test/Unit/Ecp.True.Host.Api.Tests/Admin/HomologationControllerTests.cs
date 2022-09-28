// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationControllerTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Host.Api.Controllers;
    using Ecp.True.Host.Core.Result;
    using Ecp.True.Processors.Api.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The category controller tests.
    /// </summary>
    [TestClass]
    public class HomologationControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private HomologationController controller;

        /// <summary>
        /// The mock processor.
        /// </summary>
        private Mock<IHomologationProcessor> mockProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockProcessor = new Mock<IHomologationProcessor>();
            this.controller = new HomologationController(this.mockProcessor.Object);
        }

        /// <summary>
        /// Gets the homologations asynchronous should return homologations.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetHomologationsAsync_ShouldInvokeProcessor_ToReturnHomologationsAsync()
        {
            var homologations = new[] { new Homologation() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<Homologation>(null)).ReturnsAsync(homologations);

            var result = await this.controller.QueryHomologationsAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, homologations);

            this.mockProcessor.Verify(c => c.QueryAllAsync<Homologation>(null), Times.Once());
        }

        /// <summary>
        /// Gets the homologation by identifier asynchronous should invoke processor to get homologation by identifier.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetHomologationByIdAsync_ShouldInvokeProcessor_ToGetHomologationByIdAsync()
        {
            var homologation = new Homologation();
            this.mockProcessor.Setup(m => m.GetHomologationByIdAsync(It.IsAny<int>())).ReturnsAsync(homologation);

            var result = await this.controller.GetHomologationByIdAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetHomologationByIdAsync(1), Times.Once());
        }

        /// <summary>
        /// Gets the homologation group by identifier asynchronous should invoke processor to get homologation group by identifier.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetHomologationGroupByIdAndGroupIdAsync_ShouldInvokeProcessor_ToGetHomologationGroupAsync()
        {
            var group = new HomologationGroup();
            this.mockProcessor.Setup(m => m.GetHomologationByIdAndGroupIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(group);

            var result = await this.controller.GetHomologationByIdAndGroupIdAsync(1, 1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetHomologationByIdAndGroupIdAsync(1, 1), Times.Once());
        }

        /// <summary>
        /// Creates the homologation asynchronous should invoke processor to return200 success.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task CreateHomologationAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var homologation = new Homologation();
            homologation.HomologationGroups.Add(new HomologationGroup());
            this.mockProcessor.Setup(m => m.SaveHomologationAsync(homologation));

            var result = await this.controller.CreateHomologationAsync(homologation).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveHomologationAsync(homologation), Times.Once());
        }

        /// <summary>
        /// Update the homologation asynchronous should invoke processor to return200 success.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task UpdateHomologationAsync_ShouldInvokeProcessor_ToReturn200SuccessAsync()
        {
            var homologation = new Homologation();
            homologation.HomologationGroups.Add(new HomologationGroup() { HomologationGroupId = 1 });
            this.mockProcessor.Setup(m => m.SaveHomologationAsync(homologation));

            var result = await this.controller.CreateHomologationAsync(homologation).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.SaveHomologationAsync(homologation), Times.Once());
        }

        /// <summary>
        /// Gets the homologations group asynchronous should return homologations group.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetHomologationsGroupAsync_ShouldInvokeProcessor_ToReturnHomologationGroupsAsync()
        {
            var homologationGroups = new[] { new HomologationGroup() }.AsQueryable();
            this.mockProcessor.Setup(m => m.QueryAllAsync<HomologationGroup>(null)).ReturnsAsync(homologationGroups);

            var result = await this.controller.QueryHomologationGroupAsync().ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");
            Assert.AreEqual(result, homologationGroups);

            this.mockProcessor.Verify(c => c.QueryAllAsync<HomologationGroup>(null), Times.Once());
        }

        /// <summary>
        /// Delete Homologation group Async and return result.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task DeleteHomologationGroup_ShouldInvokeProcessor_ToReturnDeleteResultAsync()
        {
            this.mockProcessor.Setup(m => m.DeleteHomologationGroupAsync(It.IsAny<DeleteHomologationGroup>()));
            var homologation = new Homologation { HomologationId = 1 };
            var homologationGroup = new HomologationGroup { HomologationGroupId = 2 };

            var deleteHomologationGroup = new DeleteHomologationGroup()
            {
                HomologationGroupId = homologationGroup.HomologationGroupId,
                HomologationId = homologation.HomologationId,
                RowVersion = It.IsAny<byte[]>(),
            };

            var result = await this.controller.DeleteHomologationGroupAsync(deleteHomologationGroup).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.IsNotNull(result, "Ok Result should not be null");

            this.mockProcessor.Verify(c => c.DeleteHomologationGroupAsync(It.IsAny<DeleteHomologationGroup>()), Times.Once());
        }

        /// <summary>
        /// Get All Homologation Object Types.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetAllHomologationObjectTypes_ShouldInvokeProcessor_ToGetHomologationObjectTypesAsync()
        {
            var lstHomologations = new List<HomologationObjectType>();
            this.mockProcessor.Setup(m => m.GetHomologationObjectTypesAsync()).ReturnsAsync(lstHomologations);

            var result = await this.controller.GetHomologationObjectTypesAsync().ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetHomologationObjectTypesAsync(), Times.Once());
        }

        /// <summary>
        /// Determines whether [is homologation group exists by group type id, source system id, destination system id asynchronous should invoke processor to verify if homologation group exists].
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task IsHomologationGroupExistsByGroupIdAsync_ShouldInvokeProcessor_ToVerifyIfCategoryExistsByNameAsync()
        {
            var homologation = new Homologation { SourceSystemId = 1, DestinationSystemId = 2 };
            var homologationGroup = new HomologationGroup { GroupTypeId = 13 };

            this.mockProcessor.Setup(m => m.CheckHomologationGroupExistsAsync(homologation.SourceSystemId.Value, homologation.DestinationSystemId.Value, homologationGroup.GroupTypeId.Value)).ReturnsAsync(homologationGroup);

            var result = await this.controller.CheckHomologationGroupExistsAsync(homologation.SourceSystemId.Value, homologation.DestinationSystemId.Value, homologationGroup.GroupTypeId.Value).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityExistsResult));
            this.mockProcessor.Verify(c => c.CheckHomologationGroupExistsAsync(homologation.SourceSystemId.Value, homologation.DestinationSystemId.Value, homologationGroup.GroupTypeId.Value), Times.Once());
        }

        /// <summary>
        /// Gets the homologation group by identifier asynchronous should invoke processor to get homologation group by identifier.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        [TestMethod]
        public async Task GetHomologationGroupMappingAsync_ShouldInvokeProcessor_ToGetHomologationMappingAsync()
        {
            var homologationDataMappings = new List<HomologationDataMapping>();
            this.mockProcessor.Setup(m => m.GetHomologationGroupMappingsAsync(It.IsAny<int>())).ReturnsAsync(homologationDataMappings);

            var result = await this.controller.GetHomologationGroupMappingsAsync(1).ConfigureAwait(false);

            Assert.IsInstanceOfType(result, typeof(EntityResult));
            this.mockProcessor.Verify(c => c.GetHomologationGroupMappingsAsync(1), Times.Once());
        }
    }
}
