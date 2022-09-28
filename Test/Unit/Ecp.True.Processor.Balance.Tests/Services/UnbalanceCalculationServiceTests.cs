// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnbalanceCalculationServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Balance.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Processors.Balance.Calculation;
    using Ecp.True.Processors.Balance.Calculation.Input;
    using Ecp.True.Processors.Balance.Calculation.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The interface movement generator tests.
    /// </summary>
    [TestClass]
    public class UnbalanceCalculationServiceTests
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The balance communicator.
        /// </summary>
        private Mock<IUnbalanceCalculator> unbalanceCalculator;

        /// <summary>
        /// The unbalance repository mock.
        /// </summary>
        private Mock<IRepository<Unbalance>> unbalanceRepositoryMock;

        /// <summary>
        /// The mock balance service.
        /// </summary>
        private UnbalanceCalculationService unbalanceCalculationService;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unbalanceCalculator = new Mock<IUnbalanceCalculator>();
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unbalanceRepositoryMock = new Mock<IRepository<Unbalance>>();
            this.unbalanceRepositoryMock.Setup(s => s.UpdateAll(It.IsAny<List<Unbalance>>()));
            this.unitOfWorkMock.Setup(s => s.CreateRepository<Unbalance>()).Returns(this.unbalanceRepositoryMock.Object);
            this.unbalanceCalculationService = new UnbalanceCalculationService(this.unbalanceCalculator.Object);
        }

        /// <summary>
        /// Unidentified Loss MovementGenerator.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UnbalanceCalculationService_ShouldUpdateUnbalance_WithSuccessAsync()
        {
            var nodes = new List<NodeInput>();
            var movements = new List<MovementCalculationInput>();
            var initialInventories = new List<InventoryInput>();
            var finalInventories = new List<InventoryInput>();
            nodes.Add(new NodeInput
            {
                NodeId = 1,
                Name = "Test 1",
                AcceptableBalancePercentage = 10,
                CalculationDate = new DateTime(2019, 02, 12),
                ControlLimit = 1,
            });

            movements.Add(new MovementCalculationInput
            {
                MovementId = "1",
                MeasurementUnit = "9",
                DestinationNodeId = 1,
                DestinationProductId = "1",
                DestinationProductName = "Test 2",
                MessageTypeId = 1,
                NetStandardVolume = 10,
                SourceNodeId = 2,
                SourceProductId = "2",
                SourceProductName = "Test 3",
                OperationalDate = new DateTime(2019, 02, 12),
                UncertaintyPercentage = 9,
            });

            initialInventories.Add(new InventoryInput
            {
                NodeId = 1,
                ProductId = "1",
                MeasurementUnit = "Bbl",
                ProductName = "Test 4",
                ProductVolume = 8,
                UncertaintyPercentage = 9,
                InventoryId = "1",
                InventoryDate = new DateTime(2019, 02, 11),
            });

            finalInventories.Add(new InventoryInput
            {
                NodeId = 1,
                ProductId = "1",
                MeasurementUnit = "Bbl",
                ProductName = "Test 4",
                ProductVolume = 8,
                UncertaintyPercentage = 9,
                InventoryId = "1",
                InventoryDate = new DateTime(2019, 02, 12),
            });

            var balanceInputInfo = new BalanceInputInfo
            {
                CalculationDate = DateTime.Now,
                InitialInventories = initialInventories,
                FinalInventories = finalInventories,
                Movements = movements,
                Nodes = nodes,
            };

            var unbalances = new List<Unbalance>
            {
                new Unbalance
                {
                    NodeId = 1,
                    ProductId = "1",
                },
            };

            var unbalanceComments = new List<UnbalanceComment>
            {
                new UnbalanceComment
                {
                    NodeId = 1,
                    ProductId = "1",
                    Unbalance = 1.0m,
                },
            };

            this.unbalanceCalculator.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult<IEnumerable<UnbalanceComment>>(unbalanceComments));
            this.unbalanceRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<Unbalance, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(unbalances);
            var ticket = new Ticket { TicketId = 23678, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };

            // Act
            var result = await this.unbalanceCalculationService.ProcessAsync(balanceInputInfo, ticket, this.unitOfWorkMock.Object).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Unbalances.ToList().Count);
            Assert.AreEqual("1", result.Unbalances.ToList()[0].ProductId);
            this.unbalanceRepositoryMock.Verify(x => x.UpdateAll(It.IsAny<List<Unbalance>>()), Times.Never);
        }

        /// <summary>
        /// Unidentified Loss MovementGenerator.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task UnbalanceCalculationService_ShouldNotUpdateUnbalanceWithUnitOfWorkNull_WithSuccessAsync()
        {
            var nodes = new List<NodeInput>();
            var movements = new List<MovementCalculationInput>();
            var initialInventories = new List<InventoryInput>();
            var finalInventories = new List<InventoryInput>();
            nodes.Add(new NodeInput
            {
                NodeId = 1,
                Name = "Test 1",
                AcceptableBalancePercentage = 10,
                CalculationDate = new DateTime(2019, 02, 12),
                ControlLimit = 1,
            });

            movements.Add(new MovementCalculationInput
            {
                MovementId = "1",
                MeasurementUnit = "9",
                DestinationNodeId = 1,
                DestinationProductId = "1",
                DestinationProductName = "Test 2",
                MessageTypeId = 1,
                NetStandardVolume = 10,
                SourceNodeId = 2,
                SourceProductId = "2",
                SourceProductName = "Test 3",
                OperationalDate = new DateTime(2019, 02, 12),
                UncertaintyPercentage = 9,
            });

            initialInventories.Add(new InventoryInput
            {
                NodeId = 1,
                ProductId = "1",
                MeasurementUnit = "Bbl",
                ProductName = "Test 4",
                ProductVolume = 8,
                UncertaintyPercentage = 9,
                InventoryId = "1",
                InventoryDate = new DateTime(2019, 02, 11),
            });

            finalInventories.Add(new InventoryInput
            {
                NodeId = 1,
                ProductId = "1",
                MeasurementUnit = "Bbl",
                ProductName = "Test 4",
                ProductVolume = 8,
                UncertaintyPercentage = 9,
                InventoryId = "1",
                InventoryDate = new DateTime(2019, 02, 12),
            });

            var balanceInputInfo = new BalanceInputInfo
            {
                CalculationDate = DateTime.Now,
                InitialInventories = initialInventories,
                FinalInventories = finalInventories,
                Movements = movements,
                Nodes = nodes,
            };

            var unbalances = new List<Unbalance>
            {
                new Unbalance
                {
                    NodeId = 1,
                    ProductId = "1",
                },
            };

            var unbalanceComments = new List<UnbalanceComment>
            {
                new UnbalanceComment
                {
                    NodeId = 1,
                    ProductId = "1",
                    Unbalance = 1.0m,
                },
            };

            this.unbalanceCalculator.Setup(x => x.CalculateAsync(It.IsAny<CalculationInput>())).Returns(Task.FromResult<IEnumerable<UnbalanceComment>>(unbalanceComments));
            this.unbalanceRepositoryMock.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<Unbalance, bool>>>(), It.IsAny<string[]>())).ReturnsAsync(unbalances);
            var ticket = new Ticket { TicketId = 23678, CategoryElementId = 2, StartDate = new DateTime(2019, 02, 12), EndDate = new DateTime(2019, 02, 16), Status = 0, CreatedBy = "System", CreatedDate = new DateTime(2019, 02, 18), LastModifiedBy = null, LastModifiedDate = null };

            // Act
            var result = await this.unbalanceCalculationService.ProcessAsync(balanceInputInfo, ticket, null).ConfigureAwait(false);

            // Assert or Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Unbalances.ToList().Count);
            Assert.AreEqual("1", result.Unbalances.ToList()[0].ProductId);
            this.unbalanceRepositoryMock.Verify(x => x.UpdateAll(It.IsAny<List<Unbalance>>()), Times.Never);
        }
    }
}
