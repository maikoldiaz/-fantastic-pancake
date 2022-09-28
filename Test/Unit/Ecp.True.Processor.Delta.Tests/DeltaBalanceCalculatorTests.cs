// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaBalanceCalculatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Delta.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Calculation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The delta balance calculator tests.
    /// </summary>
    [TestClass]
    public class DeltaBalanceCalculatorTests
    {
        /// <summary>
        /// The movement map.
        /// </summary>
        private static readonly ConcurrentDictionary<string, List<Movement>> MovementMap =
            new ConcurrentDictionary<string, List<Movement>>();

        /// <summary>
        /// The consolidated movement map.
        /// </summary>
        private static readonly ConcurrentDictionary<string, List<ConsolidatedMovement>> ConsolidatedMovementMap =
            new ConcurrentDictionary<string, List<ConsolidatedMovement>>();

        /// <summary>
        /// The consolidated inventory map.
        /// </summary>
        private static readonly ConcurrentDictionary<string, List<ConsolidatedInventoryProduct>> ConsolidatedInventoryMap =
            new ConcurrentDictionary<string, List<ConsolidatedInventoryProduct>>();

        /// <summary>
        /// The delta balance calculator.
        /// </summary>
        private DeltaBalanceCalculator deltaBalanceCalculator;

        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<DeltaBalanceCalculator>> mockLogger;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<DeltaBalanceCalculator>>();
            this.deltaBalanceCalculator = new DeltaBalanceCalculator(this.mockLogger.Object);
            var movements1 = new List<Movement>
            {
                new Movement
                {
                    MovementTransactionId = 1,
                    MeasurementUnit = 31,
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                    },
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 2,
                        DestinationProductId = "2",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 6, 1),
                        EndTime = new DateTime(2020, 6, 30),
                    },
                    OfficialDeltaMessageTypeId = OfficialDeltaMessageType.OfficialMovementDelta,
                },
                new Movement
                {
                    MovementTransactionId = 2,
                    MeasurementUnit = 31,
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 2,
                        SourceProductId = "2",
                    },
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 1,
                        DestinationProductId = "1",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 6, 1),
                        EndTime = new DateTime(2020, 6, 30),
                    },
                    SourceSystemId = Core.Constants.ManualMovOfficial,
                },
                new Movement
                {
                    MovementTransactionId = 3,
                    MeasurementUnit = 31,
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 5, 31),
                        EndTime = new DateTime(2020, 5, 31),
                    },
                    OfficialDeltaMessageTypeId = OfficialDeltaMessageType.OfficialInventoryDelta,
                    OperationalDate = new DateTime(2020, 5, 31),
                },
                new Movement
                {
                    MovementTransactionId = 4,
                    MeasurementUnit = 31,
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 6, 30),
                        EndTime = new DateTime(2020, 6, 30),
                    },
                    SourceSystemId = Core.Constants.ManualInvOfficial,
                    OperationalDate = new DateTime(2020, 5, 31),
                },
                new Movement
                {
                    MovementTransactionId = 6,
                    MeasurementUnit = 31,
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 1,
                        DestinationProductId = "1",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 6, 1),
                        EndTime = new DateTime(2020, 6, 30),
                    },
                    SourceSystemId = Core.Constants.ManualInvOfficial,
                    OperationalDate = new DateTime(2020, 6, 30),
                },
                new Movement
                {
                    MovementTransactionId = 5,
                    MeasurementUnit = 31,
                    MovementSource = new MovementSource
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 6, 1),
                        EndTime = new DateTime(2020, 6, 30),
                    },
                    SourceSystemId = Core.Constants.ManualInvOfficial,
                    OperationalDate = new DateTime(2020, 5, 31),
                },
                new Movement
                {
                    MovementTransactionId = 6,
                    MeasurementUnit = 31,
                    MovementDestination = new MovementDestination
                    {
                        DestinationNodeId = 1,
                        DestinationProductId = "1",
                    },
                    Period = new MovementPeriod
                    {
                        StartTime = new DateTime(2020, 6, 1),
                        EndTime = new DateTime(2020, 6, 30),
                    },
                    SourceSystemId = Core.Constants.ManualInvOfficial,
                    OperationalDate = new DateTime(2020, 6, 30),
                },
            };

            movements1[0].Owners.Add(
            new Owner
            {
                OwnerId = 1,
                OwnershipValue = 100,
            });

            movements1[0].Owners.Add(
            new Owner
            {
                OwnerId = 2,
                OwnershipValue = 200,
            });

            movements1[1].Owners.Add(
            new Owner
            {
                OwnerId = 1,
                OwnershipValue = 300,
            });

            movements1[1].Owners.Add(
            new Owner
            {
                OwnerId = 2,
                OwnershipValue = 400,
            });

            movements1[2].Owners.Add(
            new Owner
            {
                OwnerId = 1,
                OwnershipValue = 100,
            });

            movements1[2].Owners.Add(
            new Owner
            {
                OwnerId = 2,
                OwnershipValue = 200,
            });

            movements1[3].Owners.Add(
            new Owner
            {
                OwnerId = 1,
                OwnershipValue = 300,
            });

            movements1[3].Owners.Add(
            new Owner
            {
                OwnerId = 2,
                OwnershipValue = 400,
            });

            movements1[4].Owners.Add(
            new Owner
            {
                OwnerId = 1,
                OwnershipValue = 500,
            });

            movements1[4].Owners.Add(
            new Owner
            {
                OwnerId = 2,
                OwnershipValue = 400,
            });

            movements1[5].Owners.Add(
            new Owner
            {
                OwnerId = 1,
                OwnershipValue = 600,
            });

            movements1[5].Owners.Add(
            new Owner
            {
                OwnerId = 2,
                OwnershipValue = 700,
            });

            MovementMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("1", "1", "1", "31"), movements1, (key, oldValue) => movements1);
            MovementMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("1", "1", "2", "31"), movements1, (key, oldValue) => movements1);
            MovementMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("2", "2", "1", "31"), movements1, (key, oldValue) => movements1);
            MovementMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("2", "2", "2", "31"), movements1, (key, oldValue) => movements1);

            var consolidatedMovements = new List<ConsolidatedMovement>
                {
                    new ConsolidatedMovement
                    {
                        SourceNodeId = 1,
                        SourceProductId = "1",
                        DestinationNodeId = 2,
                        DestinationProductId = "2",
                        StartDate = new DateTime(2020, 6, 1),
                        EndDate = new DateTime(2020, 6, 30),
                    },
                    new ConsolidatedMovement
                    {
                        SourceNodeId = 2,
                        SourceProductId = "2",
                        DestinationNodeId = 1,
                        DestinationProductId = "1",
                        StartDate = new DateTime(2020, 6, 1),
                        EndDate = new DateTime(2020, 6, 30),
                    },
                };
            consolidatedMovements[0].ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 100,
                    });
            consolidatedMovements[0].ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 200,
                    });
            consolidatedMovements[1].ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 300,
                    });
            consolidatedMovements[1].ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 400,
                    });
            ConsolidatedMovementMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("1", "1", "1", "31"), consolidatedMovements, (key, oldValue) => consolidatedMovements);
            ConsolidatedMovementMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("1", "1", "2", "31"), consolidatedMovements, (key, oldValue) => consolidatedMovements);
            ConsolidatedMovementMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("2", "2", "1", "31"), consolidatedMovements, (key, oldValue) => consolidatedMovements);
            ConsolidatedMovementMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("2", "2", "2", "31"), consolidatedMovements, (key, oldValue) => consolidatedMovements);

            var consolidatedInventories = new List<ConsolidatedInventoryProduct>
                {
                    new ConsolidatedInventoryProduct
                    {
                        NodeId = 1,
                        ProductId = "1",
                        MeasurementUnit = "31",
                        InventoryDate = new DateTime(2020, 5, 31),
                    },
                    new ConsolidatedInventoryProduct
                    {
                        NodeId = 1,
                        ProductId = "1",
                        MeasurementUnit = "31",
                        InventoryDate = new DateTime(2020, 6, 30),
                    },
                };
            consolidatedInventories[0].ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 100,
                    });
            consolidatedInventories[0].ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 200,
                    });
            consolidatedInventories[1].ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 300,
                    });
            consolidatedInventories[1].ConsolidatedOwners.Add(
                    new ConsolidatedOwner
                    {
                        OwnerId = 1,
                        OwnershipVolume = 400,
                    });

            ConsolidatedInventoryMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("1", "1", "1", "31"), consolidatedInventories, (key, oldValue) => consolidatedInventories);
            ConsolidatedInventoryMap.AddOrUpdate(IdHelper.BuildOfficialCalculationUniqueKey("1", "1", "2", "31"), consolidatedInventories, (key, oldValue) => consolidatedInventories);
        }

        /// <summary>
        /// delta balance calculator should calculate.
        /// </summary>
        [TestMethod]
        public void DeltaBalanceCalculator_ShouldCalculate_WithSuccess()
        {
            var result = this.deltaBalanceCalculator.Calculate(new Entities.TransportBalance.Ticket { StartDate = new DateTime(2020, 6, 1), EndDate = new DateTime(2020, 6, 30), CategoryElementId = 1 }, MovementMap, ConsolidatedMovementMap, ConsolidatedInventoryMap);

            Assert.IsNotNull(result);

            var deltaBalance1 = result.FirstOrDefault(x => x.NodeId == 2 && x.ProductId == "2" && x.ElementOwnerId == 2);
            Assert.AreEqual(deltaBalance1.InitialInventory, new decimal(0));
            Assert.AreEqual(deltaBalance1.FinalInventory, new decimal(0));
            Assert.AreEqual(deltaBalance1.Input, new decimal(0));
            Assert.AreEqual(deltaBalance1.Output, new decimal(0));
            Assert.AreEqual(deltaBalance1.DeltaInitialInventory, new decimal(0));
            Assert.AreEqual(deltaBalance1.DeltaFinalInventory, new decimal(0));
            Assert.AreEqual(deltaBalance1.DeltaInput, new decimal(200));
            Assert.AreEqual(deltaBalance1.DeltaOutput, new decimal(400));

            var deltaBalance2 = result.FirstOrDefault(x => x.NodeId == 2 && x.ProductId == "2" && x.ElementOwnerId == 1);
            Assert.AreEqual(deltaBalance2.InitialInventory, new decimal(0));
            Assert.AreEqual(deltaBalance2.FinalInventory, new decimal(0));
            Assert.AreEqual(deltaBalance2.Input, new decimal(300));
            Assert.AreEqual(deltaBalance2.Output, new decimal(700));
            Assert.AreEqual(deltaBalance2.DeltaInitialInventory, new decimal(0));
            Assert.AreEqual(deltaBalance2.DeltaFinalInventory, new decimal(0));
            Assert.AreEqual(deltaBalance2.DeltaInput, new decimal(100));
            Assert.AreEqual(deltaBalance2.DeltaOutput, new decimal(300));

            var deltaBalance3 = result.FirstOrDefault(x => x.NodeId == 1 && x.ProductId == "1" && x.ElementOwnerId == 2);
            Assert.AreEqual(deltaBalance3.InitialInventory, new decimal(0));
            Assert.AreEqual(deltaBalance3.FinalInventory, new decimal(0));
            Assert.AreEqual(deltaBalance3.Input, new decimal(0));
            Assert.AreEqual(deltaBalance3.Output, new decimal(0));
            Assert.AreEqual(deltaBalance3.DeltaInitialInventory, new decimal(-1300));
            Assert.AreEqual(deltaBalance3.DeltaFinalInventory, new decimal(400));
            Assert.AreEqual(deltaBalance3.DeltaInput, new decimal(400));
            Assert.AreEqual(deltaBalance3.DeltaOutput, new decimal(200));

            var deltaBalance4 = result.FirstOrDefault(x => x.NodeId == 1 && x.ProductId == "1" && x.ElementOwnerId == 1);
            Assert.AreEqual(deltaBalance4.InitialInventory, new decimal(300));
            Assert.AreEqual(deltaBalance4.FinalInventory, new decimal(700));
            Assert.AreEqual(deltaBalance4.Input, new decimal(700));
            Assert.AreEqual(deltaBalance4.Output, new decimal(300));
            Assert.AreEqual(deltaBalance4.DeltaInitialInventory, new decimal(-1000));
            Assert.AreEqual(deltaBalance4.DeltaFinalInventory, new decimal(500));
            Assert.AreEqual(deltaBalance4.DeltaInput, new decimal(300));
            Assert.AreEqual(deltaBalance4.DeltaOutput, new decimal(100));
        }
    }
}
