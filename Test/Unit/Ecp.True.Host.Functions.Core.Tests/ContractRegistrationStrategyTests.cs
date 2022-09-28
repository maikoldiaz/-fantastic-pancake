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

namespace Ecp.True.Host.Functions.Core.Tests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ContractRegistrationStrategyTests
    {
        /// <summary>
        /// The mock azure client factory.
        /// </summary>
        private readonly Mock<IAzureClientFactory> mockAzureClientFactory = new Mock<IAzureClientFactory>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<ContractRegistrationStrategy>> mockLogger;

        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private Mock<IConfigurationHandler> mockConfigurationHandler;

        /// <summary>
        /// The mock unit of work.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The mock contract repository.
        /// </summary>
        private Mock<IRepository<Contract>> mockContractRepository;

        /// <summary>
        /// The contract registration strategy.
        /// </summary>
        private ContractRegistrationStrategy contractRegistrationStrategy;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<ContractRegistrationStrategy>>();
            this.mockConfigurationHandler = new Mock<IConfigurationHandler>();
            this.mockContractRepository = new Mock<IRepository<Contract>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();

            this.contractRegistrationStrategy =
                new ContractRegistrationStrategy(
                    this.mockLogger.Object,
                    this.mockAzureClientFactory.Object);
        }

        /// <summary>
        /// Contracts the registration strategy insert should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ContractRegistrationStrategy_Insert_Should_RegisterAsync()
        {
            var contract = this.GetContract("Insert");

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Contract>()).Returns(this.mockContractRepository.Object);
            this.mockContractRepository.Setup(a => a.Insert(It.IsAny<Contract>()));
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            await this.contractRegistrationStrategy.RegisterAsync(contract, this.mockUnitOfWork.Object).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.CreateRepository<Contract>(), Times.Once);
            this.mockContractRepository.Verify(a => a.Insert(It.IsAny<Contract>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Contracts the registration strategy update should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ContractRegistrationStrategy_Update_Should_RegisterAsync()
        {
            var contract = this.GetContract("Update");

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Contract>()).Returns(this.mockContractRepository.Object);
            this.mockContractRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contract, bool>>>())).ReturnsAsync(contract);
            this.mockContractRepository.Setup(a => a.Update(It.IsAny<Contract>()));
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            await this.contractRegistrationStrategy.RegisterAsync(contract, this.mockUnitOfWork.Object).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.CreateRepository<Contract>(), Times.Once);
            this.mockContractRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contract, bool>>>()), Times.Once);
            this.mockContractRepository.Verify(a => a.Update(It.IsAny<Contract>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Contracts the registration strategy delete should register asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task ContractRegistrationStrategy_Delete_Should_RegisterAsync()
        {
            var contract = this.GetContract("Delete");

            this.mockUnitOfWork.Setup(a => a.CreateRepository<Contract>()).Returns(this.mockContractRepository.Object);
            this.mockContractRepository.Setup(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contract, bool>>>())).ReturnsAsync(contract);
            this.mockContractRepository.Setup(a => a.Update(It.IsAny<Contract>()));
            this.mockUnitOfWork.Setup(a => a.SaveAsync(It.IsAny<CancellationToken>()));

            await this.contractRegistrationStrategy.RegisterAsync(contract, this.mockUnitOfWork.Object).ConfigureAwait(false);

            this.mockUnitOfWork.Verify(a => a.CreateRepository<Contract>(), Times.Once);
            this.mockContractRepository.Verify(a => a.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contract, bool>>>()), Times.Once);
            this.mockContractRepository.Verify(a => a.Update(It.IsAny<Contract>()), Times.Once);
            this.mockUnitOfWork.Verify(a => a.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <param name="actionType">Type of the action.</param>
        /// <returns>The Contract.</returns>
        private Contract GetContract(string actionType)
        {
            var contract = new Contract
            {
               ActionType = actionType,
            };

            return contract;
        }
    }
}
