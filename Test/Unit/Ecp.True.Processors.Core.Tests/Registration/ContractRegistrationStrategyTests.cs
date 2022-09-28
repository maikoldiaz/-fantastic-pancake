// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractRegistrationStrategyTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Core.Tests.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// ContractRegistrationStrategy tests.
    /// </summary>
    [TestClass]
    public class ContractRegistrationStrategyTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private readonly Mock<ITrueLogger<ContractRegistrationStrategy>> mockLogger = new Mock<ITrueLogger<ContractRegistrationStrategy>>();

        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private readonly Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

        /// <summary>
        /// The mock contract repository.
        /// </summary>
        private readonly Mock<IRepository<Contract>> mockContractRepository = new Mock<IRepository<Contract>>();

        /// <summary>
        /// The mock contract.
        /// </summary>
        private readonly Mock<Contract> mockContract = new Mock<Contract>();

        /// <summary>
        /// The contract registration strategy.
        /// </summary>
        private ContractRegistrationStrategy contractRegistrationStrategy;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockContract.Setup(m => m.CopyFrom(It.IsAny<IEntity>()));
            this.mockContractRepository.Setup(m => m.Insert(It.IsAny<Contract>()));
            this.mockContractRepository.Setup(m => m.Update(It.IsAny<Contract>()));
            this.mockContractRepository.Setup(m => m.Delete(It.IsAny<Contract>()));
            this.mockContractRepository.Setup(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contract, bool>>>())).ReturnsAsync(this.mockContract.Object);
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Contract>()).Returns(this.mockContractRepository.Object);
            this.contractRegistrationStrategy = new ContractRegistrationStrategy(this.mockLogger.Object, this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Registers the asynchronous should throw argument exception when entity is null asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterAsync_ShouldThrowArgumentException_WhenEntityIsNullAsync()
        {
            await this.contractRegistrationStrategy.RegisterAsync(null, this.mockUnitOfWork.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the asynchronous should throw argument exception when unit of work is null asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterAsync_ShouldThrowArgumentException_WhenUnitOfWorkIsNullAsync()
        {
            await this.contractRegistrationStrategy.RegisterAsync(It.IsAny<object>(), null).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the asynchronous should register entity when invoked.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldInsertEntity_WhenActionTypeInsertAsync()
        {
            // Arrange
            var entity = new Contract { ActionType = "Insert" };

            // Act
            await this.contractRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Contract>(), Times.Exactly(1));
            this.mockContractRepository.Verify(m => m.Insert(It.IsAny<Contract>()), Times.Exactly(1));
            this.mockContractRepository.Verify(m => m.Update(It.IsAny<Contract>()), Times.Never);
            this.mockContractRepository.Verify(m => m.Delete(It.IsAny<Contract>()), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(1));
        }

        /// <summary>
        /// Registers the asynchronous should update entity when action type update asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldUpdateEntity_WhenActionTypeUpdateAsync()
        {
            // Arrange
            var entity = new Contract { ActionType = "Update" };

            // Act
            await this.contractRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Contract>(), Times.Exactly(1));
            this.mockContractRepository.Verify(m => m.Insert(It.IsAny<Contract>()), Times.Never);
            this.mockContractRepository.Verify(m => m.Update(It.IsAny<Contract>()), Times.Exactly(1));
            this.mockContractRepository.Verify(m => m.Delete(It.IsAny<Contract>()), Times.Never);
            this.mockContractRepository.Verify(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contract, bool>>>()), Times.Exactly(1));
            this.mockContract.Verify(m => m.CopyFrom(It.IsAny<IEntity>()), Times.Exactly(1));
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(1));
        }

        /// <summary>
        /// Registers the asynchronous should delete entity when action type delete asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task RegisterAsync_ShouldDeleteEntity_WhenActionTypeDeleteAsync()
        {
            // Arrange
            var entity = new Contract { ActionType = "Delete" };

            // Act
            await this.contractRegistrationStrategy.RegisterAsync(entity, this.mockUnitOfWork.Object).ConfigureAwait(false);

            // Assert
            this.mockUnitOfWork.Verify(m => m.CreateRepository<Contract>(), Times.Exactly(1));
            this.mockContractRepository.Verify(m => m.Insert(It.IsAny<Contract>()), Times.Never);
            this.mockContractRepository.Verify(m => m.Update(It.IsAny<Contract>()), Times.Exactly(1));
            this.mockContractRepository.Verify(m => m.Delete(It.IsAny<Contract>()), Times.Never);
            this.mockContractRepository.Verify(m => m.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contract, bool>>>()), Times.Exactly(1));
            this.mockContract.Verify(m => m.CopyFrom(It.IsAny<IEntity>()), Times.Never);
            this.mockUnitOfWork.Verify(m => m.SaveAsync(CancellationToken.None), Times.Exactly(1));
        }

        /// <summary>
        /// Inserts the should log information message when invoke.
        /// </summary>
        [TestMethod]
        public void Insert_ShouldLogInformationMessage_WhenInvoke()
        {
            // Arrange
            var entity = new Contract { ActionType = "Delete" };

            // Act
            this.contractRegistrationStrategy.Insert(new List<Contract> { entity }, this.mockUnitOfWork.Object);

            // Assert
            this.mockLogger.Verify(a => a.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }
    }
}
