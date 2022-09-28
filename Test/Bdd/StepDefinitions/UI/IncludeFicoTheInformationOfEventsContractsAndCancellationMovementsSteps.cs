// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncludeFicoTheInformationOfEventsContractsAndCancellationMovementsSteps.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
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
    public class IncludeFicoTheInformationOfEventsContractsAndCancellationMovementsSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have ownershipcalculation for segment with events information for same segment within the Range of OwnershipCalculation date")]
        public async Task GivenIHaveOwnershipcalculationForSegmentWithEventsInformationForSameSegmentAsync()
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated and \"with\" Events of same sagments are processed");
            await this.CalculatedOwnershipForSegmentwithEventsofSameSegmentAndTicketGeneratedExAsync("with").ConfigureAwait(false);
        }

        [Given(@"I have ownershipcalculation for segment which did not have events information for same segment within the Range of OwnershipCalculation date")]
        public async Task GivenIHaveOwnershipcalculationForSegmentWhichDidNotHaveEventsAsync()
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated and \"without\" Events of same sagments are processed");
            await this.CalculatedOwnershipForSegmentwithEventsofSameSegmentAndTicketGeneratedExAsync("without").ConfigureAwait(false);
        }

        [Given(@"I have events information for segment with Movements and inventories but not within the range of OwnershipCalculation date")]
        public async Task GivenIHaveEventsInformationForSegmentWithMovementsAndInventoriesButNotWithinTheRangeOfOwnershipDateAsync()
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated and \"within the ownership calculated range none of the\" Events of same sagments are processed");
            await this.CalculatedOwnershipForSegmentwithEventsofSameSegmentAndTicketGeneratedExAsync("within the ownership calculated range none of the").ConfigureAwait(false);
        }

        [StepDefinition(@"Verify that System should send the information details to the FICO ""(.*)"" Data")]
        public async Task ThenVerifyThatSystemShouldSendTheInformationDetailsToTheFICODataAsync(string state)
        {
            if (state == "with")
            {
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithTicketDesc).ConfigureAwait(false);
                this.SetValue("fileName", ticketData["TicketId"].ToString());
            }
            else if (state == "without")
            {
                var ticketData = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketIdwithCreatedDateDesc).ConfigureAwait(false);
                this.SetValue("fileName", ticketData["TicketId"].ToString());
            }

            var fileName = this.GetValue("fileName");
            this.ScenarioContext["json"] = await fileName.OwnershipdatafromBlobAsync("ownershiprule").ConfigureAwait(false);
            Assert.IsNotNull(this.ScenarioContext["json"]);
        }

        [Then(@"Verify that system should send the events details in the ""(.*)"" array of FICO Collection")]
        public void ThenVerifyThatSystemShouldSendTheEventsDetailsInTheArrayOfFICOCollection(string collection)
        {
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"][collection];
            Assert.IsNotNull(messageData);
            for (var i = 0; i < messageData.Count(); i++)
            {
                Assert.AreEqual(ConstantValues.EcoPetrolId, messageData[i].SelectToken("idPropietario1").ToString());
                Assert.AreEqual(ConstantValues.EquionId, messageData[i].SelectToken("idPropietario2").ToString());
                Assert.AreEqual(ConstantValues.NetVolume, messageData[i].SelectToken("valorPropiedad").ToString());
                Assert.AreEqual(ConstantValues.Volumen, messageData[i].SelectToken("unidadPropiedad").ToString());
            }
        }

        [Then(@"Verify that ""(.*)"" array of FICO Collection should not contains any data")]
        public void ThenVerifyThatArrayOfFICOCollectionShouldNotContainsAnyData(string collection)
        {
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"][collection];
            Assert.IsFalse(messageData.HasValues);
        }

        [Given(@"I have ownershipcalculation for segment and SalesAndPurchase information for same segment within the Range of OwnershipCalculation date")]
        public async Task GivenIWantToCalculateTheOwnershipOfInventoriesAndMovementsOfASegmentwithContractAsync()
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated and \"with\" Contracts are processed");
            await this.UserCalculatedOwnershipForSegmentwithContractsAndTicketGeneratedAsync("with").ConfigureAwait(false);
        }

        [Given(@"I have ownershipcalculation for segment and did not have SalesAndPurchase information for same segment within the Range of OwnershipCalculation date")]
        public async Task GivenIDidNotHaveContractsInformationForSegmentAsync()
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated and \"without\" Contracts are processed");
            await this.UserCalculatedOwnershipForSegmentwithContractsAndTicketGeneratedAsync("without").ConfigureAwait(false);
        }

        [Given(@"I have SalesAndPurchase information for segment but not within the range of OwnershipCalculation date")]
        public async Task GivenIHaveSalesAndPurchaseInformationForSegmentButNotWithinTheRangeOfOwnershipCalculationDateAsync()
        {
            ////this.Given("Ownership is Calculated for Segment and Ticket is Generated and \"within the ownership calculated range none of the\" Contracts are processed");
            await this.UserCalculatedOwnershipForSegmentwithContractsAndTicketGeneratedAsync("within the ownership calculated range none of the").ConfigureAwait(false);
        }

        [Then(@"Verify that system should send the contracts details in the ""(.*)"" array of FICO Collection")]
        public void ThenVerifyThatSystemShouldSendTheContractsDetailsInTheArrayOfFICOCollection(string collection)
        {
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"][collection];
            Assert.IsNotNull(messageData);
            for (var i = 0; i < messageData.Count(); i++)
            {
                Assert.AreEqual(ConstantValues.EcoPetrolId, messageData[i].SelectToken("idPropietarioComprador").ToString());
                Assert.AreEqual(ConstantValues.EquionId, messageData[i].SelectToken("idPropietarioVendedor").ToString());
                Assert.AreEqual(ConstantValues.NetVolume, messageData[i].SelectToken("valor").ToString());
                Assert.AreEqual(ConstantValues.Volumen, messageData[i].SelectToken("unidad").ToString());
            }
        }

        [Then(@"Verify that system should send the ownership percentage details within ""(.*)"" object in ""(.*)"" array of FICO Collection")]
        public async Task ThenVerifyThatSystemShouldSendOwnershipPercentageWithinObjectInArrayOfFICOAsync(string field, string collectionName)
        {
            var ownerResult = await this.ReadSqlAsDictionaryAsync(SqlQueries.StorageLocationOwnershipowner, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"][collectionName];
            Assert.IsNotNull(messageData);
            Assert.AreEqual(messageData[0].SelectToken(field).ToString(), ownerResult["OwnershipPercentage"].ToInt().ToString(CultureInfo.InvariantCulture));
        }

        [Then(@"Verify that system should send null details within ""(.*)"" object in ""(.*)"" array of FICO Collection")]
        public void ThenVerifyThatSystemShouldSendNullDetailsWithinObjectInArrayOfFICOCollection(string field, string collectionName)
        {
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"][collectionName];
            Assert.IsNotNull(messageData);
            Assert.AreEqual("0", messageData[0].SelectToken(field).ToString());
        }

        [Given(@"Variable data for node storage location for which ""(.*)"" value is set to True")]
        public async System.Threading.Tasks.Task GivenVariableDataForNodeStorageLocationForWhichValueIsSetToTrueAsync(string variable)
        {
            var storagelocationProductIds = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetStorageLocationProduct, args: new { @node = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            foreach (var storagelocationProductId in storagelocationProductIds)
            {
                this.ScenarioContext["StorageLocationProductId"] = storagelocationProductId["StorageLocationProductId"].ToString();
                var variableTypeId = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetVariableIds, args: new { @fico = variable }).ConfigureAwait(false);
                this.ScenarioContext["VariableId"] = variableTypeId["VariableTypeId"];
                await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertvariableintoStorage, args: new { storageid = this.ScenarioContext["StorageLocationProductId"], variableid = this.ScenarioContext["VariableId"].ToString() }).ConfigureAwait(false);
            }
        }

        [Given(@"I have movements of the evacuation type registered the day before the day of the period being executed through FICO Service Response")]
        public async Task GivenIHaveMovementsOfTheEvacuationTypeRegisteredTheDayBeforeTheDayOfThePeriodBeingExecutedThroughFICOServiceResponseAsync()
        {
            var evacuationMovements = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetEvacuationMovement, args: new { OperationDate = DateTime.UtcNow.AddDays(-4).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) }).ConfigureAwait(false);
            Assert.IsNotNull(evacuationMovements);
        }

        [Given(@"I have created a new cancellation movements through the ownership edition page with Annuluation movement type")]
        public void GivenIHaveCreatedANewCancellationMovementsThroughTheOwnershipEditionPageWithAnnuluationMovementType()
        {
            ////This step left intentionally blank.
        }

        [Then(@"System should generates and temporarily stores the cancellation movements to send them FICO with movement type as ""(.*)"" and ""(.*)""")]
        public async Task ThenSystemShouldGeneratesAndTemporarilyStoresTheCancellationMovementsToSendThemFICOWithMovementTypeAsAndAsync(string collection1, string collection2)
        {
            var cancellationMovements = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetCancellationMovements, args: new { OperationDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), Category1 = collection1, Category2 = collection2 }).ConfigureAwait(false);
            Assert.IsNotNull(cancellationMovements);
        }

        [Then(@"System should generates and temporarily stores the cancellation movements for both the movements registered through service and UI to send them FICO with movement type as ""(.*)"" and ""(.*)""")]
        [Then(@"Verify that system should add the new cancellation movements generated for the day of the period in movements array of FICO Collection for ""(.*)"" and ""(.*)""")]
        public async Task ThenVerifyThatSystemShouldAddTheNewCancellationMovementsGeneratedForTheDayOfThePeriodInMovementsArrayOfFICOCollectionForAndAsync(string collection1, string collection2)
        {
            var cancellationMovementsinDB = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetCancellationMovements, args: new { OperationDate = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), Category1 = collection1, Category2 = collection2 }).ConfigureAwait(false);
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"]["movimientos"];
            Assert.IsNotNull(messageData);
            for (var i = 0; i < messageData.Count(); i++)
            {
                if (messageData[i].SelectToken("tipoMovimiento").ToString() == "ANULACIONENTRADA")
                {
                    Assert.AreEqual(ConstantValues.OriginSourceProductId, messageData[i].SelectToken("idProductoOrigen").ToString());
                    Assert.AreEqual(ConstantValues.Volumen, messageData[i].SelectToken("unidadPropiedad").ToString());
                    Assert.AreEqual(ConstantValues.NetVolume, messageData[i].SelectToken("valorPropiedad").ToString());
                    Assert.AreEqual(ConstantValues.NetVolume, messageData[i].SelectToken("volumenNeto").ToString());
                    Assert.AreEqual(ConstantValues.EquionId, messageData[i].SelectToken("idPropietario").ToString());
                    this.SetValue("ProductOrigininJSON", messageData[i].SelectToken("idProductoOrigen").ToString());
                    this.SetValue("NodeOrigininJSON", messageData[i].SelectToken("idNodoOrigen").ToString());
                }
                else if (messageData[i].SelectToken("tipoMovimiento").ToString() == "ANULACIONSALIDA")
                {
                    Assert.AreEqual(ConstantValues.DestinationSourceProductId, messageData[i].SelectToken("idProductoDestino").ToString());
                    Assert.AreEqual(ConstantValues.Volumen, messageData[i].SelectToken("unidadPropiedad").ToString());
                    Assert.AreEqual(ConstantValues.NetVolume, messageData[i].SelectToken("valorPropiedad").ToString());
                    Assert.AreEqual(ConstantValues.NetVolume, messageData[i].SelectToken("volumenNeto").ToString());
                    this.SetValue("ProductDestinationinJSON", messageData[i].SelectToken("idProductoDestino").ToString());
                    this.SetValue("NodeDestinationinJSON", messageData[i].SelectToken("idNodoDestino").ToString());
                }
            }

            var movementsList = cancellationMovementsinDB.ToDictionaryList();
            foreach (var movements in movementsList)
            {
                Assert.AreEqual(ConstantValues.FICO, movements["SourceSystem"]);
                Assert.AreEqual(ConstantValues.NetVolume, movements["NetStandardVolume"].ToInt().ToString(CultureInfo.InvariantCulture));

                var cancellationDestinationDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetCancellationDestinationDetails, args: new { MovementTransactionID = movements["MovementTransactionId"].ToString() }).ConfigureAwait(false);
                if (cancellationDestinationDetails != null)
                {
                    Assert.AreEqual(this.GetValue("ProductDestinationinJSON"), cancellationDestinationDetails["DestinationProductId"].ToString());
                    Assert.AreEqual(this.GetValue("NodeDestinationinJSON"), cancellationDestinationDetails["DestinationNodeId"].ToString());
                }

                var cancellationSourceDetails = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetCancellationSourceDetails, args: new { MovementTransactionID = movements["MovementTransactionId"].ToString() }).ConfigureAwait(false);
                if (cancellationSourceDetails != null)
                {
                    Assert.AreEqual(this.GetValue("ProductOrigininJSON"), cancellationSourceDetails["SourceProductId"].ToString());
                    Assert.AreEqual(this.GetValue("NodeOrigininJSON"), cancellationSourceDetails["SourceNodeId"].ToString());
                }
            }
        }

        [When(@"the service returns data in ""(.*)"" collection")]
        public void WhenTheServiceReturnsDataInCollection(string collectionName)
        {
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volOutput"][collectionName];
            this.ScenarioContext["MovementTransactioninResponse"] = messageData[0].SelectToken("idMovimiento").ToString();
            Assert.IsNotNull(messageData);
        }

        [Then(@"system should register the new contract movements with ownership details")]
        public async Task ThenSystemShouldRegisterTheNewContractMovementsWithOwnershipDetailsAsync()
        {
            var contractsMovementsinDB = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNewContractMovements, args: new { OperationDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), SegmentName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            Assert.IsNotNull(contractsMovementsinDB);
        }

        [Then(@"Verify that the the movements must be assigned to the ownership ticket generated for the day of the period")]
        [Then(@"Verify that source system value should be FICO")]
        [Then(@"Verify that net volume must be equal to the value volumen returned by FICO")]
        public async Task ThenVerifyThatValueInShouldBeEitherOrAsync()
        {
            var contractsMovementsinDB = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNewContractMovements, args: new { OperationDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), SegmentName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            var contractMovementList = contractsMovementsinDB.ToDictionaryList();
            foreach (var movement in contractMovementList)
            {
                this.ScenarioContext["MovementTransactionId"] = movement["MovementTransactionId"].ToString();
                Assert.AreEqual(ConstantValues.FICO, movement["SourceSystem"]);
                Assert.AreEqual(ConstantValues.NetVolume, movement["NetStandardVolume"].ToInt().ToString(CultureInfo.InvariantCulture));
            }
        }

        [Then(@"Verify that source node for new movement should be equal to the Destination Node of original Movement for Movement Types")]
        public async Task ThenVerifyThatSourceNodeForNewMovementShouldBeEqualToTheDestinationNodeOfOriginalMovementForVentaMovementTypesAsync()
        {
            var newMovementsSourceinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementSource, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactionId"] }).ConfigureAwait(false);
            var newMovementsDestinationinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementDestination, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninResponse"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(newMovementsSourceinDB["SourceNodeId"].ToString(), newMovementsDestinationinDB["DestinationNodeId"].ToString());
        }

        [Then(@"Verify that the source product type for new movement should be equal to the destination product of original Movement for Movement Types")]
        public async Task ThenVerifyThatTheSourceProductTypeForNewMovementShouldBeEqualToTheDestinationProductOfOriginalMovementForVentaMovementTypesAsync()
        {
            var newMovementsSourceinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementSource, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactionId"] }).ConfigureAwait(false);
            var newMovementsDestinationinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementDestination, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninResponse"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(newMovementsSourceinDB["SourceProductId"].ToString(), newMovementsDestinationinDB["DestinationProductId"].ToString());
        }

        [When(@"the value of the field ""(.*)"" is equal to ""(.*)""")]
        public void WhenTheValueOfTheFieldIsEqualTo(string collectionObject, string value)
        {
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volOutput"]["movimientosNuevos"];
            if (value == ConstantValues.Colaboracion)
            {
                Assert.AreEqual(value, messageData[1].SelectToken(collectionObject).ToString());
            }
            else if (value == ConstantValues.Evacuacion)
            {
                Assert.AreEqual(value, messageData[0].SelectToken(collectionObject).ToString());
            }
        }

        [Then(@"system should register the new movements with ownership details")]
        public async Task ThenSystemShouldRegisterTheNewMovementsWithOwnershipDetailsAsync()
        {
            var colloboracioneventMovementsinDB = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNewCollaborationEventMovements, args: new { OperationDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), SegmentName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            Assert.IsNotNull(colloboracioneventMovementsinDB);
        }

        [Then(@"Verify that source system value should be FICO for Collaboracion Movements")]
        [Then(@"Verify that net volume must be equal to the value volumenPropiedad returned by FICO for Collaboracion Movements")]
        [Then(@"Verify that the the movements must be assigned to the ownership ticket generated for the day of the period for Collaboracion Movements")]
        public async Task ThenVerifyThatSourceSystemValueShouldBeFICOForCollaboracionMovementsAsync()
        {
            var eventsMovementsinDB = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNewCollaborationEventMovements, args: new { OperationDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), SegmentName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            var eventsMovementList = eventsMovementsinDB.ToDictionaryList();
            foreach (var movement in eventsMovementList)
            {
                this.ScenarioContext["MovementTransactioninCollaborationEventsDB"] = movement["MovementTransactionId"].ToString();
                Assert.AreEqual(ConstantValues.FICO, movement["SourceSystem"]);
                Assert.AreEqual(ConstantValues.NetVolume, movement["NetStandardVolume"].ToInt().ToString(CultureInfo.InvariantCulture));
            }
        }

        [Then(@"Verify that source node for new movement should be equal to the Destination Node of original Movement for Collaboracion Movement Types")]
        public async Task ThenVerifyThatSourceNodeForNewMovementShouldBeEqualToTheDestinationNodeOfOriginalMovementForCollaboracionMovementTypesAsync()
        {
            var newMovementsSourceinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementSource, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninCollaborationEventsDB"] }).ConfigureAwait(false);
            var newMovementsDestinationinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementDestination, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninResponse"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(newMovementsSourceinDB["SourceNodeId"].ToString(), newMovementsDestinationinDB["DestinationNodeId"].ToString());
        }

        [Then(@"Verify that the source product type for new movement should be equal to the destination product of original Movement for Collaboracion Movement Types")]
        public async Task ThenVerifyThatTheSourceProductTypeForNewMovementShouldBeEqualToTheDestinationProductOfOriginalMovementForCollaboracionMovementTypesAsync()
        {
            var newMovementsSourceinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementSource, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninCollaborationEventsDB"] }).ConfigureAwait(false);
            var newMovementsDestinationinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementDestination, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninResponse"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(newMovementsSourceinDB["SourceProductId"].ToString(), newMovementsDestinationinDB["DestinationProductId"].ToString());
        }

        [Then(@"system should register the new movements with ownership details for Evacuation Movements")]
        public async Task ThenSystemShouldRegisterTheNewMovementsWithOwnershipDetailsForEvacuationMovementsAsync()
        {
            var evacuationeventMovementsinDB = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNewCollaborationEventMovements, args: new { OperationDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), SegmentName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            Assert.IsNotNull(evacuationeventMovementsinDB);
        }

        [Then(@"Verify that the the movements must be assigned to the ownership ticket generated for the day of the period for Evacuation Movements")]
        [Then(@"Verify that net volume must be equal to the value volumenPropiedad returned by FICO")]
        public async Task ThenVerifyThatTheTheMovementsMustBeAssignedToTheOwnershipTicketGeneratedForTheDayOfThePeriodForEvacuationMovementsAsync()
        {
            var eventsMovementsinDB = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNewEvacuationEventMovements, args: new { OperationDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), SegmentName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            var eventsMovementList = eventsMovementsinDB.ToDictionaryList();
            foreach (var movement in eventsMovementList)
            {
                this.ScenarioContext["MovementTransactioninEvacutaionEventsDB"] = movement["MovementTransactionId"].ToString();
                Assert.AreEqual(ConstantValues.FICO, movement["SourceSystem"]);
                Assert.AreEqual(ConstantValues.NetVolume, movement["NetStandardVolume"].ToInt().ToString(CultureInfo.InvariantCulture));
            }
        }

        [Then(@"Verify that source node for new movement should be equal to the Destination Node of original Movement for Evacuation Movement Types")]
        public async Task ThenVerifyThatSourceNodeForNewMovementShouldBeEqualToTheDestinationNodeOfOriginalMovementForEvacuationMovementTypesAsync()
        {
            var newMovementsSourceinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementSource, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninEvacutaionEventsDB"] }).ConfigureAwait(false);
            var newMovementsDestinationinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementDestination, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninResponse"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(newMovementsSourceinDB["SourceNodeId"].ToString(), newMovementsDestinationinDB["DestinationNodeId"].ToString());
        }

        [Then(@"Verify that the source product type for new movement should be equal to the destination product of original Movement for Evacuation Movement Types")]
        public async Task ThenVerifyThatTheSourceProductTypeForNewMovementShouldBeEqualToTheDestinationProductOfOriginalMovementForEvacuationMovementTypesAsync()
        {
            var newMovementsSourceinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementSource, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninEvacutaionEventsDB"] }).ConfigureAwait(false);
            var newMovementsDestinationinDB = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNewMovementDestination, args: new { MovementTransactionId = this.ScenarioContext["MovementTransactioninResponse"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(newMovementsSourceinDB["SourceProductId"].ToString(), newMovementsDestinationinDB["DestinationProductId"].ToString());
        }

        [Then(@"the service returns empty data one or both of the collection")]
        public void ThenTheServiceReturnsEmptyDataOneOrBothOfTheCollection()
        {
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var eventosCollectionData = ficoRequestData["volPayload"]["volInput"]["eventos"];
            Assert.IsFalse(eventosCollectionData.HasValues);
            var contractCollectionDataCollectionData = ficoRequestData["volPayload"]["volInput"]["movimientosComerciales"];
            Assert.IsFalse(contractCollectionDataCollectionData.HasValues);
        }

        [Then(@"Verify that system should not sent data in ""(.*)"" array of FICO Collection")]
        public void ThenVerifyThatSystemShouldNotSentDataInArrayOfFICOCollection(string collectionName)
        {
            var ficoRequestData = JObject.Parse(this.ScenarioContext["json"].ToString());
            var messageData = ficoRequestData["volPayload"]["volInput"][collectionName];
            Assert.IsNotNull(messageData);
            for (var i = 0; i < messageData.Count(); i++)
            {
                Assert.IsFalse(messageData[i].SelectToken("tipoMovimiento").ToString() == "ANULACIONENTRADA");
                Assert.IsFalse(messageData[i].SelectToken("tipoMovimiento").ToString() == "ANULACIONSALIDA");
            }
        }
    }
}