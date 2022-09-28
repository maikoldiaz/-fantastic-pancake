// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRegistrationStrategyTests.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Registration;
    using Ecp.True.Proxies.Azure;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OwnershipRegistrationStrategyTests
    {
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
        private readonly Mock<IRepository<Ownership>> mockOwnershipRepository = new Mock<IRepository<Ownership>>();

        /// <summary>
        /// The mock logger.
        /// </summary>
        private readonly Mock<ITrueLogger> mockLogger = new Mock<ITrueLogger>();

        /// <summary>
        /// The ownership registration strategy.
        /// </summary>
        private OwnershipRegistrationStrategy ownershipRegistrationStrategy;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.mockUnitOfWork.Setup(m => m.CreateRepository<Ownership>()).Returns(this.mockOwnershipRepository.Object);
            this.ownershipRegistrationStrategy = new OwnershipRegistrationStrategy(this.mockLogger.Object, this.mockAzureClientFactory.Object);
            this.mockOwnershipRepository.Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Movement, bool>>>())).ReturnsAsync(new List<Movement> { new Movement { NetStandardVolume = 1, CreatedDate = DateTime.Now } });
        }

        /// <summary>
        /// Registers the asynchronous should throw argument exception when entity is null asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterAsync_ShouldThrowArgumentException_WhenEntityIsNullAsync()
        {
            await this.ownershipRegistrationStrategy.RegisterAsync(null, this.mockUnitOfWork.Object).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers the asynchronous should throw argument exception when unit of work is null asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RegisterAsync_ShouldThrowArgumentException_WhenUnitOfWorkIsNullAsync()
        {
            await this.ownershipRegistrationStrategy.RegisterAsync(It.IsAny<object>(), null).ConfigureAwait(false);
        }
    }
}
