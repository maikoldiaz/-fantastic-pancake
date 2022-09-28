// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SegmentCalculatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Balance.Tests.Calculation
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Balance.Calculation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The segment calculator tests.
    /// </summary>
    [TestClass]
    public class SegmentCalculatorTests
    {
        /// <summary>
        /// The segment calculator.
        /// </summary>
        private SegmentCalculator segmentCalculator;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock;

        /// <summary>
        /// The logger.
        /// </summary>
        private Mock<IUnitOfWork> unitOfWorkMock;

        /// <summary>
        /// The segment repository mock.
        /// </summary>
        private Mock<IRepository<SegmentUnbalance>> mockSegmentRepository;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();
            this.unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            this.mockSegmentRepository = new Mock<IRepository<SegmentUnbalance>>();
            this.unitOfWorkFactoryMock.Setup(x => x.GetUnitOfWork()).Returns(this.unitOfWorkMock.Object);
            this.unitOfWorkMock.Setup(s => s.CreateRepository<SegmentUnbalance>()).Returns(this.mockSegmentRepository.Object);
            this.mockSegmentRepository.Setup(s => s.Insert(It.IsAny<SegmentUnbalance>()));
            this.segmentCalculator = new SegmentCalculator();
        }

        /// <summary>
        /// system calculator should calculate.
        /// </summary>
        [TestMethod]
        public void SegmentCalculator_ShouldCalculate_WithSuccess()
        {
            var inputMovements = new List<Movement>
            {
                new Movement
                {
                    MovementId = "1",
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 1,
                        DestinationProductId = "1",
                        DestinationProduct = new Product
                        {
                            ProductId = "1",
                        },
                    },
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 2,
                        SourceProductId = "2",
                        SourceProduct = new Product
                        {
                            ProductId = "2",
                        },
                    },
                    OperationalDate = DateTime.Now.AddDays(-1),
                    MessageTypeId = 1,
                    NetStandardVolume = 100,
                },
            };

            var outputMovements = new List<Movement>
            {
                new Movement
                {
                    MovementId = "2",
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 3,
                        DestinationProductId = "3",
                        DestinationProduct = new Product
                        {
                            ProductId = "3",
                        },
                    },
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                        SourceProduct = new Product
                        {
                            ProductId = "1",
                        },
                    },
                    OperationalDate = DateTime.Now.AddDays(-1),
                    MessageTypeId = 1,
                    NetStandardVolume = 200,
                },
            };

            var initialInventories = new List<InventoryProduct>
            {
                new InventoryProduct
                {
                    InventoryProductId = 1,
                    ProductVolume = 300,
                },
            };

            var finalInventories = new List<InventoryProduct>
            {
                new InventoryProduct
                {
                    InventoryProductId = 1,
                    ProductVolume = 200,
                },
            };

            var movements = new List<Movement>
            {
                new Movement
                {
                    MovementId = "1",
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 1,
                        DestinationProductId = "1",
                        DestinationProduct = new Product
                        {
                            ProductId = "1",
                        },
                    },
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 2,
                        SourceProductId = "2",
                        SourceProduct = new Product
                        {
                            ProductId = "2",
                        },
                    },
                    OperationalDate = DateTime.Now.AddDays(-1),
                    MessageTypeId = 1,
                    NetStandardVolume = 100,
                    VariableTypeId = VariableType.Interface,
                },
                new Movement
                {
                    MovementId = "2",
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 3,
                        DestinationProductId = "3",
                        DestinationProduct = new Product
                        {
                            ProductId = "3",
                        },
                    },
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                        SourceProduct = new Product
                        {
                            ProductId = "1",
                        },
                    },
                    OperationalDate = DateTime.Now.AddDays(-1),
                    MessageTypeId = 1,
                    NetStandardVolume = 200,
                    VariableTypeId = VariableType.Interface,
                },
                new Movement
                {
                    MovementId = "3",
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 1,
                        DestinationProductId = "1",
                        DestinationProduct = new Product
                        {
                            ProductId = "1",
                        },
                    },
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 4,
                        SourceProductId = "4",
                        SourceProduct = new Product
                        {
                            ProductId = "4",
                        },
                    },
                    OperationalDate = DateTime.Now.AddDays(-1),
                    MessageTypeId = 1,
                    NetStandardVolume = 400,
                    VariableTypeId = VariableType.BalanceTolerance,
                },
                new Movement
                {
                    MovementId = "4",
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 5,
                        DestinationProductId = "5",
                        DestinationProduct = new Product
                        {
                            ProductId = "5",
                        },
                    },
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                        SourceProduct = new Product
                        {
                            ProductId = "1",
                        },
                    },
                    OperationalDate = DateTime.Now.AddDays(-1),
                    MessageTypeId = 1,
                    NetStandardVolume = 200,
                    VariableTypeId = VariableType.BalanceTolerance,
                },
            };

            // Act
            var result = this.segmentCalculator.CalculateAndGetSegmentUnbalance("1", DateTime.Now.AddDays(-1), 1, 1, inputMovements, outputMovements, initialInventories, finalInventories, movements, movements, this.unitOfWorkMock.Object);

            // Assert or Verify
            Assert.IsNotNull(result);
            Assert.AreEqual(result.InitialInventoryVolume, new decimal(300));
            Assert.AreEqual(result.FinalInventoryVolume, new decimal(200));
            Assert.AreEqual(result.InputVolume, new decimal(100));
            Assert.AreEqual(result.OutputVolume, new decimal(200));
            Assert.AreEqual(result.InterfaceVolume, new decimal(-100));
            Assert.AreEqual(result.ToleranceVolume, new decimal(200));
            Assert.AreEqual(result.UnidentifiedLossesVolume, new decimal(0));
            Assert.AreEqual(result.UnbalanceVolume, new decimal(100));
        }
    }
}
