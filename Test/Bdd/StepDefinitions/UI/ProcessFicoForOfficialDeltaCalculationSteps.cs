// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessFicoForOfficialDeltaCalculationSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions
{
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ProcessFicoForOfficialDeltaCalculationSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"Verify that System should send the information details to get ""(.*)""")]
        [StepDefinition(@"Verify that System should send the information details to the ""(.*)""")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldSendTheInformationDetailsToTheAsync(string state)
        {
            var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetLastOfficialDeltaTicket).ConfigureAwait(false);
            this.SetValue("OfficialDeltaTicketId", "31575");

            if (state == "FICO Request")
            {
                this.SetValue("fileName", string.Concat(this.GetValue("OfficialDeltaTicketId"), "_request"));
                var fileName = this.GetValue("fileName");
                this.ScenarioContext["json"] = await fileName.OfficialDeltafromBlobAsync("officialDelta").ConfigureAwait(false);
            }
            else if (state == "FICO Response")
            {
                this.SetValue("fileName", string.Concat(this.GetValue("OfficialDeltaTicketId"), "_response"));
                var fileName = this.GetValue("fileName");
                this.ScenarioContext["json"] = await fileName.OfficialDeltafromBlobAsync("officialDelta").ConfigureAwait(false);
            }

            Assert.IsNotNull(this.ScenarioContext["json"]);
        }

        [Then(@"Verify that system should send the official inventory details in the ""(.*)"" array of FICO Collection")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldSendTheOfficialInventoryDetailsInTheArrayOfFICOCollectionAsync(string collection)
        {
            var officialInventoryData = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.OFficialInventoryProductInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            Assert.AreEqual(officialInventoryData.Count(), ficoData[collection].Count());
            var officialInventoryList = officialInventoryData.ToDictionaryList();
            var i = 0;
            foreach (var officialInventory in officialInventoryList.Reverse())
            {
                Assert.AreEqual(officialInventory["InventoryProductId"].ToString(), ficoData[collection][i].SelectToken("idInventarioTRUE").ToString());
                i++;
            }
        }

        [Given(@"True System is processing the data to generate official ticket with consolidated details")]
        public async Task GivenTrueSystemIsProcessingTheDataToGenerateOfficialTicketWithConsolidatedDetailsAsync()
        {
            await this.TheTRUESystemIsProcessingTheOfficialMovementsAndInventoriesConsolidationAsync().ConfigureAwait(false);
        }

        [Then(@"Verify that the inventory date in ""(.*)"" of ""(.*)"" should be minus one day from inventory date")]
        public async Task ThenVerifyThatTheInventoryDateInOfShouldBeMinusOneDayFromInventoryDateAsync(string field, string collection)
        {
            var officialInventoryData = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.OFficialInventoryProductInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var officialInventoryList = officialInventoryData.ToDictionaryList();
            var i = 0;
            foreach (var officialInventory in officialInventoryList)
            {
                Assert.AreEqual(officialInventory["InventoryDate"].ToDateTime().AddDays(-1).GetDateTimeFormats(CultureInfo.InvariantCulture)[6].ToString(), ficoData[collection][i].SelectToken(field).ToDateTime().GetDateTimeFormats(CultureInfo.InvariantCulture)[6].ToString());
                i++;
            }
        }

        [Then(@"Verify that the inventory date in ""(.*)"" of ""(.*)"" should be the inventory date of the segment")]
        public void ThenVerifyThatTheInventoryDateInOfShouldBeTheInventoryDateOfTheSegment(string field, string collection)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            for (var i = 0; i < ficoData[collection].Count(); i++)
            {
                Assert.AreEqual(ConstantValues.TestData, ficoData[collection][i].SelectToken(field).ToString());  //// need to work
            }
        }

        [Then(@"Verify that system should send the official movements details in the ""(.*)"" array of FICO Collection")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldSendTheOfficialMovementsDetailsInTheArrayOfFICOCollectionAsync(string collection)
        {
            var officialMovementData = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.OFficialMovementInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            Assert.AreEqual(officialMovementData.Count(), ficoData[collection].Count());
            var officialMovementDataList = officialMovementData.ToDictionaryList();
            var i = 0;
            foreach (var officialMovement in officialMovementDataList.Reverse())
            {
                Assert.AreEqual(officialMovement["MovementTransactionId"].ToString(), ficoData[collection][i].SelectToken("idMovimientoTRUE").ToString());
                i++;
            }
        }

        [Then(@"Verify that system should send the information of the consolidated inventories details in the ""(.*)"" array of FICO Collection")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldSendTheInformationOfTheConsolidatedInventoriesDetailsInTheArrayOfFICOCollectionAsync(string collection)
        {
            var consolidationInventoryData = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.ConsolidationInventoryProductInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var consolidationInventoryList = consolidationInventoryData.ToDictionaryList();
            var i = 0;
            foreach (var consolidationInventory in consolidationInventoryList)
            {
                Assert.AreEqual(consolidationInventory["ConsolidatedInventoryProductId"].ToString(), ficoData[collection][i].SelectToken("idInventarioTRUE").ToString());
                i++;
            }
        }

        [Then(@"Verify that system should send the information of the consolidated movements details in the ""(.*)"" array of FICO Collection")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldSendTheInformationOfTheConsolidatedMovementsDetailsInTheArrayOfFICOCollectionAsync(string collection)
        {
            var consolidatedMovementData = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.ConsolidationMovementInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            Assert.AreEqual(consolidatedMovementData.Count(), ficoData[collection].Count());
            var consolidatedDataList = consolidatedMovementData.ToDictionaryList();
            var i = 0;
            foreach (var consolidatedMovement in consolidatedDataList)
            {
                Assert.AreEqual(consolidatedMovement["ConsolidatedMovementId"].ToString(), ficoData[collection][i].SelectToken("idMovimientoTRUE").ToString());
                i++;
            }
        }

        [Then(@"Verify that system should send the configuration of active relationships between movement types in the ""(.*)"" array of FICO Collection")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldSendTheConfigurationOfActiveRelationshipsBetweenMovementTypesInTheArrayOfFICOCollectionAsync(string collection)
        {
            var annulationMovementIds = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAnnulationMovementIds).ConfigureAwait(false);
            var annulationMovementIdsAsDict = annulationMovementIds.ToDictionaryList();
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            var i = 0;
            foreach (var annulationMovementId in annulationMovementIdsAsDict.Reverse())
            {
                Assert.AreEqual(annulationMovementId["AnnulationMovementTypeId"].ToString(), ficoData[collection][i].SelectToken("idTipoAnulacion").ToString());
                i++;
            }
        }

        [StepDefinition(@"service return data in the ""(.*)"" object of FICO Collection")]
        public void WhenServiceReturnDataInTheObjectOfFICOCollection(string collection)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData["payload"]["payloadOutput"][collection]);
            this.SetValue("FicoCollection", collection);
        }

        [When(@"there is no movement in system in which the id of the source movement is equal to the value of the ""(.*)"" field in ""(.*)"" collection")]
        public async System.Threading.Tasks.Task WhenThereIsNoMovementInSystemInWhichTheIdOfTheSourceMovementIsEqualToTheValueOfTheFieldInCollectionAsync(string field, string collection)
        {
            var officialMovementData = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.OFficialMovementInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            var officialMovementDataList = officialMovementData.ToDictionaryList();
            var i = 0;
            foreach (var officialMovement in officialMovementDataList)
            {
                if (officialMovement["SourceMovementTransactionId"].ToString() != null)
                {
                    Assert.AreEqual(officialMovement["SourceMovementTransactionId"].ToString(), ficoData[collection][i].SelectToken(field).ToString());
                }

                i++;
            }
        }

        [When(@"there is no movement in system in which the id of the source movement from inventory is equal to the value of the ""(.*)"" field in ""(.*)"" collection")]
        public async System.Threading.Tasks.Task WhenThereIsNoMovementInSystemInWhichTheIdOfTheSourceMovementFromInventoryIsEqualToTheValueOfTheFieldInCollectionAsync(string field, string collection)
        {
            var officialMovementData = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.OFficialMovementInformationByTicketId, args: new { officialDeltaTicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            var officialMovementDataList = officialMovementData.ToDictionaryList();
            var i = 0;
            foreach (var officialMovement in officialMovementDataList)
            {
                if (officialMovement["SourceInventoryProductId"].ToString() != null)
                {
                    Assert.AreNotEqual(officialMovement["SourceInventoryProductId"].ToString(), ficoData[collection][i].SelectToken(field).ToString());
                }

                i++;
            }
        }

        [When(@"the origin returned by FICO service is official in ""(.*)"" field in ""(.*)"" collection")]
        public void WhenTheOriginReturnedByFICOServiceIsOfficialInFieldInCollection(string field, string collection)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            for (var i = 0; i < ficoData["payload"]["payloadOutput"][collection].Count(); i++)
            {
                if (ficoData["payload"]["payloadOutput"][collection][i].SelectToken(field).ToString() != "CONSOLIDADO")
                {
                    Assert.AreEqual("OFICIAL", ficoData["payload"]["payloadOutput"][collection][i].SelectToken(field).ToString());
                }
            }
        }

        [When(@"the origin returned by FICO service is Consolidated in ""(.*)"" field in ""(.*)"" collection")]
        public void WhenTheOriginReturnedByFICOServiceIsConsolidatedInFieldInCollection(string field, string collection)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            for (var i = 0; i < ficoData["payload"]["payloadOutput"][collection].Count(); i++)
            {
                if (ficoData["payload"]["payloadOutput"][collection][i].SelectToken(field).ToString() != "OFICIAL")
                {
                    Assert.AreEqual("CONSOLIDADO", ficoData["payload"]["payloadOutput"][collection][i].SelectToken(field).ToString());
                }
            }
        }

        [Then(@"Verify that delta ticket status should be DeltaStatus")]
        public async System.Threading.Tasks.Task ThenVerifyThatDeltaTicketStatusShouldBeAsync()
        {
            var ticketData = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTicketDetailsWithTicketId, args: new { TicketId = this.GetValue("OfficialDeltaTicketId") }).ConfigureAwait(false);
            Assert.AreEqual(ConstantValues.DeltaStatus, ticketData["Status"]);
        }

        [When(@"The inventory date is equal to the start date of the period minus one day or equal to the end date of the period")]
        [When(@"I have an consolidated inventories in the system that correspond to the nodes of the segment official delta ticket")]
        [Then(@"Verify that system should create new movement with source node and source product\. Use the node and product of the inventory registered to TRUE\. The destination node and product must be null\.")]
        [Then(@"Verify that system should create new movement with destination node and destination product\. Use the node and product of the inventory registered to TRUE\. The source node and product must be null\.")]
        [Then(@"Verify that system should create the new movement with source node and source product\. Use the node and product of the inventory registered to TRUE\. The destination node and product must be null")]
        [Then(@"Verify that the scenario of the new movement must be official")]
        [Then(@"Verify that Movement type should be corresponding cancellation type obtain by getting the movement type identifier and search it in the configuration of relationships between movement types")]
        [Then(@"Verify that the ownership must be stored in the movement owners table")]
        [Then(@"Verify that the new movement must be assigned the official delta ticket of the segment")]
        [Then(@"Verify that System should register the ownership of the movement")]
        [Then(@"ownership should be registered for these movement")]
        [Then(@"movement should be registered into the system")]
        [When(@"the official delta ticket is equal to the segment ticket")]
        [Then(@"verify that the operational date must be equal to the start date of the consolidated movement\.")]
        [Then(@"Verify that system should Create the new movement with the same movement data stored in TRUE: operational date, source node, destination node, source product, destination product, product type if it exists, unit, segment, start date, and end date")]
        public void WhenTheOriginReturnedByFICOServiceIsOfficial()
        {
            ////Step handled in the next StepDefinition
        }

        [Then(@"Verify that system should get the  owner of the consolidated ownership information registered in TRUE\. To get the information of a inventory owner use the data of the field ""(.*)"" returned by FICO")]
        [Then(@"Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO ""(.*)""\.")]
        [Then(@"Verify that source inventory identifier must be the inventory identifier returned by FICO in the ""(.*)"" field\.")]
        [Then(@"Verify that system should get the owner of the information reported by the source applications\. To get the information of a inventory owner use the data of the field ""(.*)"" returned by FICO")]
        [Then(@"Verify that The movement type must be ""(.*)""")]
        [Then(@"Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO ""(.*)""")]
        [Then(@"Verify that the value of the net quantity must be the value returned by FICO in the ""(.*)"" field")]
        [Then(@"Verify that system should get the owner of the consolidated ownership information\. To get the information of a movement owner use the data of the field ""(.*)"" returned by FICO")]
        [Then(@"Verify that system should get the owner of the information reported by the source applications\. To get the information of a movement owner use the data of the field ""(.*)"" returned by FICO")]
        [Then(@"Verify that the source system must be ""(.*)""")]
        [Then(@"Verify that the value for the source movement identifier must be the movement ID returned by FICO in the ""(.*)"" field\.")]
        public void ThenVerifyThatSystemShouldCreateTheNewMovement(string val1)
        {
            this.LogToReport(val1);
            ////Step handled in the next StepDefinition
        }

        [Then(@"Verify that system should register the ownership of the movement using the owner identifier obtained in the previous step and the value of the official delta returned by FICO ""(.*)"" which must be multiplied by (.*)\.")]
        [Then(@"Verify that the value of the net quantity must be the value returned by FICO in the ""(.*)"" field multiplied by (.*)")]
        public void ThenVerifyThatSystemShouldCreateMovement(string val1, string val2)
        {
            this.LogToReport(val1);
            this.LogToReport(val2);
            ////Step handled in the next StepDefinition
        }

        [Then(@"Verify that system should obtain the data of the official movement stored in TRUE using the value of the field ""(.*)"" returned by FICO for Negative Movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldObtainTheDataOfTheOfficialMovementStoredInTRUEUsingTheValueOfTheFieldReturnedByFICOAsync(string field)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData["payload"]["payloadOutput"]["resultadoMovimientos"]);
            for (var i = 0; i < ficoData["payload"]["payloadOutput"]["resultadoMovimientos"].Count(); i++)
            {
                if (ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("origen").ToString() == "OFICIAL" && ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("signo").ToString() == "NEGATIVO")
                {
                    var movementData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementwithSourceMovementId, args: new { SourceMovementTransactionId = ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("idMovimientoTRUE").ToString() }).ConfigureAwait(false);
                    Assert.AreEqual(movementData["NetStandardVolume"].ToInt().ToString(CultureInfo.InvariantCulture), ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("cantidadNeta").ToInt().ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(2, movementData["ScenarioId"].ToInt());
                    Assert.AreEqual(movementData["SourceMovementTransactionId"].ToString(), ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken(field).ToString());
                    Assert.AreEqual("188", movementData["SourceSystemId"].ToString());
                    Assert.AreEqual("1", movementData["OfficialDeltaMessageTypeId"].ToString());
                    Assert.AreEqual(this.GetValue("OfficialDeltaTicketId"), movementData["OfficialDeltaTicketId"].ToString());

                    var movementOwners = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOwnershipOfMovement, args: new { MovementTransactionId = movementData["MovementTransactionId"].ToInt() }).ConfigureAwait(false);
                    var movementOwnersList = movementOwners.ToDictionaryList();
                    for (var j = 0; j < movementOwnersList.Count; j++)
                    {
                        Assert.IsTrue(movementOwnersList[j]["OwnershipValue"].ToInt().Equals(ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("deltaOficial").ToInt() * -1));
                    }
                }
            }
        }

        [Then(@"Verify that system should obtain the data of the official movement stored in TRUE using the value of the field ""(.*)"" returned by FICO for Positive Movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldObtainTheDataOfTheOfficialMovementStoredInTRUEUsingTheValueOfTheFieldReturnedByFICOForPositiveMovementsAsync(string field)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData["payload"]["payloadOutput"]["resultadoMovimientos"]);
            for (var i = 0; i < ficoData["payload"]["payloadOutput"]["resultadoMovimientos"].Count(); i++)
            {
                if (ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("origen").ToString() == "OFICIAL" && ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("signo").ToString() == "POSITIVO")
                {
                    var movementData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementwithSourceMovementId, args: new { SourceMovementTransactionId = ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("idMovimientoTRUE").ToString() }).ConfigureAwait(false);
                    Assert.AreEqual(movementData["NetStandardVolume"], ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("cantidadNeta").ToString());
                    Assert.AreEqual(2, movementData["ScenarioId"].ToInt());
                    Assert.AreEqual(movementData["SourceMovementTransactionId"].ToString(), ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken(field).ToString());
                    Assert.AreEqual("188", movementData["SourceSystem"].ToString());
                    Assert.AreEqual("3", movementData["OfficialDeltaMessageTypeId"].ToString());
                    Assert.AreEqual(this.GetValue("OfficialDeltaTicketId"), movementData["OfficialDeltaTicketId"].ToString());

                    var movementOwners = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOwnershipOfMovement, args: new { MovementTransactionId = movementData["MovementTransactionId"].ToInt() }).ConfigureAwait(false);
                    var movementOwnersList = movementOwners.ToDictionaryList();
                    for (var j = 0; j < movementOwnersList.Count; j++)
                    {
#pragma warning disable CA1307 // Specify StringComparison
                        Assert.IsTrue(movementOwnersList[j]["OwnershipValue"].ToString().Equals(ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("deltaOficial").ToString()));
#pragma warning restore CA1307 // Specify StringComparison
                    }
                }
            }
        }

        [Then(@"Verify that system should obtain the data of the official movement stored in TRUE using the value of the field ""(.*)"" returned by FICO for Negative Operative Movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldObtainTheDataOfTheOfficialMovementStoredInTRUEUsingTheValueOfTheFieldReturnedByFICOForNegativeOperativeMovementsAsync(string field)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData["payload"]["payloadOutput"]["resultadoMovimientos"]);
            for (var i = 0; i < ficoData["payload"]["payloadOutput"]["resultadoMovimientos"].Count(); i++)
            {
                if (ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("origen").ToString() == "CONSOLIDADO" && ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("signo").ToString() == "NEGATIVO")
                {
                    var movementData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementwithConsolidatedMovementId, args: new { ConsolidatedMovementTransactionID = ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("idMovimientoTRUE").ToString() }).ConfigureAwait(false);
                    Assert.AreEqual(movementData["NetStandardVolume"].ToInt().ToString(CultureInfo.InvariantCulture), ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("cantidadNeta").ToInt().ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(2, movementData["ScenarioId"].ToInt());
                    Assert.AreEqual(movementData["ConsolidatedMovementTransactionID"].ToString(), ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken(field).ToString());
                    Assert.AreEqual("188", movementData["SourceSystemId"].ToString());
                    Assert.AreEqual("4", movementData["OfficialDeltaMessageTypeId"].ToString());
                    Assert.AreEqual(this.GetValue("OfficialDeltaTicketId"), movementData["OfficialDeltaTicketId"].ToString());

                    var movementOwners = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOwnershipOfMovement, args: new { MovementTransactionId = movementData["MovementTransactionId"].ToInt() }).ConfigureAwait(false);
                    var movementOwnersList = movementOwners.ToDictionaryList();
                    for (var j = 0; j < movementOwnersList.Count; j++)
                    {
                        Assert.IsTrue(movementOwnersList[j]["OwnershipValue"].ToInt().Equals(ficoData["payload"]["payloadOutput"]["resultadoMovimientos"][i].SelectToken("deltaOficial").ToInt() * -1));
                    }
                }
            }
        }

        [Then(@"Verify that system should obtain the data of the official movement stored in TRUE using the value of the field ""(.*)"" returned by FICO for Positive Operative Movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldObtainTheDataOfTheOfficialMovementStoredInTRUEUsingTheValueOfTheFieldReturnedByFICOForPositiveOperativeMovementsAsync(string collection)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData[collection]);
            for (var i = 0; i < ficoData[collection].Count(); i++)
            {
                if (ficoData[collection][i].SelectToken("origen").ToString() == "CONSOLIDADO" && ficoData[collection][i].SelectToken("signo").ToString() == "POSITIVO")
                {
                    var movementData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementwithSourceMovementId, args: new { SourceMovementTransactionId = ficoData[collection][i].SelectToken("idMovimientoTRUE").ToString() }).ConfigureAwait(false);
                    Assert.AreEqual(movementData["NetStandardVolume"], ficoData[collection][i].SelectToken("cantidadNeta").ToString());
                    Assert.AreEqual(2, movementData["ScenarioId"].ToInt());
                    Assert.AreEqual(this.GetValue("AnnulationMovementTypeId"), movementData["MovementTypeId"].ToString());
                    Assert.AreEqual(movementData["SourceMovementTransactionId"].ToString(), ficoData[collection][i].SelectToken("idMovimientoTRUE").ToString());
                    Assert.AreEqual("FICO", movementData["SourceSystem"].ToString());
                    Assert.AreEqual("FICO", movementData["OperationalDate"].ToString()); //// Need to Work
                    Assert.AreEqual(ConstantValues.EcoPetrolId, movementData["SourceSystem"].ToString()); //// Need to work

                    var movementOwners = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOwnershipOfMovement, args: new { MovementTransactionId = movementData["MovementTransactionId"].ToInt() }).ConfigureAwait(false);
                    var movementOwnersList = movementOwners.ToDictionaryList();
                    for (var j = 0; j < movementOwnersList.Count; j++)
                    {
#pragma warning disable CA1307 // Specify StringComparison
                        Assert.IsTrue(movementOwnersList[j]["OwnershipValue"].ToString().Equals(ficoData[collection][i].SelectToken("deltaOficial").ToString()));
#pragma warning restore CA1307 // Specify StringComparison
                    }
                }
            }
        }

        [Then(@"Verify that system should obtain the data of the official inventory stored in TRUE using the value of the field ""(.*)"" returned by FICO for Negative Official Movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldObtainTheDataOfTheOfficialInventoryStoredInTRUEUsingTheValueOfTheFieldReturnedByFICOForNegativeOfficialMovementsAsync(string field)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData["payload"]["payloadOutput"]["resultadoInventarios"]);
            for (var i = 0; i < ficoData["payload"]["payloadOutput"]["resultadoInventarios"].Count(); i++)
            {
                if (ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("origen").ToString() == "OFICIAL" && ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("signo").ToString() == "NEGATIVO")
                {
                    var movementData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementwithSourceInventoryId, args: new { SourceInventoryProductId = ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("idInventarioTRUE").ToString() }).ConfigureAwait(false);
                    Assert.AreEqual(movementData["NetStandardVolume"].ToInt().ToString(CultureInfo.InvariantCulture), ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("cantidadNeta").ToInt().ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(2, movementData["ScenarioId"].ToInt());
                    Assert.AreEqual(movementData["SourceInventoryProductId"].ToString(), ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken(field).ToString());
                    Assert.AreEqual("188", movementData["SourceSystemId"].ToString());
                    Assert.AreEqual("1", movementData["OfficialDeltaMessageTypeId"].ToString());
                    Assert.AreEqual(this.GetValue("OfficialDeltaTicketId"), movementData["OfficialDeltaTicketId"].ToString());

                    var movementOwners = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOwnershipOfMovement, args: new { MovementTransactionId = movementData["MovementTransactionId"].ToInt() }).ConfigureAwait(false);
                    var movementOwnersList = movementOwners.ToDictionaryList();
                    for (var j = 0; j < movementOwnersList.Count; j++)
                    {
#pragma warning disable CA1307 // Specify StringComparison
                        Assert.IsTrue(movementOwnersList[j]["OwnershipValue"].ToString().Equals(ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("deltaOficial").ToString()));
#pragma warning restore CA1307 // Specify StringComparison
                    }
                }
            }
        }

        [Then(@"Verify that system should obtain the data of the official inventory stored in TRUE using the value of the field ""(.*)"" returned by FICO for Positive Official Movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldObtainTheDataOfTheOfficialInventoryStoredInTRUEUsingTheValueOfTheFieldReturnedByFICOForPositiveOfficialMovementsAsync(string field)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData["payload"]["payloadOutput"]["resultadoInventarios"]);
            for (var i = 0; i < ficoData["payload"]["payloadOutput"]["resultadoInventarios"].Count(); i++)
            {
                if (ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("origen").ToString() == "OFICIAL" && ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("signo").ToString() == "POSITIVO")
                {
                    var movementData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementwithSourceInventoryId, args: new { SourceInventoryProductId = ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("idInventarioTRUE").ToString() }).ConfigureAwait(false);
                    Assert.AreEqual(movementData["NetStandardVolume"].ToInt().ToString(CultureInfo.InvariantCulture), ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("cantidadNeta").ToInt().ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(2, movementData["ScenarioId"].ToInt());
                    Assert.AreEqual(movementData["SourceInventoryProductId"].ToString(), ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken(field).ToString());
                    Assert.AreEqual("188", movementData["SourceSystemId"].ToString());
                    Assert.AreEqual("1", movementData["OfficialDeltaMessageTypeId"].ToString());
                    Assert.AreEqual(this.GetValue("OfficialDeltaTicketId"), movementData["OfficialDeltaTicketId"].ToString());

                    var movementOwners = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOwnershipOfMovement, args: new { MovementTransactionId = movementData["MovementTransactionId"].ToInt() }).ConfigureAwait(false);
                    var movementOwnersList = movementOwners.ToDictionaryList();
                    for (var j = 0; j < movementOwnersList.Count; j++)
                    {
#pragma warning disable CA1307 // Specify StringComparison
                        Assert.IsTrue(movementOwnersList[j]["OwnershipValue"].ToString().Equals(ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("deltaOficial").ToString()));
#pragma warning restore CA1307 // Specify StringComparison
                    }
                }
            }
        }

        [When(@"The movements start and end dates match the start and end dates of the period")]
        [When(@"I have an consolidated movements in the system where source or destination node is equal to the nodes of the segments official delta ticket")]
        public void WhenIHaveAnConsolidatedMovementsInTheSystemWhereSourceOrDestinationNodeIsEqualToTheNodesOfTheSegmentsOfficialDeltaTicket()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Verify that system should obtain the data of the official inventory stored in TRUE using the value of the field ""(.*)"" returned by FICO for Negative Consolidation Movements")]
        public async System.Threading.Tasks.Task ThenVerifyThatSystemShouldObtainTheDataOfTheOfficialInventoryStoredInTRUEUsingTheValueOfTheFieldReturnedByFICOForNegativeConsolidationMovementsAsync(string field)
        {
            var ficoData = JObject.Parse(this.ScenarioContext["json"].ToString());
            Assert.IsNotNull(ficoData["payload"]["payloadOutput"]["resultadoInventarios"]);
            for (var i = 0; i < ficoData["payload"]["payloadOutput"]["resultadoInventarios"].Count(); i++)
            {
                if (ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("origen").ToString() == "CONSOLIDADO" && ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("signo").ToString() == "NEGATIVO")
                {
                    var movementData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetMovementwithConsolidatedInventoryId, args: new { ConsolidatedInventoryProductId = ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("idInventarioTRUE").ToString() }).ConfigureAwait(false);
                    Assert.AreEqual(movementData["NetStandardVolume"].ToInt().ToString(CultureInfo.InvariantCulture), ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("cantidadNeta").ToInt().ToString(CultureInfo.InvariantCulture));
                    Assert.AreEqual(2, movementData["ScenarioId"].ToInt());
                    Assert.AreEqual(movementData["ConsolidatedInventoryProductId"].ToString(), ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken(field).ToString());
                    Assert.AreEqual("188", movementData["SourceSystemId"].ToString());
                    Assert.AreEqual("2", movementData["OfficialDeltaMessageTypeId"].ToString());
                    Assert.AreEqual(this.GetValue("OfficialDeltaTicketId"), movementData["OfficialDeltaTicketId"].ToString());

                    var movementOwners = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOwnershipOfMovement, args: new { MovementTransactionId = movementData["MovementTransactionId"].ToInt() }).ConfigureAwait(false);
                    var movementOwnersList = movementOwners.ToDictionaryList();
                    for (var j = 0; j < movementOwnersList.Count; j++)
                    {
#pragma warning disable CA1307 // Specify StringComparison
                        Assert.IsTrue(movementOwnersList[j]["OwnershipValue"].ToInt().Equals(ficoData["payload"]["payloadOutput"]["resultadoInventarios"][i].SelectToken("deltaOficial").ToInt()));
#pragma warning restore CA1307 // Specify StringComparison
                    }
                }
            }
        }
    }
}