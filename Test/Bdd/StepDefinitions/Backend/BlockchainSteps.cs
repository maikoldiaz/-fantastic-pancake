// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Backend
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.DataSources;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TechTalk.SpecFlow;

    [Binding]
    public class BlockchainSteps : EcpApiStepDefinitionBase
    {
        ////private const string MovementMessageBody = "{\"movementId\":\"{id}\",\"movementDate\":\"2020-05-03T06:18:22\",\"volume\":12,\"productId\":\"10000002093\",\"units\":\"Barrels\",\"sourceNodeId\":\"2461\",\"destinationNodeId\":\"2465\"}";
        ////private const string InventoryMessageBody = "{\"inventoryId\":\"{id}\",\"inventoryDate\":\"2020-05-03T06:18:22\",\"volume\":104,\"nodeId\":\"2461\",\"productId\":\"10000002093\",\"locationId\":\"1006:C001\",\"units\":\"Barrels\"}";

        ////private const string MovementsQueue = "movements";
        ////private const string InventoryQueue = "inventory";

        private readonly BlockchainDataSource blockchainDataSource = new BlockchainDataSource();

        public BlockchainSteps(FeatureContext featureContext)
          : base(featureContext)
        {
        }

        [Given(@"I have movement data")]
        public void GivenIHaveMovementData()
        {
            Assert.IsTrue(true);
        }

        [Given(@"I have inventory data")]
        public void GivenIHaveInventoryData()
        {
            Assert.IsTrue(true);
        }

        [When(@"I submit inventory data")]
        public void WhenIPassInventoryData()
        {
            ////int id = new Faker().Random.Number(9999, 999999);

            ////await ServiceBusHelper.PutAsync(InventoryQueue, InventoryMessageBody.Replace($"{{{nameof(id)}}}", id.ToString(CultureInfo.InvariantCulture))).ConfigureAwait(false);
            ////this.ScenarioContext["id"] = id;
        }

        [When(@"I submit movement data")]
        public void WhenIPassMovementData()
        {
            ////int id = new Faker().Random.Number(9999, 999999);
            ////await ServiceBusHelper.PutAsync(MovementsQueue, MovementMessageBody.Replace($"{{{nameof(id)}}}", id.ToString(CultureInfo.InvariantCulture))).ConfigureAwait(false);
            ////this.ScenarioContext["id"] = id;
        }

        [Then(@"movement details should be registered in Blockchain")]
        public async Task ThenMovementDetailsShouldBeRegisteredInBlockchainAsync()
        {
            var result = await this.blockchainDataSource.ReadAsync<MovementStruct>("1245284").ConfigureAwait(false);
            Assert.IsTrue(result.MovementId == this.ScenarioContext["id"].ToString());
        }

        [Then(@"Inventory details should be registered in Blockchain")]
        public async Task ThenInventoryDetailsShouldBeRegisteredInBlockchainAsync()
        {
            var blockchaintransactionId = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastInventoryTransactionId).ConfigureAwait(false);
            var data = blockchaintransactionId["BlockchainInventoryProductTransactionId"];
            var inventoryProductUniqueId = blockchaintransactionId["InventoryProductUniqueId"];
            var productVolume = blockchaintransactionId["ProductVolume"];
            var measurementUnit = blockchaintransactionId["MeasurementUnit"];
            var inventoryDate = blockchaintransactionId["InventoryDate"];
            var createdDate = blockchaintransactionId["CreatedDate"];
            var uncertainityPercentage = blockchaintransactionId["UncertaintyPercentage"];
            var batchId = blockchaintransactionId["BatchId"];
            var eventType = blockchaintransactionId["EventType"];
            var segmentId = blockchaintransactionId["SegmentId"];
            var scenarioId = blockchaintransactionId["ScenarioId"];
            var version = blockchaintransactionId["Version"];
            var tankName = blockchaintransactionId["TankName"];
            var producttype = blockchaintransactionId["ProductType"];
            var sourceSystemId = blockchaintransactionId["SourceSystemId"];
            var nodeId = blockchaintransactionId["NodeId"];
            var productId = blockchaintransactionId["ProductId"];
            var inventoryId = blockchaintransactionId["InventoryId"];
            string blockData = data.ToLowerInvariant(); //// blockchaintransactionId["BlockchainInventoryProductTransactionId"];
            var parameters = new Dictionary<string, object>() { { "BlockchainInventoryProductTransactionId", blockData } };
            ////var res = await this.blockchainDataSource.GetDataAsync<InventoryOwnershipDetailsAfterCalculation>("InventoryOwnershipFactory", "GetInventoryOwnershipByBlockchainInventoryProductId", parameters).ConfigureAwait(false);
            var result = await this.blockchainDataSource.GetDataAsync<InventoryDetailStruct>("InventoryProductsFactory", "GetByTransactionId", parameters).ConfigureAwait(false);
            Assert.AreEqual(result.InventoryProductId, inventoryProductUniqueId.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(result.MeasurementUnit, measurementUnit.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(result.BlockchainInventoryProductTransactionId, data.ToString(CultureInfo.InvariantCulture));
            DateTime dt = new DateTime(result.InventoryDate);
            Assert.AreEqual(dt.ToString("M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture), inventoryDate);
            Assert.AreEqual((float)result.ProductVolume, float.Parse(productVolume, CultureInfo.InvariantCulture) * 100);
            string[] metadata = result.Metadata.Split(',');
            Assert.AreEqual(batchId, metadata[0]);
            Assert.AreEqual(Convert.ToDateTime(createdDate, CultureInfo.InvariantCulture).ToString("dd-MMM-yy", CultureInfo.InvariantCulture), metadata[1]);
            Assert.AreEqual(eventType, metadata[2]);
            Assert.AreEqual(producttype, metadata[3]);
            Assert.AreEqual(scenarioId, metadata[4]);
            Assert.AreEqual(segmentId, metadata[5]);
            Assert.AreEqual(sourceSystemId, metadata[6]);
            Assert.AreEqual(tankName, metadata[7]);
            Assert.AreEqual(uncertainityPercentage, metadata[8]);
            Assert.AreEqual(version, metadata[9]);
            Assert.AreEqual(nodeId, metadata[10]);
            Assert.AreEqual(productId, metadata[11]);
            Assert.AreEqual(inventoryId, metadata[12]);
            Assert.AreEqual(inventoryProductUniqueId, metadata[13]);
        }

        [Then(@"Calculated Movement details should be registered in Blockchain")]
        public async Task ThenCalculatedMovementDetailsShouldBeRegisteredInBlockchainAsync()
        {
            var movementDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastMovementTransactionDetails).ConfigureAwait(false);
            var data = movementDetails["BlockchainMovementTransactionId"];
            var movementTransacId = movementDetails["MovementTransactionId"];
            var netStandardVolume = movementDetails["NetStandardVolume"];
            var movementId = movementDetails["MovementId"];
            var measurementUnit = movementDetails["MeasurementUnit"];
            var operationalDate = movementDetails["OperationalDate"];
            var backupMovementId = movementDetails["BackupMovementId"];
            if (string.IsNullOrEmpty(backupMovementId))
            {
                backupMovementId = string.Empty;
            }

            var createdDate = movementDetails["CreatedDate"];
            var eventType = movementDetails["EventType"];
            var globalMovementId = movementDetails["GlobalMovementId"];
            if (string.IsNullOrEmpty(globalMovementId))
            {
                globalMovementId = string.Empty;
            }

            var isOfficial = movementDetails["IsOfficial"];
            var scenarioId = movementDetails["ScenarioId"];
            var movementEventId = movementDetails["MovementEventId"];
            if (string.IsNullOrEmpty(movementEventId))
            {
                movementEventId = string.Empty;
            }

            var movementContractId = movementDetails["MovementContractId"];
            if (string.IsNullOrEmpty(movementContractId))
            {
                movementContractId = string.Empty;
            }

            var segmentId = movementDetails["SegmentId"];
            var sourceSystemId = movementDetails["SourceSystemId"];
            var uncertaintyPercentage = movementDetails["UncertaintyPercentage"];
            if (string.IsNullOrEmpty(uncertaintyPercentage))
            {
                uncertaintyPercentage = string.Empty;
            }

            var version = movementDetails["Version"];
            if (string.IsNullOrEmpty(version))
            {
                version = string.Empty;
            }

            var movementTypeId = movementDetails["MovementTypeId"];

            var movementSourceDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastMovementSourceDetails, args: new { MovementTransactionId = movementTransacId }).ConfigureAwait(false);
            var movementDestinationDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastMovementDestinationDetails, args: new { MovementTransactionId = movementTransacId }).ConfigureAwait(false);
            var movementPeriodDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastMovementPeriodDetails, args: new { MovementTransactionId = movementTransacId }).ConfigureAwait(false);
            var sourceNodeId = movementSourceDetails["SourceNodeId"];
            var sourceProductId = movementSourceDetails["SourceProductId"];
            var destinationNodeId = movementDestinationDetails["DestinationNodeId"];
            var destinationProductId = movementDestinationDetails["DestinationProductId"];
            var startTime = movementPeriodDetails["StartTime"];
            var endTime = movementPeriodDetails["EndTime"];
            string blockData = data.ToLowerInvariant(); //// blockchaintransactionId["BlockchainMovementTransactionId"];
            var parameters = new Dictionary<string, object>() { { "BlockchainMovementTransactionId", blockData } };
            ////var resultb = await this.blockchainDataSource.GetDataAsync<MovementOwnershipDetailsAfterCalculation>("MovementOwnershipFactory", "GetMovementOwnershipByBlockchainMovementTransactionId", parametersb).ConfigureAwait(false);
            ////var resulto = await this.blockchainDataSource.GetDataAsync<MovementOwnershipDetailsAfterCalculation>("MovementOwnershipFactory", "GetMovementOwnershipHistoryByIndex", parametersa).ConfigureAwait(false);
            var result = await this.blockchainDataSource.GetDataAsync<MovementDetailStruct>("MovementsFactory", "GetByTransactionId", parameters).ConfigureAwait(false);
            Assert.AreEqual((float)result.NetStandardVolume, float.Parse(netStandardVolume, CultureInfo.InvariantCulture) * 100);
            Assert.AreEqual(movementId, result.MovementId.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(measurementUnit, result.MeasurementUnit);
            Assert.AreEqual(data, result.BlockchainMovementTransactionId);
            DateTime dt = new DateTime(result.OperationalDate);
            Assert.AreEqual(dt.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture), operationalDate);
            string[] metadata = result.Metadata.Split(',');
            Assert.AreEqual(backupMovementId, metadata[0]);
            Assert.AreEqual(this.ToTrueString(Convert.ToDateTime(createdDate, CultureInfo.InvariantCulture)), metadata[1]);
            Assert.AreEqual(this.ToTrueString(Convert.ToDateTime(endTime, CultureInfo.InvariantCulture)), metadata[2]);
            Assert.AreEqual(this.ToTrueString(Convert.ToDateTime(startTime, CultureInfo.InvariantCulture)), metadata[3]);
            Assert.AreEqual(eventType, metadata[4]);
            Assert.AreEqual(globalMovementId, metadata[5]);
            Assert.AreEqual(isOfficial, metadata[6]);
            Assert.AreEqual(movementContractId, metadata[7]);
            Assert.AreEqual(movementEventId, metadata[8]);
            Assert.AreEqual(scenarioId, metadata[9]);
            Assert.AreEqual(segmentId, metadata[10]);
            Assert.AreEqual(sourceSystemId, metadata[11]);
            Assert.AreEqual(uncertaintyPercentage, metadata[12]);
            Assert.AreEqual(version, metadata[13]);
            Assert.AreEqual(sourceNodeId, metadata[14]);
            Assert.AreEqual(sourceProductId, metadata[15]);
            Assert.AreEqual(destinationNodeId, metadata[16]);
            Assert.AreEqual(destinationProductId, metadata[17]);
            Assert.AreEqual(movementId, metadata[18]);
            Assert.AreEqual(movementTypeId, metadata[19]);
        }

        public string ToTrueString(DateTime dt)
        {
            var trueDate = dt.ToString("dd-MMM-yy", new CultureInfo("es-co"));
            var dateParts = trueDate.Split('-');
            var formattedMonth = string.Join(string.Empty, new List<string> { dateParts[1].Substring(0, 1).ToUpper(CultureInfo.InvariantCulture), dateParts[1].Substring(1, 1), dateParts[1].Substring(2, 1) });
            return string.Join("-", new List<string> { dateParts[0], formattedMonth, dateParts[2] });
        }
    }
}