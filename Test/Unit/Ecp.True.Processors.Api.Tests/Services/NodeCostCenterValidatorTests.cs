// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeCostCenterValidatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Api.Tests.Services
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Api.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class NodeCostCenterValidatorTests
    {
        private NodeCostCenterValidator nodeCostCenterValidator;

        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mock<IRepositoryFactory> mockFactory;
        private Mock<IRepository<LogisticMovement>> repoMock;

        [TestInitialize]
        public void Initialize()
        {
            this.repoMock = new Mock<IRepository<LogisticMovement>>();
            ConfigureMockRepoFactory(this.repoMock);

            this.nodeCostCenterValidator = new NodeCostCenterValidator(this.mockFactory.Object);

            void ConfigureMockRepoFactory(Mock<IRepository<LogisticMovement>> repoMock)
            {
                repoMock
                    .Setup(r => r.GetCountAsync(It.IsAny<Expression<Func<LogisticMovement, bool>>>()))
                    .Returns(Task.FromResult(5L));

                this.mockFactory = new Mock<IRepositoryFactory>();
                this.mockFactory.Setup(f => f.CreateRepository<LogisticMovement>())
                    .Returns(repoMock.Object);
            }
        }

        /// <summary>
        /// Validate for deletion should fail when the costCneter has movements.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ValidateForDeletion_ShouldFail_WhenNodeCostCenterHasMovementsAsync()
        {
            // Execute
            var sut = await this.nodeCostCenterValidator.ValidateForDeletionAsync(new NodeCostCenter()).ConfigureAwait(false);

            // Assert
            Assert.IsFalse(sut.IsSuccess);

            this.mockFactory.Verify(
                f => f.CreateRepository<LogisticMovement>(),
                Times.Once);
            this.repoMock.Verify(
                r => r.GetCountAsync(It.IsAny<Expression<Func<LogisticMovement, bool>>>()),
                Times.Once);
        }
    }
}
