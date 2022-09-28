// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGeneratorServiceTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Ownership.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Processors.Ownership;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The Data Generator Service Tests.
    /// </summary>
    [TestClass]
    public class DataGeneratorServiceTests
    {
        /// <summary>
        /// The data generator service.
        /// </summary>
        private DataGeneratorService dataGeneratorService;

        /// <summary>
        /// Generates the data transform logistics data throws exception test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GenerateData_TransformLogisticsData_ThrowsException_Test()
        {
            this.dataGeneratorService.TransformLogisticsData(null);
        }

        /// <summary>
        /// Transforms the logistics data should transform test.
        /// </summary>
        [TestMethod]
        public void TransformLogisticsData_ShouldTransform_Test()
        {
            var logisticsInfo = new LogisticsInfo();

            logisticsInfo.LogisticMovementDetail = new List<OperativeLogisticsMovement>
            {
                new OperativeLogisticsMovement
                {
                    Movement = $"Movement-0",
                    StorageSource = $"StorageSource-0",
                    ProductOrigin = $"ProductOrigin-0",
                    StorageDestination = $"StorageDestination-0",
                    ProductDestination = $"ProductDestination-0",
                    OrderPurchase = 1,
                    PosPurchase = 1,
                    Value = 10,
                    Uom = $"UON-0",
                    Finding = string.Empty,
                    Diagnostic = string.Empty,
                    Impact = string.Empty,
                    Solution = string.Empty,
                    Status = $"Status-0",
                    Order = 1,
                    DateOperation = Convert.ToDateTime("02/25/2020", CultureInfo.InvariantCulture),
                },
            };

            logisticsInfo.LogisticInventoryDetail = new List<LogisticsInventoryDetail>
            {
                new LogisticsInventoryDetail
                {
                    Inventory = $"Inventory-0",
                    StorageLocation = $"StorageSource-0",
                    Product = $"ProductOrigin-0",
                    Value = 10,
                    Uom = $"UON-0",
                    Finding = string.Empty,
                    Diagnostic = string.Empty,
                    Impact = string.Empty,
                    Solution = string.Empty,
                    Status = $"Status-0",
                    Order = $"Order-0",
                    DateOperation = Convert.ToDateTime("02/25/2020", CultureInfo.InvariantCulture),
                },
            };

            this.dataGeneratorService = new DataGeneratorService();

            var transformedData = this.dataGeneratorService.TransformLogisticsData(logisticsInfo);

            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[0], logisticsInfo.LogisticMovementDetail.First().Movement);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[1], logisticsInfo.LogisticMovementDetail.First().StorageSource);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[2], logisticsInfo.LogisticMovementDetail.First().ProductOrigin);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[3], logisticsInfo.LogisticMovementDetail.First().StorageDestination);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[4], logisticsInfo.LogisticMovementDetail.First().ProductDestination);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[5], logisticsInfo.LogisticMovementDetail.First().OrderPurchase);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[6], logisticsInfo.LogisticMovementDetail.First().PosPurchase);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[7], logisticsInfo.LogisticMovementDetail.First().Value);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[8], logisticsInfo.LogisticMovementDetail.First().Uom);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[9], logisticsInfo.LogisticMovementDetail.First().Finding);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[10], logisticsInfo.LogisticMovementDetail.First().Diagnostic);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[11], logisticsInfo.LogisticMovementDetail.First().Impact);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[12], logisticsInfo.LogisticMovementDetail.First().Solution);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[13], logisticsInfo.LogisticMovementDetail.First().Status);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[14], logisticsInfo.LogisticMovementDetail.First().Order);
            Assert.AreEqual(transformedData.Tables[0].Rows[0].ItemArray[15], logisticsInfo.LogisticMovementDetail.First().DateOperation);

            Assert.IsNotNull(transformedData);

            var propCountInv = this.GetPropertyCount(logisticsInfo.LogisticMovementDetail.First().GetType());
            Assert.AreEqual(transformedData.Tables[0].Columns.Count, propCountInv);
            Assert.AreEqual(transformedData.Tables[0].Rows.Count, logisticsInfo.LogisticMovementDetail.ToList().Count);
            Assert.AreEqual(transformedData.Tables[0].TableName, this.GetDisplayName(logisticsInfo.LogisticMovementDetail.First().GetType()));
            for (int i = 0; i < transformedData.Tables[0].Columns.Count; i++)
            {
                Assert.AreEqual(transformedData.Tables[0].Columns[i].ColumnName, this.GetPropertyDisplayName(logisticsInfo.LogisticMovementDetail.First().GetType().GetProperties()[i]));
            }
        }

        /// <summary>
        /// Transforms the logistics data throws exception test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TransformLogisticsData_ThrowsException_Test()
        {
            this.dataGeneratorService.TransformLogisticsData(null);
        }

        /// <summary>
        /// Gets the display name of the property.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>The property display name.</returns>
        private string GetPropertyDisplayName(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().FirstOrDefault().DisplayName;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The display name.</returns>
        private string GetDisplayName(Type type)
        {
            return type.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().FirstOrDefault().DisplayName;
        }

        /// <summary>
        /// Gets the property count.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The count.</returns>
        private int GetPropertyCount(Type type)
        {
            return type.
                GetProperties(BindingFlags.Public | BindingFlags.Instance).
                Where(p => !p.GetCustomAttributes().Any(a => string.Equals(a.GetType().Name, "ColumnIgnoreAttribute", StringComparison.Ordinal))).
                ToList().Count;
        }
    }
}