// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdGeneratorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Tests
{
    using System;
    using System.Data;
    using Ecp.True.Processors.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// IdGeneratorTests.
    /// </summary>
    [TestClass]
    public class IdGeneratorTests
    {
        /// <summary>
        /// Generates the movement identifier jtoken input.
        /// </summary>
        [TestMethod]
        public void GenerateMovementId_JtokenInput()
        {
            var homologatedMovement = GetHomologatedMovement();
            IdGenerator.GenerateMovementId(homologatedMovement);

            Assert.IsTrue(true);
        }

        /////// <summary>
        /////// Generates the inventory identifier jtoken input.
        /////// </summary>
        ////[TestMethod]
        ////public void GenerateInventoryId_JtokenInput()
        ////{
        ////    var homologatedInventory = GetHomologatedInventory();
        ////    IdGenerator.GenerateInventoryId(homologatedInventory);

        ////    Assert.IsTrue(true);
        ////}

        /// <summary>
        /// Generates the inventory product unique identifier data row input.
        /// </summary>
        [TestMethod]
        public void GenerateInventoryProductUniqueId_DataRowInput()
        {
            using (DataSet ds = new DataSet())
            {
                using (DataTable dt = new DataTable("INVENTARIOS"))
                {
                    dt.Columns.Add("IdInventario");
                    dt.Columns.Add("IdNodo");
                    dt.Columns.Add("Producto");
                    dt.Columns.Add("FechaInventario");
                    dt.Columns.Add("NodeId");
                    dt.Columns.Add("BatchId");
                    dt.Columns.Add("Tanque");

                    dt.Rows.Add("123", "IdNodo", "Producto", DateTime.Now, "NodeId", "BatchId", "Tanque");
                    ds.Tables.Add(dt);
                    var result = IdGenerator.GenerateInventoryProductUniqueId(dt.Rows[0]);
                    Assert.IsNotNull(result);
                }
            }
        }

        /// <summary>
        /// Validates the generated movement identifier and inventory identifier is not unique when same identifier sent jtoken input.
        /// </summary>
        [TestMethod]
        public void ValidateGeneratedMovementIdAndInventoryIdIsNotUniqueWhenSameIdSent_JtokenInput()
        {
            var homologatedMovement = GetHomologatedMovement();
            var movementId = IdGenerator.GenerateMovementId(homologatedMovement);

            var homologatedInventory = GetHomologatedInventory();
            var inventoryId = IdGenerator.GenerateInventoryProductUniqueId(homologatedInventory);

            Assert.IsTrue(movementId != inventoryId);
        }

        private static JToken GetHomologatedMovement()
        {
            string movementString = "{\"SourceSystem\": \"SINOPER\",\"SystemName\": \"SINOPER\"," +
                " \"EventType\": \"INSERT\", \"MovementId\": 751326, \"MovementTypeId\": 9273," +
                " \"OperationalDate\": \"2020-04-13T00:00:00\", \"GrossStandardVolume\": 0," +
                " \"NetStandardVolume\": 118020.43,\"MeasurementUnit\": 31, \"ScenarioId\": \"2\", " +
                " \"Observations\": null,\"Classification\": \"Movimiento\", \"SegmentId\": 10," +
                " \"MovementDestination\": {\"DestinationNodeId\": 1877,\"DestinationStorageLocationId\": null," +
                "   \"DestinationProductId\": \"10000002049\",\"DestinationProductTypeId\": 9276}," +
                " \"MovementSource\": {\"SourceNodeId\": 1876,\"SourceStorageLocationId\": null," +
                " \"SourceProductId\": \"10000002318\",  \"SourceProductTypeId\": 9275  }, " +
                "\"Period\": {  \"StartTime\": \"2020-04-13T00:00:00\",\"EndTime\": \"2020-04-15T00:00:00\" }," +
                " \"Owners\": [ {\"OwnerId\": 9277,   \"OwnershipValue\": 118020.43, " +
                " \"OwnershipValueUnit\": \"Bbl\"} ],\"Attributes\": [ { " +
                "\"AttributeId\": \"Producto Destino\", \"AttributeValue\": \"CRUDOS IMPORTADOS\", " +
                "\"ValueAttributeUnit\": \"ADM\",  \"AttributeDescription\": \"Producto Destino\"  " +
                "}, { \"AttributeId\": \"Neto-Volumen L\u00EDquido\"," +
                "\"AttributeValue\": \"118020.43\", \"ValueAttributeUnit\": \"Bbl\", " +
                "\"AttributeDescription\": null  }  ],  \"MessageId\": 751326,  \"Type\": \"Movement\", " +
                " \"BlobPath\": \"sinoper\\/json\\/movement\\/qu1rievbstaylkquuu0gif6ucq0gl5h==\\/751326\\/751326_1862_1\", " +
                " \"IsHomologated\": true}";

            return JToken.Parse(movementString);
        }

        private static JToken GetHomologatedInventory()
        {
            string inventoryString = "{\"ProductId\": \"10000002049\",\"ProductType\": 9090," +
                "\"BatchId\": \"pwa5bsfbxf1w5cvzbuk7j8ksf\",\"ProductVolume\": 177506," +
                "\"MeasurementUnit\": 31,\"Owners\": [{ \"OwnerId\": 9093," +
                "\"OwnershipValue\": 177506, \"OwnershipValueUnit\": \"Bbl\" } ]," +
                "\"Attributes\": [{\"AttributeId\": \"Neto-Volumen L\u00EDquido\"," +
                "\"AttributeValue\": 177506, \"ValueAttributeUnit\": \"Bbl\", \"AttributeDescription\": null" +
                "}],\"SourceSystem\": \"SINOPER\", \"SystemName\": \"SINOPER\"," +
                "\"DestinationSystem\": \"TRUE\", \"EventType\": \"INSERT\", \"InventoryId\": 751326, " +
                "\"InventoryDate\": \"2020-04-14T00:00:00\", \"Observations\": \"\", \"ScenarioId\": \"2\"," +
                "\"NodeId\": 1836, \"TankName\": null, \"SegmentId\": 10, " +
                "\"MessageId\": \"MjAyMDA0MTRfQVlBQ1VDSE9fQ1JVRE9TIElNUE9SVEFET1NfMDQvMTQvMjAyMCAwMDowMDowMF9wd2E1YnNmYnhmMXc1Y3Z6YnVrN2o4a3NmX05B\"," +
                "\"Type\": \"Inventory\"," +
                "  \"BlobPath\": \"sinoper\\/json\\/inventory\\/qu1rievbstaylkquuu0gif6ucq0gl4ds\\/MjAyMDA0MTRfQVlBQ1VDSE9fQ1JVRE9TIElNUE9SVEFET1NfMDQvMTQvMjAyMCAwMDowMDowMF9wd2E1YnNmYnhmMXc1Y3Z6YnVrN2o4a3NmX05B\\/MjAyMDA0MTRfQVlBQ1VDSE9fQ1JVRE9TIElNUE9SVEFET1NfMDQvMTQvMjAyMCAwMDowMDowMF9wd2E1YnNmYnhmMXc1Y3Z6YnVrN2o4a3NmX05B_1861_1\"," +
                " \"IsHomologated\": true,\r\n  \"InventoryProductUniqueId\": \"MjAyMDA0MTRfMTgzNl8xMDAwMDAwMjA0OV8wNC8xNC8yMDIwIDAwOjAwOjAwX3B3YTVic2ZieGYxdzVjdnpidWs3ajhrc2ZfTkE=\"\r\n}";

            return JToken.Parse(inventoryString);
        }
    }
}
