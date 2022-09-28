// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InventoryDeltaStrategyTests.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Entities.Query;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Delta.Delta;
    using Ecp.True.Processors.Delta.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The InventoryDeltaStrategyTests.
    /// </summary>
    [TestClass]
    public class InventoryDeltaStrategyTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger> mockLogger;

        /// <summary>
        /// The inventoryDeltaStrategy.
        /// </summary>
        private InventoryDeltaStrategy inventoryDeltaStrategy;

        /// <summary>
        /// The initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockLogger = new Mock<ITrueLogger>();
            this.inventoryDeltaStrategy = new InventoryDeltaStrategy(this.mockLogger.Object);
        }

        [TestMethod]
        public void CreateInventoryMovement_WhenDeltaNegetiveInvoked()
        {
            var originalInventories = new List<OriginalInventory>
            {
                new OriginalInventory
              {
            InventoryProductUniqueId = "22",
            CreatedBy = "User",
            InventoryProductId = 3255,
              },
            };
            var inventoryProducts = new List<InventoryProduct>
            {
                new InventoryProduct
                {
              InventoryId = "22",
              CreatedBy = "FICO",
              CreatedDate = DateTime.UtcNow,
              InventoryProductId = 3255,
              LastModifiedBy = "User",
              LastModifiedDate = DateTime.UtcNow,
              ProductVolume = 5000,
              SystemTypeId = 1,
              DeltaTicketId = 2,
              MeasurementUnit = 31,
              SegmentId = 3 ,
              NodeId = 4,
              ProductId = "5" ,
                },
            };
            var resultInventories = new List<ResultInventory>
            {
                new ResultInventory
               {
                Delta = 3500,
                InventoryProductUniqueId = "22",
                InventoryProductId = 3255,
                Sign = false,
               },
            };
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OriginalInventories = originalInventories;
            deltaData.InventoryProducts = inventoryProducts;
            deltaData.ResultInventories = resultInventories;
            deltaData.NextCutOffDate = DateTime.UtcNow;
            var result = this.inventoryDeltaStrategy.Build(deltaData);
            Assert.AreEqual(4 ,result.FirstOrDefault().MovementSource.SourceNodeId);
            Assert.AreEqual(3500, result.FirstOrDefault().NetStandardVolume);
            Assert.AreEqual(31, result.FirstOrDefault().MeasurementUnit);
            Assert.AreEqual(44, result.FirstOrDefault().MovementTypeId);
            Assert.IsNull(result.Select(x => x.MovementDestination).SingleOrDefault());
            Assert.IsNotNull(result.FirstOrDefault().MovementSource);
            Assert.AreEqual(4, result.FirstOrDefault().MovementSource.SourceNodeId);
            Assert.AreEqual("5", result.FirstOrDefault().MovementSource.SourceProductId);
            Assert.AreEqual(44, result.FirstOrDefault().MovementTypeId);
        }

        [TestMethod]
        public void CreateInventoryMovement_WhenPositveInventoryStrategy_IsInvoked()
        {
            var originalInventories = new List<OriginalInventory>
            {
                new OriginalInventory
              {
            InventoryProductUniqueId = "22",
            CreatedBy = "User",
            InventoryProductId = 3255,
              },
            };
            var inventoryProducts = new List<InventoryProduct>
            {
                new InventoryProduct
                {
              InventoryId = "22",
              CreatedBy = "FICO",
              CreatedDate = DateTime.UtcNow,
              InventoryProductId = 3255,
              LastModifiedBy = "User",
              LastModifiedDate = DateTime.UtcNow,
              ProductVolume = 5000,
              SystemTypeId = 1,
              DeltaTicketId = 2,
              MeasurementUnit = 31,
              SegmentId = 3 ,
              NodeId = 4,
              ProductId = "5" ,
                },
            };
            var resultInventory = new List<ResultInventory>
            {
                new ResultInventory
            {
                    Delta = 1800,
                    InventoryProductId = 3255,
                    InventoryProductUniqueId = "22",
                    Sign = true,
            },
            };
            var deltaData = new DeltaData { Ticket = new Ticket { TicketId = 123 } };
            deltaData.OriginalInventories = originalInventories;
            deltaData.InventoryProducts = inventoryProducts;
            deltaData.ResultInventories = resultInventory;
            deltaData.NextCutOffDate = DateTime.UtcNow;
            var result = this.inventoryDeltaStrategy.Build(deltaData);
            var inventoryMovements = result.Count();
            Assert.AreEqual(1 , inventoryMovements);
            Assert.AreEqual(1800, result.FirstOrDefault().NetStandardVolume);
            Assert.AreEqual(3255, result.FirstOrDefault().SourceInventoryProductId);
            Assert.IsNotNull(result.FirstOrDefault().MovementDestination);
            Assert.AreEqual(4, result.FirstOrDefault().MovementDestination.DestinationNodeId);
            Assert.AreEqual("5", result.FirstOrDefault().MovementDestination.DestinationProductId);
            Assert.IsNull(result.Select(x => x.MovementSource).SingleOrDefault());
            Assert.AreEqual(1, result.FirstOrDefault().SystemTypeId);
            Assert.AreEqual(true, result.FirstOrDefault().IsSystemGenerated);
            Assert.AreEqual("Insert", result.FirstOrDefault().EventType);
            Assert.AreEqual(44, result.FirstOrDefault().MovementTypeId);
        }
    }
}
